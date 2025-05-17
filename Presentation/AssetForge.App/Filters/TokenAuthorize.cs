using AssetForge.App.Extensions;
using AssetForge.App.Models.Api;
using AssetForge.App.Models.Common;
using AssetForge.Core.Infrastructure;
using AssetForge.Services.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using static LinqToDB.Reflection.Methods.LinqToDB.Insert;

namespace AssetForge.App.Filters
{
    public class TokenAuthorizeAttribute : TypeFilterAttribute
    {
        #region Ctor

        public TokenAuthorizeAttribute() : base(typeof(TokenAuthorizeAttributeFilter))
        {
        }

        #endregion

        #region Nested class

        public class TokenAuthorizeAttributeFilter : IAuthorizationFilter
        {
            public void OnAuthorization(AuthorizationFilterContext actionContext)
            {
                var identity = ParseAuthorizationHeader(actionContext);
                if (identity == false)
                {
                    Challenge(actionContext);
                    return;
                }
            }

            protected virtual bool ParseAuthorizationHeader(AuthorizationFilterContext actionContext)
            {
                bool check = true;

                if (actionContext.HttpContext.Request.Headers.TryGetValue(WebApiCustomerDefaults.Token, out StringValues checkToken))
                {
                    var token = checkToken.FirstOrDefault();
                    try
                    {
                        var payload = JwtHelper.JwtDecoder.DecodeToObject(token, WebApiCustomerDefaults.JwtSecretKey, true);
                        check = true;
                    }
                    catch
                    {
                        check = false;
                    }
                }

                return check;
            }

            private void Challenge(AuthorizationFilterContext actionContext)
            {
                var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
                var response = new BaseResponseModel
                {
                    ErrorList = new List<string>
                    {
                        localizationService.GetResourceAsync("NopStation.WebApi.Response.InvalidToken").Result
                    }
                };

                actionContext.Result = new ObjectResult(response)
                {
                    StatusCode = 403
                };

                return;
            }
        }

        #endregion
    }
}