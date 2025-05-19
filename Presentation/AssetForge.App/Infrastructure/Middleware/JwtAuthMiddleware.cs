using AssetForge.App.Extensions;
using AssetForge.App.Filters;
using AssetForge.App.Models.Api;
using AssetForge.Core;
using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Infrastructure;
using AssetForge.Services.Customers;
using AssetForge.Services.Localization;
using AssetForge.Services.Logging;
using Newtonsoft.Json;

namespace AssetForge.App.Infrastructure.Middleware
{
    public class JwtAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        protected async Task<Customer> InsertDeviceGuestCustomerAsync(string deviceId)
        {
            var customerGuid = HelperExtension.GetGuid(deviceId);
            var customer = new Customer
            {
                CustomerGuid = customerGuid,
                Active = true,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow
            };

            var customerService = EngineContext.Current.Resolve<ICustomerService>();
            //add to 'Guests' role
            var guestRole = await customerService.GetCustomerRoleBySystemNameAsync(CustomerDefaults.GuestsRoleName);
            if (guestRole == null)
                throw new AssetForgeException("'Guests' role could not be loaded");

            await customerService.InsertCustomerAsync(customer);

            await customerService.AddCustomerRoleMappingAsync(new CustomerCustomerRoleMapping
            {
                CustomerId = customer.Id,
                CustomerRoleId = guestRole.Id
            });

            return customer;
        }


        protected virtual void SetCustomerTokenCookie(HttpContext context, string token)
        {
            //delete current cookie value
            var cookieName = $".Nop.Customer.Token";
            context.Response.Cookies.Delete(cookieName);

            //get date of cookie expiration
            var cookieExpires = 24 * 365; //TODO make configurable
            var cookieExpiresDate = DateTime.Now.AddHours(cookieExpires);

            //if passed guid is empty set cookie as expired
            if (string.IsNullOrWhiteSpace(token))
                cookieExpiresDate = DateTime.Now.AddMonths(-1);

            //set new cookie value
            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = cookieExpiresDate
            };
            context.Response.Cookies.Append(cookieName, token, options);
        }

        protected virtual void SetCustomerDeviceIdCookie(HttpContext context, string deviceId)
        {
            //delete current cookie value
            var cookieName = $".Nop.Customer.DeviceId";
            context.Response.Cookies.Delete(cookieName);

            //get date of cookie expiration
            var cookieExpires = 24 * 365; //TODO make configurable
            var cookieExpiresDate = DateTime.Now.AddHours(cookieExpires);

            //if passed guid is empty set cookie as expired
            if (string.IsNullOrWhiteSpace(deviceId))
                cookieExpiresDate = DateTime.Now.AddMonths(-1);

            //set new cookie value
            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = cookieExpiresDate
            };
            context.Response.Cookies.Append(cookieName, deviceId, options);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //, IWorkContext workContext, IWebHelper webHelper, ICustomerService customerService
            string token;
            if (context.Request.Headers.TryGetValue(WebApiCustomerDefaults.Token, out var tokenKey))
            {
                token = tokenKey.FirstOrDefault();
            }
            else
            {
                var cookieName = $".Nop.Customer.Token";
                token = context.Request?.Cookies[cookieName];

                if (string.IsNullOrWhiteSpace(token))
                {
                    var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                    token = webHelper.QueryString<string>("customerToken");
                }
            }
            var customerService = EngineContext.Current.Resolve<ICustomerService>();
            var workContext = EngineContext.Current.Resolve<IWorkContext>();

            if (!string.IsNullOrWhiteSpace(token))
            {
                SetCustomerTokenCookie(context, token);
                try
                {
                    var load = JwtHelper.JwtDecoder.DecodeToObject(token, WebApiCustomerDefaults.JwtSecretKey, true);
                    if (load != null)
                    {
                        var customerId = Convert.ToInt32(load[WebApiCustomerDefaults.CustomerId]);
                        var customer = await customerService.GetCustomerByIdAsync(customerId);
                        await workContext.SetCurrentCustomerAsync(customer);
                    }
                }
                catch (Exception ex)
                {
                    var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
                    var baseResponse = new BaseResponseModel();
                    baseResponse.ErrorList.Add(localizationService.GetResourceAsync("NopStation.WebApi.Response.InvalidToken").Result);
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 403;
                    //await _logger.ErrorAsync(ex.Message, ex);
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(baseResponse));
                }
            }
            else
            {
                var deviceId = Guid.NewGuid().ToString();
                SetCustomerDeviceIdCookie(context, deviceId);
                var customerGuid = HelperExtension.GetGuid(deviceId);
                var customer = await customerService.GetCustomerByGuidAsync(customerGuid);
                if (customer != null && await customerService.IsRegisteredAsync(customer))
                {
                    customer.CustomerGuid = Guid.NewGuid();
                    await customerService.UpdateCustomerAsync(customer);
                    customer = await InsertDeviceGuestCustomerAsync(deviceId);
                }
                else if (customer == null)
                    customer = await InsertDeviceGuestCustomerAsync(deviceId);

                await workContext.SetCurrentCustomerAsync(customer);
            }

            await _next(context);
        }
    }
}