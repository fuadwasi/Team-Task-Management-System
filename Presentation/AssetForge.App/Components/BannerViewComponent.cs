using AssetForge.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace AssetForge.App.Components
{
    public class BannerViewComponent : AssetForgeViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(object id)
        {
            return View(id);
        }
    }
}
