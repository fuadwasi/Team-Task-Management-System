using AssetForge.App.Factories;
using AssetForge.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace AssetForge.App.Components;

public partial class AdminHeaderLinksViewComponent : AssetForgeViewComponent
{
    protected readonly ICommonModelFactory _commonModelFactory;

    public AdminHeaderLinksViewComponent(ICommonModelFactory commonModelFactory)
    {
        _commonModelFactory = commonModelFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = await _commonModelFactory.PrepareAdminHeaderLinksModelAsync();
        return View(model);
    }
}