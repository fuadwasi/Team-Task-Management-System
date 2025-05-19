using AssetForge.App.Areas.Admin.Factories;
using AssetForge.App.Areas.Admin.Helpers;
using AssetForge.Core.Infrastructure;
using AssetForge.Web.Infrastructure.Installation;

namespace AssetForge.Web.Infrastructure;

/// <summary>
/// Represents the registering services on application startup
/// </summary>
public partial class Startup : IAssetForgeStartup
{
    /// <summary>
    /// Add and configure any of the middleware
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Configuration of the application</param>
    public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // Admin Factory
        services.AddScoped<IActivityLogModelFactory, ActivityLogModelFactory>();
        services.AddScoped<IBaseAdminModelFactory, BaseAdminModelFactory>();
        services.AddScoped<ICommonModelFactory, CommonModelFactory>();
        services.AddScoped<ICountryModelFactory, CountryModelFactory>();
        services.AddScoped<ICustomerModelFactory, CustomerModelFactory>();
        services.AddScoped<ICustomerAttributeModelFactory, CustomerAttributeModelFactory>();
        services.AddScoped<ICustomerRoleModelFactory, CustomerRoleModelFactory>();
        services.AddScoped<IHomeModelFactory, HomeModelFactory>();
        services.AddScoped<ILanguageModelFactory, LanguageModelFactory>();
        services.AddScoped<ILocalizedModelFactory, LocalizedModelFactory>();
        services.AddScoped<ILogModelFactory, LogModelFactory>();
        services.AddScoped<IMultiFactorAuthenticationMethodModelFactory, MultiFactorAuthenticationMethodModelFactory>();
        services.AddScoped<IScheduleTaskModelFactory, ScheduleTaskModelFactory>();
        services.AddScoped<ISecurityModelFactory, SecurityModelFactory>();
        services.AddScoped<ISettingModelFactory, SettingModelFactory>();
        services.AddScoped<ISiteModelFactory, SiteModelFactory>();

        services.AddScoped<App.Factories.ICommonModelFactory, App.Factories.CommonModelFactory>();
        services.AddScoped<App.Factories.ICustomerModelFactory, App.Factories.CustomerModelFactory>();

        //installation localization service
        services.AddScoped<IInstallationLocalizationService, InstallationLocalizationService>();

        //helpers classes
        services.AddScoped<ITinyMceHelper, TinyMceHelper>();
    }

    /// <summary>
    /// Configure the using of added middleware
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public void Configure(IApplicationBuilder application)
    {
        //application.UseApiExceptionHandler();
        //application.UseApiNotFound();
        //application.UseCors("AllowAll");
        //application.UseMiddleware<JwtAuthMiddleware>();
    }

    /// <summary>
    /// Gets order of this startup configuration implementation
    /// </summary>
    public int Order => 2002;
}