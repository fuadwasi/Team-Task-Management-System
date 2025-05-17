using AssetForge.Core.Infrastructure;
using AssetForge.Web.Framework.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AssetForge.Web.Framework.Infrastructure;

/// <summary>
/// Represents object for the configuring MVC on application startup
/// </summary>
public partial class MvcStartup : IAssetForgeStartup
{
    /// <summary>
    /// Add and configure any of the middleware
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Configuration of the application</param>
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        //add and configure MVC feature
        services.AddAssetForgeMvc();

        services.AddWebEncoders();

        //add custom redirect result executor
        services.AddAssetForgeRedirectResultExecutor();
    }

    /// <summary>
    /// Configure the using of added middleware
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public void Configure(IApplicationBuilder application)
    {
    }

    /// <summary>
    /// Gets order of this startup configuration implementation
    /// </summary>
    public int Order => 1000; //MVC should be loaded last
}