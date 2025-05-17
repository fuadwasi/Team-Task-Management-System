using AssetForge.Web.Framework.Components;
using AssetForge.Web.Framework.Factories;
using Microsoft.AspNetCore.Mvc;

namespace AssetForge.Web.Components;

public partial class WidgetViewComponent : AssetForgeViewComponent
{
    protected readonly IWidgetModelFactory _widgetModelFactory;

    public WidgetViewComponent(IWidgetModelFactory widgetModelFactory)
    {
        _widgetModelFactory = widgetModelFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData = null)
    {
        var model = await _widgetModelFactory.PrepareRenderWidgetModelAsync(widgetZone, additionalData);

        //no data?
        if (!model.Any())
            return Content("");

        return View(model);
    }
}