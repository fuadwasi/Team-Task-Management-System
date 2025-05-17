using AssetForge.Core;
using AssetForge.Core.Caching;
using AssetForge.Services.Cms;
using AssetForge.Services.Customers;
using AssetForge.Web.Framework.Models.Cms;
using AssetForge.Web.Framework.Themes;
using Microsoft.AspNetCore.Routing;

namespace AssetForge.Web.Framework.Factories;

/// <summary>
/// Represents the widget model factory
/// </summary>
public partial class WidgetModelFactory : IWidgetModelFactory
{
    #region Fields

    protected readonly ICustomerService _customerService;
    protected readonly IShortTermCacheManager _shortTermCacheManager;
    protected readonly IStaticCacheManager _staticCacheManager;
    protected readonly ISiteContext _siteContext;
    protected readonly IThemeContext _themeContext;
    protected readonly IWidgetPluginManager _widgetPluginManager;
    protected readonly IWorkContext _workContext;

    #endregion

    #region Ctor

    public WidgetModelFactory(ICustomerService customerService,
        IShortTermCacheManager shortTermCacheManager,
        IStaticCacheManager staticCacheManager,
        ISiteContext siteContext,
        IThemeContext themeContext,
        IWidgetPluginManager widgetPluginManager,
        IWorkContext workContext)
    {
        _customerService = customerService;
        _shortTermCacheManager = shortTermCacheManager;
        _staticCacheManager = staticCacheManager;
        _siteContext = siteContext;
        _themeContext = themeContext;
        _widgetPluginManager = widgetPluginManager;
        _workContext = workContext;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Get the render widget models
    /// </summary>
    /// <param name="widgetZone">Name of widget zone</param>
    /// <param name="additionalData">Additional data object</param>
    /// <param name="useCache">Value indicating whether to get widget models from cache</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of the render widget models
    /// </returns>
    public virtual async Task<List<RenderWidgetModel>> PrepareRenderWidgetModelAsync(string widgetZone, object additionalData = null, bool useCache = true)
    {
        var theme = await _themeContext.GetWorkingThemeNameAsync();
        var customer = await _workContext.GetCurrentCustomerAsync();
        var customerRoleIds = await _customerService.GetCustomerRoleIdsAsync(customer);
        var site = await _siteContext.GetCurrentSiteAsync();

        if (!useCache)
            return (await _widgetPluginManager.LoadActivePluginsAsync(customer, site.Id, widgetZone))
                .Select(widget => new RenderWidgetModel
                {
                    WidgetViewComponent = widget.GetWidgetViewComponent(widgetZone),
                    WidgetViewComponentArguments = new RouteValueDictionary { ["widgetZone"] = widgetZone, ["additionalData"] = additionalData }
                }).ToList();

        var widgetModels = await _shortTermCacheManager.GetAsync(async () =>
            (await _widgetPluginManager.LoadActivePluginsAsync(customer, site.Id, widgetZone))
            .Select(widget => new RenderWidgetModel
            {
                WidgetViewComponent = widget.GetWidgetViewComponent(widgetZone),
                WidgetViewComponentArguments = new RouteValueDictionary { ["widgetZone"] = widgetZone }
            }), WidgetModelDefaults.WidgetModelKey, customerRoleIds, site, widgetZone, theme);

        //"WidgetViewComponentArguments" property of widget models depends on "additionalData".
        //We need to clone the cached model before modifications (the updated one should not be cached)
        var models = widgetModels.Select(renderModel => new RenderWidgetModel
        {
            WidgetViewComponent = renderModel.WidgetViewComponent,
            WidgetViewComponentArguments = new RouteValueDictionary { ["widgetZone"] = widgetZone, ["additionalData"] = additionalData }
        }).ToList();

        return models;
    }

    #endregion
}