using AssetForge.Core.Infrastructure;
using AssetForge.Web.Framework.Infrastructure.Extensions;
using AssetForge.Web.Framework.Mvc.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AssetForge.Web.Framework.Infrastructure
{
    public partial class CommonStartup : IAssetForgeStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //add options feature
            services.AddOptions();

            //add distributed cache
            services.AddDistributedCache();

            //add HTTP session state feature
            services.AddHttpSession();

            //add default HTTP clients
            services.AddAssetForgeHttpClients();

            //add anti-forgery
            services.AddAntiForgery();

            //add theme support
            services.AddThemes();

            //add routing
            services.AddRouting(options =>
            {
                //add constraint key for language
                options.ConstraintMap[RoutingDefaults.LanguageParameterTransformer] = typeof(LanguageParameterTransformer);
            });
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
            //check whether requested page is keep alive page
            application.UseKeepAlive();

            //check whether database is installed
            application.UseInstallUrl();

            //use HTTP session
            application.UseSession();

            //use request localization
            application.UseAssetForgeRequestLocalization();

            //configure PDF
            application.UseAssetForgePdf();
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order => 100; //common services should be loaded after error handlers
    }
}