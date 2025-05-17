using AssetForge.App.Areas.Admin.Factories;
using AssetForge.App.Areas.Admin.Infrastructure.Mapper.Extensions;
using AssetForge.App.Areas.Admin.Models.Settings;
using AssetForge.Core;
using AssetForge.Core.Configuration;
using AssetForge.Core.Domain;
using AssetForge.Core.Domain.Common;
using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Localization;
using AssetForge.Core.Domain.Media;
using AssetForge.Core.Domain.Security;
using AssetForge.Core.Domain.Seo;
using AssetForge.Core.Events;
using AssetForge.Core.Infrastructure;
using AssetForge.Data;
using AssetForge.Data.Configuration;
using AssetForge.Services.Authentication.MultiFactor;
using AssetForge.Services.Common;
using AssetForge.Services.Configuration;
using AssetForge.Services.Customers;
using AssetForge.Services.Helpers;
using AssetForge.Services.Localization;
using AssetForge.Services.Logging;
using AssetForge.Services.Media;
using AssetForge.Services.Messages;
using AssetForge.Services.Plugins;
using AssetForge.Services.Security;
using AssetForge.Services.Sites;
using AssetForge.Web.Framework;
using AssetForge.Web.Framework.Controllers;
using AssetForge.Web.Framework.Mvc;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using AssetForge.Web.Framework.WebOptimizer;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.RegularExpressions;

namespace AssetForge.App.Areas.Admin.Controllers;

public partial class SettingController : BaseAdminController
{
    #region Fields

    protected readonly AppSettings _appSettings;
    protected readonly IAddressService _addressService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ICustomerService _customerService;
    protected readonly IDataProvider _dataProvider;
    protected readonly IEncryptionService _encryptionService;
    protected readonly IEventPublisher _eventPublisher;
    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly ILocalizedEntityService _localizedEntityService;
    protected readonly ILocalizationService _localizationService;
    protected readonly IMultiFactorAuthenticationPluginManager _multiFactorAuthenticationPluginManager;
    protected readonly IAssetForgeFileProvider _fileProvider;
    protected readonly INotificationService _notificationService;
    protected readonly IPermissionService _permissionService;
    protected readonly IPictureService _pictureService;
    protected readonly ISettingModelFactory _settingModelFactory;
    protected readonly ISettingService _settingService;
    protected readonly ISiteContext _siteContext;
    protected readonly ISiteService _siteService;
    protected readonly IWorkContext _workContext;
    protected readonly IUploadService _uploadService;
    private static readonly char[] _separator = [','];

    #endregion

    #region Ctor

    public SettingController(AppSettings appSettings,
        IAddressService addressService,
        ICustomerActivityService customerActivityService,
        ICustomerService customerService,
        IDataProvider dataProvider,
        IEncryptionService encryptionService,
        IEventPublisher eventPublisher,
        IGenericAttributeService genericAttributeService,
        ILocalizedEntityService localizedEntityService,
        ILocalizationService localizationService,
        IMultiFactorAuthenticationPluginManager multiFactorAuthenticationPluginManager,
        IAssetForgeFileProvider fileProvider,
        INotificationService notificationService,
        IPermissionService permissionService,
        IPictureService pictureService,
        ISettingModelFactory settingModelFactory,
        ISettingService settingService,
        ISiteContext siteContext,
        ISiteService siteService,
        IWorkContext workContext,
        IUploadService uploadService)
    {
        _appSettings = appSettings;
        _addressService = addressService;
        _customerActivityService = customerActivityService;
        _customerService = customerService;
        _dataProvider = dataProvider;
        _encryptionService = encryptionService;
        _eventPublisher = eventPublisher;
        _genericAttributeService = genericAttributeService;
        _localizedEntityService = localizedEntityService;
        _localizationService = localizationService;
        _multiFactorAuthenticationPluginManager = multiFactorAuthenticationPluginManager;
        _fileProvider = fileProvider;
        _notificationService = notificationService;
        _permissionService = permissionService;
        _pictureService = pictureService;
        _settingModelFactory = settingModelFactory;
        _settingService = settingService;
        _siteContext = siteContext;
        _siteService = siteService;
        _workContext = workContext;
        _uploadService = uploadService;
    }

    #endregion

    #region Utilities

    //protected virtual async Task UpdateGdprConsentLocalesAsync(GdprConsent gdprConsent, GdprConsentModel model)
    //{
    //    foreach (var localized in model.Locales)
    //    {
    //        await _localizedEntityService.SaveLocalizedValueAsync(gdprConsent,
    //            x => x.Message,
    //            localized.Message,
    //            localized.LanguageId);

    //        await _localizedEntityService.SaveLocalizedValueAsync(gdprConsent,
    //            x => x.RequiredMessage,
    //            localized.RequiredMessage,
    //            localized.LanguageId);
    //    }
    //}

    #endregion

    #region Methods

    //public virtual async Task<IActionResult> ChangeSiteScopeConfiguration(int siteId, string returnUrl = "")
    //{
    //    var site = await _siteService.GetSiteByIdAsync(siteId);
    //    if (site != null || siteId == 0)
    //    {
    //        await _genericAttributeService
    //            .SaveAttributeAsync(await _workContext.GetCurrentCustomerAsync(), CustomerDefaults.AdminAreaSiteScopeConfigurationAttribute, siteId);
    //    }

    //    //home page
    //    if (string.IsNullOrEmpty(returnUrl))
    //        returnUrl = Url.Action("Index", "Home", new { area = AreaNames.ADMIN });

    //    //prevent open redirection attack
    //    if (!Url.IsLocalUrl(returnUrl))
    //        return RedirectToAction("Index", "Home", new { area = AreaNames.ADMIN });

    //    return Redirect(returnUrl);
    //}

    public virtual async Task<IActionResult> AppSettings()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageAppSettings))
            return AccessDeniedView();

        //prepare model
        var model = await _settingModelFactory.PrepareAppSettingsModel();

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> AppSettings(AppSettingsModel model)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageAppSettings))
            return AccessDeniedView();

        if (ModelState.IsValid)
        {
            var configurations = new List<IConfig>
            {
                model.CacheConfigModel.ToConfig(_appSettings.Get<CacheConfig>()),
                model.HostingConfigModel.ToConfig(_appSettings.Get<HostingConfig>()),
                model.DistributedCacheConfigModel.ToConfig(_appSettings.Get<DistributedCacheConfig>()),
                model.AzureBlobConfigModel.ToConfig(_appSettings.Get<AzureBlobConfig>()),
                model.InstallationConfigModel.ToConfig(_appSettings.Get<InstallationConfig>()),
                model.PluginConfigModel.ToConfig(_appSettings.Get<PluginConfig>()),
                model.CommonConfigModel.ToConfig(_appSettings.Get<CommonConfig>()),
                model.DataConfigModel.ToConfig(_appSettings.Get<DataConfig>()),
                model.WebOptimizerConfigModel.ToConfig(_appSettings.Get<WebOptimizerConfig>())
            };

            await _eventPublisher.PublishAsync(new AppSettingsSavingEvent(configurations));

            AppSettingsHelper.SaveAppSettings(configurations, _fileProvider);

            await _customerActivityService.InsertActivityAsync("EditSettings",
                await _localizationService.GetResourceAsync("ActivityLog.EditSettings"));

            _notificationService.SuccessNotification(
                await _localizationService.GetResourceAsync("Admin.Configuration.Updated"));

            var returnUrl = Url.Action("AppSettings", "Setting", new { area = AreaNames.ADMIN });
            return View("RestartApplication", returnUrl);
        }

        //prepare model
        model = await _settingModelFactory.PrepareAppSettingsModel(model);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    //public virtual async Task<IActionResult> Blog()
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
    //        return AccessDeniedView();

    //    //prepare model
    //    var model = await _settingModelFactory.PrepareBlogSettingsModelAsync();

    //    return View(model);
    //}

    //[HttpPost]
    //public virtual async Task<IActionResult> Blog(BlogSettingsModel model)
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
    //        return AccessDeniedView();

    //    if (ModelState.IsValid)
    //    {
    //        //load settings for a chosen site scope
    //        var siteScope = await _siteContext.GetActiveSiteScopeConfigurationAsync();
    //        var blogSettings = await _settingService.LoadSettingAsync<BlogSettings>(siteScope);
    //        blogSettings = model.ToSettings(blogSettings);

    //        //we do not clear cache after each setting update.
    //        //this behavior can increase performance because cached settings will not be cleared 
    //        //and loaded from database after each update
    //        await _settingService.SaveSettingOverridablePerSiteAsync(blogSettings, x => x.Enabled, model.Enabled_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(blogSettings, x => x.PostsPageSize, model.PostsPageSize_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(blogSettings, x => x.AllowNotRegisteredUsersToLeaveComments, model.AllowNotRegisteredUsersToLeaveComments_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(blogSettings, x => x.NotifyAboutNewBlogComments, model.NotifyAboutNewBlogComments_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(blogSettings, x => x.NumberOfTags, model.NumberOfTags_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(blogSettings, x => x.ShowHeaderRssUrl, model.ShowHeaderRssUrl_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(blogSettings, x => x.BlogCommentsMustBeApproved, model.BlogCommentsMustBeApproved_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingAsync(blogSettings, x => x.ShowBlogCommentsPerSite, clearCache: false);

    //        //now clear settings cache
    //        await _settingService.ClearCacheAsync();

    //        //activity log
    //        await _customerActivityService.InsertActivityAsync("EditSettings", await _localizationService.GetResourceAsync("ActivityLog.EditSettings"));

    //        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Updated"));

    //        return RedirectToAction("Blog");
    //    }

    //    //prepare model
    //    model = await _settingModelFactory.PrepareBlogSettingsModelAsync(model);

    //    //if we got this far, something failed, redisplay form
    //    return View(model);
    //}

    //public virtual async Task<IActionResult> Vendor()
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
    //        return AccessDeniedView();

    //    //prepare model
    //    var model = await _settingModelFactory.PrepareVendorSettingsModelAsync();

    //    return View(model);
    //}

    //[HttpPost]
    //public virtual async Task<IActionResult> Vendor(VendorSettingsModel model)
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
    //        return AccessDeniedView();

    //    if (ModelState.IsValid)
    //    {
    //        //load settings for a chosen site scope
    //        var siteScope = await _siteContext.GetActiveSiteScopeConfigurationAsync();
    //        var vendorSettings = await _settingService.LoadSettingAsync<VendorSettings>(siteScope);
    //        vendorSettings = model.ToSettings(vendorSettings);

    //        //we do not clear cache after each setting update.
    //        //this behavior can increase performance because cached settings will not be cleared 
    //        //and loaded from database after each update
    //        await _settingService.SaveSettingOverridablePerSiteAsync(vendorSettings, x => x.VendorsBlockItemsToDisplay, model.VendorsBlockItemsToDisplay_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(vendorSettings, x => x.ShowVendorOnProductDetailsPage, model.ShowVendorOnProductDetailsPage_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(vendorSettings, x => x.ShowVendorOnOrderDetailsPage, model.ShowVendorOnOrderDetailsPage_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(vendorSettings, x => x.AllowCustomersToContactVendors, model.AllowCustomersToContactVendors_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(vendorSettings, x => x.AllowCustomersToApplyForVendorAccount, model.AllowCustomersToApplyForVendorAccount_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(vendorSettings, x => x.TermsOfServiceEnabled, model.TermsOfServiceEnabled_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(vendorSettings, x => x.AllowSearchByVendor, model.AllowSearchByVendor_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(vendorSettings, x => x.AllowVendorsToEditInfo, model.AllowVendorsToEditInfo_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(vendorSettings, x => x.NotifySiteOwnerAboutVendorInformationChange, model.NotifySiteOwnerAboutVendorInformationChange_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(vendorSettings, x => x.MaximumProductNumber, model.MaximumProductNumber_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(vendorSettings, x => x.AllowVendorsToImportProducts, model.AllowVendorsToImportProducts_OverrideForSite, siteScope, false);

    //        //now clear settings cache
    //        await _settingService.ClearCacheAsync();

    //        //activity log
    //        await _customerActivityService.InsertActivityAsync("EditSettings", await _localizationService.GetResourceAsync("ActivityLog.EditSettings"));

    //        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Updated"));

    //        return RedirectToAction("Vendor");
    //    }

    //    //prepare model
    //    model = await _settingModelFactory.PrepareVendorSettingsModelAsync(model);

    //    //if we got this far, something failed, redisplay form
    //    return View(model);
    //}

    //public virtual async Task<IActionResult> Forum()
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
    //        return AccessDeniedView();

    //    //prepare model
    //    var model = await _settingModelFactory.PrepareForumSettingsModelAsync();

    //    return View(model);
    //}

    //[HttpPost]
    //public virtual async Task<IActionResult> Forum(ForumSettingsModel model)
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
    //        return AccessDeniedView();

    //    if (ModelState.IsValid)
    //    {
    //        //load settings for a chosen site scope
    //        var siteScope = await _siteContext.GetActiveSiteScopeConfigurationAsync();
    //        var forumSettings = await _settingService.LoadSettingAsync<ForumSettings>(siteScope);
    //        forumSettings = model.ToSettings(forumSettings);

    //        //we do not clear cache after each setting update.
    //        //this behavior can increase performance because cached settings will not be cleared 
    //        //and loaded from database after each update
    //        await _settingService.SaveSettingOverridablePerSiteAsync(forumSettings, x => x.ForumsEnabled, model.ForumsEnabled_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(forumSettings, x => x.RelativeDateTimeFormattingEnabled, model.RelativeDateTimeFormattingEnabled_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(forumSettings, x => x.ShowCustomersPostCount, model.ShowCustomersPostCount_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(forumSettings, x => x.AllowGuestsToCreatePosts, model.AllowGuestsToCreatePosts_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(forumSettings, x => x.AllowGuestsToCreatePages, model.AllowGuestsToCreatePages_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(forumSettings, x => x.AllowCustomersToEditPosts, model.AllowCustomersToEditPosts_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(forumSettings, x => x.AllowCustomersToDeletePosts, model.AllowCustomersToDeletePosts_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(forumSettings, x => x.AllowPostVoting, model.AllowPostVoting_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(forumSettings, x => x.MaxVotesPerDay, model.MaxVotesPerDay_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(forumSettings, x => x.AllowCustomersToManageSubscriptions, model.AllowCustomersToManageSubscriptions_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(forumSettings, x => x.PagesPageSize, model.PagesPageSize_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(forumSettings, x => x.PostsPageSize, model.PostsPageSize_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(forumSettings, x => x.ForumEditor, model.ForumEditor_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(forumSettings, x => x.SignaturesEnabled, model.SignaturesEnabled_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(forumSettings, x => x.AllowPrivateMessages, model.AllowPrivateMessages_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(forumSettings, x => x.ShowAlertForPM, model.ShowAlertForPM_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(forumSettings, x => x.NotifyAboutPrivateMessages, model.NotifyAboutPrivateMessages_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(forumSettings, x => x.ActiveDiscussionsFeedEnabled, model.ActiveDiscussionsFeedEnabled_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(forumSettings, x => x.ActiveDiscussionsFeedCount, model.ActiveDiscussionsFeedCount_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(forumSettings, x => x.ForumFeedsEnabled, model.ForumFeedsEnabled_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(forumSettings, x => x.ForumFeedCount, model.ForumFeedCount_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(forumSettings, x => x.SearchResultsPageSize, model.SearchResultsPageSize_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(forumSettings, x => x.ActiveDiscussionsPageSize, model.ActiveDiscussionsPageSize_OverrideForSite, siteScope, false);

    //        //now clear settings cache
    //        await _settingService.ClearCacheAsync();

    //        //activity log
    //        await _customerActivityService.InsertActivityAsync("EditSettings", await _localizationService.GetResourceAsync("ActivityLog.EditSettings"));

    //        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Updated"));

    //        return RedirectToAction("Forum");
    //    }

    //    //prepare model
    //    model = await _settingModelFactory.PrepareForumSettingsModelAsync(model);

    //    //if we got this far, something failed, redisplay form
    //    return View(model);
    //}

    //public virtual async Task<IActionResult> News()
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
    //        return AccessDeniedView();

    //    //prepare model
    //    var model = await _settingModelFactory.PrepareNewsSettingsModelAsync();

    //    return View(model);
    //}

    //[HttpPost]
    //public virtual async Task<IActionResult> News(NewsSettingsModel model)
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
    //        return AccessDeniedView();

    //    if (ModelState.IsValid)
    //    {
    //        //load settings for a chosen site scope
    //        var siteScope = await _siteContext.GetActiveSiteScopeConfigurationAsync();
    //        var newsSettings = await _settingService.LoadSettingAsync<NewsSettings>(siteScope);
    //        newsSettings = model.ToSettings(newsSettings);

    //        //we do not clear cache after each setting update.
    //        //this behavior can increase performance because cached settings will not be cleared 
    //        //and loaded from database after each update
    //        await _settingService.SaveSettingOverridablePerSiteAsync(newsSettings, x => x.Enabled, model.Enabled_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(newsSettings, x => x.AllowNotRegisteredUsersToLeaveComments, model.AllowNotRegisteredUsersToLeaveComments_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(newsSettings, x => x.NotifyAboutNewNewsComments, model.NotifyAboutNewNewsComments_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(newsSettings, x => x.ShowNewsOnMainPage, model.ShowNewsOnMainPage_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(newsSettings, x => x.MainPageNewsCount, model.MainPageNewsCount_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(newsSettings, x => x.NewsArchivePageSize, model.NewsArchivePageSize_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(newsSettings, x => x.ShowHeaderRssUrl, model.ShowHeaderRssUrl_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(newsSettings, x => x.NewsCommentsMustBeApproved, model.NewsCommentsMustBeApproved_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingAsync(newsSettings, x => x.ShowNewsCommentsPerSite, clearCache: false);

    //        //now clear settings cache
    //        await _settingService.ClearCacheAsync();

    //        //activity log
    //        await _customerActivityService.InsertActivityAsync("EditSettings", await _localizationService.GetResourceAsync("ActivityLog.EditSettings"));

    //        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Updated"));

    //        return RedirectToAction("News");
    //    }

    //    //prepare model
    //    model = await _settingModelFactory.PrepareNewsSettingsModelAsync(model);

    //    //if we got this far, something failed, redisplay form
    //    return View(model);
    //}

    //public virtual async Task<IActionResult> Catalog()
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
    //        return AccessDeniedView();

    //    //prepare model
    //    var model = await _settingModelFactory.PrepareCatalogSettingsModelAsync();

    //    return View(model);
    //}

    //[HttpPost]
    //public virtual async Task<IActionResult> Catalog(CatalogSettingsModel model)
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
    //        return AccessDeniedView();

    //    if (ModelState.IsValid)
    //    {
    //        //load settings for a chosen site scope
    //        var siteScope = await _siteContext.GetActiveSiteScopeConfigurationAsync();
    //        var catalogSettings = await _settingService.LoadSettingAsync<CatalogSettings>(siteScope);
    //        catalogSettings = model.ToSettings(catalogSettings);

    //        //we do not clear cache after each setting update.
    //        //this behavior can increase performance because cached settings will not be cleared 
    //        //and loaded from database after each update
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.AllowViewUnpublishedProductPage, model.AllowViewUnpublishedProductPage_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.DisplayDiscontinuedMessageForUnpublishedProducts, model.DisplayDiscontinuedMessageForUnpublishedProducts_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ShowSkuOnProductDetailsPage, model.ShowSkuOnProductDetailsPage_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ShowSkuOnCatalogPages, model.ShowSkuOnCatalogPages_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ShowManufacturerPartNumber, model.ShowManufacturerPartNumber_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ShowGtin, model.ShowGtin_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ShowFreeShippingNotification, model.ShowFreeShippingNotification_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ShowShortDescriptionOnCatalogPages, model.ShowShortDescriptionOnCatalogPages_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.AllowProductSorting, model.AllowProductSorting_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.AllowProductViewModeChanging, model.AllowProductViewModeChanging_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.DefaultViewMode, model.DefaultViewMode_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ShowProductsFromSubcategories, model.ShowProductsFromSubcategories_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ShowCategoryProductNumber, model.ShowCategoryProductNumber_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ShowCategoryProductNumberIncludingSubcategories, model.ShowCategoryProductNumberIncludingSubcategories_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.CategoryBreadcrumbEnabled, model.CategoryBreadcrumbEnabled_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ShowShareButton, model.ShowShareButton_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.PageShareCode, model.PageShareCode_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ProductReviewsMustBeApproved, model.ProductReviewsMustBeApproved_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.OneReviewPerProductFromCustomer, model.OneReviewPerProductFromCustomer, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.AllowAnonymousUsersToReviewProduct, model.AllowAnonymousUsersToReviewProduct_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ProductReviewPossibleOnlyAfterPurchasing, model.ProductReviewPossibleOnlyAfterPurchasing_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.NotifySiteOwnerAboutNewProductReviews, model.NotifySiteOwnerAboutNewProductReviews_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.NotifyCustomerAboutProductReviewReply, model.NotifyCustomerAboutProductReviewReply_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.EmailAFriendEnabled, model.EmailAFriendEnabled_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.AllowAnonymousUsersToEmailAFriend, model.AllowAnonymousUsersToEmailAFriend_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.RecentlyViewedProductsNumber, model.RecentlyViewedProductsNumber_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.RecentlyViewedProductsEnabled, model.RecentlyViewedProductsEnabled_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.NewProductsEnabled, model.NewProductsEnabled_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.NewProductsPageSize, model.NewProductsPageSize_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.NewProductsAllowCustomersToSelectPageSize, model.NewProductsAllowCustomersToSelectPageSize_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.NewProductsPageSizeOptions, model.NewProductsPageSizeOptions_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.CompareProductsEnabled, model.CompareProductsEnabled_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ShowBestsellersOnHomepage, model.ShowBestsellersOnHomepage_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.NumberOfBestsellersOnHomepage, model.NumberOfBestsellersOnHomepage_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.SearchPageProductsPerPage, model.SearchPageProductsPerPage_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.SearchPageAllowCustomersToSelectPageSize, model.SearchPageAllowCustomersToSelectPageSize_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.SearchPagePageSizeOptions, model.SearchPagePageSizeOptions_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.SearchPagePriceRangeFiltering, model.SearchPagePriceRangeFiltering_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.SearchPagePriceFrom, model.SearchPagePriceFrom_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.SearchPagePriceTo, model.SearchPagePriceTo_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.SearchPageManuallyPriceRange, model.SearchPageManuallyPriceRange_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ProductSearchAutoCompleteEnabled, model.ProductSearchAutoCompleteEnabled_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ProductSearchEnabled, model.ProductSearchEnabled_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ProductSearchAutoCompleteNumberOfProducts, model.ProductSearchAutoCompleteNumberOfProducts_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ShowProductImagesInSearchAutoComplete, model.ShowProductImagesInSearchAutoComplete_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ShowLinkToAllResultInSearchAutoComplete, model.ShowLinkToAllResultInSearchAutoComplete_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ProductSearchTermMinimumLength, model.ProductSearchTermMinimumLength_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ProductsAlsoPurchasedEnabled, model.ProductsAlsoPurchasedEnabled_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ProductsAlsoPurchasedNumber, model.ProductsAlsoPurchasedNumber_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.NumberOfProductTags, model.NumberOfProductTags_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ProductsByTagPageSize, model.ProductsByTagPageSize_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ProductsByTagAllowCustomersToSelectPageSize, model.ProductsByTagAllowCustomersToSelectPageSize_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ProductsByTagPageSizeOptions, model.ProductsByTagPageSizeOptions_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ProductsByTagPriceRangeFiltering, model.ProductsByTagPriceRangeFiltering_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ProductsByTagPriceFrom, model.ProductsByTagPriceFrom_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ProductsByTagPriceTo, model.ProductsByTagPriceTo_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ProductsByTagManuallyPriceRange, model.ProductsByTagManuallyPriceRange_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.IncludeShortDescriptionInCompareProducts, model.IncludeShortDescriptionInCompareProducts_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.IncludeFullDescriptionInCompareProducts, model.IncludeFullDescriptionInCompareProducts_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ManufacturersBlockItemsToDisplay, model.ManufacturersBlockItemsToDisplay_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.DisplayTaxShippingInfoFooter, model.DisplayTaxShippingInfoFooter_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.DisplayTaxShippingInfoProductDetailsPage, model.DisplayTaxShippingInfoProductDetailsPage_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.DisplayTaxShippingInfoProductBoxes, model.DisplayTaxShippingInfoProductBoxes_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.DisplayTaxShippingInfoShoppingCart, model.DisplayTaxShippingInfoShoppingCart_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.DisplayTaxShippingInfoWishlist, model.DisplayTaxShippingInfoWishlist_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.DisplayTaxShippingInfoOrderDetailsPage, model.DisplayTaxShippingInfoOrderDetailsPage_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ShowProductReviewsPerSite, model.ShowProductReviewsPerSite_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ShowProductReviewsTabOnAccountPage, model.ShowProductReviewsOnAccountPage_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ProductReviewsPageSizeOnAccountPage, model.ProductReviewsPageSizeOnAccountPage_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ProductReviewsSortByCreatedDateAscending, model.ProductReviewsSortByCreatedDateAscending_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ExportImportProductAttributes, model.ExportImportProductAttributes_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ExportImportProductSpecificationAttributes, model.ExportImportProductSpecificationAttributes_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ExportImportProductCategoryBreadcrumb, model.ExportImportProductCategoryBreadcrumb_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ExportImportCategoriesUsingCategoryName, model.ExportImportCategoriesUsingCategoryName_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ExportImportAllowDownloadImages, model.ExportImportAllowDownloadImages_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ExportImportSplitProductsFile, model.ExportImportSplitProductsFile_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.RemoveRequiredProducts, model.RemoveRequiredProducts_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ExportImportRelatedEntitiesByName, model.ExportImportRelatedEntitiesByName_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ExportImportProductUseLimitedToSites, model.ExportImportProductUseLimitedToSites_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.DisplayDatePreOrderAvailability, model.DisplayDatePreOrderAvailability_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.UseAjaxCatalogProductsLoading, model.UseAjaxCatalogProductsLoading_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.EnableManufacturerFiltering, model.EnableManufacturerFiltering_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.EnablePriceRangeFiltering, model.EnablePriceRangeFiltering_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.EnableSpecificationAttributeFiltering, model.EnableSpecificationAttributeFiltering_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.DisplayFromPrices, model.DisplayFromPrices_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.AttributeValueOutOfStockDisplayType, model.AttributeValueOutOfStockDisplayType_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.AllowCustomersToSearchWithManufacturerName, model.AllowCustomersToSearchWithManufacturerName_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.AllowCustomersToSearchWithCategoryName, model.AllowCustomersToSearchWithCategoryName_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.DisplayAllPicturesOnCatalogPages, model.DisplayAllPicturesOnCatalogPages_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(catalogSettings, x => x.ProductUrlStructureTypeId, model.ProductUrlStructureTypeId_OverrideForSite, siteScope, false);

    //        //now settings not overridable per site
    //        await _settingService.SaveSettingAsync(catalogSettings, x => x.IgnoreDiscounts, 0, false);
    //        await _settingService.SaveSettingAsync(catalogSettings, x => x.IgnoreFeaturedProducts, 0, false);
    //        await _settingService.SaveSettingAsync(catalogSettings, x => x.IgnoreAcl, 0, false);
    //        await _settingService.SaveSettingAsync(catalogSettings, x => x.IgnoreSiteLimitations, 0, false);
    //        await _settingService.SaveSettingAsync(catalogSettings, x => x.CacheProductPrices, 0, false);

    //        //now clear settings cache
    //        await _settingService.ClearCacheAsync();

    //        //activity log
    //        await _customerActivityService.InsertActivityAsync("EditSettings", await _localizationService.GetResourceAsync("ActivityLog.EditSettings"));

    //        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Updated"));

    //        return RedirectToAction("Catalog");
    //    }

    //    //prepare model
    //    model = await _settingModelFactory.PrepareCatalogSettingsModelAsync(model);

    //    //if we got this far, something failed, redisplay form
    //    return View(model);
    //}

    //[HttpPost]
    //public virtual async Task<IActionResult> SortOptionsList(SortOptionSearchModel searchModel)
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
    //        return await AccessDeniedDataTablesJson();

    //    //prepare model
    //    var model = await _settingModelFactory.PrepareSortOptionListModelAsync(searchModel);

    //    return Json(model);
    //}

    //[HttpPost]
    //public virtual async Task<IActionResult> SortOptionUpdate(SortOptionModel model)
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
    //        return await AccessDeniedDataTablesJson();

    //    var siteScope = await _siteContext.GetActiveSiteScopeConfigurationAsync();
    //    var catalogSettings = await _settingService.LoadSettingAsync<CatalogSettings>(siteScope);

    //    catalogSettings.ProductSortingEnumDisplayOrder[model.Id] = model.DisplayOrder;
    //    if (model.IsActive && catalogSettings.ProductSortingEnumDisabled.Contains(model.Id))
    //        catalogSettings.ProductSortingEnumDisabled.Remove(model.Id);
    //    if (!model.IsActive && !catalogSettings.ProductSortingEnumDisabled.Contains(model.Id))
    //        catalogSettings.ProductSortingEnumDisabled.Add(model.Id);

    //    await _settingService.SaveSettingAsync(catalogSettings, x => x.ProductSortingEnumDisplayOrder, siteScope, false);
    //    await _settingService.SaveSettingAsync(catalogSettings, x => x.ProductSortingEnumDisabled, siteScope, false);

    //    //now clear settings cache
    //    await _settingService.ClearCacheAsync();

    //    return new NullJsonResult();
    //}

    //public virtual async Task<IActionResult> RewardPoints()
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
    //        return AccessDeniedView();

    //    //prepare model
    //    var model = await _settingModelFactory.PrepareRewardPointsSettingsModelAsync();

    //    return View(model);
    //}

    //[HttpPost]
    //public virtual async Task<IActionResult> RewardPoints(RewardPointsSettingsModel model)
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
    //        return AccessDeniedView();

    //    if (ModelState.IsValid)
    //    {
    //        //load settings for a chosen site scope
    //        var siteScope = await _siteContext.GetActiveSiteScopeConfigurationAsync();
    //        var rewardPointsSettings = await _settingService.LoadSettingAsync<RewardPointsSettings>(siteScope);
    //        rewardPointsSettings = model.ToSettings(rewardPointsSettings);

    //        if (model.ActivatePointsImmediately)
    //            rewardPointsSettings.ActivationDelay = 0;

    //        //we do not clear cache after each setting update.
    //        //this behavior can increase performance because cached settings will not be cleared 
    //        //and loaded from database after each update
    //        await _settingService.SaveSettingOverridablePerSiteAsync(rewardPointsSettings, x => x.Enabled, model.Enabled_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(rewardPointsSettings, x => x.ExchangeRate, model.ExchangeRate_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(rewardPointsSettings, x => x.MinimumRewardPointsToUse, model.MinimumRewardPointsToUse_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(rewardPointsSettings, x => x.MaximumRewardPointsToUsePerOrder, model.MaximumRewardPointsToUsePerOrder_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(rewardPointsSettings, x => x.MaximumRedeemedRate, model.MaximumRedeemedRate_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(rewardPointsSettings, x => x.PointsForRegistration, model.PointsForRegistration_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(rewardPointsSettings, x => x.RegistrationPointsValidity, model.RegistrationPointsValidity_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(rewardPointsSettings, x => x.PointsForPurchases_Amount, model.PointsForPurchases_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(rewardPointsSettings, x => x.PointsForPurchases_Points, model.PointsForPurchases_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(rewardPointsSettings, x => x.MinOrderTotalToAwardPoints, model.MinOrderTotalToAwardPoints_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(rewardPointsSettings, x => x.PurchasesPointsValidity, model.PurchasesPointsValidity_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(rewardPointsSettings, x => x.ActivationDelay, model.ActivationDelay_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(rewardPointsSettings, x => x.ActivationDelayPeriodId, model.ActivationDelay_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(rewardPointsSettings, x => x.DisplayHowMuchWillBeEarned, model.DisplayHowMuchWillBeEarned_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(rewardPointsSettings, x => x.PageSize, model.PageSize_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingAsync(rewardPointsSettings, x => x.PointsAccumulatedForAllSites, 0, false);

    //        //now clear settings cache
    //        await _settingService.ClearCacheAsync();

    //        //activity log
    //        await _customerActivityService.InsertActivityAsync("EditSettings", await _localizationService.GetResourceAsync("ActivityLog.EditSettings"));

    //        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Updated"));

    //        return RedirectToAction("RewardPoints");
    //    }

    //    //prepare model
    //    model = await _settingModelFactory.PrepareRewardPointsSettingsModelAsync(model);

    //    //if we got this far, something failed, redisplay form
    //    return View(model);
    //}

    //public virtual async Task<IActionResult> Order()
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
    //        return AccessDeniedView();

    //    //prepare model
    //    var model = await _settingModelFactory.PrepareOrderSettingsModelAsync();

    //    return View(model);
    //}

    //[HttpPost]
    //public virtual async Task<IActionResult> Order(OrderSettingsModel model)
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
    //        return AccessDeniedView();

    //    if (ModelState.IsValid)
    //    {
    //        //load settings for a chosen site scope
    //        var siteScope = await _siteContext.GetActiveSiteScopeConfigurationAsync();
    //        var orderSettings = await _settingService.LoadSettingAsync<OrderSettings>(siteScope);
    //        orderSettings = model.ToSettings(orderSettings);

    //        //we do not clear cache after each setting update.
    //        //this behavior can increase performance because cached settings will not be cleared 
    //        //and loaded from database after each update
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.IsReOrderAllowed, model.IsReOrderAllowed_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.MinOrderSubtotalAmount, model.MinOrderSubtotalAmount_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.MinOrderSubtotalAmountIncludingTax, model.MinOrderSubtotalAmountIncludingTax_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.MinOrderTotalAmount, model.MinOrderTotalAmount_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.AutoUpdateOrderTotalsOnEditingOrder, model.AutoUpdateOrderTotalsOnEditingOrder_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.AnonymousCheckoutAllowed, model.AnonymousCheckoutAllowed_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.CheckoutDisabled, model.CheckoutDisabled_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.TermsOfServiceOnShoppingCartPage, model.TermsOfServiceOnShoppingCartPage_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.TermsOfServiceOnOrderConfirmPage, model.TermsOfServiceOnOrderConfirmPage_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.OnePageCheckoutEnabled, model.OnePageCheckoutEnabled_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.OnePageCheckoutDisplayOrderTotalsOnPaymentInfoTab, model.OnePageCheckoutDisplayOrderTotalsOnPaymentInfoTab_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.DisableBillingAddressCheckoutStep, model.DisableBillingAddressCheckoutStep_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.DisableOrderCompletedPage, model.DisableOrderCompletedPage_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.DisplayPickupInSiteOnShippingMethodPage, model.DisplayPickupInSiteOnShippingMethodPage_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.AttachPdfInvoiceToOrderPlacedEmail, model.AttachPdfInvoiceToOrderPlacedEmail_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.AttachPdfInvoiceToOrderPaidEmail, model.AttachPdfInvoiceToOrderPaidEmail_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.AttachPdfInvoiceToOrderProcessingEmail, model.AttachPdfInvoiceToOrderProcessingEmail_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.AttachPdfInvoiceToOrderCompletedEmail, model.AttachPdfInvoiceToOrderCompletedEmail_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.ReturnRequestsEnabled, model.ReturnRequestsEnabled_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.ReturnRequestsAllowFiles, model.ReturnRequestsAllowFiles_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.ReturnRequestNumberMask, model.ReturnRequestNumberMask_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.NumberOfDaysReturnRequestAvailable, model.NumberOfDaysReturnRequestAvailable_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.CustomOrderNumberMask, model.CustomOrderNumberMask_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.ExportWithProducts, model.ExportWithProducts_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.AllowAdminsToBuyCallForPriceProducts, model.AllowAdminsToBuyCallForPriceProducts_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.ShowProductThumbnailInOrderDetailsPage, model.ShowProductThumbnailInOrderDetailsPage_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(orderSettings, x => x.DeleteGiftCardUsageHistory, model.DeleteGiftCardUsageHistory_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingAsync(orderSettings, x => x.ActivateGiftCardsAfterCompletingOrder, 0, false);
    //        await _settingService.SaveSettingAsync(orderSettings, x => x.DeactivateGiftCardsAfterCancellingOrder, 0, false);
    //        await _settingService.SaveSettingAsync(orderSettings, x => x.DeactivateGiftCardsAfterDeletingOrder, 0, false);
    //        await _settingService.SaveSettingAsync(orderSettings, x => x.CompleteOrderWhenDelivered, 0, false);

    //        //now clear settings cache
    //        await _settingService.ClearCacheAsync();

    //        //order ident
    //        if (model.OrderIdent.HasValue)
    //        {
    //            try
    //            {
    //                await _dataProvider.SetTableIdentAsync<Order>(model.OrderIdent.Value);
    //            }
    //            catch (Exception exc)
    //            {
    //                _notificationService.ErrorNotification(exc.Message);
    //            }
    //        }

    //        //activity log
    //        await _customerActivityService.InsertActivityAsync("EditSettings", await _localizationService.GetResourceAsync("ActivityLog.EditSettings"));

    //        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Updated"));

    //        return RedirectToAction("Order");
    //    }

    //    //prepare model
    //    model = await _settingModelFactory.PrepareOrderSettingsModelAsync(model);

    //    //if we got this far, something failed, redisplay form
    //    return View(model);
    //}

    //public virtual async Task<IActionResult> ShoppingCart()
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
    //        return AccessDeniedView();

    //    //prepare model
    //    var model = await _settingModelFactory.PrepareShoppingCartSettingsModelAsync();

    //    return View(model);
    //}

    //[HttpPost]
    //public virtual async Task<IActionResult> ShoppingCart(ShoppingCartSettingsModel model)
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
    //        return AccessDeniedView();

    //    if (ModelState.IsValid)
    //    {
    //        //load settings for a chosen site scope
    //        var siteScope = await _siteContext.GetActiveSiteScopeConfigurationAsync();
    //        var shoppingCartSettings = await _settingService.LoadSettingAsync<ShoppingCartSettings>(siteScope);
    //        shoppingCartSettings = model.ToSettings(shoppingCartSettings);

    //        //we do not clear cache after each setting update.
    //        //this behavior can increase performance because cached settings will not be cleared 
    //        //and loaded from database after each update
    //        await _settingService.SaveSettingOverridablePerSiteAsync(shoppingCartSettings, x => x.DisplayCartAfterAddingProduct, model.DisplayCartAfterAddingProduct_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(shoppingCartSettings, x => x.DisplayWishlistAfterAddingProduct, model.DisplayWishlistAfterAddingProduct_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(shoppingCartSettings, x => x.MaximumShoppingCartItems, model.MaximumShoppingCartItems_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(shoppingCartSettings, x => x.MaximumWishlistItems, model.MaximumWishlistItems_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(shoppingCartSettings, x => x.AllowOutOfStockItemsToBeAddedToWishlist, model.AllowOutOfStockItemsToBeAddedToWishlist_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(shoppingCartSettings, x => x.MoveItemsFromWishlistToCart, model.MoveItemsFromWishlistToCart_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(shoppingCartSettings, x => x.CartsSharedBetweenSites, model.CartsSharedBetweenSites_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(shoppingCartSettings, x => x.ShowProductImagesOnShoppingCart, model.ShowProductImagesOnShoppingCart_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(shoppingCartSettings, x => x.ShowProductImagesOnWishList, model.ShowProductImagesOnWishList_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(shoppingCartSettings, x => x.ShowDiscountBox, model.ShowDiscountBox_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(shoppingCartSettings, x => x.ShowGiftCardBox, model.ShowGiftCardBox_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(shoppingCartSettings, x => x.CrossSellsNumber, model.CrossSellsNumber_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(shoppingCartSettings, x => x.EmailWishlistEnabled, model.EmailWishlistEnabled_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(shoppingCartSettings, x => x.AllowAnonymousUsersToEmailWishlist, model.AllowAnonymousUsersToEmailWishlist_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(shoppingCartSettings, x => x.MiniShoppingCartEnabled, model.MiniShoppingCartEnabled_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(shoppingCartSettings, x => x.ShowProductImagesInMiniShoppingCart, model.ShowProductImagesInMiniShoppingCart_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(shoppingCartSettings, x => x.MiniShoppingCartProductNumber, model.MiniShoppingCartProductNumber_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(shoppingCartSettings, x => x.AllowCartItemEditing, model.AllowCartItemEditing_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(shoppingCartSettings, x => x.GroupTierPricesForDistinctShoppingCartItems, model.GroupTierPricesForDistinctShoppingCartItems_OverrideForSite, siteScope, false);

    //        //now clear settings cache
    //        await _settingService.ClearCacheAsync();

    //        //activity log
    //        await _customerActivityService.InsertActivityAsync("EditSettings", await _localizationService.GetResourceAsync("ActivityLog.EditSettings"));

    //        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Updated"));

    //        return RedirectToAction("ShoppingCart");
    //    }

    //    //prepare model
    //    model = await _settingModelFactory.PrepareShoppingCartSettingsModelAsync(model);

    //    //if we got this far, something failed, redisplay form
    //    return View(model);
    //}

    public virtual async Task<IActionResult> Media()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
            return AccessDeniedView();

        //prepare model
        var model = await _settingModelFactory.PrepareMediaSettingsModelAsync();

        return View(model);
    }

    [HttpPost]
    [FormValueRequired("save")]
    public virtual async Task<IActionResult> Media(MediaSettingsModel model)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
            return AccessDeniedView();

        if (ModelState.IsValid)
        {
            //load settings for a chosen site scope
            var siteScope = await _siteContext.GetActiveSiteScopeConfigurationAsync();
            var mediaSettings = await _settingService.LoadSettingAsync<MediaSettings>(siteScope);
            mediaSettings = model.ToSettings(mediaSettings);

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            await _settingService.SaveSettingOverridablePerSiteAsync(mediaSettings, x => x.AvatarPictureSize, model.AvatarPictureSize_OverrideForSite, siteScope, false); ;
            await _settingService.SaveSettingOverridablePerSiteAsync(mediaSettings, x => x.CatelogThumbPictureSize, model.CatelogThumbPictureSize_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(mediaSettings, x => x.MaximumImageSize, model.MaximumImageSize_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(mediaSettings, x => x.MultipleThumbDirectories, model.MultipleThumbDirectories_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(mediaSettings, x => x.DefaultImageQuality, model.DefaultImageQuality_OverrideForSite, siteScope, false);
            //await _settingService.SaveSettingOverridablePerSiteAsync(mediaSettings, x => x.ImportProductImagesUsingHash, model.ImportProductImagesUsingHash_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(mediaSettings, x => x.DefaultPictureZoomEnabled, model.DefaultPictureZoomEnabled_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(mediaSettings, x => x.AllowSVGUploads, model.AllowSVGUploads_OverrideForSite, siteScope, false);

            //now clear settings cache
            await _settingService.ClearCacheAsync();

            //activity log
            await _customerActivityService.InsertActivityAsync("EditSettings", await _localizationService.GetResourceAsync("ActivityLog.EditSettings"));

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Updated"));

            return RedirectToAction("Media");
        }

        //prepare model
        model = await _settingModelFactory.PrepareMediaSettingsModelAsync(model);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [HttpPost, ActionName("Media")]
    [FormValueRequired("change-picture-storage")]
    public virtual async Task<IActionResult> ChangePictureStorage()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
            return AccessDeniedView();

        await _pictureService.SetIsSiteInDbAsync(!await _pictureService.IsSiteInDbAsync());

        //activity log
        await _customerActivityService.InsertActivityAsync("EditSettings", await _localizationService.GetResourceAsync("ActivityLog.EditSettings"));

        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Updated"));

        return RedirectToAction("Media");
    }

    public virtual async Task<IActionResult> CustomerUser()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
            return AccessDeniedView();

        //prepare model
        var model = await _settingModelFactory.PrepareCustomerUserSettingsModelAsync();

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> CustomerUser(CustomerUserSettingsModel model)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
            return AccessDeniedView();

        if (ModelState.IsValid)
        {
            var siteScope = await _siteContext.GetActiveSiteScopeConfigurationAsync();
            var customerSettings = await _settingService.LoadSettingAsync<CustomerSettings>(siteScope);

            var lastUsernameValidationRule = customerSettings.UsernameValidationRule;
            var lastUsernameValidationEnabledValue = customerSettings.UsernameValidationEnabled;
            var lastUsernameValidationUseRegexValue = customerSettings.UsernameValidationUseRegex;

            //Phone number validation settings
            var lastPhoneNumberValidationRule = customerSettings.PhoneNumberValidationRule;
            var lastPhoneNumberValidationEnabledValue = customerSettings.PhoneNumberValidationEnabled;
            var lastPhoneNumberValidationUseRegexValue = customerSettings.PhoneNumberValidationUseRegex;

            var addressSettings = await _settingService.LoadSettingAsync<AddressSettings>(siteScope);
            var dateTimeSettings = await _settingService.LoadSettingAsync<DateTimeSettings>(siteScope);
            var externalAuthenticationSettings = await _settingService.LoadSettingAsync<ExternalAuthenticationSettings>(siteScope);
            var multiFactorAuthenticationSettings = await _settingService.LoadSettingAsync<MultiFactorAuthenticationSettings>(siteScope);

            customerSettings = model.CustomerSettings.ToSettings(customerSettings);

            if (customerSettings.UsernameValidationEnabled && customerSettings.UsernameValidationUseRegex)
            {
                try
                {
                    //validate regex rule
                    var unused = Regex.IsMatch("test_user_name", customerSettings.UsernameValidationRule);
                }
                catch (ArgumentException)
                {
                    //restoring previous settings
                    customerSettings.UsernameValidationRule = lastUsernameValidationRule;
                    customerSettings.UsernameValidationEnabled = lastUsernameValidationEnabledValue;
                    customerSettings.UsernameValidationUseRegex = lastUsernameValidationUseRegexValue;

                    _notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Settings.CustomerSettings.RegexValidationRule.Error"));
                }
            }

            if (customerSettings.PhoneNumberValidationEnabled && customerSettings.PhoneNumberValidationUseRegex)
            {
                try
                {
                    //validate regex rule
                    var unused = Regex.IsMatch("123456789", customerSettings.PhoneNumberValidationRule);
                }
                catch (ArgumentException)
                {
                    //restoring previous settings
                    customerSettings.PhoneNumberValidationRule = lastPhoneNumberValidationRule;
                    customerSettings.PhoneNumberValidationEnabled = lastPhoneNumberValidationEnabledValue;
                    customerSettings.PhoneNumberValidationUseRegex = lastPhoneNumberValidationUseRegexValue;

                    _notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Settings.CustomerSettings.PhoneNumberRegexValidationRule.Error"));
                }
            }

            await _settingService.SaveSettingAsync(customerSettings);

            //addressSettings = model.AddressSettings.ToSettings(addressSettings);
            //await _settingService.SaveSettingAsync(addressSettings);

            dateTimeSettings.DefaultSiteTimeZoneId = model.DateTimeSettings.DefaultSiteTimeZoneId;
            dateTimeSettings.AllowCustomersToSetTimeZone = model.DateTimeSettings.AllowCustomersToSetTimeZone;
            await _settingService.SaveSettingAsync(dateTimeSettings);

            externalAuthenticationSettings.AllowCustomersToRemoveAssociations = model.ExternalAuthenticationSettings.AllowCustomersToRemoveAssociations;
            await _settingService.SaveSettingAsync(externalAuthenticationSettings);

            multiFactorAuthenticationSettings = model.MultiFactorAuthenticationSettings.ToSettings(multiFactorAuthenticationSettings);
            await _settingService.SaveSettingAsync(multiFactorAuthenticationSettings);

            //activity log
            await _customerActivityService.InsertActivityAsync("EditSettings", await _localizationService.GetResourceAsync("ActivityLog.EditSettings"));

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Updated"));

            return RedirectToAction("CustomerUser");
        }

        //prepare model
        model = await _settingModelFactory.PrepareCustomerUserSettingsModelAsync(model);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    //#region GDPR

    //public virtual async Task<IActionResult> Gdpr()
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
    //        return AccessDeniedView();

    //    //prepare model
    //    var model = await _settingModelFactory.PrepareGdprSettingsModelAsync();

    //    return View(model);
    //}

    //[HttpPost]
    //public virtual async Task<IActionResult> Gdpr(GdprSettingsModel model)
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
    //        return AccessDeniedView();

    //    if (ModelState.IsValid)
    //    {
    //        //load settings for a chosen site scope
    //        var siteScope = await _siteContext.GetActiveSiteScopeConfigurationAsync();
    //        var gdprSettings = await _settingService.LoadSettingAsync<GdprSettings>(siteScope);
    //        gdprSettings = model.ToSettings(gdprSettings);

    //        //we do not clear cache after each setting update.
    //        //this behavior can increase performance because cached settings will not be cleared 
    //        //and loaded from database after each update
    //        await _settingService.SaveSettingOverridablePerSiteAsync(gdprSettings, x => x.GdprEnabled, model.GdprEnabled_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(gdprSettings, x => x.LogPrivacyPolicyConsent, model.LogPrivacyPolicyConsent_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(gdprSettings, x => x.LogNewsletterConsent, model.LogNewsletterConsent_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(gdprSettings, x => x.LogUserProfileChanges, model.LogUserProfileChanges_OverrideForSite, siteScope, false);
    //        await _settingService.SaveSettingOverridablePerSiteAsync(gdprSettings, x => x.DeleteInactiveCustomersAfterMonths, model.DeleteInactiveCustomersAfterMonths_OverrideForSite, siteScope, false);

    //        //now clear settings cache
    //        await _settingService.ClearCacheAsync();

    //        //activity log
    //        await _customerActivityService.InsertActivityAsync("EditSettings", await _localizationService.GetResourceAsync("ActivityLog.EditSettings"));

    //        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Updated"));

    //        return RedirectToAction("Gdpr");
    //    }

    //    //prepare model
    //    model = await _settingModelFactory.PrepareGdprSettingsModelAsync(model);

    //    //if we got this far, something failed, redisplay form
    //    return View(model);
    //}

    //[HttpPost]
    //public virtual async Task<IActionResult> GdprConsentList(GdprConsentSearchModel searchModel)
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
    //        return await AccessDeniedDataTablesJson();

    //    //prepare model
    //    var model = await _settingModelFactory.PrepareGdprConsentListModelAsync(searchModel);

    //    return Json(model);
    //}

    //public virtual async Task<IActionResult> CreateGdprConsent()
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
    //        return AccessDeniedView();

    //    //prepare model
    //    var model = await _settingModelFactory.PrepareGdprConsentModelAsync(new GdprConsentModel(), null);

    //    return View(model);
    //}

    //[HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    //public virtual async Task<IActionResult> CreateGdprConsent(GdprConsentModel model, bool continueEditing)
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
    //        return AccessDeniedView();

    //    if (ModelState.IsValid)
    //    {
    //        var gdprConsent = model.ToEntity<GdprConsent>();
    //        await _gdprService.InsertConsentAsync(gdprConsent);

    //        //locales                
    //        await UpdateGdprConsentLocalesAsync(gdprConsent, model);

    //        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Settings.Gdpr.Consent.Added"));

    //        return continueEditing ? RedirectToAction("EditGdprConsent", new { gdprConsent.Id }) : RedirectToAction("Gdpr");
    //    }

    //    //prepare model
    //    model = await _settingModelFactory.PrepareGdprConsentModelAsync(model, null, true);

    //    //if we got this far, something failed, redisplay form
    //    return View(model);
    //}

    //public virtual async Task<IActionResult> EditGdprConsent(int id)
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
    //        return AccessDeniedView();

    //    //try to get a consent with the specified id
    //    var gdprConsent = await _gdprService.GetConsentByIdAsync(id);
    //    if (gdprConsent == null)
    //        return RedirectToAction("Gdpr");

    //    //prepare model
    //    var model = await _settingModelFactory.PrepareGdprConsentModelAsync(null, gdprConsent);

    //    return View(model);
    //}

    //[HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    //public virtual async Task<IActionResult> EditGdprConsent(GdprConsentModel model, bool continueEditing)
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
    //        return AccessDeniedView();

    //    //try to get a GDPR consent with the specified id
    //    var gdprConsent = await _gdprService.GetConsentByIdAsync(model.Id);
    //    if (gdprConsent == null)
    //        return RedirectToAction("Gdpr");

    //    if (ModelState.IsValid)
    //    {
    //        gdprConsent = model.ToEntity(gdprConsent);
    //        await _gdprService.UpdateConsentAsync(gdprConsent);

    //        //locales                
    //        await UpdateGdprConsentLocalesAsync(gdprConsent, model);

    //        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Settings.Gdpr.Consent.Updated"));

    //        return continueEditing ? RedirectToAction("EditGdprConsent", gdprConsent.Id) : RedirectToAction("Gdpr");
    //    }

    //    //prepare model
    //    model = await _settingModelFactory.PrepareGdprConsentModelAsync(model, gdprConsent, true);

    //    //if we got this far, something failed, redisplay form
    //    return View(model);
    //}

    //[HttpPost]
    //public virtual async Task<IActionResult> DeleteGdprConsent(int id)
    //{
    //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
    //        return AccessDeniedView();

    //    //try to get a GDPR consent with the specified id
    //    var gdprConsent = await _gdprService.GetConsentByIdAsync(id);
    //    if (gdprConsent == null)
    //        return RedirectToAction("Gdpr");

    //    await _gdprService.DeleteConsentAsync(gdprConsent);

    //    _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Settings.Gdpr.Consent.Deleted"));

    //    return RedirectToAction("Gdpr");
    //}

    //#endregion

    public virtual async Task<IActionResult> GeneralCommon(bool showtour = false)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
            return AccessDeniedView();

        //prepare model
        var model = await _settingModelFactory.PrepareGeneralCommonSettingsModelAsync();

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

    [HttpPost]
    [FormValueRequired("save")]
    public virtual async Task<IActionResult> GeneralCommon(GeneralCommonSettingsModel model)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
            return AccessDeniedView();

        if (ModelState.IsValid)
        {
            //load settings for a chosen site scope
            var siteScope = await _siteContext.GetActiveSiteScopeConfigurationAsync();

            //site information settings
            var siteInformationSettings = await _settingService.LoadSettingAsync<SiteInformationSettings>(siteScope);
            var commonSettings = await _settingService.LoadSettingAsync<CommonSettings>(siteScope);
            var sitemapSettings = await _settingService.LoadSettingAsync<SitemapSettings>(siteScope);

            siteInformationSettings.SiteClosed = model.SiteInformationSettings.SiteClosed;
            siteInformationSettings.DefaultSiteTheme = model.SiteInformationSettings.DefaultSiteTheme;
            siteInformationSettings.AllowCustomerToSelectTheme = model.SiteInformationSettings.AllowCustomerToSelectTheme;
            siteInformationSettings.LogoPictureId = model.SiteInformationSettings.LogoPictureId;
            //EU Cookie law
            siteInformationSettings.DisplayEuCookieLawWarning = model.SiteInformationSettings.DisplayEuCookieLawWarning;
            //social pages
            siteInformationSettings.FacebookLink = model.SiteInformationSettings.FacebookLink;
            siteInformationSettings.TwitterLink = model.SiteInformationSettings.TwitterLink;
            siteInformationSettings.YoutubeLink = model.SiteInformationSettings.YoutubeLink;
            siteInformationSettings.InstagramLink = model.SiteInformationSettings.InstagramLink;
            //contact us
            commonSettings.SubjectFieldOnContactUsForm = model.SiteInformationSettings.SubjectFieldOnContactUsForm;
            commonSettings.UseSystemEmailForContactUsForm = model.SiteInformationSettings.UseSystemEmailForContactUsForm;
            //terms of service
            commonSettings.PopupForTermsOfServiceLinks = model.SiteInformationSettings.PopupForTermsOfServiceLinks;
            //sitemap
            sitemapSettings.SitemapEnabled = model.SitemapSettings.SitemapEnabled;
            sitemapSettings.SitemapPageSize = model.SitemapSettings.SitemapPageSize;
            sitemapSettings.SitemapIncludeBlogPosts = model.SitemapSettings.SitemapIncludeBlogPosts;
            sitemapSettings.SitemapIncludeNews = model.SitemapSettings.SitemapIncludeNews;
            sitemapSettings.SitemapIncludePages = model.SitemapSettings.SitemapIncludePages;

            //minification
            commonSettings.EnableHtmlMinification = model.MinificationSettings.EnableHtmlMinification;
            //use response compression
            commonSettings.UseResponseCompression = model.MinificationSettings.UseResponseCompression;
            //custom header and footer HTML
            commonSettings.HeaderCustomHtml = model.CustomHtmlSettings.HeaderCustomHtml;
            commonSettings.FooterCustomHtml = model.CustomHtmlSettings.FooterCustomHtml;

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            await _settingService.SaveSettingOverridablePerSiteAsync(siteInformationSettings, x => x.SiteClosed, model.SiteInformationSettings.SiteClosed_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(siteInformationSettings, x => x.DefaultSiteTheme, model.SiteInformationSettings.DefaultSiteTheme_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(siteInformationSettings, x => x.AllowCustomerToSelectTheme, model.SiteInformationSettings.AllowCustomerToSelectTheme_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(siteInformationSettings, x => x.LogoPictureId, model.SiteInformationSettings.LogoPictureId_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(siteInformationSettings, x => x.DisplayEuCookieLawWarning, model.SiteInformationSettings.DisplayEuCookieLawWarning_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(siteInformationSettings, x => x.FacebookLink, model.SiteInformationSettings.FacebookLink_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(siteInformationSettings, x => x.TwitterLink, model.SiteInformationSettings.TwitterLink_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(siteInformationSettings, x => x.YoutubeLink, model.SiteInformationSettings.YoutubeLink_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(siteInformationSettings, x => x.InstagramLink, model.SiteInformationSettings.InstagramLink_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(commonSettings, x => x.SubjectFieldOnContactUsForm, model.SiteInformationSettings.SubjectFieldOnContactUsForm_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(commonSettings, x => x.UseSystemEmailForContactUsForm, model.SiteInformationSettings.UseSystemEmailForContactUsForm_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(commonSettings, x => x.PopupForTermsOfServiceLinks, model.SiteInformationSettings.PopupForTermsOfServiceLinks_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(sitemapSettings, x => x.SitemapEnabled, model.SitemapSettings.SitemapEnabled_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(sitemapSettings, x => x.SitemapPageSize, model.SitemapSettings.SitemapPageSize_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(sitemapSettings, x => x.SitemapIncludeBlogPosts, model.SitemapSettings.SitemapIncludeBlogPosts_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(sitemapSettings, x => x.SitemapIncludeNews, model.SitemapSettings.SitemapIncludeNews_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(sitemapSettings, x => x.SitemapIncludePages, model.SitemapSettings.SitemapIncludePages_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(commonSettings, x => x.EnableHtmlMinification, model.MinificationSettings.EnableHtmlMinification_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(commonSettings, x => x.UseResponseCompression, model.MinificationSettings.UseResponseCompression_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(commonSettings, x => x.HeaderCustomHtml, model.CustomHtmlSettings.HeaderCustomHtml_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(commonSettings, x => x.FooterCustomHtml, model.CustomHtmlSettings.FooterCustomHtml_OverrideForSite, siteScope, false);

            //now clear settings cache
            await _settingService.ClearCacheAsync();

            //seo settings
            var seoSettings = await _settingService.LoadSettingAsync<SeoSettings>(siteScope);
            seoSettings.PageTitleSeparator = model.SeoSettings.PageTitleSeparator;
            seoSettings.PageTitleSeoAdjustment = (PageTitleSeoAdjustment)model.SeoSettings.PageTitleSeoAdjustment;
            seoSettings.ConvertNonWesternChars = model.SeoSettings.ConvertNonWesternChars;
            seoSettings.CanonicalUrlsEnabled = model.SeoSettings.CanonicalUrlsEnabled;
            seoSettings.WwwRequirement = (WwwRequirement)model.SeoSettings.WwwRequirement;
            seoSettings.TwitterMetaTags = model.SeoSettings.TwitterMetaTags;
            seoSettings.OpenGraphMetaTags = model.SeoSettings.OpenGraphMetaTags;
            seoSettings.MicrodataEnabled = model.SeoSettings.MicrodataEnabled;
            seoSettings.CustomHeadTags = model.SeoSettings.CustomHeadTags;

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            await _settingService.SaveSettingOverridablePerSiteAsync(seoSettings, x => x.PageTitleSeparator, model.SeoSettings.PageTitleSeparator_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(seoSettings, x => x.PageTitleSeoAdjustment, model.SeoSettings.PageTitleSeoAdjustment_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(seoSettings, x => x.ConvertNonWesternChars, model.SeoSettings.ConvertNonWesternChars_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(seoSettings, x => x.CanonicalUrlsEnabled, model.SeoSettings.CanonicalUrlsEnabled_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(seoSettings, x => x.WwwRequirement, model.SeoSettings.WwwRequirement_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(seoSettings, x => x.TwitterMetaTags, model.SeoSettings.TwitterMetaTags_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(seoSettings, x => x.OpenGraphMetaTags, model.SeoSettings.OpenGraphMetaTags_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(seoSettings, x => x.CustomHeadTags, model.SeoSettings.CustomHeadTags_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(seoSettings, x => x.MicrodataEnabled, model.SeoSettings.MicrodataEnabled_OverrideForSite, siteScope, false);

            //now clear settings cache
            await _settingService.ClearCacheAsync();

            //security settings
            var securitySettings = await _settingService.LoadSettingAsync<SecuritySettings>(siteScope);
            if (securitySettings.AdminAreaAllowedIpAddresses == null)
                securitySettings.AdminAreaAllowedIpAddresses = new List<string>();
            securitySettings.AdminAreaAllowedIpAddresses.Clear();
            if (!string.IsNullOrEmpty(model.SecuritySettings.AdminAreaAllowedIpAddresses))
                foreach (var s in model.SecuritySettings.AdminAreaAllowedIpAddresses.Split(_separator, StringSplitOptions.RemoveEmptyEntries))
                    if (!string.IsNullOrWhiteSpace(s))
                        securitySettings.AdminAreaAllowedIpAddresses.Add(s.Trim());
            securitySettings.HoneypotEnabled = model.SecuritySettings.HoneypotEnabled;
            await _settingService.SaveSettingAsync(securitySettings);

            //robots.txt settings
            var robotsTxtSettings = await _settingService.LoadSettingAsync<RobotsTxtSettings>(siteScope);
            robotsTxtSettings.AllowSitemapXml = model.RobotsTxtSettings.AllowSitemapXml;
            robotsTxtSettings.AdditionsRules = model.RobotsTxtSettings.AdditionsRules?.Split(Environment.NewLine).ToList();
            robotsTxtSettings.DisallowLanguages = model.RobotsTxtSettings.DisallowLanguages?.ToList() ?? new List<int>();
            robotsTxtSettings.DisallowPaths = model.RobotsTxtSettings.DisallowPaths?.Split(Environment.NewLine).ToList();
            robotsTxtSettings.LocalizableDisallowPaths = model.RobotsTxtSettings.LocalizableDisallowPaths?.Split(Environment.NewLine).ToList();

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            await _settingService.SaveSettingOverridablePerSiteAsync(robotsTxtSettings, x => x.AllowSitemapXml, model.RobotsTxtSettings.AllowSitemapXml_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(robotsTxtSettings, x => x.AdditionsRules, model.RobotsTxtSettings.AdditionsRules_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(robotsTxtSettings, x => x.DisallowLanguages, model.RobotsTxtSettings.DisallowLanguages_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(robotsTxtSettings, x => x.DisallowPaths, model.RobotsTxtSettings.DisallowPaths_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(robotsTxtSettings, x => x.LocalizableDisallowPaths, model.RobotsTxtSettings.LocalizableDisallowPaths_OverrideForSite, siteScope, false);

            // now clear settings cache
            await _settingService.ClearCacheAsync();

            //captcha settings
            var captchaSettings = await _settingService.LoadSettingAsync<CaptchaSettings>(siteScope);
            captchaSettings.Enabled = model.CaptchaSettings.Enabled;
            captchaSettings.ShowOnLoginPage = model.CaptchaSettings.ShowOnLoginPage;
            captchaSettings.ShowOnRegistrationPage = model.CaptchaSettings.ShowOnRegistrationPage;
            captchaSettings.ShowOnContactUsPage = model.CaptchaSettings.ShowOnContactUsPage;
            captchaSettings.ShowOnBlogCommentPage = model.CaptchaSettings.ShowOnBlogCommentPage;
            captchaSettings.ShowOnNewsCommentPage = model.CaptchaSettings.ShowOnNewsCommentPage;
            captchaSettings.ShowOnNewsletterPage = model.CaptchaSettings.ShowOnNewsletterPage;
            captchaSettings.ShowOnForgotPasswordPage = model.CaptchaSettings.ShowOnForgotPasswordPage;
            captchaSettings.ShowOnForum = model.CaptchaSettings.ShowOnForum;
            captchaSettings.ReCaptchaPublicKey = model.CaptchaSettings.ReCaptchaPublicKey;
            captchaSettings.ReCaptchaPrivateKey = model.CaptchaSettings.ReCaptchaPrivateKey;
            captchaSettings.CaptchaType = (CaptchaType)model.CaptchaSettings.CaptchaType;
            captchaSettings.ReCaptchaV3ScoreThreshold = model.CaptchaSettings.ReCaptchaV3ScoreThreshold;

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            await _settingService.SaveSettingOverridablePerSiteAsync(captchaSettings, x => x.Enabled, model.CaptchaSettings.Enabled_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(captchaSettings, x => x.ShowOnLoginPage, model.CaptchaSettings.ShowOnLoginPage_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(captchaSettings, x => x.ShowOnRegistrationPage, model.CaptchaSettings.ShowOnRegistrationPage_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(captchaSettings, x => x.ShowOnContactUsPage, model.CaptchaSettings.ShowOnContactUsPage_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(captchaSettings, x => x.ShowOnBlogCommentPage, model.CaptchaSettings.ShowOnBlogCommentPage_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(captchaSettings, x => x.ShowOnNewsCommentPage, model.CaptchaSettings.ShowOnNewsCommentPage_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(captchaSettings, x => x.ShowOnNewsletterPage, model.CaptchaSettings.ShowOnNewsletterPage_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(captchaSettings, x => x.ShowOnForgotPasswordPage, model.CaptchaSettings.ShowOnForgotPasswordPage_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(captchaSettings, x => x.ShowOnForum, model.CaptchaSettings.ShowOnForum_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(captchaSettings, x => x.ReCaptchaPublicKey, model.CaptchaSettings.ReCaptchaPublicKey_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(captchaSettings, x => x.ReCaptchaPrivateKey, model.CaptchaSettings.ReCaptchaPrivateKey_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(captchaSettings, x => x.ReCaptchaV3ScoreThreshold, model.CaptchaSettings.ReCaptchaV3ScoreThreshold_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(captchaSettings, x => x.CaptchaType, model.CaptchaSettings.CaptchaType_OverrideForSite, siteScope, false);

            // now clear settings cache
            await _settingService.ClearCacheAsync();

            if (captchaSettings.Enabled &&
                (string.IsNullOrWhiteSpace(captchaSettings.ReCaptchaPublicKey) || string.IsNullOrWhiteSpace(captchaSettings.ReCaptchaPrivateKey)))
            {
                //captcha is enabled but the keys are not entered
                _notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Settings.GeneralCommon.CaptchaAppropriateKeysNotEnteredError"));
            }

            //PDF settings
            var pdfSettings = await _settingService.LoadSettingAsync<PdfSettings>(siteScope);
            pdfSettings.LetterPageSizeEnabled = model.PdfSettings.LetterPageSizeEnabled;
            pdfSettings.LogoPictureId = model.PdfSettings.LogoPictureId;
            //pdfSettings.DisablePdfInvoicesForPendingOrders = model.PdfSettings.DisablePdfInvoicesForPendingOrders;
            pdfSettings.PdfFooterTextColumn1 = model.PdfSettings.PdfFooterTextColumn1;
            pdfSettings.PdfFooterTextColumn2 = model.PdfSettings.PdfFooterTextColumn2;

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            await _settingService.SaveSettingOverridablePerSiteAsync(pdfSettings, x => x.LetterPageSizeEnabled, model.PdfSettings.LetterPageSizeEnabled_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(pdfSettings, x => x.LogoPictureId, model.PdfSettings.LogoPictureId_OverrideForSite, siteScope, false);
            //await _settingService.SaveSettingOverridablePerSiteAsync(pdfSettings, x => x.DisablePdfInvoicesForPendingOrders, model.PdfSettings.DisablePdfInvoicesForPendingOrders_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(pdfSettings, x => x.PdfFooterTextColumn1, model.PdfSettings.PdfFooterTextColumn1_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(pdfSettings, x => x.PdfFooterTextColumn2, model.PdfSettings.PdfFooterTextColumn2_OverrideForSite, siteScope, false);

            //now clear settings cache
            await _settingService.ClearCacheAsync();

            //localization settings
            var localizationSettings = await _settingService.LoadSettingAsync<LocalizationSettings>(siteScope);
            localizationSettings.UseImagesForLanguageSelection = model.LocalizationSettings.UseImagesForLanguageSelection;
            if (localizationSettings.SeoFriendlyUrlsForLanguagesEnabled != model.LocalizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
            {
                localizationSettings.SeoFriendlyUrlsForLanguagesEnabled = model.LocalizationSettings.SeoFriendlyUrlsForLanguagesEnabled;
            }

            localizationSettings.AutomaticallyDetectLanguage = model.LocalizationSettings.AutomaticallyDetectLanguage;
            localizationSettings.LoadAllLocaleRecordsOnStartup = model.LocalizationSettings.LoadAllLocaleRecordsOnStartup;
            localizationSettings.LoadAllLocalizedPropertiesOnStartup = model.LocalizationSettings.LoadAllLocalizedPropertiesOnStartup;
            localizationSettings.LoadAllUrlRecordsOnStartup = model.LocalizationSettings.LoadAllUrlRecordsOnStartup;
            await _settingService.SaveSettingAsync(localizationSettings);

            //display default menu item
            var displayDefaultMenuItemSettings = await _settingService.LoadSettingAsync<DisplayDefaultMenuItemSettings>(siteScope);

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            displayDefaultMenuItemSettings.DisplayHomepageMenuItem = model.DisplayDefaultMenuItemSettings.DisplayHomepageMenuItem;
            displayDefaultMenuItemSettings.DisplayCustomerInfoMenuItem = model.DisplayDefaultMenuItemSettings.DisplayCustomerInfoMenuItem;
            displayDefaultMenuItemSettings.DisplayBlogMenuItem = model.DisplayDefaultMenuItemSettings.DisplayBlogMenuItem;
            displayDefaultMenuItemSettings.DisplayForumsMenuItem = model.DisplayDefaultMenuItemSettings.DisplayForumsMenuItem;
            displayDefaultMenuItemSettings.DisplayContactUsMenuItem = model.DisplayDefaultMenuItemSettings.DisplayContactUsMenuItem;

            await _settingService.SaveSettingOverridablePerSiteAsync(displayDefaultMenuItemSettings, x => x.DisplayHomepageMenuItem, model.DisplayDefaultMenuItemSettings.DisplayHomepageMenuItem_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(displayDefaultMenuItemSettings, x => x.DisplayCustomerInfoMenuItem, model.DisplayDefaultMenuItemSettings.DisplayCustomerInfoMenuItem_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(displayDefaultMenuItemSettings, x => x.DisplayBlogMenuItem, model.DisplayDefaultMenuItemSettings.DisplayBlogMenuItem_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(displayDefaultMenuItemSettings, x => x.DisplayForumsMenuItem, model.DisplayDefaultMenuItemSettings.DisplayForumsMenuItem_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(displayDefaultMenuItemSettings, x => x.DisplayContactUsMenuItem, model.DisplayDefaultMenuItemSettings.DisplayContactUsMenuItem_OverrideForSite, siteScope, false);

            //now clear settings cache
            await _settingService.ClearCacheAsync();

            //display default footer item
            var displayDefaultFooterItemSettings = await _settingService.LoadSettingAsync<DisplayDefaultFooterItemSettings>(siteScope);

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            displayDefaultFooterItemSettings.DisplaySitemapFooterItem = model.DisplayDefaultFooterItemSettings.DisplaySitemapFooterItem;
            displayDefaultFooterItemSettings.DisplayContactUsFooterItem = model.DisplayDefaultFooterItemSettings.DisplayContactUsFooterItem;
            displayDefaultFooterItemSettings.DisplayNewsFooterItem = model.DisplayDefaultFooterItemSettings.DisplayNewsFooterItem;
            displayDefaultFooterItemSettings.DisplayBlogFooterItem = model.DisplayDefaultFooterItemSettings.DisplayBlogFooterItem;
            displayDefaultFooterItemSettings.DisplayForumsFooterItem = model.DisplayDefaultFooterItemSettings.DisplayForumsFooterItem;
            displayDefaultFooterItemSettings.DisplayCustomerInfoFooterItem = model.DisplayDefaultFooterItemSettings.DisplayCustomerInfoFooterItem;
            displayDefaultFooterItemSettings.DisplayCustomerAddressesFooterItem = model.DisplayDefaultFooterItemSettings.DisplayCustomerAddressesFooterItem;

            await _settingService.SaveSettingOverridablePerSiteAsync(displayDefaultFooterItemSettings, x => x.DisplaySitemapFooterItem, model.DisplayDefaultFooterItemSettings.DisplaySitemapFooterItem_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(displayDefaultFooterItemSettings, x => x.DisplayContactUsFooterItem, model.DisplayDefaultFooterItemSettings.DisplayContactUsFooterItem_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(displayDefaultFooterItemSettings, x => x.DisplayNewsFooterItem, model.DisplayDefaultFooterItemSettings.DisplayNewsFooterItem_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(displayDefaultFooterItemSettings, x => x.DisplayBlogFooterItem, model.DisplayDefaultFooterItemSettings.DisplayBlogFooterItem_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(displayDefaultFooterItemSettings, x => x.DisplayForumsFooterItem, model.DisplayDefaultFooterItemSettings.DisplayForumsFooterItem_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(displayDefaultFooterItemSettings, x => x.DisplayCustomerInfoFooterItem, model.DisplayDefaultFooterItemSettings.DisplayCustomerInfoFooterItem_OverrideForSite, siteScope, false);
            await _settingService.SaveSettingOverridablePerSiteAsync(displayDefaultFooterItemSettings, x => x.DisplayCustomerAddressesFooterItem, model.DisplayDefaultFooterItemSettings.DisplayCustomerAddressesFooterItem_OverrideForSite, siteScope, false);

            //now clear settings cache
            await _settingService.ClearCacheAsync();

            //admin area
            var adminAreaSettings = await _settingService.LoadSettingAsync<AdminAreaSettings>(siteScope);

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            adminAreaSettings.UseRichEditorInMessageTemplates = model.AdminAreaSettings.UseRichEditorInMessageTemplates;

            await _settingService.SaveSettingOverridablePerSiteAsync(adminAreaSettings, x => x.UseRichEditorInMessageTemplates, model.AdminAreaSettings.UseRichEditorInMessageTemplates_OverrideForSite, siteScope, false);

            //now clear settings cache
            await _settingService.ClearCacheAsync();

            //activity log
            await _customerActivityService.InsertActivityAsync("EditSettings", await _localizationService.GetResourceAsync("ActivityLog.EditSettings"));

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Updated"));

            return RedirectToAction("GeneralCommon");
        }

        //prepare model
        model = await _settingModelFactory.PrepareGeneralCommonSettingsModelAsync(model);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [HttpPost, ActionName("GeneralCommon")]
    [FormValueRequired("changeencryptionkey")]
    public virtual async Task<IActionResult> ChangeEncryptionKey(GeneralCommonSettingsModel model)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
            return AccessDeniedView();

        var siteScope = await _siteContext.GetActiveSiteScopeConfigurationAsync();
        var securitySettings = await _settingService.LoadSettingAsync<SecuritySettings>(siteScope);

        try
        {
            if (model.SecuritySettings.EncryptionKey == null)
                model.SecuritySettings.EncryptionKey = string.Empty;

            var newEncryptionPrivateKey = model.SecuritySettings.EncryptionKey;
            if (string.IsNullOrEmpty(newEncryptionPrivateKey) || newEncryptionPrivateKey.Length != 16)
                throw new AssetForgeException(await _localizationService.GetResourceAsync("Admin.Configuration.Settings.GeneralCommon.EncryptionKey.TooShort"));

            var oldEncryptionPrivateKey = securitySettings.EncryptionKey;
            if (oldEncryptionPrivateKey == newEncryptionPrivateKey)
                throw new AssetForgeException(await _localizationService.GetResourceAsync("Admin.Configuration.Settings.GeneralCommon.EncryptionKey.TheSame"));

            //update password information
            //optimization - load only passwords with PasswordFormat.Encrypted
            var customerPasswords = await _customerService.GetCustomerPasswordsAsync(passwordFormat: PasswordFormat.Encrypted);
            foreach (var customerPassword in customerPasswords)
            {
                var decryptedPassword = _encryptionService.DecryptText(customerPassword.Password, oldEncryptionPrivateKey);
                var encryptedPassword = _encryptionService.EncryptText(decryptedPassword, newEncryptionPrivateKey);

                customerPassword.Password = encryptedPassword;
                await _customerService.UpdateCustomerPasswordAsync(customerPassword);
            }

            securitySettings.EncryptionKey = newEncryptionPrivateKey;
            await _settingService.SaveSettingAsync(securitySettings);
            await _eventPublisher.PublishAsync(new SecuritySettingsChangedEvent(securitySettings, oldEncryptionPrivateKey));

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Settings.GeneralCommon.EncryptionKey.Changed"));
        }
        catch (Exception exc)
        {
            await _notificationService.ErrorNotificationAsync(exc);
        }

        return RedirectToAction("GeneralCommon");
    }

    [HttpPost]
    public virtual async Task<IActionResult> UploadLocalePattern()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
            return AccessDeniedView();

        try
        {
            await _uploadService.UploadLocalePatternAsync();
            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.Settings.GeneralCommon.LocalePattern.SuccessUpload"));
        }
        catch (Exception exc)
        {
            await _notificationService.ErrorNotificationAsync(exc);
        }

        return RedirectToAction("GeneralCommon");
    }

    [HttpPost]
    public virtual async Task<IActionResult> UploadIcons(IFormFile iconsFile)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
            return AccessDeniedView();

        try
        {
            if (iconsFile == null || iconsFile.Length == 0)
            {
                _notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Admin.Common.UploadFile"));
                return RedirectToAction("GeneralCommon");
            }

            //load settings for a chosen site scope
            var siteScope = await _siteContext.GetActiveSiteScopeConfigurationAsync();
            var commonSettings = await _settingService.LoadSettingAsync<CommonSettings>(siteScope);

            switch (_fileProvider.GetFileExtension(iconsFile.FileName))
            {
                case ".ico":
                    await _uploadService.UploadFaviconAsync(iconsFile);
                    commonSettings.FaviconAndAppIconsHeadCode = string.Format(CommonDefaults.SingleFaviconHeadLink, siteScope, iconsFile.FileName);

                    break;

                case ".zip":
                    await _uploadService.UploadIconsArchiveAsync(iconsFile);

                    var headCodePath = _fileProvider.GetAbsolutePath(string.Format(CommonDefaults.FaviconAndAppIconsPath, siteScope), CommonDefaults.HeadCodeFileName);
                    if (!_fileProvider.FileExists(headCodePath))
                        throw new Exception(string.Format(await _localizationService.GetResourceAsync("Admin.Configuration.Settings.GeneralCommon.FaviconAndAppIcons.MissingFile"), CommonDefaults.HeadCodeFileName));

                    using (var sr = new StreamReader(headCodePath))
                        commonSettings.FaviconAndAppIconsHeadCode = await sr.ReadToEndAsync();

                    break;

                default:
                    throw new InvalidOperationException("File is not supported.");
            }

            await _settingService.SaveSettingOverridablePerSiteAsync(commonSettings, x => x.FaviconAndAppIconsHeadCode, true, siteScope);

            //delete old favicon icon if exist
            var oldFaviconIconPath = _fileProvider.GetAbsolutePath(string.Format(CommonDefaults.OldFaviconIconName, siteScope));
            if (_fileProvider.FileExists(oldFaviconIconPath))
            {
                _fileProvider.DeleteFile(oldFaviconIconPath);
            }

            //activity log
            await _customerActivityService.InsertActivityAsync("UploadIcons", string.Format(await _localizationService.GetResourceAsync("ActivityLog.UploadNewIcons"), siteScope));
            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Configuration.FaviconAndAppIcons.Uploaded"));
        }
        catch (Exception exc)
        {
            await _notificationService.ErrorNotificationAsync(exc);
        }

        return RedirectToAction("GeneralCommon");
    }

    public virtual async Task<IActionResult> AllSettings(string settingName)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
            return AccessDeniedView();

        //prepare model
        var model = await _settingModelFactory.PrepareSettingSearchModelAsync(new SettingSearchModel { SearchSettingName = WebUtility.HtmlEncode(settingName) });

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> AllSettings(SettingSearchModel searchModel)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
            return await AccessDeniedDataTablesJson();

        //prepare model
        var model = await _settingModelFactory.PrepareSettingListModelAsync(searchModel);

        return Json(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> SettingUpdate(SettingModel model)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
            return await AccessDeniedDataTablesJson();

        if (!ModelState.IsValid)
            return ErrorJson(ModelState.SerializeErrors());

        //try to get a setting with the specified id
        var setting = await _settingService.GetSettingByIdAsync(model.Id)
            ?? throw new ArgumentException("No setting found with the specified id");

        if (!setting.Name.Equals(model.Name, StringComparison.InvariantCultureIgnoreCase))
        {
            //setting name has been changed
            await _settingService.DeleteSettingAsync(setting);
        }

        await _settingService.SetSettingAsync(model.Name, model.Value, setting.SiteId);

        //activity log
        await _customerActivityService.InsertActivityAsync("EditSettings", await _localizationService.GetResourceAsync("ActivityLog.EditSettings"), setting);

        return new NullJsonResult();
    }

    [HttpPost]
    public virtual async Task<IActionResult> SettingAdd(SettingModel model)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
            return await AccessDeniedDataTablesJson();

        if (!ModelState.IsValid)
            return ErrorJson(ModelState.SerializeErrors());

        var siteId = model.SiteId;
        await _settingService.SetSettingAsync(model.Name, model.Value, siteId);

        //activity log
        await _customerActivityService.InsertActivityAsync("AddNewSetting",
            string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewSetting"), model.Name),
            await _settingService.GetSettingAsync(model.Name, siteId));

        return Json(new { Result = true });
    }

    [HttpPost]
    public virtual async Task<IActionResult> SettingDelete(int id)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSettings))
            return await AccessDeniedDataTablesJson();

        //try to get a setting with the specified id
        var setting = await _settingService.GetSettingByIdAsync(id)
            ?? throw new ArgumentException("No setting found with the specified id", nameof(id));

        await _settingService.DeleteSettingAsync(setting);

        //activity log
        await _customerActivityService.InsertActivityAsync("DeleteSetting",
            string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteSetting"), setting.Name), setting);

        return new NullJsonResult();
    }

    //action displaying notification (warning) to a site owner about a lot of traffic 
    //between the distributed cache server and the application when LoadAllLocaleRecordsOnStartup setting is set
    public async Task<IActionResult> DistributedCacheHighTrafficWarning(bool loadAllLocaleRecordsOnStartup)
    {
        //LoadAllLocaleRecordsOnStartup is set and distributed cache is used, so display warning
        if (_appSettings.Get<DistributedCacheConfig>().Enabled && _appSettings.Get<DistributedCacheConfig>().DistributedCacheType != DistributedCacheType.Memory && loadAllLocaleRecordsOnStartup)
        {
            return Json(new
            {
                Result = await _localizationService
                    .GetResourceAsync("Admin.Configuration.Settings.GeneralCommon.LoadAllLocaleRecordsOnStartup.Warning")
            });
        }

        return Json(new { Result = string.Empty });
    }

    //Action that displays a notification (warning) to the site owner about the absence of active authentication providers
    public async Task<IActionResult> ForceMultifactorAuthenticationWarning(bool forceMultifactorAuthentication)
    {
        //ForceMultifactorAuthentication is set and the site haven't active Authentication provider , so display warning
        if (forceMultifactorAuthentication && !await _multiFactorAuthenticationPluginManager.HasActivePluginsAsync())
        {
            return Json(new
            {
                Result = await _localizationService
                    .GetResourceAsync("Admin.Configuration.Settings.CustomerUser.ForceMultifactorAuthentication.Warning")
            });
        }

        return Json(new { Result = string.Empty });
    }

    //Action that displays a notification (warning) to the site owner about the need to restart the application after changing the setting
    public async Task<IActionResult> SeoFriendlyUrlsForLanguagesEnabledWarning(bool seoFriendlyUrlsForLanguagesEnabled)
    {
        //load settings for a chosen site scope
        var siteScope = await _siteContext.GetActiveSiteScopeConfigurationAsync();
        var localizationSettings = await _settingService.LoadSettingAsync<LocalizationSettings>(siteScope);

        if (seoFriendlyUrlsForLanguagesEnabled != localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
        {
            return Json(new
            {
                Result = await _localizationService
                    .GetResourceAsync("Admin.Configuration.Settings.GeneralCommon.SeoFriendlyUrlsForLanguagesEnabled.Warning")
            });
        }

        return Json(new { Result = string.Empty });
    }

    #endregion
}