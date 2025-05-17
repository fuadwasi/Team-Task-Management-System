using AssetForge.Web.Framework.Components;
using AssetForge.Web.Framework.Factories;
using Microsoft.AspNetCore.Mvc;

namespace AssetForge.App.Areas.Admin.Components;

/// <summary>
/// Represents a view component that displays an admin widgets
/// </summary>
public partial class AdminWidgetViewComponent : AssetForgeViewComponent
{
    #region Fields

    protected readonly IWidgetModelFactory _widgetModelFactory;

    #endregion

    #region Ctor

    public AdminWidgetViewComponent(IWidgetModelFactory widgetModelFactory)
    {
        _widgetModelFactory = widgetModelFactory;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Invoke view component
    /// </summary>
    /// <param name="widgetZone">Widget zone name</param>
    /// <param name="additionalData">Additional data</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the view component result
    /// </returns>
    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData = null)
    {
        //prepare model
        var models = await _widgetModelFactory.PrepareRenderWidgetModelAsync(widgetZone, additionalData, false);

        //no data?
        if (!models.Any())
            return Content(string.Empty);

        return View(models);
    }

    #endregion
}