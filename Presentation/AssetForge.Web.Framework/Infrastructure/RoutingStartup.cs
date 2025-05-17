using AssetForge.Core.Configuration;
using AssetForge.Core.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AssetForge.Web.Framework.Infrastructure;

public partial class RoutingStartup : IAssetForgeStartup
{
    /// <summary>
    /// Add and configure any of the middleware
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Configuration of the application</param>
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
    }

    /// <summary>
    /// Configure the using of added middleware
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public void Configure(IApplicationBuilder application)
    {
        //Add the RoutingMiddleware
        application.UseRouting();

        var settings = Singleton<AppSettings>.Instance.Get<CommonConfig>();
        if (settings.PermitLimit > 0)
            application.UseRateLimiter();
    }

    /// <summary>
    /// Gets order of this startup configuration implementation
    /// </summary>
    public int Order => 400; // Routing should be loaded before authentication
}