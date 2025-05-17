using AssetForge.Data;
using AssetForge.Web.Framework.Mvc.Routing;

namespace AssetForge.Web.Infrastructure;

/// <summary>
/// Represents provider that provided generic routes
/// </summary>
public partial class GenericUrlRouteProvider : BaseRouteProvider, IRouteProvider
{
    #region Methods

    /// <summary>
    /// Register routes
    /// </summary>
    /// <param name="endpointRouteBuilder">Route builder</param>
    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {
        var lang = GetLanguageRoutePattern();

        //default routes
        //these routes are not generic, they are just default to map requests that don't match other patterns,
        //but we define them here since this route provider is with the lowest priority, to allow to add additional routes before them
        if (!string.IsNullOrEmpty(lang))
        {
            endpointRouteBuilder.MapControllerRoute(name: "DefaultWithLanguageCode",
                pattern: $"{lang}/{{controller=Home}}/{{action=Index}}/{{id?}}");
        }

        endpointRouteBuilder.MapControllerRoute(name: "Default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        if (!DataSettingsManager.IsDatabaseInstalled())
            return;

        //generic routes (actually routing is processed later in SlugRouteTransformer)
        var genericCatalogPattern = $"{lang}/{{{RoutingDefaults.RouteValue.CatalogSeName}}}/{{{RoutingDefaults.RouteValue.SeName}}}";
        endpointRouteBuilder.MapDynamicControllerRoute<SlugRouteTransformer>(genericCatalogPattern);

        var genericPattern = $"{lang}/{{{RoutingDefaults.RouteValue.SeName}}}";
        endpointRouteBuilder.MapDynamicControllerRoute<SlugRouteTransformer>(genericPattern);

        //routes for not found slugs
        if (!string.IsNullOrEmpty(lang))
        {
            endpointRouteBuilder.MapControllerRoute(name: RoutingDefaults.RouteName.Generic.GenericUrlWithLanguageCode,
                pattern: genericPattern,
                defaults: new { controller = "Common", action = "GenericUrl" });

            //endpointRouteBuilder.MapControllerRoute(name: AssetForgeRoutingDefaults.RouteName.Generic.GenericCatalogUrlWithLanguageCode,
            //    pattern: genericCatalogPattern,
            //    defaults: new { controller = "Common", action = "GenericUrl" });
        }

        endpointRouteBuilder.MapControllerRoute(name: RoutingDefaults.RouteName.Generic.GenericUrl,
            pattern: $"{{{RoutingDefaults.RouteValue.SeName}}}",
            defaults: new { controller = "Common", action = "GenericUrl" });

        //endpointRouteBuilder.MapControllerRoute(name: AssetForgeRoutingDefaults.RouteName.Generic.GenericCatalogUrl,
        //    pattern: $"{{{AssetForgeRoutingDefaults.RouteValue.CatalogSeName}}}/{{{AssetForgeRoutingDefaults.RouteValue.SeName}}}",
        //    defaults: new { controller = "Common", action = "GenericUrl" });

        endpointRouteBuilder.MapControllerRoute(name: RoutingDefaults.RouteName.Generic.Category,
            pattern: genericPattern,
            defaults: new { controller = "Catalog", action = "Category" });

        endpointRouteBuilder.MapControllerRoute(name: RoutingDefaults.RouteName.Generic.NewsItem,
            pattern: genericPattern,
            defaults: new { controller = "News", action = "NewsItem" });

        endpointRouteBuilder.MapControllerRoute(name: RoutingDefaults.RouteName.Generic.BlogPost,
            pattern: genericPattern,
            defaults: new { controller = "Blog", action = "BlogPost" });

        endpointRouteBuilder.MapControllerRoute(name: RoutingDefaults.RouteName.Generic.Page,
            pattern: genericPattern,
            defaults: new { controller = "Page", action = "PageDetails" });
    }

    #endregion Methods

    #region Properties

    /// <summary>
    /// Gets a priority of route provider
    /// </summary>
    /// <remarks>
    /// it should be the last route. we do not set it to -int.MaxValue so it could be overridden (if required)
    /// </remarks>
    public int Priority => -1000000;

    #endregion Properties
}