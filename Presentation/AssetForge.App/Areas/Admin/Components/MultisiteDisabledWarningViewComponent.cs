using AssetForge.Services.Configuration;
using AssetForge.Services.Sites;
using AssetForge.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace AssetForge.App.Areas.Admin.Components;

public partial class MultisiteDisabledWarningViewComponent : AssetForgeViewComponent
{
    protected readonly ISettingService _settingService;
    protected readonly ISiteService _siteService;

    public MultisiteDisabledWarningViewComponent(
        ISettingService settingService,
        ISiteService siteService)
    {
        _settingService = settingService;
        _siteService = siteService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {

        return Content("");
        //action displaying notification (warning) to a site owner that "limit per site" feature is ignored

        //default setting
        var enabled = false;
        if (!enabled)
        {
            //overridden settings
            var sites = await _siteService.GetAllSitesAsync();
            foreach (var site in sites)
            {
                //var catalogSettings = await _settingService.LoadSettingAsync<CatalogSettings>(site.Id);
                //enabled = catalogSettings.IgnoreSiteLimitations;

                if (enabled)
                    break;
            }
        }

        //This setting is disabled. No warnings.
        if (!enabled)
            return Content("");

        return View();
    }
}