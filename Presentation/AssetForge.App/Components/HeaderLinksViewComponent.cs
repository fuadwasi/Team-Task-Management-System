using AssetForge.App.Factories;
using AssetForge.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace AssetForge.App.Components;

public partial class HeaderLinksViewComponent : AssetForgeViewComponent
{
    protected readonly ICommonModelFactory _commonModelFactory;

    public HeaderLinksViewComponent(ICommonModelFactory commonModelFactory)
    {
        _commonModelFactory = commonModelFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = await _commonModelFactory.PrepareHeaderLinksModelAsync();
        return View(model);
    }
}