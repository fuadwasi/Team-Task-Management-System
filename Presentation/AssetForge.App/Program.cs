using Autofac.Extensions.DependencyInjection;
using AssetForge.Core.Configuration;
using AssetForge.Core.Infrastructure;
using AssetForge.Web.Framework.Infrastructure.Extensions;

public partial class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddJsonFile(ConfigurationDefaults.AppSettingsFilePath, true, true);

        if (!string.IsNullOrEmpty(builder.Environment?.EnvironmentName))
        {
            var path = string.Format(ConfigurationDefaults.AppSettingsEnvironmentFilePath, builder.Environment.EnvironmentName);
            builder.Configuration.AddJsonFile(path, true, true);
        }
        builder.Configuration.AddEnvironmentVariables();

        //load application settings
        builder.Services.ConfigureApplicationSettings(builder);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var appSettings = Singleton<AppSettings>.Instance;

        var useAutofac = appSettings.Get<CommonConfig>().UseAutofac;

        if (useAutofac)
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        else
            builder.Host.UseDefaultServiceProvider(options =>
            {
                //we don't validate the scopes, since at the app start and the initial configuration we need
                //to resolve some services (registered as "scoped") through the root container
                options.ValidateScopes = false;
                options.ValidateOnBuild = true;
            });

        //add services to the application and configure service provider
        builder.Services.ConfigureApplicationServices(builder);

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(); // generates swagger.json
            app.UseSwaggerUI(); // renders the Swagger UI
        }
        //configure the application HTTP request pipeline
        app.ConfigureRequestPipeline();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(); // generates swagger.json
            app.UseSwaggerUI(); // renders the Swagger UI
        }

        await app.StartEngineAsync();

        await app.RunAsync();
    }
}