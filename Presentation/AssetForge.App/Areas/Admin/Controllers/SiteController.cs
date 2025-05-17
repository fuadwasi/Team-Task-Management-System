using AssetForge.App.Areas.Admin.Factories;
using AssetForge.App.Areas.Admin.Infrastructure.Mapper.Extensions;
using AssetForge.App.Areas.Admin.Models.Sites;
using AssetForge.Core;
using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Sites;
using AssetForge.Services.Common;
using AssetForge.Services.Configuration;
using AssetForge.Services.Localization;
using AssetForge.Services.Logging;
using AssetForge.Services.Messages;
using AssetForge.Services.Security;
using AssetForge.Services.Sites;
using AssetForge.Web.Framework.Controllers;
using AssetForge.Web.Framework.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AssetForge.App.Areas.Admin.Controllers;

public partial class SiteController : BaseAdminController
{
    #region Fields

    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedEntityService _localizedEntityService;
    protected readonly INotificationService _notificationService;
    protected readonly IPermissionService _permissionService;
    protected readonly ISettingService _settingService;
    protected readonly ISiteModelFactory _siteModelFactory;
    protected readonly ISiteService _siteService;
    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly IWebHelper _webHelper;
    protected readonly IWorkContext _workContext;

    #endregion

    #region Ctor

    public SiteController(ICustomerActivityService customerActivityService,
        ILocalizationService localizationService,
        ILocalizedEntityService localizedEntityService,
        INotificationService notificationService,
        IPermissionService permissionService,
        ISettingService settingService,
        ISiteModelFactory siteModelFactory,
        ISiteService siteService,
        IGenericAttributeService genericAttributeService,
        IWebHelper webHelper,
        IWorkContext workContext)
    {
        _customerActivityService = customerActivityService;
        _localizationService = localizationService;
        _localizedEntityService = localizedEntityService;
        _notificationService = notificationService;
        _permissionService = permissionService;
        _settingService = settingService;
        _siteModelFactory = siteModelFactory;
        _siteService = siteService;
        _genericAttributeService = genericAttributeService;
        _webHelper = webHelper;
        _workContext = workContext;

    }

    #endregion

    #region Utilities

    protected virtual async Task UpdateLocalesAsync(Site site, SiteModel model)
    {
        foreach (var localized in model.Locales)
        {
            await _localizedEntityService.SaveLocalizedValueAsync(site,
                x => x.Name,
                localized.Name,
                localized.LanguageId);

            await _localizedEntityService.SaveLocalizedValueAsync(site,
                x => x.DefaultTitle,
                localized.DefaultTitle,
                localized.LanguageId);

            await _localizedEntityService.SaveLocalizedValueAsync(site,
                x => x.DefaultMetaDescription,
                localized.DefaultMetaDescription,
                localized.LanguageId);

            await _localizedEntityService.SaveLocalizedValueAsync(site,
                x => x.DefaultMetaKeywords,
                localized.DefaultMetaKeywords,
                localized.LanguageId);

            await _localizedEntityService.SaveLocalizedValueAsync(site,
                x => x.HomepageDescription,
                localized.HomepageDescription,
                localized.LanguageId);

            await _localizedEntityService.SaveLocalizedValueAsync(site,
                x => x.HomepageTitle,
                localized.HomepageTitle,
                localized.LanguageId);
        }
    }

    #endregion

    #region Methods

    public virtual async Task<IActionResult> List()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSites))
            return AccessDeniedView();

        //prepare model
        var model = await _siteModelFactory.PrepareSiteSearchModelAsync(new SiteSearchModel());

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> List(SiteSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSites))
            return await AccessDeniedDataTablesJson();

        //prepare model
        var model = await _siteModelFactory.PrepareSiteListModelAsync(searchModel);

        return Json(model);
    }

    public virtual async Task<IActionResult> Create()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSites))
            return AccessDeniedView();

        //prepare model
        var model = await _siteModelFactory.PrepareSiteModelAsync(new SiteModel(), null);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    public virtual async Task<IActionResult> Create(SiteModel model, bool continueEditing)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSites))
            return AccessDeniedView();

        if (ModelState.IsValid)
        {
            var site = model.ToEntity<Site>();

            //ensure we have "/" at the end
            if (!site.Url.EndsWith("/"))
                site.Url += "/";

            await _siteService.InsertSiteAsync(site);

            //activity log
            await _customerActivityService.InsertActivityAsync("AddNewSite",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewSite"), site.Id), site);

            //locales
            await UpdateLocalesAsync(site, model);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Sites.Added"));

            return continueEditing ? RedirectToAction("Edit", new { id = site.Id }) : RedirectToAction("List");
        }

        //prepare model
        model = await _siteModelFactory.PrepareSiteModelAsync(model, null, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [HttpsRequirement(ignore: true)]
    public virtual async Task<IActionResult> SetSiteSslByCurrentRequestScheme(int id)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSites))
            return AccessDeniedView();

        //try to get a site with the specified id
        var site = await _siteService.GetSiteByIdAsync(id);
        if (site == null)
            return RedirectToAction("List");

        var value = _webHelper.IsCurrentConnectionSecured();

        if (site.SslEnabled != value)
        {
            site.SslEnabled = value;
            await _siteService.UpdateSiteAsync(site);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Sites.Ssl.Updated"));
        }

        return RedirectToAction("Edit", new { id = id });
    }

    [HttpsRequirement(ignore: true)]
    public virtual async Task<IActionResult> Edit(int id, bool showtour = false)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSites))
            return AccessDeniedView();

        //try to get a site with the specified id
        var site = await _siteService.GetSiteByIdAsync(id);
        if (site == null)
            return RedirectToAction("List");

        //prepare model
        var model = await _siteModelFactory.PrepareSiteModelAsync(null, site);

        //show configuration tour
        if (showtour)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            var hideCard = await _genericAttributeService.GetAttributeAsync<bool>(customer, CustomerDefaults.HideConfigurationStepsAttribute);
            var closeCard = await _genericAttributeService.GetAttributeAsync<bool>(customer, CustomerDefaults.CloseConfigurationStepsAttribute);

            if (!hideCard && !closeCard)
                ViewBag.ShowTour = true;
        }

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [FormValueRequired("save", "save-continue")]
    public virtual async Task<IActionResult> Edit(SiteModel model, bool continueEditing)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSites))
            return AccessDeniedView();

        //try to get a site with the specified id
        var site = await _siteService.GetSiteByIdAsync(model.Id);
        if (site == null)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            site = model.ToEntity(site);

            //ensure we have "/" at the end
            if (!site.Url.EndsWith("/"))
                site.Url += "/";

            await _siteService.UpdateSiteAsync(site);

            //activity log
            await _customerActivityService.InsertActivityAsync("EditSite",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditSite"), site.Id), site);

            //locales
            await UpdateLocalesAsync(site, model);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Sites.Updated"));

            return continueEditing ? RedirectToAction("Edit", new { id = site.Id }) : RedirectToAction("List");
        }

        //prepare model
        model = await _siteModelFactory.PrepareSiteModelAsync(model, site, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> Delete(int id)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSites))
            return AccessDeniedView();

        //try to get a site with the specified id
        var site = await _siteService.GetSiteByIdAsync(id);
        if (site == null)
            return RedirectToAction("List");

        try
        {
            await _siteService.DeleteSiteAsync(site);

            //activity log
            await _customerActivityService.InsertActivityAsync("DeleteSite",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteSite"), site.Id), site);

            //when we delete a site we should also ensure that all "per site" settings will also be deleted
            var settingsToDelete = (await _settingService
                    .GetAllSettingsAsync())
                .Where(s => s.SiteId == id)
                .ToList();
            await _settingService.DeleteSettingsAsync(settingsToDelete);

            //when we had two sites and now have only one site, we also should delete all "per site" settings
            var allSites = await _siteService.GetAllSitesAsync();
            if (allSites.Count == 1)
            {
                settingsToDelete = (await _settingService
                        .GetAllSettingsAsync())
                    .Where(s => s.SiteId == allSites[0].Id)
                    .ToList();
                await _settingService.DeleteSettingsAsync(settingsToDelete);
            }

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Sites.Deleted"));

            return RedirectToAction("List");
        }
        catch (Exception exc)
        {
            await _notificationService.ErrorNotificationAsync(exc);
            return RedirectToAction("Edit", new { id = site.Id });
        }
    }

    #endregion
}