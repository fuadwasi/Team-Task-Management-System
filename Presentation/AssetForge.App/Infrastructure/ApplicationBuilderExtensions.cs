using AssetForge.App.Filters;
using AssetForge.App.Models.Api;
using AssetForge.Core;
using AssetForge.Core.Configuration;
using AssetForge.Core.Infrastructure;
using AssetForge.Data;
using AssetForge.Services.Localization;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Runtime.ExceptionServices;

namespace AssetForge.App.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseApiNotFound(this IApplicationBuilder application)
        {
            application.UseStatusCodePages(async context =>
            {
                //handle 404 Not Found
                if (context.HttpContext.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    if (context.HttpContext.Request.Headers.ContainsKey(WebApiCustomerDefaults.DeviceId))
                    {
                        var res = new BaseResponseModel();

                        res.Message = EngineContext.Current.Resolve<ILocalizationService>().GetResourceAsync("NopStation.WebApi.Response.PageNotFound").Result;
                        var json = JsonConvert.SerializeObject(res);
                        context.HttpContext.Response.ContentType = "application/json";
                        await context.HttpContext.Response.WriteAsync(json);
                    }
                }
            });
        }

        public static void UseApiExceptionHandler(this IApplicationBuilder application)
        {
            var appSettings = EngineContext.Current.Resolve<AppSettings>();
            var webHostEnvironment = EngineContext.Current.Resolve<IWebHostEnvironment>();
            var useDetailedExceptionPage = appSettings.Get<CommonConfig>().DisplayFullErrorStack || webHostEnvironment.IsDevelopment();
            if (useDetailedExceptionPage)
            {
                //get detailed exceptions for developing and testing purposes
                application.UseDeveloperExceptionPage();
            }
            else
            {
                //or use special exception handler
                application.UseExceptionHandler("/Error/Error");
            }

            //log errors
            application.UseExceptionHandler(handler =>
            {
                handler.Run(async context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    if (exception == null)
                        return;

                    try
                    {
                        //check whether database is installed
                        if (DataSettingsManager.IsDatabaseInstalled())
                        {
                            //get current customer
                            var currentCustomer = EngineContext.Current.Resolve<IWorkContext>().GetCurrentCustomerAsync().Result;

                            //log error
                            await EngineContext.Current.Resolve<Services.Logging.ILogger>().ErrorAsync(exception.Message, exception, currentCustomer);
                        }
                    }
                    finally
                    {
                        if (context.Request.Headers.ContainsKey(WebApiCustomerDefaults.DeviceId))
                        {
                            var baseResponse = new BaseResponseModel();
                            baseResponse.ErrorList.Add(exception.Message);
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(baseResponse));
                        }
                        //rethrow the exception to show the error page
                        ExceptionDispatchInfo.Throw(exception);
                    }
                });
            });
        }
    }
}
