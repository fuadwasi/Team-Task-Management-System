using AssetForge.App.Extensions;
using AssetForge.App.Models.Api;
using AssetForge.Core;
using AssetForge.Core.Domain;
using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Infrastructure;
using AssetForge.Data;
using AssetForge.Services.Customers;
using AssetForge.Services.Localization;
using AssetForge.Services.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;


namespace AssetForge.App.Filters;

public sealed class JwtAuthAttribute : TypeFilterAttribute
{
    #region Ctor

    /// <summary>
    /// Create instance of the filter attribute
    /// </summary>
    /// <param name="ignore">Whether to ignore the execution of filter actions</param>
    public JwtAuthAttribute(bool ignore = false) : base(typeof(JwtAuthFilter))
    {
        IgnoreFilter = ignore;
        Arguments = [ignore];
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets a value indicating whether to ignore the execution of filter actions
    /// </summary>
    public bool IgnoreFilter { get; }

    #endregion

    #region Nested filter

    /// <summary>
    /// Represents a filter that confirms access to closed site
    /// </summary>
    private class JwtAuthFilter : IAsyncActionFilter
    {
        #region Fields

        protected readonly bool _ignoreFilter;

        #endregion

        #region Ctor

        public JwtAuthFilter(bool ignoreFilter = false)
        {
            _ignoreFilter = ignoreFilter;
        }

        #endregion

        #region Utilities


        #endregion

        #region Methods

        /// <summary>
        /// Called asynchronously before the action, after model binding is complete.
        /// </summary>
        /// <param name="context">A context for action filters</param>
        /// <param name="next">A delegate invoked to execute the next action filter or the action itself</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        //public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        //{
        //    await JwtAuthAsync(context);
        //    if (context.Result == null)
        //        await next();
        //}

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!DataSettingsManager.IsDatabaseInstalled())
                return;

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var actionFilter = context.ActionDescriptor.FilterDescriptors
                .Where(f => f.Scope == FilterScope.Action)
                .Select(f => f.Filter)
                .OfType<JwtAuthAttribute>()
                .FirstOrDefault();

            if (actionFilter?.IgnoreFilter ?? _ignoreFilter)
                return;

            var httpContext = context.HttpContext;
            var customerService = EngineContext.Current.Resolve<ICustomerService>();
            var workContext = EngineContext.Current.Resolve<IWorkContext>();

            string token = null;
            if (httpContext.Request.Headers.TryGetValue(WebApiCustomerDefaults.Token, out var tokenKey))
            {
                token = tokenKey.FirstOrDefault();
            }
            else
            {
                var cookieName = $".AssetForge.Customer.Token";
                token = httpContext.Request?.Cookies[cookieName];

                if (string.IsNullOrWhiteSpace(token))
                {
                    var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                    token = webHelper.QueryString<string>("customerToken");
                }
            }

            if (!string.IsNullOrWhiteSpace(token))
            {
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
                    httpContext.Response.ContentType = "application/json";
                    httpContext.Response.StatusCode = 403;
                    await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(baseResponse));
                    return;
                }
            }
            else
            {
                var deviceId = Guid.NewGuid().ToString();
                SetCustomerDeviceIdCookie(httpContext, deviceId);
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

            await next(); // Proceed to the controller action
        }

        private async Task<Customer> InsertDeviceGuestCustomerAsync(string deviceId)
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

        protected virtual void SetCustomerDeviceIdCookie(HttpContext context, string deviceId)
        {
            //delete current cookie value
            var cookieName = $".AssetForge.Customer.DeviceId";
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
        #endregion
    }

    #endregion
}