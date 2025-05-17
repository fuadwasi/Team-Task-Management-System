using AssetForge.App.Factories;
using AssetForge.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace AssetForge.App.Components;

public partial class LogoViewComponent : AssetForgeViewComponent
{
    protected readonly ICommonModelFactory _commonModelFactory;

    public LogoViewComponent(ICommonModelFactory commonModelFactory)
    {
        _commonModelFactory = commonModelFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = await _commonModelFactory.PrepareLogoModelAsync();
        return View(model);
    }
}