using AssetForge.App.Areas.Admin.Infrastructure.Mapper.Extensions;
using AssetForge.App.Areas.Admin.Models.Settings;
using AssetForge.App.Areas.Admin.Models.Sites;
using AssetForge.Core;
using AssetForge.Core.Configuration;
using AssetForge.Core.Domain;
using AssetForge.Core.Domain.Common;
using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Localization;
using AssetForge.Core.Domain.Media;
using AssetForge.Core.Domain.Security;
using AssetForge.Core.Domain.Seo;
using AssetForge.Core.Infrastructure;
using AssetForge.Data;
using AssetForge.Data.Configuration;
using AssetForge.Services;
using AssetForge.Services.Common;
using AssetForge.Services.Configuration;
using AssetForge.Services.Directory;
using AssetForge.Services.Helpers;
using AssetForge.Services.Localization;
using AssetForge.Services.Media;
using AssetForge.Services.Sites;
using AssetForge.Services.Themes;
using AssetForge.Web.Framework.Models.Extensions;
using AssetForge.Web.Framework.WebOptimizer;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssetForge.App.Areas.Admin.Factories;

/// <summary>
/// Represents the setting model factory implementation
/// </summary>
public partial class SettingModelFactory : ISettingModelFactory
{
    #region Fields

    protected readonly AppSettings _appSettings;
    protected readonly IAddressService _addressService;
    private readonly IBaseAdminModelFactory _baseAdminModelFactory;
    private readonly ICustomerAttributeModelFactory _customerAttributeModelFactory;
    protected readonly IDataProvider _dataProvider;
    protected readonly IAssetForgeFileProvider _fileProvider;
    protected readonly IDateTimeHelper _dateTimeHelper;
    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly ILanguageService _languageService;
    protected readonly ILocalizationService _localizationService;
    protected readonly IPictureService _pictureService;
    protected readonly ISettingService _settingService;
    protected readonly ISiteContext _siteContext;
    protected readonly ISiteService _siteService;
    protected readonly IThemeProvider _themeProvider;
    protected readonly IWorkContext _workContext;

    #endregion

    #region Ctor

    public SettingModelFactory(AppSettings appSettings,
        IAddressService addressService,
        IBaseAdminModelFactory baseAdminModelFactory,
        ICustomerAttributeModelFactory customerAttributeModelFactory,
        IDataProvider dataProvider,
        IAssetForgeFileProvider fileProvider,
        IDateTimeHelper dateTimeHelper,
        IGenericAttributeService genericAttributeService,
        ILanguageService languageService,
        ILocalizationService localizationService,
        IPictureService pictureService,
        ISettingService settingService,
        ISiteContext siteContext,
        ISiteService siteService,
        IThemeProvider themeProvider,
        IWorkContext workContext)
    {
        _appSettings = appSettings;
        _addressService = addressService;
        _baseAdminModelFactory = baseAdminModelFactory;
        _customerAttributeModelFactory = customerAttributeModelFactory;
        _dataProvider = dataProvider;
        _fileProvider = fileProvider;
        _dateTimeHelper = dateTimeHelper;
        _genericAttributeService = genericAttributeService;
        _languageService = languageService;
        _localizationService = localizationService;
        _pictureService = pictureService;
        _settingService = settingService;
        _siteContext = siteContext;
        _siteService = siteService;
        _themeProvider = themeProvider;
        _workContext = workContext;
    }

    #endregion

    #region Utilities


    /// <summary>
    /// Prepare setting model to add
    /// </summary>
    /// <param name="model">Setting model to add</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    protected virtual async Task PrepareAddSettingModelAsync(SettingModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        //prepare available sites
        await _baseAdminModelFactory.PrepareSitesAsync(model.AvailableSites);
    }

    /// <summary>
    /// Prepare site theme models
    /// </summary>
    /// <param name="models">List of site theme models</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    protected virtual async Task PrepareSiteThemeModelsAsync(IList<SiteInformationSettingsModel.ThemeModel> models)
    {
        ArgumentNullException.ThrowIfNull(models);

        //load settings for a chosen site scope
        var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
        var siteInformationSettings = await _settingService.LoadSettingAsync<SiteInformationSettings>(siteId);

        //get available themes
        var availableThemes = await _themeProvider.GetThemesAsync();
        foreach (var theme in availableThemes)
        {
            models.Add(new SiteInformationSettingsModel.ThemeModel
            {
                FriendlyName = theme.FriendlyName,
                SystemName = theme.SystemName,
                PreviewImageUrl = theme.PreviewImageUrl,
                PreviewText = theme.PreviewText,
                SupportRtl = theme.SupportRtl,
                Selected = theme.SystemName.Equals(siteInformationSettings.DefaultSiteTheme, StringComparison.InvariantCultureIgnoreCase)
            });
        }
    }

    /// <summary>
    /// Prepare customer settings model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the customer settings model
    /// </returns>
    protected virtual async Task<CustomerSettingsModel> PrepareCustomerSettingsModelAsync()
    {
        //load settings for a chosen site scope
        var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
        var customerSettings = await _settingService.LoadSettingAsync<CustomerSettings>(siteId);

        //fill in model values from the entity
        var model = customerSettings.ToSettingsModel<CustomerSettingsModel>();

        return model;
    }

    /// <summary>
    /// Prepare multi-factor authentication settings model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the multiFactorAuthenticationSettingsModel
    /// </returns>
    protected virtual async Task<MultiFactorAuthenticationSettingsModel> PrepareMultiFactorAuthenticationSettingsModelAsync()
    {
        //load settings for a chosen site scope
        var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
        var multiFactorAuthenticationSettings = await _settingService.LoadSettingAsync<MultiFactorAuthenticationSettings>(siteId);

        //fill in model values from the entity
        var model = multiFactorAuthenticationSettings.ToSettingsModel<MultiFactorAuthenticationSettingsModel>();

        return model;

    }

    /// <summary>
    /// Prepare external authentication settings model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the external authentication settings model
    /// </returns>
    protected virtual async Task<ExternalAuthenticationSettingsModel> PrepareExternalAuthenticationSettingsModelAsync()
    {
        //load settings for a chosen site scope
        var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
        var externalAuthenticationSettings = await _settingService.LoadSettingAsync<ExternalAuthenticationSettings>(siteId);

        //fill in model values from the entity
        var model = new ExternalAuthenticationSettingsModel
        {
            AllowCustomersToRemoveAssociations = externalAuthenticationSettings.AllowCustomersToRemoveAssociations
        };

        return model;
    }

    /// <summary>
    /// Prepare date time settings model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the date time settings model
    /// </returns>
    protected virtual async Task<DateTimeSettingsModel> PrepareDateTimeSettingsModelAsync()
    {
        //load settings for a chosen site scope
        var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
        var dateTimeSettings = await _settingService.LoadSettingAsync<DateTimeSettings>(siteId);

        //fill in model values from the entity
        var model = new DateTimeSettingsModel
        {
            AllowCustomersToSetTimeZone = dateTimeSettings.AllowCustomersToSetTimeZone
        };

        //fill in additional values (not existing in the entity)
        model.DefaultSiteTimeZoneId = _dateTimeHelper.DefaultSiteTimeZone.Id;

        //prepare available time zones
        await _baseAdminModelFactory.PrepareTimeZonesAsync(model.AvailableTimeZones, false);

        return model;
    }


    /// <summary>
    /// Prepare site information settings model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the site information settings model
    /// </returns>
    protected virtual async Task<SiteInformationSettingsModel> PrepareSiteInformationSettingsModelAsync()
    {
        //load settings for a chosen site scope
        var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
        var siteInformationSettings = await _settingService.LoadSettingAsync<SiteInformationSettings>(siteId);
        var commonSettings = await _settingService.LoadSettingAsync<CommonSettings>(siteId);

        //fill in model values from the entity
        var model = new SiteInformationSettingsModel
        {
            SiteClosed = siteInformationSettings.SiteClosed,
            DefaultSiteTheme = siteInformationSettings.DefaultSiteTheme,
            AllowCustomerToSelectTheme = siteInformationSettings.AllowCustomerToSelectTheme,
            LogoPictureId = siteInformationSettings.LogoPictureId,
            DisplayEuCookieLawWarning = siteInformationSettings.DisplayEuCookieLawWarning,
            FacebookLink = siteInformationSettings.FacebookLink,
            TwitterLink = siteInformationSettings.TwitterLink,
            YoutubeLink = siteInformationSettings.YoutubeLink,
            InstagramLink = siteInformationSettings.InstagramLink,
            SubjectFieldOnContactUsForm = commonSettings.SubjectFieldOnContactUsForm,
            UseSystemEmailForContactUsForm = commonSettings.UseSystemEmailForContactUsForm,
            PopupForTermsOfServiceLinks = commonSettings.PopupForTermsOfServiceLinks
        };

        //prepare available themes
        await PrepareSiteThemeModelsAsync(model.AvailableSiteThemes);

        if (siteId <= 0)
            return model;

        //fill in overridden values
        model.SiteClosed_OverrideForSite = await _settingService.SettingExistsAsync(siteInformationSettings, x => x.SiteClosed, siteId);
        model.DefaultSiteTheme_OverrideForSite = await _settingService.SettingExistsAsync(siteInformationSettings, x => x.DefaultSiteTheme, siteId);
        model.AllowCustomerToSelectTheme_OverrideForSite = await _settingService.SettingExistsAsync(siteInformationSettings, x => x.AllowCustomerToSelectTheme, siteId);
        model.LogoPictureId_OverrideForSite = await _settingService.SettingExistsAsync(siteInformationSettings, x => x.LogoPictureId, siteId);
        model.DisplayEuCookieLawWarning_OverrideForSite = await _settingService.SettingExistsAsync(siteInformationSettings, x => x.DisplayEuCookieLawWarning, siteId);
        model.FacebookLink_OverrideForSite = await _settingService.SettingExistsAsync(siteInformationSettings, x => x.FacebookLink, siteId);
        model.TwitterLink_OverrideForSite = await _settingService.SettingExistsAsync(siteInformationSettings, x => x.TwitterLink, siteId);
        model.YoutubeLink_OverrideForSite = await _settingService.SettingExistsAsync(siteInformationSettings, x => x.YoutubeLink, siteId);
        model.InstagramLink_OverrideForSite = await _settingService.SettingExistsAsync(siteInformationSettings, x => x.InstagramLink, siteId);
        model.SubjectFieldOnContactUsForm_OverrideForSite = await _settingService.SettingExistsAsync(commonSettings, x => x.SubjectFieldOnContactUsForm, siteId);
        model.UseSystemEmailForContactUsForm_OverrideForSite = await _settingService.SettingExistsAsync(commonSettings, x => x.UseSystemEmailForContactUsForm, siteId);
        model.PopupForTermsOfServiceLinks_OverrideForSite = await _settingService.SettingExistsAsync(commonSettings, x => x.PopupForTermsOfServiceLinks, siteId);

        return model;
    }

    /// <summary>
    /// Prepare Sitemap settings model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the sitemap settings model
    /// </returns>
    protected virtual async Task<SitemapSettingsModel> PrepareSitemapSettingsModelAsync()
    {
        //load settings for a chosen site scope
        var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
        var sitemapSettings = await _settingService.LoadSettingAsync<SitemapSettings>(siteId);

        //fill in model values from the entity
        var model = new SitemapSettingsModel
        {
            SitemapEnabled = sitemapSettings.SitemapEnabled,
            SitemapPageSize = sitemapSettings.SitemapPageSize,
            SitemapIncludeBlogPosts = sitemapSettings.SitemapIncludeBlogPosts,
            SitemapIncludeNews = sitemapSettings.SitemapIncludeNews,
            SitemapIncludePages = sitemapSettings.SitemapIncludePages
        };

        if (siteId <= 0)
            return model;

        //fill in overridden values
        model.SitemapEnabled_OverrideForSite = await _settingService.SettingExistsAsync(sitemapSettings, x => x.SitemapEnabled, siteId);
        model.SitemapPageSize_OverrideForSite = await _settingService.SettingExistsAsync(sitemapSettings, x => x.SitemapPageSize, siteId);
        model.SitemapIncludeBlogPosts_OverrideForSite = await _settingService.SettingExistsAsync(sitemapSettings, x => x.SitemapIncludeBlogPosts, siteId);
        model.SitemapIncludeNews_OverrideForSite = await _settingService.SettingExistsAsync(sitemapSettings, x => x.SitemapIncludeNews, siteId);
        model.SitemapIncludePages_OverrideForSite = await _settingService.SettingExistsAsync(sitemapSettings, x => x.SitemapIncludePages, siteId);

        return model;
    }

    /// <summary>
    /// Prepare minification settings model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the minification settings model
    /// </returns>
    protected virtual async Task<MinificationSettingsModel> PrepareMinificationSettingsModelAsync()
    {
        //load settings for a chosen site scope
        var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
        var minificationSettings = await _settingService.LoadSettingAsync<CommonSettings>(siteId);

        //fill in model values from the entity
        var model = new MinificationSettingsModel
        {
            EnableHtmlMinification = minificationSettings.EnableHtmlMinification,
            UseResponseCompression = minificationSettings.UseResponseCompression
        };

        if (siteId <= 0)
            return model;

        //fill in overridden values
        model.EnableHtmlMinification_OverrideForSite = await _settingService.SettingExistsAsync(minificationSettings, x => x.EnableHtmlMinification, siteId);
        model.UseResponseCompression_OverrideForSite = await _settingService.SettingExistsAsync(minificationSettings, x => x.UseResponseCompression, siteId);

        return model;
    }

    /// <summary>
    /// Prepare SEO settings model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the sEO settings model
    /// </returns>
    protected virtual async Task<SeoSettingsModel> PrepareSeoSettingsModelAsync()
    {
        //load settings for a chosen site scope
        var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
        var seoSettings = await _settingService.LoadSettingAsync<SeoSettings>(siteId);

        //fill in model values from the entity
        var model = new SeoSettingsModel
        {
            PageTitleSeparator = seoSettings.PageTitleSeparator,
            PageTitleSeoAdjustment = (int)seoSettings.PageTitleSeoAdjustment,
            PageTitleSeoAdjustmentValues = await seoSettings.PageTitleSeoAdjustment.ToSelectListAsync(),
            ConvertNonWesternChars = seoSettings.ConvertNonWesternChars,
            CanonicalUrlsEnabled = seoSettings.CanonicalUrlsEnabled,
            WwwRequirement = (int)seoSettings.WwwRequirement,
            WwwRequirementValues = await seoSettings.WwwRequirement.ToSelectListAsync(),

            TwitterMetaTags = seoSettings.TwitterMetaTags,
            OpenGraphMetaTags = seoSettings.OpenGraphMetaTags,
            CustomHeadTags = seoSettings.CustomHeadTags,
            MicrodataEnabled = seoSettings.MicrodataEnabled
        };

        if (siteId <= 0)
            return model;

        //fill in overridden values
        model.PageTitleSeparator_OverrideForSite = await _settingService.SettingExistsAsync(seoSettings, x => x.PageTitleSeparator, siteId);
        model.PageTitleSeoAdjustment_OverrideForSite = await _settingService.SettingExistsAsync(seoSettings, x => x.PageTitleSeoAdjustment, siteId);
        model.ConvertNonWesternChars_OverrideForSite = await _settingService.SettingExistsAsync(seoSettings, x => x.ConvertNonWesternChars, siteId);
        model.CanonicalUrlsEnabled_OverrideForSite = await _settingService.SettingExistsAsync(seoSettings, x => x.CanonicalUrlsEnabled, siteId);
        model.WwwRequirement_OverrideForSite = await _settingService.SettingExistsAsync(seoSettings, x => x.WwwRequirement, siteId);
        model.TwitterMetaTags_OverrideForSite = await _settingService.SettingExistsAsync(seoSettings, x => x.TwitterMetaTags, siteId);
        model.OpenGraphMetaTags_OverrideForSite = await _settingService.SettingExistsAsync(seoSettings, x => x.OpenGraphMetaTags, siteId);
        model.CustomHeadTags_OverrideForSite = await _settingService.SettingExistsAsync(seoSettings, x => x.CustomHeadTags, siteId);
        model.MicrodataEnabled_OverrideForSite = await _settingService.SettingExistsAsync(seoSettings, x => x.MicrodataEnabled, siteId);

        return model;
    }

    /// <summary>
    /// Prepare security settings model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the security settings model
    /// </returns>
    protected virtual async Task<SecuritySettingsModel> PrepareSecuritySettingsModelAsync()
    {
        //load settings for a chosen site scope
        var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
        var securitySettings = await _settingService.LoadSettingAsync<SecuritySettings>(siteId);

        //fill in model values from the entity
        var model = new SecuritySettingsModel
        {
            EncryptionKey = securitySettings.EncryptionKey,
            HoneypotEnabled = securitySettings.HoneypotEnabled
        };

        //fill in additional values (not existing in the entity)
        if (securitySettings.AdminAreaAllowedIpAddresses != null)
            model.AdminAreaAllowedIpAddresses = string.Join(",", securitySettings.AdminAreaAllowedIpAddresses);

        return model;
    }

    /// <summary>
    /// Prepare captcha settings model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the captcha settings model
    /// </returns>
    protected virtual async Task<CaptchaSettingsModel> PrepareCaptchaSettingsModelAsync()
    {
        //load settings for a chosen site scope
        var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
        var captchaSettings = await _settingService.LoadSettingAsync<CaptchaSettings>(siteId);

        //fill in model values from the entity
        var model = captchaSettings.ToSettingsModel<CaptchaSettingsModel>();

        model.CaptchaTypeValues = await captchaSettings.CaptchaType.ToSelectListAsync();

        if (siteId <= 0)
            return model;

        model.Enabled_OverrideForSite = await _settingService.SettingExistsAsync(captchaSettings, x => x.Enabled, siteId);
        model.ShowOnLoginPage_OverrideForSite = await _settingService.SettingExistsAsync(captchaSettings, x => x.ShowOnLoginPage, siteId);
        model.ShowOnRegistrationPage_OverrideForSite = await _settingService.SettingExistsAsync(captchaSettings, x => x.ShowOnRegistrationPage, siteId);
        model.ShowOnContactUsPage_OverrideForSite = await _settingService.SettingExistsAsync(captchaSettings, x => x.ShowOnContactUsPage, siteId);
        model.ShowOnBlogCommentPage_OverrideForSite = await _settingService.SettingExistsAsync(captchaSettings, x => x.ShowOnBlogCommentPage, siteId);
        model.ShowOnNewsCommentPage_OverrideForSite = await _settingService.SettingExistsAsync(captchaSettings, x => x.ShowOnNewsCommentPage, siteId);
        model.ShowOnNewsletterPage_OverrideForSite = await _settingService.SettingExistsAsync(captchaSettings, x => x.ShowOnNewsletterPage, siteId);
        model.ShowOnForgotPasswordPage_OverrideForSite = await _settingService.SettingExistsAsync(captchaSettings, x => x.ShowOnForgotPasswordPage, siteId);
        model.ShowOnForum_OverrideForSite = await _settingService.SettingExistsAsync(captchaSettings, x => x.ShowOnForum, siteId);
        model.ReCaptchaPublicKey_OverrideForSite = await _settingService.SettingExistsAsync(captchaSettings, x => x.ReCaptchaPublicKey, siteId);
        model.ReCaptchaPrivateKey_OverrideForSite = await _settingService.SettingExistsAsync(captchaSettings, x => x.ReCaptchaPrivateKey, siteId);
        model.CaptchaType_OverrideForSite = await _settingService.SettingExistsAsync(captchaSettings, x => x.CaptchaType, siteId);
        model.ReCaptchaV3ScoreThreshold_OverrideForSite = await _settingService.SettingExistsAsync(captchaSettings, x => x.ReCaptchaV3ScoreThreshold, siteId);

        return model;
    }

    /// <summary>
    /// Prepare PDF settings model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the pDF settings model
    /// </returns>
    protected virtual async Task<PdfSettingsModel> PreparePdfSettingsModelAsync()
    {
        //load settings for a chosen site scope
        var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
        var pdfSettings = await _settingService.LoadSettingAsync<PdfSettings>(siteId);

        //fill in model values from the entity
        var model = new PdfSettingsModel
        {
            LetterPageSizeEnabled = pdfSettings.LetterPageSizeEnabled,
            LogoPictureId = pdfSettings.LogoPictureId,
            //DisablePdfInvoicesForPendingOrders = pdfSettings.DisablePdfInvoicesForPendingOrders,
            PdfFooterTextColumn1 = pdfSettings.PdfFooterTextColumn1,
            PdfFooterTextColumn2 = pdfSettings.PdfFooterTextColumn2
        };

        if (siteId <= 0)
            return model;

        //fill in overridden values
        model.LetterPageSizeEnabled_OverrideForSite = await _settingService.SettingExistsAsync(pdfSettings, x => x.LetterPageSizeEnabled, siteId);
        model.LogoPictureId_OverrideForSite = await _settingService.SettingExistsAsync(pdfSettings, x => x.LogoPictureId, siteId);
        //model.DisablePdfInvoicesForPendingOrders_OverrideForSite = await _settingService.SettingExistsAsync(pdfSettings, x => x.DisablePdfInvoicesForPendingOrders, siteId);
        model.PdfFooterTextColumn1_OverrideForSite = await _settingService.SettingExistsAsync(pdfSettings, x => x.PdfFooterTextColumn1, siteId);
        model.PdfFooterTextColumn2_OverrideForSite = await _settingService.SettingExistsAsync(pdfSettings, x => x.PdfFooterTextColumn2, siteId);

        return model;
    }

    /// <summary>
    /// Prepare localization settings model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the localization settings model
    /// </returns>
    protected virtual async Task<LocalizationSettingsModel> PrepareLocalizationSettingsModelAsync()
    {
        //load settings for a chosen site scope
        var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
        var localizationSettings = await _settingService.LoadSettingAsync<LocalizationSettings>(siteId);

        //fill in model values from the entity
        var model = new LocalizationSettingsModel
        {
            UseImagesForLanguageSelection = localizationSettings.UseImagesForLanguageSelection,
            SeoFriendlyUrlsForLanguagesEnabled = localizationSettings.SeoFriendlyUrlsForLanguagesEnabled,
            AutomaticallyDetectLanguage = localizationSettings.AutomaticallyDetectLanguage,
            LoadAllLocaleRecordsOnStartup = localizationSettings.LoadAllLocaleRecordsOnStartup,
            LoadAllLocalizedPropertiesOnStartup = localizationSettings.LoadAllLocalizedPropertiesOnStartup,
            LoadAllUrlRecordsOnStartup = localizationSettings.LoadAllUrlRecordsOnStartup
        };

        return model;
    }

    /// <summary>
    /// Prepare admin area settings model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the admin area settings model
    /// </returns>
    protected virtual async Task<AdminAreaSettingsModel> PrepareAdminAreaSettingsModelAsync()
    {
        //load settings for a chosen site scope
        var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
        var adminAreaSettings = await _settingService.LoadSettingAsync<AdminAreaSettings>(siteId);

        //fill in model values from the entity
        var model = new AdminAreaSettingsModel
        {
            UseRichEditorInMessageTemplates = adminAreaSettings.UseRichEditorInMessageTemplates
        };

        //fill in overridden values
        if (siteId > 0)
        {
            model.UseRichEditorInMessageTemplates_OverrideForSite = await _settingService.SettingExistsAsync(adminAreaSettings, x => x.UseRichEditorInMessageTemplates, siteId);
        }

        return model;
    }


    /// <summary>
    /// Prepare robots.txt settings model
    /// </summary>
    /// <param name="model">robots.txt model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the robots.txt settings model
    /// </returns>
    protected virtual async Task<RobotsTxtSettingsModel> PrepareRobotsTxtSettingsModelAsync(RobotsTxtSettingsModel model = null)
    {
        var additionsInstruction =
            string.Format(
                await _localizationService.GetResourceAsync("Admin.Configuration.Settings.GeneralCommon.RobotsAdditionsInstruction"),
                RobotsTxtDefaults.RobotsAdditionsFileName);

        if (_fileProvider.FileExists(_fileProvider.Combine(_fileProvider.MapPath("~/wwwroot"), RobotsTxtDefaults.RobotsCustomFileName)))
            return new RobotsTxtSettingsModel { CustomFileExists = string.Format(await _localizationService.GetResourceAsync("Admin.Configuration.Settings.GeneralCommon.RobotsCustomFileExists"), RobotsTxtDefaults.RobotsCustomFileName), AdditionsInstruction = additionsInstruction };

        //load settings for a chosen site scope
        var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
        var robotsTxtSettings = await _settingService.LoadSettingAsync<RobotsTxtSettings>(siteId);

        model ??= new RobotsTxtSettingsModel
        {
            AllowSitemapXml = robotsTxtSettings.AllowSitemapXml,
            DisallowPaths = string.Join(Environment.NewLine, robotsTxtSettings.DisallowPaths),
            LocalizableDisallowPaths =
                string.Join(Environment.NewLine, robotsTxtSettings.LocalizableDisallowPaths),
            DisallowLanguages = robotsTxtSettings.DisallowLanguages.ToList(),
            AdditionsRules = string.Join(Environment.NewLine, robotsTxtSettings.AdditionsRules),
            AvailableLanguages = new List<SelectListItem>()
        };

        if (!model.AvailableLanguages.Any())
            (model.AvailableLanguages as List<SelectListItem>)?.AddRange((await _languageService.GetAllLanguagesAsync(siteId: siteId)).Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }));

        model.AdditionsInstruction = additionsInstruction;

        if (siteId <= 0)
            return model;

        model.AdditionsRules_OverrideForSite = await _settingService.SettingExistsAsync(robotsTxtSettings, x => x.AdditionsRules, siteId);
        model.AllowSitemapXml_OverrideForSite = await _settingService.SettingExistsAsync(robotsTxtSettings, x => x.AllowSitemapXml, siteId);
        model.DisallowLanguages_OverrideForSite = await _settingService.SettingExistsAsync(robotsTxtSettings, x => x.DisallowLanguages, siteId);
        model.DisallowPaths_OverrideForSite = await _settingService.SettingExistsAsync(robotsTxtSettings, x => x.DisallowPaths, siteId);
        model.LocalizableDisallowPaths_OverrideForSite = await _settingService.SettingExistsAsync(robotsTxtSettings, x => x.LocalizableDisallowPaths, siteId);

        return model;
    }

    /// <summary>
    /// Prepare display default menu item settings model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the display default menu item settings model
    /// </returns>
    protected virtual async Task<DisplayDefaultMenuItemSettingsModel> PrepareDisplayDefaultMenuItemSettingsModelAsync()
    {
        //load settings for a chosen site scope
        var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
        var displayDefaultMenuItemSettings = await _settingService.LoadSettingAsync<DisplayDefaultMenuItemSettings>(siteId);

        //fill in model values from the entity
        var model = new DisplayDefaultMenuItemSettingsModel
        {
            DisplayHomepageMenuItem = displayDefaultMenuItemSettings.DisplayHomepageMenuItem,
            DisplayCustomerInfoMenuItem = displayDefaultMenuItemSettings.DisplayCustomerInfoMenuItem,
            DisplayBlogMenuItem = displayDefaultMenuItemSettings.DisplayBlogMenuItem,
            DisplayForumsMenuItem = displayDefaultMenuItemSettings.DisplayForumsMenuItem,
            DisplayContactUsMenuItem = displayDefaultMenuItemSettings.DisplayContactUsMenuItem
        };

        if (siteId <= 0)
            return model;

        //fill in overridden values
        model.DisplayHomepageMenuItem_OverrideForSite = await _settingService.SettingExistsAsync(displayDefaultMenuItemSettings, x => x.DisplayHomepageMenuItem, siteId);
        model.DisplayCustomerInfoMenuItem_OverrideForSite = await _settingService.SettingExistsAsync(displayDefaultMenuItemSettings, x => x.DisplayCustomerInfoMenuItem, siteId);
        model.DisplayBlogMenuItem_OverrideForSite = await _settingService.SettingExistsAsync(displayDefaultMenuItemSettings, x => x.DisplayBlogMenuItem, siteId);
        model.DisplayForumsMenuItem_OverrideForSite = await _settingService.SettingExistsAsync(displayDefaultMenuItemSettings, x => x.DisplayForumsMenuItem, siteId);
        model.DisplayContactUsMenuItem_OverrideForSite = await _settingService.SettingExistsAsync(displayDefaultMenuItemSettings, x => x.DisplayContactUsMenuItem, siteId);

        return model;
    }

    /// <summary>
    /// Prepare display default footer item settings model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the display default footer item settings model
    /// </returns>
    protected virtual async Task<DisplayDefaultFooterItemSettingsModel> PrepareDisplayDefaultFooterItemSettingsModelAsync()
    {
        //load settings for a chosen site scope
        var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
        var displayDefaultFooterItemSettings = await _settingService.LoadSettingAsync<DisplayDefaultFooterItemSettings>(siteId);

        //fill in model values from the entity
        var model = new DisplayDefaultFooterItemSettingsModel
        {
            DisplaySitemapFooterItem = displayDefaultFooterItemSettings.DisplaySitemapFooterItem,
            DisplayContactUsFooterItem = displayDefaultFooterItemSettings.DisplayContactUsFooterItem,
            DisplayNewsFooterItem = displayDefaultFooterItemSettings.DisplayNewsFooterItem,
            DisplayBlogFooterItem = displayDefaultFooterItemSettings.DisplayBlogFooterItem,
            DisplayForumsFooterItem = displayDefaultFooterItemSettings.DisplayForumsFooterItem,
            DisplayCustomerInfoFooterItem = displayDefaultFooterItemSettings.DisplayCustomerInfoFooterItem,
            DisplayCustomerAddressesFooterItem = displayDefaultFooterItemSettings.DisplayCustomerAddressesFooterItem,
        };

        if (siteId <= 0)
            return model;

        //fill in overridden values
        model.DisplaySitemapFooterItem_OverrideForSite = await _settingService.SettingExistsAsync(displayDefaultFooterItemSettings, x => x.DisplaySitemapFooterItem, siteId);
        model.DisplayContactUsFooterItem_OverrideForSite = await _settingService.SettingExistsAsync(displayDefaultFooterItemSettings, x => x.DisplayContactUsFooterItem, siteId);
        model.DisplayNewsFooterItem_OverrideForSite = await _settingService.SettingExistsAsync(displayDefaultFooterItemSettings, x => x.DisplayNewsFooterItem, siteId);
        model.DisplayBlogFooterItem_OverrideForSite = await _settingService.SettingExistsAsync(displayDefaultFooterItemSettings, x => x.DisplayBlogFooterItem, siteId);
        model.DisplayForumsFooterItem_OverrideForSite = await _settingService.SettingExistsAsync(displayDefaultFooterItemSettings, x => x.DisplayForumsFooterItem, siteId);
        model.DisplayCustomerInfoFooterItem_OverrideForSite = await _settingService.SettingExistsAsync(displayDefaultFooterItemSettings, x => x.DisplayCustomerInfoFooterItem, siteId);
        model.DisplayCustomerAddressesFooterItem_OverrideForSite = await _settingService.SettingExistsAsync(displayDefaultFooterItemSettings, x => x.DisplayCustomerAddressesFooterItem, siteId);

        return model;
    }

    /// <summary>
    /// Prepare custom HTML settings model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the custom HTML settings model
    /// </returns>
    protected virtual async Task<CustomHtmlSettingsModel> PrepareCustomHtmlSettingsModelAsync()
    {
        //load settings for a chosen site scope
        var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
        var commonSettings = await _settingService.LoadSettingAsync<CommonSettings>(siteId);

        //fill in model values from the entity
        var model = new CustomHtmlSettingsModel
        {
            HeaderCustomHtml = commonSettings.HeaderCustomHtml,
            FooterCustomHtml = commonSettings.FooterCustomHtml
        };

        //fill in overridden values
        if (siteId > 0)
        {
            model.HeaderCustomHtml_OverrideForSite = await _settingService.SettingExistsAsync(commonSettings, x => x.HeaderCustomHtml, siteId);
            model.FooterCustomHtml_OverrideForSite = await _settingService.SettingExistsAsync(commonSettings, x => x.FooterCustomHtml, siteId);
        }

        return model;
    }


    #endregion

    #region Methods

    /// <summary>
    /// Prepare app settings model
    /// </summary>
    /// <param name="model">AppSettings model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the app settings model
    /// </returns>
    public virtual async Task<AppSettingsModel> PrepareAppSettingsModel(AppSettingsModel model = null)
    {
        model ??= new AppSettingsModel
        {
            CacheConfigModel = _appSettings.Get<CacheConfig>().ToConfigModel<CacheConfigModel>(),
            HostingConfigModel = _appSettings.Get<HostingConfig>().ToConfigModel<HostingConfigModel>(),
            DistributedCacheConfigModel = _appSettings.Get<DistributedCacheConfig>().ToConfigModel<DistributedCacheConfigModel>(),
            AzureBlobConfigModel = _appSettings.Get<AzureBlobConfig>().ToConfigModel<AzureBlobConfigModel>(),
            InstallationConfigModel = _appSettings.Get<InstallationConfig>().ToConfigModel<InstallationConfigModel>(),
            PluginConfigModel = _appSettings.Get<PluginConfig>().ToConfigModel<PluginConfigModel>(),
            CommonConfigModel = _appSettings.Get<CommonConfig>().ToConfigModel<CommonConfigModel>(),
            DataConfigModel = _appSettings.Get<DataConfig>().ToConfigModel<DataConfigModel>(),
            WebOptimizerConfigModel = _appSettings.Get<WebOptimizerConfig>().ToConfigModel<WebOptimizerConfigModel>(),
        };

        model.DistributedCacheConfigModel.DistributedCacheTypeValues = await _appSettings.Get<DistributedCacheConfig>().DistributedCacheType.ToSelectListAsync();

        model.DataConfigModel.DataProviderTypeValues = await _appSettings.Get<DataConfig>().DataProvider.ToSelectListAsync();

        //Since we decided to use the naming of the DB connections section as in the .net core - "ConnectionStrings",
        //we are forced to adjust our internal model naming to this convention in this check.
        model.EnvironmentVariables.AddRange(from property in model.GetType().GetProperties()
                                            where property.Name != nameof(AppSettingsModel.EnvironmentVariables)
                                            from pp in property.PropertyType.GetProperties()
                                            where Environment.GetEnvironmentVariables().Contains($"{property.Name.Replace("Model", "").Replace("DataConfig", "ConnectionStrings")}__{pp.Name}")
                                            select $"{property.Name}_{pp.Name}");
        return model;
    }

    ///// <summary>
    ///// Prepare blog settings model
    ///// </summary>
    ///// <param name="model">Blog settings model</param>
    ///// <returns>
    ///// A task that represents the asynchronous operation
    ///// The task result contains the blog settings model
    ///// </returns>
    //public virtual async Task<BlogSettingsModel> PrepareBlogSettingsModelAsync(BlogSettingsModel model = null)
    //{
    //    //load settings for a chosen site scope
    //    var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
    //    var blogSettings = await _settingService.LoadSettingAsync<BlogSettings>(siteId);

    //    //fill in model values from the entity
    //    model ??= blogSettings.ToSettingsModel<BlogSettingsModel>();

    //    //fill in additional values (not existing in the entity)
    //    model.ActiveSiteScopeConfiguration = siteId;

    //    if (siteId <= 0)
    //        return model;

    //    //fill in overridden values
    //    model.Enabled_OverrideForSite = await _settingService.SettingExistsAsync(blogSettings, x => x.Enabled, siteId);
    //    model.PostsPageSize_OverrideForSite = await _settingService.SettingExistsAsync(blogSettings, x => x.PostsPageSize, siteId);
    //    model.AllowNotRegisteredUsersToLeaveComments_OverrideForSite = await _settingService.SettingExistsAsync(blogSettings, x => x.AllowNotRegisteredUsersToLeaveComments, siteId);
    //    model.NotifyAboutNewBlogComments_OverrideForSite = await _settingService.SettingExistsAsync(blogSettings, x => x.NotifyAboutNewBlogComments, siteId);
    //    model.NumberOfTags_OverrideForSite = await _settingService.SettingExistsAsync(blogSettings, x => x.NumberOfTags, siteId);
    //    model.ShowHeaderRssUrl_OverrideForSite = await _settingService.SettingExistsAsync(blogSettings, x => x.ShowHeaderRssUrl, siteId);
    //    model.BlogCommentsMustBeApproved_OverrideForSite = await _settingService.SettingExistsAsync(blogSettings, x => x.BlogCommentsMustBeApproved, siteId);

    //    return model;
    //}


    ///// <summary>
    ///// Prepare forum settings model
    ///// </summary>
    ///// <param name="model">Forum settings model</param>
    ///// <returns>
    ///// A task that represents the asynchronous operation
    ///// The task result contains the forum settings model
    ///// </returns>
    //public virtual async Task<ForumSettingsModel> PrepareForumSettingsModelAsync(ForumSettingsModel model = null)
    //{
    //    //load settings for a chosen site scope
    //    var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
    //    var forumSettings = await _settingService.LoadSettingAsync<ForumSettings>(siteId);

    //    //fill in model values from the entity
    //    model ??= forumSettings.ToSettingsModel<ForumSettingsModel>();

    //    //fill in additional values (not existing in the entity)
    //    model.ActiveSiteScopeConfiguration = siteId;
    //    model.ForumEditorValues = await forumSettings.ForumEditor.ToSelectListAsync();

    //    if (siteId <= 0)
    //        return model;

    //    //fill in overridden values
    //    model.ForumsEnabled_OverrideForSite = await _settingService.SettingExistsAsync(forumSettings, x => x.ForumsEnabled, siteId);
    //    model.RelativeDateTimeFormattingEnabled_OverrideForSite = await _settingService.SettingExistsAsync(forumSettings, x => x.RelativeDateTimeFormattingEnabled, siteId);
    //    model.ShowCustomersPostCount_OverrideForSite = await _settingService.SettingExistsAsync(forumSettings, x => x.ShowCustomersPostCount, siteId);
    //    model.AllowGuestsToCreatePosts_OverrideForSite = await _settingService.SettingExistsAsync(forumSettings, x => x.AllowGuestsToCreatePosts, siteId);
    //    model.AllowGuestsToCreatePages_OverrideForSite = await _settingService.SettingExistsAsync(forumSettings, x => x.AllowGuestsToCreatePages, siteId);
    //    model.AllowCustomersToEditPosts_OverrideForSite = await _settingService.SettingExistsAsync(forumSettings, x => x.AllowCustomersToEditPosts, siteId);
    //    model.AllowCustomersToDeletePosts_OverrideForSite = await _settingService.SettingExistsAsync(forumSettings, x => x.AllowCustomersToDeletePosts, siteId);
    //    model.AllowPostVoting_OverrideForSite = await _settingService.SettingExistsAsync(forumSettings, x => x.AllowPostVoting, siteId);
    //    model.MaxVotesPerDay_OverrideForSite = await _settingService.SettingExistsAsync(forumSettings, x => x.MaxVotesPerDay, siteId);
    //    model.AllowCustomersToManageSubscriptions_OverrideForSite = await _settingService.SettingExistsAsync(forumSettings, x => x.AllowCustomersToManageSubscriptions, siteId);
    //    model.PagesPageSize_OverrideForSite = await _settingService.SettingExistsAsync(forumSettings, x => x.PagesPageSize, siteId);
    //    model.PostsPageSize_OverrideForSite = await _settingService.SettingExistsAsync(forumSettings, x => x.PostsPageSize, siteId);
    //    model.ForumEditor_OverrideForSite = await _settingService.SettingExistsAsync(forumSettings, x => x.ForumEditor, siteId);
    //    model.SignaturesEnabled_OverrideForSite = await _settingService.SettingExistsAsync(forumSettings, x => x.SignaturesEnabled, siteId);
    //    model.AllowPrivateMessages_OverrideForSite = await _settingService.SettingExistsAsync(forumSettings, x => x.AllowPrivateMessages, siteId);
    //    model.ShowAlertForPM_OverrideForSite = await _settingService.SettingExistsAsync(forumSettings, x => x.ShowAlertForPM, siteId);
    //    model.NotifyAboutPrivateMessages_OverrideForSite = await _settingService.SettingExistsAsync(forumSettings, x => x.NotifyAboutPrivateMessages, siteId);
    //    model.ActiveDiscussionsFeedEnabled_OverrideForSite = await _settingService.SettingExistsAsync(forumSettings, x => x.ActiveDiscussionsFeedEnabled, siteId);
    //    model.ActiveDiscussionsFeedCount_OverrideForSite = await _settingService.SettingExistsAsync(forumSettings, x => x.ActiveDiscussionsFeedCount, siteId);
    //    model.ForumFeedsEnabled_OverrideForSite = await _settingService.SettingExistsAsync(forumSettings, x => x.ForumFeedsEnabled, siteId);
    //    model.ForumFeedCount_OverrideForSite = await _settingService.SettingExistsAsync(forumSettings, x => x.ForumFeedCount, siteId);
    //    model.SearchResultsPageSize_OverrideForSite = await _settingService.SettingExistsAsync(forumSettings, x => x.SearchResultsPageSize, siteId);
    //    model.ActiveDiscussionsPageSize_OverrideForSite = await _settingService.SettingExistsAsync(forumSettings, x => x.ActiveDiscussionsPageSize, siteId);

    //    return model;
    //}

    ///// <summary>
    ///// Prepare news settings model
    ///// </summary>
    ///// <param name="model">News settings model</param>
    ///// <returns>
    ///// A task that represents the asynchronous operation
    ///// The task result contains the news settings model
    ///// </returns>
    //public virtual async Task<NewsSettingsModel> PrepareNewsSettingsModelAsync(NewsSettingsModel model = null)
    //{
    //    //load settings for a chosen site scope
    //    var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
    //    var newsSettings = await _settingService.LoadSettingAsync<NewsSettings>(siteId);

    //    //fill in model values from the entity
    //    model ??= newsSettings.ToSettingsModel<NewsSettingsModel>();

    //    //fill in additional values (not existing in the entity)
    //    model.ActiveSiteScopeConfiguration = siteId;

    //    if (siteId <= 0)
    //        return model;

    //    //fill in overridden values
    //    model.Enabled_OverrideForSite = await _settingService.SettingExistsAsync(newsSettings, x => x.Enabled, siteId);
    //    model.AllowNotRegisteredUsersToLeaveComments_OverrideForSite = await _settingService.SettingExistsAsync(newsSettings, x => x.AllowNotRegisteredUsersToLeaveComments, siteId);
    //    model.NotifyAboutNewNewsComments_OverrideForSite = await _settingService.SettingExistsAsync(newsSettings, x => x.NotifyAboutNewNewsComments, siteId);
    //    model.ShowNewsOnMainPage_OverrideForSite = await _settingService.SettingExistsAsync(newsSettings, x => x.ShowNewsOnMainPage, siteId);
    //    model.MainPageNewsCount_OverrideForSite = await _settingService.SettingExistsAsync(newsSettings, x => x.MainPageNewsCount, siteId);
    //    model.NewsArchivePageSize_OverrideForSite = await _settingService.SettingExistsAsync(newsSettings, x => x.NewsArchivePageSize, siteId);
    //    model.ShowHeaderRssUrl_OverrideForSite = await _settingService.SettingExistsAsync(newsSettings, x => x.ShowHeaderRssUrl, siteId);
    //    model.NewsCommentsMustBeApproved_OverrideForSite = await _settingService.SettingExistsAsync(newsSettings, x => x.NewsCommentsMustBeApproved, siteId);

    //    return model;
    //}

    ///// <summary>
    ///// Prepare shipping settings model
    ///// </summary>
    ///// <param name="model">Shipping settings model</param>
    ///// <returns>
    ///// A task that represents the asynchronous operation
    ///// The task result contains the shipping settings model
    ///// </returns>
    //public virtual async Task<ShippingSettingsModel> PrepareShippingSettingsModelAsync(ShippingSettingsModel model = null)
    //{
    //    //load settings for a chosen site scope
    //    var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
    //    var shippingSettings = await _settingService.LoadSettingAsync<ShippingSettings>(siteId);

    //    //fill in model values from the entity
    //    model ??= shippingSettings.ToSettingsModel<ShippingSettingsModel>();

    //    //fill in additional values (not existing in the entity)
    //    model.ActiveSiteScopeConfiguration = siteId;
    //    model.PrimarySiteCurrencyCode = (await _currencyService.GetCurrencyByIdAsync(_currencySettings.PrimarySiteCurrencyId))?.CurrencyCode;
    //    model.SortShippingValues = await shippingSettings.ShippingSorting.ToSelectListAsync();

    //    //fill in overridden values
    //    if (siteId > 0)
    //    {
    //        model.ShipToSameAddress_OverrideForSite = await _settingService.SettingExistsAsync(shippingSettings, x => x.ShipToSameAddress, siteId);
    //        model.AllowPickupInSite_OverrideForSite = await _settingService.SettingExistsAsync(shippingSettings, x => x.AllowPickupInSite, siteId);
    //        model.DisplayPickupPointsOnMap_OverrideForSite = await _settingService.SettingExistsAsync(shippingSettings, x => x.DisplayPickupPointsOnMap, siteId);
    //        model.IgnoreAdditionalShippingChargeForPickupInSite_OverrideForSite = await _settingService.SettingExistsAsync(shippingSettings, x => x.IgnoreAdditionalShippingChargeForPickupInSite, siteId);
    //        model.GoogleMapsApiKey_OverrideForSite = await _settingService.SettingExistsAsync(shippingSettings, x => x.GoogleMapsApiKey, siteId);
    //        model.UseWarehouseLocation_OverrideForSite = await _settingService.SettingExistsAsync(shippingSettings, x => x.UseWarehouseLocation, siteId);
    //        model.NotifyCustomerAboutShippingFromMultipleLocations_OverrideForSite = await _settingService.SettingExistsAsync(shippingSettings, x => x.NotifyCustomerAboutShippingFromMultipleLocations, siteId);
    //        model.FreeShippingOverXEnabled_OverrideForSite = await _settingService.SettingExistsAsync(shippingSettings, x => x.FreeShippingOverXEnabled, siteId);
    //        model.FreeShippingOverXValue_OverrideForSite = await _settingService.SettingExistsAsync(shippingSettings, x => x.FreeShippingOverXValue, siteId);
    //        model.FreeShippingOverXIncludingTax_OverrideForSite = await _settingService.SettingExistsAsync(shippingSettings, x => x.FreeShippingOverXIncludingTax, siteId);
    //        model.EstimateShippingCartPageEnabled_OverrideForSite = await _settingService.SettingExistsAsync(shippingSettings, x => x.EstimateShippingCartPageEnabled, siteId);
    //        model.EstimateShippingProductPageEnabled_OverrideForSite = await _settingService.SettingExistsAsync(shippingSettings, x => x.EstimateShippingProductPageEnabled, siteId);
    //        model.EstimateShippingCityNameEnabled_OverrideForSite = await _settingService.SettingExistsAsync(shippingSettings, x => x.EstimateShippingCityNameEnabled, siteId);
    //        model.DisplayShipmentEventsToCustomers_OverrideForSite = await _settingService.SettingExistsAsync(shippingSettings, x => x.DisplayShipmentEventsToCustomers, siteId);
    //        model.DisplayShipmentEventsToSiteOwner_OverrideForSite = await _settingService.SettingExistsAsync(shippingSettings, x => x.DisplayShipmentEventsToSiteOwner, siteId);
    //        model.HideShippingTotal_OverrideForSite = await _settingService.SettingExistsAsync(shippingSettings, x => x.HideShippingTotal, siteId);
    //        model.BypassShippingMethodSelectionIfOnlyOne_OverrideForSite = await _settingService.SettingExistsAsync(shippingSettings, x => x.BypassShippingMethodSelectionIfOnlyOne, siteId);
    //        model.ConsiderAssociatedProductsDimensions_OverrideForSite = await _settingService.SettingExistsAsync(shippingSettings, x => x.ConsiderAssociatedProductsDimensions, siteId);
    //        model.ShippingOriginAddress_OverrideForSite = await _settingService.SettingExistsAsync(shippingSettings, x => x.ShippingOriginAddressId, siteId);
    //        model.ShippingSorting_OverrideForSite = await _settingService.SettingExistsAsync(shippingSettings, x => x.ShippingSorting, siteId);
    //    }

    //    //prepare shipping origin address
    //    var originAddress = await _addressService.GetAddressByIdAsync(shippingSettings.ShippingOriginAddressId);
    //    if (originAddress != null)
    //        model.ShippingOriginAddress = originAddress.ToModel(model.ShippingOriginAddress);
    //    await _addressModelFactory.PrepareAddressModelAsync(model.ShippingOriginAddress, originAddress);
    //    model.ShippingOriginAddress.ZipPostalCodeRequired = true;

    //    return model;
    //}

    ///// <summary>
    ///// Prepare tax settings model
    ///// </summary>
    ///// <param name="model">Tax settings model</param>
    ///// <returns>
    ///// A task that represents the asynchronous operation
    ///// The task result contains the ax settings model
    ///// </returns>
    //public virtual async Task<TaxSettingsModel> PrepareTaxSettingsModelAsync(TaxSettingsModel model = null)
    //{
    //    //load settings for a chosen site scope
    //    var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
    //    var taxSettings = await _settingService.LoadSettingAsync<TaxSettings>(siteId);

    //    //fill in model values from the entity
    //    model ??= taxSettings.ToSettingsModel<TaxSettingsModel>();
    //    model.TaxBasedOnValues = await taxSettings.TaxBasedOn.ToSelectListAsync();
    //    model.TaxDisplayTypeValues = await taxSettings.TaxDisplayType.ToSelectListAsync();

    //    //fill in additional values (not existing in the entity)
    //    model.ActiveSiteScopeConfiguration = siteId;

    //    //fill in overridden values
    //    if (siteId > 0)
    //    {
    //        model.AutomaticallyDetectCountry_OverrideForSite = await _settingService.SettingExistsAsync(taxSettings, x => x.AutomaticallyDetectCountry, siteId);
    //        model.PricesIncludeTax_OverrideForSite = await _settingService.SettingExistsAsync(taxSettings, x => x.PricesIncludeTax, siteId);
    //        model.AllowCustomersToSelectTaxDisplayType_OverrideForSite = await _settingService.SettingExistsAsync(taxSettings, x => x.AllowCustomersToSelectTaxDisplayType, siteId);
    //        model.TaxDisplayType_OverrideForSite = await _settingService.SettingExistsAsync(taxSettings, x => x.TaxDisplayType, siteId);
    //        model.DisplayTaxSuffix_OverrideForSite = await _settingService.SettingExistsAsync(taxSettings, x => x.DisplayTaxSuffix, siteId);
    //        model.DisplayTaxRates_OverrideForSite = await _settingService.SettingExistsAsync(taxSettings, x => x.DisplayTaxRates, siteId);
    //        model.HideZeroTax_OverrideForSite = await _settingService.SettingExistsAsync(taxSettings, x => x.HideZeroTax, siteId);
    //        model.HideTaxInOrderSummary_OverrideForSite = await _settingService.SettingExistsAsync(taxSettings, x => x.HideTaxInOrderSummary, siteId);
    //        model.ForceTaxExclusionFromOrderSubtotal_OverrideForSite = await _settingService.SettingExistsAsync(taxSettings, x => x.ForceTaxExclusionFromOrderSubtotal, siteId);
    //        model.DefaultTaxCategoryId_OverrideForSite = await _settingService.SettingExistsAsync(taxSettings, x => x.DefaultTaxCategoryId, siteId);
    //        model.TaxBasedOn_OverrideForSite = await _settingService.SettingExistsAsync(taxSettings, x => x.TaxBasedOn, siteId);
    //        model.TaxBasedOnPickupPointAddress_OverrideForSite = await _settingService.SettingExistsAsync(taxSettings, x => x.TaxBasedOnPickupPointAddress, siteId);
    //        model.DefaultTaxAddress_OverrideForSite = await _settingService.SettingExistsAsync(taxSettings, x => x.DefaultTaxAddressId, siteId);
    //        model.ShippingIsTaxable_OverrideForSite = await _settingService.SettingExistsAsync(taxSettings, x => x.ShippingIsTaxable, siteId);
    //        model.ShippingPriceIncludesTax_OverrideForSite = await _settingService.SettingExistsAsync(taxSettings, x => x.ShippingPriceIncludesTax, siteId);
    //        model.ShippingTaxClassId_OverrideForSite = await _settingService.SettingExistsAsync(taxSettings, x => x.ShippingTaxClassId, siteId);
    //        model.PaymentMethodAdditionalFeeIsTaxable_OverrideForSite = await _settingService.SettingExistsAsync(taxSettings, x => x.PaymentMethodAdditionalFeeIsTaxable, siteId);
    //        model.PaymentMethodAdditionalFeeIncludesTax_OverrideForSite = await _settingService.SettingExistsAsync(taxSettings, x => x.PaymentMethodAdditionalFeeIncludesTax, siteId);
    //        model.PaymentMethodAdditionalFeeTaxClassId_OverrideForSite = await _settingService.SettingExistsAsync(taxSettings, x => x.PaymentMethodAdditionalFeeTaxClassId, siteId);
    //        model.EuVatEnabled_OverrideForSite = await _settingService.SettingExistsAsync(taxSettings, x => x.EuVatEnabled, siteId);
    //        model.EuVatEnabledForGuests_OverrideForSite = await _settingService.SettingExistsAsync(taxSettings, x => x.EuVatEnabledForGuests, siteId);
    //        model.EuVatShopCountryId_OverrideForSite = await _settingService.SettingExistsAsync(taxSettings, x => x.EuVatShopCountryId, siteId);
    //        model.EuVatAllowVatExemption_OverrideForSite = await _settingService.SettingExistsAsync(taxSettings, x => x.EuVatAllowVatExemption, siteId);
    //        model.EuVatUseWebService_OverrideForSite = await _settingService.SettingExistsAsync(taxSettings, x => x.EuVatUseWebService, siteId);
    //        model.EuVatAssumeValid_OverrideForSite = await _settingService.SettingExistsAsync(taxSettings, x => x.EuVatAssumeValid, siteId);
    //        model.EuVatEmailAdminWhenNewVatSubmitted_OverrideForSite = await _settingService.SettingExistsAsync(taxSettings, x => x.EuVatEmailAdminWhenNewVatSubmitted, siteId);
    //    }

    //    //prepare available tax categories
    //    await _baseAdminModelFactory.PrepareTaxCategoriesAsync(model.TaxCategories);

    //    //prepare available EU VAT countries
    //    await _baseAdminModelFactory.PrepareCountriesAsync(model.EuVatShopCountries);

    //    //prepare default tax address
    //    var defaultAddress = await _addressService.GetAddressByIdAsync(taxSettings.DefaultTaxAddressId);
    //    if (defaultAddress != null)
    //        model.DefaultTaxAddress = defaultAddress.ToModel(model.DefaultTaxAddress);
    //    await _addressModelFactory.PrepareAddressModelAsync(model.DefaultTaxAddress, defaultAddress);
    //    model.DefaultTaxAddress.ZipPostalCodeRequired = true;

    //    return model;
    //}

    ///// <summary>
    ///// Prepare catalog settings model
    ///// </summary>
    ///// <param name="model">Catalog settings model</param>
    ///// <returns>
    ///// A task that represents the asynchronous operation
    ///// The task result contains the catalog settings model
    ///// </returns>
    //public virtual async Task<CatalogSettingsModel> PrepareCatalogSettingsModelAsync(CatalogSettingsModel model = null)
    //{
    //    //load settings for a chosen site scope
    //    var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
    //    var catalogSettings = await _settingService.LoadSettingAsync<CatalogSettings>(siteId);

    //    //fill in model values from the entity
    //    model ??= catalogSettings.ToSettingsModel<CatalogSettingsModel>();

    //    //fill in additional values (not existing in the entity)
    //    model.ActiveSiteScopeConfiguration = siteId;
    //    model.PrimarySiteCurrencyCode = (await _currencyService.GetCurrencyByIdAsync(_currencySettings.PrimarySiteCurrencyId))?.CurrencyCode;
    //    model.AttributeValueOutOfStockDisplayTypes = await catalogSettings.AttributeValueOutOfStockDisplayType.ToSelectListAsync();
    //    model.ProductUrlStructureTypes = await ((ProductUrlStructureType)catalogSettings.ProductUrlStructureTypeId).ToSelectListAsync();
    //    model.AvailableViewModes.Add(new SelectListItem
    //    {
    //        Text = await _localizationService.GetResourceAsync("Admin.Catalog.ViewMode.Grid"),
    //        Value = "grid"
    //    });
    //    model.AvailableViewModes.Add(new SelectListItem
    //    {
    //        Text = await _localizationService.GetResourceAsync("Admin.Catalog.ViewMode.List"),
    //        Value = "list"
    //    });

    //    //fill in overridden values
    //    if (siteId > 0)
    //    {
    //        model.AllowViewUnpublishedProductPage_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.AllowViewUnpublishedProductPage, siteId);
    //        model.DisplayDiscontinuedMessageForUnpublishedProducts_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.DisplayDiscontinuedMessageForUnpublishedProducts, siteId);
    //        model.ShowSkuOnProductDetailsPage_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ShowSkuOnProductDetailsPage, siteId);
    //        model.ShowSkuOnCatalogPages_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ShowSkuOnCatalogPages, siteId);
    //        model.ShowManufacturerPartNumber_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ShowManufacturerPartNumber, siteId);
    //        model.ShowGtin_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ShowGtin, siteId);
    //        model.ShowFreeShippingNotification_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ShowFreeShippingNotification, siteId);
    //        model.ShowShortDescriptionOnCatalogPages_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ShowShortDescriptionOnCatalogPages, siteId);
    //        model.AllowProductSorting_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.AllowProductSorting, siteId);
    //        model.AllowProductViewModeChanging_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.AllowProductViewModeChanging, siteId);
    //        model.DefaultViewMode_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.DefaultViewMode, siteId);
    //        model.ShowProductsFromSubcategories_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ShowProductsFromSubcategories, siteId);
    //        model.ShowCategoryProductNumber_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ShowCategoryProductNumber, siteId);
    //        model.ShowCategoryProductNumberIncludingSubcategories_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ShowCategoryProductNumberIncludingSubcategories, siteId);
    //        model.CategoryBreadcrumbEnabled_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.CategoryBreadcrumbEnabled, siteId);
    //        model.ShowShareButton_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ShowShareButton, siteId);
    //        model.PageShareCode_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.PageShareCode, siteId);
    //        model.ProductReviewsMustBeApproved_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ProductReviewsMustBeApproved, siteId);
    //        model.OneReviewPerProductFromCustomer_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.OneReviewPerProductFromCustomer, siteId);
    //        model.AllowAnonymousUsersToReviewProduct_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.AllowAnonymousUsersToReviewProduct, siteId);
    //        model.ProductReviewPossibleOnlyAfterPurchasing_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ProductReviewPossibleOnlyAfterPurchasing, siteId);
    //        model.NotifySiteOwnerAboutNewProductReviews_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.NotifySiteOwnerAboutNewProductReviews, siteId);
    //        model.NotifyCustomerAboutProductReviewReply_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.NotifyCustomerAboutProductReviewReply, siteId);
    //        model.EmailAFriendEnabled_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.EmailAFriendEnabled, siteId);
    //        model.AllowAnonymousUsersToEmailAFriend_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.AllowAnonymousUsersToEmailAFriend, siteId);
    //        model.RecentlyViewedProductsNumber_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.RecentlyViewedProductsNumber, siteId);
    //        model.RecentlyViewedProductsEnabled_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.RecentlyViewedProductsEnabled, siteId);
    //        model.NewProductsEnabled_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.NewProductsEnabled, siteId);
    //        model.NewProductsPageSize_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.NewProductsPageSize, siteId);
    //        model.NewProductsAllowCustomersToSelectPageSize_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.NewProductsAllowCustomersToSelectPageSize, siteId);
    //        model.NewProductsPageSizeOptions_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.NewProductsPageSizeOptions, siteId);
    //        model.CompareProductsEnabled_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.CompareProductsEnabled, siteId);
    //        model.ShowBestsellersOnHomepage_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ShowBestsellersOnHomepage, siteId);
    //        model.NumberOfBestsellersOnHomepage_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.NumberOfBestsellersOnHomepage, siteId);
    //        model.SearchPageProductsPerPage_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.SearchPageProductsPerPage, siteId);
    //        model.SearchPageAllowCustomersToSelectPageSize_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.SearchPageAllowCustomersToSelectPageSize, siteId);
    //        model.SearchPagePriceRangeFiltering_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.SearchPagePriceRangeFiltering, siteId);
    //        model.SearchPagePriceFrom_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.SearchPagePriceFrom, siteId);
    //        model.SearchPagePriceTo_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.SearchPagePriceTo, siteId);
    //        model.SearchPageManuallyPriceRange_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.SearchPageManuallyPriceRange, siteId);
    //        model.SearchPagePageSizeOptions_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.SearchPagePageSizeOptions, siteId);
    //        model.ProductSearchAutoCompleteEnabled_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ProductSearchAutoCompleteEnabled, siteId);
    //        model.ProductSearchAutoCompleteNumberOfProducts_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ProductSearchAutoCompleteNumberOfProducts, siteId);
    //        model.ShowProductImagesInSearchAutoComplete_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ShowProductImagesInSearchAutoComplete, siteId);
    //        model.ShowLinkToAllResultInSearchAutoComplete_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ShowLinkToAllResultInSearchAutoComplete, siteId);
    //        model.ProductSearchTermMinimumLength_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ProductSearchTermMinimumLength, siteId);
    //        model.ProductsAlsoPurchasedEnabled_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ProductsAlsoPurchasedEnabled, siteId);
    //        model.ProductsAlsoPurchasedNumber_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ProductsAlsoPurchasedNumber, siteId);
    //        model.NumberOfProductTags_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.NumberOfProductTags, siteId);
    //        model.ProductsByTagPageSize_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ProductsByTagPageSize, siteId);
    //        model.ProductsByTagAllowCustomersToSelectPageSize_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ProductsByTagAllowCustomersToSelectPageSize, siteId);
    //        model.ProductsByTagPageSizeOptions_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ProductsByTagPageSizeOptions, siteId);
    //        model.ProductsByTagPriceRangeFiltering_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ProductsByTagPriceRangeFiltering, siteId);
    //        model.ProductsByTagPriceFrom_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ProductsByTagPriceFrom, siteId);
    //        model.ProductsByTagPriceTo_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ProductsByTagPriceTo, siteId);
    //        model.ProductsByTagManuallyPriceRange_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ProductsByTagManuallyPriceRange, siteId);
    //        model.IncludeShortDescriptionInCompareProducts_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.IncludeShortDescriptionInCompareProducts, siteId);
    //        model.IncludeFullDescriptionInCompareProducts_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.IncludeFullDescriptionInCompareProducts, siteId);
    //        model.ManufacturersBlockItemsToDisplay_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ManufacturersBlockItemsToDisplay, siteId);
    //        model.DisplayTaxShippingInfoFooter_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.DisplayTaxShippingInfoFooter, siteId);
    //        model.DisplayTaxShippingInfoProductDetailsPage_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.DisplayTaxShippingInfoProductDetailsPage, siteId);
    //        model.DisplayTaxShippingInfoProductBoxes_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.DisplayTaxShippingInfoProductBoxes, siteId);
    //        model.DisplayTaxShippingInfoShoppingCart_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.DisplayTaxShippingInfoShoppingCart, siteId);
    //        model.DisplayTaxShippingInfoWishlist_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.DisplayTaxShippingInfoWishlist, siteId);
    //        model.DisplayTaxShippingInfoOrderDetailsPage_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.DisplayTaxShippingInfoOrderDetailsPage, siteId);
    //        model.ShowProductReviewsPerSite_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ShowProductReviewsPerSite, siteId);
    //        model.ShowProductReviewsOnAccountPage_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ShowProductReviewsTabOnAccountPage, siteId);
    //        model.ProductReviewsPageSizeOnAccountPage_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ProductReviewsPageSizeOnAccountPage, siteId);
    //        model.ProductReviewsSortByCreatedDateAscending_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ProductReviewsSortByCreatedDateAscending, siteId);
    //        model.ExportImportProductAttributes_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ExportImportProductAttributes, siteId);
    //        model.ExportImportProductSpecificationAttributes_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ExportImportProductSpecificationAttributes, siteId);
    //        model.ExportImportProductCategoryBreadcrumb_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ExportImportProductCategoryBreadcrumb, siteId);
    //        model.ExportImportCategoriesUsingCategoryName_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ExportImportCategoriesUsingCategoryName, siteId);
    //        model.ExportImportAllowDownloadImages_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ExportImportAllowDownloadImages, siteId);
    //        model.ExportImportSplitProductsFile_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ExportImportSplitProductsFile, siteId);
    //        model.RemoveRequiredProducts_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.RemoveRequiredProducts, siteId);
    //        model.ExportImportRelatedEntitiesByName_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ExportImportRelatedEntitiesByName, siteId);
    //        model.ExportImportProductUseLimitedToSites_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ExportImportProductUseLimitedToSites, siteId);
    //        model.DisplayDatePreOrderAvailability_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.DisplayDatePreOrderAvailability, siteId);
    //        model.UseAjaxCatalogProductsLoading_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.UseAjaxCatalogProductsLoading, siteId);
    //        model.EnableManufacturerFiltering_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.EnableManufacturerFiltering, siteId);
    //        model.EnablePriceRangeFiltering_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.EnablePriceRangeFiltering, siteId);
    //        model.EnableSpecificationAttributeFiltering_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.EnableSpecificationAttributeFiltering, siteId);
    //        model.DisplayFromPrices_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.DisplayFromPrices, siteId);
    //        model.AttributeValueOutOfStockDisplayType_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.AttributeValueOutOfStockDisplayType, siteId);
    //        model.AllowCustomersToSearchWithManufacturerName_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.AllowCustomersToSearchWithManufacturerName, siteId);
    //        model.AllowCustomersToSearchWithCategoryName_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.AllowCustomersToSearchWithCategoryName, siteId);
    //        model.DisplayAllPicturesOnCatalogPages_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.DisplayAllPicturesOnCatalogPages, siteId);
    //        model.ProductUrlStructureTypeId_OverrideForSite = await _settingService.SettingExistsAsync(catalogSettings, x => x.ProductUrlStructureTypeId, siteId);
    //    }

    //    //prepare nested search model
    //    await PrepareSortOptionSearchModelAsync(model.SortOptionSearchModel);
    //    await _reviewTypeModelFactory.PrepareReviewTypeSearchModelAsync(model.ReviewTypeSearchModel);

    //    return model;
    //}

    ///// <summary>
    ///// Prepare paged sort option list model
    ///// </summary>
    ///// <param name="searchModel">Sort option search model</param>
    ///// <returns>
    ///// A task that represents the asynchronous operation
    ///// The task result contains the sort option list model
    ///// </returns>
    //public virtual async Task<SortOptionListModel> PrepareSortOptionListModelAsync(SortOptionSearchModel searchModel)
    //{
    //    ArgumentNullException.ThrowIfNull(searchModel);

    //    var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
    //    var catalogSettings = await _settingService.LoadSettingAsync<CatalogSettings>(siteId);

    //    //get sort options
    //    var sortOptions = Enum.GetValues(typeof(ProductSortingEnum)).OfType<ProductSortingEnum>().ToList().ToPagedList(searchModel);

    //    //prepare list model
    //    var model = await new SortOptionListModel().PrepareToGridAsync(searchModel, sortOptions, () =>
    //    {
    //        return sortOptions.SelectAwait(async option =>
    //        {
    //            //fill in model values from the entity
    //            var sortOptionModel = new SortOptionModel { Id = (int)option };

    //            //fill in additional values (not existing in the entity)
    //            sortOptionModel.Name = await _localizationService.GetLocalizedEnumAsync(option);
    //            sortOptionModel.IsActive = !catalogSettings.ProductSortingEnumDisabled.Contains((int)option);
    //            sortOptionModel.DisplayOrder = catalogSettings
    //                .ProductSortingEnumDisplayOrder.TryGetValue((int)option, out var value) ? value : (int)option;

    //            return sortOptionModel;
    //        }).OrderBy(option => option.DisplayOrder);
    //    });

    //    return model;
    //}

    ///// <summary>
    ///// Prepare reward points settings model
    ///// </summary>
    ///// <param name="model">Reward points settings model</param>
    ///// <returns>
    ///// A task that represents the asynchronous operation
    ///// The task result contains the reward points settings model
    ///// </returns>
    //public virtual async Task<RewardPointsSettingsModel> PrepareRewardPointsSettingsModelAsync(RewardPointsSettingsModel model = null)
    //{
    //    //load settings for a chosen site scope
    //    var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
    //    var rewardPointsSettings = await _settingService.LoadSettingAsync<RewardPointsSettings>(siteId);

    //    //fill in model values from the entity
    //    model ??= rewardPointsSettings.ToSettingsModel<RewardPointsSettingsModel>();

    //    //fill in additional values (not existing in the entity)
    //    model.ActiveSiteScopeConfiguration = siteId;
    //    model.PrimarySiteCurrencyCode = (await _currencyService.GetCurrencyByIdAsync(_currencySettings.PrimarySiteCurrencyId))?.CurrencyCode;
    //    model.ActivatePointsImmediately = model.ActivationDelay <= 0;

    //    if (siteId <= 0)
    //        return model;

    //    //fill in overridden values
    //    model.Enabled_OverrideForSite = await _settingService.SettingExistsAsync(rewardPointsSettings, x => x.Enabled, siteId);
    //    model.ExchangeRate_OverrideForSite = await _settingService.SettingExistsAsync(rewardPointsSettings, x => x.ExchangeRate, siteId);
    //    model.MinimumRewardPointsToUse_OverrideForSite = await _settingService.SettingExistsAsync(rewardPointsSettings, x => x.MinimumRewardPointsToUse, siteId);
    //    model.MaximumRewardPointsToUsePerOrder_OverrideForSite = await _settingService.SettingExistsAsync(rewardPointsSettings, x => x.MaximumRewardPointsToUsePerOrder, siteId);
    //    model.MaximumRedeemedRate_OverrideForSite = await _settingService.SettingExistsAsync(rewardPointsSettings, x => x.MaximumRedeemedRate, siteId);
    //    model.PointsForRegistration_OverrideForSite = await _settingService.SettingExistsAsync(rewardPointsSettings, x => x.PointsForRegistration, siteId);
    //    model.RegistrationPointsValidity_OverrideForSite = await _settingService.SettingExistsAsync(rewardPointsSettings, x => x.RegistrationPointsValidity, siteId);
    //    model.PointsForPurchases_OverrideForSite = await _settingService.SettingExistsAsync(rewardPointsSettings, x => x.PointsForPurchases_Amount, siteId) || await _settingService.SettingExistsAsync(rewardPointsSettings, x => x.PointsForPurchases_Points, siteId);
    //    model.MinOrderTotalToAwardPoints_OverrideForSite = await _settingService.SettingExistsAsync(rewardPointsSettings, x => x.MinOrderTotalToAwardPoints, siteId);
    //    model.PurchasesPointsValidity_OverrideForSite = await _settingService.SettingExistsAsync(rewardPointsSettings, x => x.PurchasesPointsValidity, siteId);
    //    model.ActivationDelay_OverrideForSite = await _settingService.SettingExistsAsync(rewardPointsSettings, x => x.ActivationDelay, siteId);
    //    model.DisplayHowMuchWillBeEarned_OverrideForSite = await _settingService.SettingExistsAsync(rewardPointsSettings, x => x.DisplayHowMuchWillBeEarned, siteId);
    //    model.PageSize_OverrideForSite = await _settingService.SettingExistsAsync(rewardPointsSettings, x => x.PageSize, siteId);

    //    return model;
    //}

    ///// <summary>
    ///// Prepare order settings model
    ///// </summary>
    ///// <param name="model">Order settings model</param>
    ///// <returns>
    ///// A task that represents the asynchronous operation
    ///// The task result contains the order settings model
    ///// </returns>
    //public virtual async Task<OrderSettingsModel> PrepareOrderSettingsModelAsync(OrderSettingsModel model = null)
    //{
    //    //load settings for a chosen site scope
    //    var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
    //    var orderSettings = await _settingService.LoadSettingAsync<OrderSettings>(siteId);

    //    //fill in model values from the entity
    //    model ??= orderSettings.ToSettingsModel<OrderSettingsModel>();

    //    //fill in additional values (not existing in the entity)
    //    model.ActiveSiteScopeConfiguration = siteId;
    //    model.PrimarySiteCurrencyCode = (await _currencyService.GetCurrencyByIdAsync(_currencySettings.PrimarySiteCurrencyId))?.CurrencyCode;
    //    model.OrderIdent = await _dataProvider.GetTableIdentAsync<Order>();

    //    //fill in overridden values
    //    if (siteId > 0)
    //    {
    //        model.IsReOrderAllowed_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.IsReOrderAllowed, siteId);
    //        model.MinOrderSubtotalAmount_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.MinOrderSubtotalAmount, siteId);
    //        model.MinOrderSubtotalAmountIncludingTax_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.MinOrderSubtotalAmountIncludingTax, siteId);
    //        model.MinOrderTotalAmount_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.MinOrderTotalAmount, siteId);
    //        model.AutoUpdateOrderTotalsOnEditingOrder_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.AutoUpdateOrderTotalsOnEditingOrder, siteId);
    //        model.AnonymousCheckoutAllowed_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.AnonymousCheckoutAllowed, siteId);
    //        model.CheckoutDisabled_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.CheckoutDisabled, siteId);
    //        model.TermsOfServiceOnShoppingCartPage_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.TermsOfServiceOnShoppingCartPage, siteId);
    //        model.TermsOfServiceOnOrderConfirmPage_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.TermsOfServiceOnOrderConfirmPage, siteId);
    //        model.OnePageCheckoutEnabled_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.OnePageCheckoutEnabled, siteId);
    //        model.OnePageCheckoutDisplayOrderTotalsOnPaymentInfoTab_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.OnePageCheckoutDisplayOrderTotalsOnPaymentInfoTab, siteId);
    //        model.DisableBillingAddressCheckoutStep_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.DisableBillingAddressCheckoutStep, siteId);
    //        model.DisableOrderCompletedPage_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.DisableOrderCompletedPage, siteId);
    //        model.DisplayPickupInSiteOnShippingMethodPage_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.DisplayPickupInSiteOnShippingMethodPage, siteId);
    //        model.AttachPdfInvoiceToOrderPlacedEmail_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.AttachPdfInvoiceToOrderPlacedEmail, siteId);
    //        model.AttachPdfInvoiceToOrderPaidEmail_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.AttachPdfInvoiceToOrderPaidEmail, siteId);
    //        model.AttachPdfInvoiceToOrderProcessingEmail_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.AttachPdfInvoiceToOrderProcessingEmail, siteId);
    //        model.AttachPdfInvoiceToOrderCompletedEmail_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.AttachPdfInvoiceToOrderCompletedEmail, siteId);
    //        model.ReturnRequestsEnabled_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.ReturnRequestsEnabled, siteId);
    //        model.ReturnRequestsAllowFiles_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.ReturnRequestsAllowFiles, siteId);
    //        model.ReturnRequestNumberMask_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.ReturnRequestNumberMask, siteId);
    //        model.NumberOfDaysReturnRequestAvailable_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.NumberOfDaysReturnRequestAvailable, siteId);
    //        model.CustomOrderNumberMask_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.CustomOrderNumberMask, siteId);
    //        model.ExportWithProducts_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.ExportWithProducts, siteId);
    //        model.AllowAdminsToBuyCallForPriceProducts_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.AllowAdminsToBuyCallForPriceProducts, siteId);
    //        model.ShowProductThumbnailInOrderDetailsPage_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.ShowProductThumbnailInOrderDetailsPage, siteId);
    //        model.DeleteGiftCardUsageHistory_OverrideForSite = await _settingService.SettingExistsAsync(orderSettings, x => x.DeleteGiftCardUsageHistory, siteId);
    //    }

    //    //prepare nested search models
    //    await _returnRequestModelFactory.PrepareReturnRequestReasonSearchModelAsync(model.ReturnRequestReasonSearchModel);
    //    await _returnRequestModelFactory.PrepareReturnRequestActionSearchModelAsync(model.ReturnRequestActionSearchModel);

    //    return model;
    //}

    ///// <summary>
    ///// Prepare shopping cart settings model
    ///// </summary>
    ///// <param name="model">Shopping cart settings model</param>
    ///// <returns>
    ///// A task that represents the asynchronous operation
    ///// The task result contains the shopping cart settings model
    ///// </returns>
    //public virtual async Task<ShoppingCartSettingsModel> PrepareShoppingCartSettingsModelAsync(ShoppingCartSettingsModel model = null)
    //{
    //    //load settings for a chosen site scope
    //    var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
    //    var shoppingCartSettings = await _settingService.LoadSettingAsync<ShoppingCartSettings>(siteId);

    //    //fill in model values from the entity
    //    model ??= shoppingCartSettings.ToSettingsModel<ShoppingCartSettingsModel>();

    //    //fill in additional values (not existing in the entity)
    //    model.ActiveSiteScopeConfiguration = siteId;

    //    if (siteId <= 0)
    //        return model;

    //    //fill in overridden values
    //    model.DisplayCartAfterAddingProduct_OverrideForSite = await _settingService.SettingExistsAsync(shoppingCartSettings, x => x.DisplayCartAfterAddingProduct, siteId);
    //    model.DisplayWishlistAfterAddingProduct_OverrideForSite = await _settingService.SettingExistsAsync(shoppingCartSettings, x => x.DisplayWishlistAfterAddingProduct, siteId);
    //    model.MaximumShoppingCartItems_OverrideForSite = await _settingService.SettingExistsAsync(shoppingCartSettings, x => x.MaximumShoppingCartItems, siteId);
    //    model.MaximumWishlistItems_OverrideForSite = await _settingService.SettingExistsAsync(shoppingCartSettings, x => x.MaximumWishlistItems, siteId);
    //    model.AllowOutOfStockItemsToBeAddedToWishlist_OverrideForSite = await _settingService.SettingExistsAsync(shoppingCartSettings, x => x.AllowOutOfStockItemsToBeAddedToWishlist, siteId);
    //    model.MoveItemsFromWishlistToCart_OverrideForSite = await _settingService.SettingExistsAsync(shoppingCartSettings, x => x.MoveItemsFromWishlistToCart, siteId);
    //    model.CartsSharedBetweenSites_OverrideForSite = await _settingService.SettingExistsAsync(shoppingCartSettings, x => x.CartsSharedBetweenSites, siteId);
    //    model.ShowProductImagesOnShoppingCart_OverrideForSite = await _settingService.SettingExistsAsync(shoppingCartSettings, x => x.ShowProductImagesOnShoppingCart, siteId);
    //    model.ShowProductImagesOnWishList_OverrideForSite = await _settingService.SettingExistsAsync(shoppingCartSettings, x => x.ShowProductImagesOnWishList, siteId);
    //    model.ShowDiscountBox_OverrideForSite = await _settingService.SettingExistsAsync(shoppingCartSettings, x => x.ShowDiscountBox, siteId);
    //    model.ShowGiftCardBox_OverrideForSite = await _settingService.SettingExistsAsync(shoppingCartSettings, x => x.ShowGiftCardBox, siteId);
    //    model.CrossSellsNumber_OverrideForSite = await _settingService.SettingExistsAsync(shoppingCartSettings, x => x.CrossSellsNumber, siteId);
    //    model.EmailWishlistEnabled_OverrideForSite = await _settingService.SettingExistsAsync(shoppingCartSettings, x => x.EmailWishlistEnabled, siteId);
    //    model.AllowAnonymousUsersToEmailWishlist_OverrideForSite = await _settingService.SettingExistsAsync(shoppingCartSettings, x => x.AllowAnonymousUsersToEmailWishlist, siteId);
    //    model.MiniShoppingCartEnabled_OverrideForSite = await _settingService.SettingExistsAsync(shoppingCartSettings, x => x.MiniShoppingCartEnabled, siteId);
    //    model.ShowProductImagesInMiniShoppingCart_OverrideForSite = await _settingService.SettingExistsAsync(shoppingCartSettings, x => x.ShowProductImagesInMiniShoppingCart, siteId);
    //    model.MiniShoppingCartProductNumber_OverrideForSite = await _settingService.SettingExistsAsync(shoppingCartSettings, x => x.MiniShoppingCartProductNumber, siteId);
    //    model.AllowCartItemEditing_OverrideForSite = await _settingService.SettingExistsAsync(shoppingCartSettings, x => x.AllowCartItemEditing, siteId);
    //    model.GroupTierPricesForDistinctShoppingCartItems_OverrideForSite = await _settingService.SettingExistsAsync(shoppingCartSettings, x => x.GroupTierPricesForDistinctShoppingCartItems, siteId);

    //    return model;
    //}

    /// <summary>
    /// Prepare media settings model
    /// </summary>
    /// <param name="model">Media settings model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the media settings model
    /// </returns>
    public virtual async Task<MediaSettingsModel> PrepareMediaSettingsModelAsync(MediaSettingsModel model = null)
    {
        //load settings for a chosen site scope
        var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
        var mediaSettings = await _settingService.LoadSettingAsync<MediaSettings>(siteId);

        //fill in model values from the entity
        model ??= mediaSettings.ToSettingsModel<MediaSettingsModel>();

        //fill in additional values (not existing in the entity)
        model.ActiveSiteScopeConfiguration = siteId;
        model.PicturesSitedIntoDatabase = await _pictureService.IsSiteInDbAsync();

        if (siteId <= 0)
            return model;

        //fill in overridden values
        model.AvatarPictureSize_OverrideForSite = await _settingService.SettingExistsAsync(mediaSettings, x => x.AvatarPictureSize, siteId);
        model.MaximumImageSize_OverrideForSite = await _settingService.SettingExistsAsync(mediaSettings, x => x.MaximumImageSize, siteId);
        model.MultipleThumbDirectories_OverrideForSite = await _settingService.SettingExistsAsync(mediaSettings, x => x.MultipleThumbDirectories, siteId);
        model.DefaultImageQuality_OverrideForSite = await _settingService.SettingExistsAsync(mediaSettings, x => x.DefaultImageQuality, siteId);
        //model.ImportProductImagesUsingHash_OverrideForSite = await _settingService.SettingExistsAsync(mediaSettings, x => x.ImportProductImagesUsingHash, siteId);
        model.DefaultPictureZoomEnabled_OverrideForSite = await _settingService.SettingExistsAsync(mediaSettings, x => x.DefaultPictureZoomEnabled, siteId);
        model.CatelogThumbPictureSize_OverrideForSite = await _settingService.SettingExistsAsync(mediaSettings, x => x.CatelogThumbPictureSize, siteId);

        return model;
    }

    /// <summary>
    /// Prepare customer user settings model
    /// </summary>
    /// <param name="model">Customer user settings model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the customer user settings model
    /// </returns>
    public virtual async Task<CustomerUserSettingsModel> PrepareCustomerUserSettingsModelAsync(CustomerUserSettingsModel model = null)
    {
        model ??= new CustomerUserSettingsModel
        {
            ActiveSiteScopeConfiguration = await _siteContext.GetActiveSiteScopeConfigurationAsync()
        };

        //prepare customer settings model
        model.CustomerSettings = await PrepareCustomerSettingsModelAsync();

        //prepare CustomerSettings list availableCountries
        await _baseAdminModelFactory.PrepareCountriesAsync(model.CustomerSettings.AvailableCountries);

        //prepare multi-factor authentication settings model
        model.MultiFactorAuthenticationSettings = await PrepareMultiFactorAuthenticationSettingsModelAsync();

        //prepare date time settings model
        model.DateTimeSettings = await PrepareDateTimeSettingsModelAsync();

        //prepare external authentication settings model
        model.ExternalAuthenticationSettings = await PrepareExternalAuthenticationSettingsModelAsync();

        //prepare nested search models
        await _customerAttributeModelFactory.PrepareCustomerAttributeSearchModelAsync(model.CustomerAttributeSearchModel);
        //await _addressAttributeModelFactory.PrepareAddressAttributeSearchModelAsync(model.AddressAttributeSearchModel);

        return model;
    }

    ///// <summary>
    ///// Prepare GDPR settings model
    ///// </summary>
    ///// <param name="model">Gdpr settings model</param>
    ///// <returns>
    ///// A task that represents the asynchronous operation
    ///// The task result contains the gDPR settings model
    ///// </returns>
    //public virtual async Task<GdprSettingsModel> PrepareGdprSettingsModelAsync(GdprSettingsModel model = null)
    //{
    //    //load settings for a chosen site scope
    //    var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
    //    var gdprSettings = await _settingService.LoadSettingAsync<GdprSettings>(siteId);

    //    //fill in model values from the entity
    //    model ??= gdprSettings.ToSettingsModel<GdprSettingsModel>();

    //    //fill in additional values (not existing in the entity)
    //    model.ActiveSiteScopeConfiguration = siteId;

    //    //prepare nested search model
    //    await PrepareGdprConsentSearchModelAsync(model.GdprConsentSearchModel);

    //    if (siteId <= 0)
    //        return model;

    //    //fill in overridden values
    //    model.GdprEnabled_OverrideForSite = await _settingService.SettingExistsAsync(gdprSettings, x => x.GdprEnabled, siteId);
    //    model.LogPrivacyPolicyConsent_OverrideForSite = await _settingService.SettingExistsAsync(gdprSettings, x => x.LogPrivacyPolicyConsent, siteId);
    //    model.LogNewsletterConsent_OverrideForSite = await _settingService.SettingExistsAsync(gdprSettings, x => x.LogNewsletterConsent, siteId);
    //    model.LogUserProfileChanges_OverrideForSite = await _settingService.SettingExistsAsync(gdprSettings, x => x.LogUserProfileChanges, siteId);
    //    model.DeleteInactiveCustomersAfterMonths_OverrideForSite = await _settingService.SettingExistsAsync(gdprSettings, x => x.DeleteInactiveCustomersAfterMonths, siteId);

    //    return model;
    //}

    ///// <summary>
    ///// Prepare paged GDPR consent list model
    ///// </summary>
    ///// <param name="searchModel">GDPR search model</param>
    ///// <returns>
    ///// A task that represents the asynchronous operation
    ///// The task result contains the gDPR consent list model
    ///// </returns>
    //public virtual async Task<GdprConsentListModel> PrepareGdprConsentListModelAsync(GdprConsentSearchModel searchModel)
    //{
    //    ArgumentNullException.ThrowIfNull(searchModel);

    //    //get sort options
    //    var consentList = (await _gdprService.GetAllConsentsAsync()).ToPagedList(searchModel);

    //    //prepare list model
    //    var model = await new GdprConsentListModel().PrepareToGridAsync(searchModel, consentList, () =>
    //    {
    //        return consentList.SelectAwait(async consent =>
    //        {
    //            var gdprConsentModel = consent.ToModel<GdprConsentModel>();

    //            var gdprConsent = await _gdprService.GetConsentByIdAsync(gdprConsentModel.Id);
    //            gdprConsentModel.Message = await _localizationService.GetLocalizedAsync(gdprConsent, entity => entity.Message);
    //            gdprConsentModel.RequiredMessage = await _localizationService.GetLocalizedAsync(gdprConsent, entity => entity.RequiredMessage);

    //            return gdprConsentModel;
    //        });
    //    });

    //    return model;
    //}

    ///// <summary>
    ///// Prepare GDPR consent model
    ///// </summary>
    ///// <param name="model">GDPR consent model</param>
    ///// <param name="gdprConsent">GDPR consent</param>
    ///// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
    ///// <returns>
    ///// A task that represents the asynchronous operation
    ///// The task result contains the gDPR consent model
    ///// </returns>
    //public virtual async Task<GdprConsentModel> PrepareGdprConsentModelAsync(GdprConsentModel model, GdprConsent gdprConsent, bool excludeProperties = false)
    //{
    //    Func<GdprConsentLocalizedModel, int, Task> localizedModelConfiguration = null;

    //    //fill in model values from the entity
    //    if (gdprConsent != null)
    //    {
    //        model ??= gdprConsent.ToModel<GdprConsentModel>();

    //        //define localized model configuration action
    //        localizedModelConfiguration = async (locale, languageId) =>
    //        {
    //            locale.Message = await _localizationService.GetLocalizedAsync(gdprConsent, entity => entity.Message, languageId, false, false);
    //            locale.RequiredMessage = await _localizationService.GetLocalizedAsync(gdprConsent, entity => entity.RequiredMessage, languageId, false, false);
    //        };
    //    }

    //    //set default values for the new model
    //    if (gdprConsent == null)
    //        model.DisplayOrder = 1;

    //    //prepare localized models
    //    if (!excludeProperties)
    //        model.Locales = await _localizedModelFactory.PrepareLocalizedModelsAsync(localizedModelConfiguration);

    //    return model;
    //}

    /// <summary>
    /// Prepare general and common settings model
    /// </summary>
    /// <param name="model">General common settings model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the general and common settings model
    /// </returns>
    public virtual async Task<GeneralCommonSettingsModel> PrepareGeneralCommonSettingsModelAsync(GeneralCommonSettingsModel model = null)
    {
        model ??= new GeneralCommonSettingsModel
        {
            ActiveSiteScopeConfiguration = await _siteContext.GetActiveSiteScopeConfigurationAsync()
        };

        //prepare site information settings model
        model.SiteInformationSettings = await PrepareSiteInformationSettingsModelAsync();

        //prepare Sitemap settings model
        model.SitemapSettings = await PrepareSitemapSettingsModelAsync();

        //prepare Minification settings model
        model.MinificationSettings = await PrepareMinificationSettingsModelAsync();

        //prepare SEO settings model
        model.SeoSettings = await PrepareSeoSettingsModelAsync();

        //prepare security settings model
        model.SecuritySettings = await PrepareSecuritySettingsModelAsync();

        //prepare robots.txt settings model
        model.RobotsTxtSettings = await PrepareRobotsTxtSettingsModelAsync();

        //prepare captcha settings model
        model.CaptchaSettings = await PrepareCaptchaSettingsModelAsync();

        //prepare PDF settings model
        model.PdfSettings = await PreparePdfSettingsModelAsync();

        //prepare localization settings model
        model.LocalizationSettings = await PrepareLocalizationSettingsModelAsync();

        //prepare admin area settings model
        model.AdminAreaSettings = await PrepareAdminAreaSettingsModelAsync();

        //prepare display default menu item settings model
        model.DisplayDefaultMenuItemSettings = await PrepareDisplayDefaultMenuItemSettingsModelAsync();

        //prepare display default footer item settings model
        model.DisplayDefaultFooterItemSettings = await PrepareDisplayDefaultFooterItemSettingsModelAsync();

        //prepare custom HTML settings model
        model.CustomHtmlSettings = await PrepareCustomHtmlSettingsModelAsync();

        return model;
    }

    ///// <summary>
    ///// Prepare product editor settings model
    ///// </summary>
    ///// <returns>
    ///// A task that represents the asynchronous operation
    ///// The task result contains the product editor settings model
    ///// </returns>
    //public virtual async Task<ProductEditorSettingsModel> PrepareProductEditorSettingsModelAsync()
    //{
    //    //load settings for a chosen site scope
    //    var siteId = await _siteContext.GetActiveSiteScopeConfigurationAsync();
    //    var productEditorSettings = await _settingService.LoadSettingAsync<ProductEditorSettings>(siteId);

    //    //fill in model values from the entity
    //    var model = productEditorSettings.ToSettingsModel<ProductEditorSettingsModel>();

    //    return model;
    //}

    /// <summary>
    /// Prepare setting search model
    /// </summary>
    /// <param name="searchModel">Setting search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the setting search model
    /// </returns>
    public virtual async Task<SettingSearchModel> PrepareSettingSearchModelAsync(SettingSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //prepare model to add
        await PrepareAddSettingModelAsync(searchModel.AddSetting);

        //prepare page parameters
        searchModel.SetGridPageSize();

        return searchModel;
    }

    /// <summary>
    /// Prepare paged setting list model
    /// </summary>
    /// <param name="searchModel">Setting search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the setting list model
    /// </returns>
    public virtual async Task<SettingListModel> PrepareSettingListModelAsync(SettingSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //get settings
        var settings = (await _settingService.GetAllSettingsAsync()).AsQueryable();

        //filter settings
        if (!string.IsNullOrEmpty(searchModel.SearchSettingName))
            settings = settings.Where(setting => setting.Name.ToLowerInvariant().Contains(searchModel.SearchSettingName.ToLowerInvariant()));
        if (!string.IsNullOrEmpty(searchModel.SearchSettingValue))
            settings = settings.Where(setting => setting.Value.ToLowerInvariant().Contains(searchModel.SearchSettingValue.ToLowerInvariant()));

        var pagedSettings = settings.ToList().ToPagedList(searchModel);

        //prepare list model
        var model = await new SettingListModel().PrepareToGridAsync(searchModel, pagedSettings, () =>
        {
            return pagedSettings.SelectAwait(async setting =>
            {
                //fill in model values from the entity
                var settingModel = setting.ToModel<SettingModel>();

                //fill in additional values (not existing in the entity)
                settingModel.Site = setting.SiteId > 0
                    ? (await _siteService.GetSiteByIdAsync(setting.SiteId))?.Name ?? "Deleted"
                    : await _localizationService.GetResourceAsync("Admin.Configuration.Settings.AllSettings.Fields.SiteName.AllSites");

                return settingModel;
            });
        });

        return model;
    }

    /// <summary>
    /// Prepare setting mode model
    /// </summary>
    /// <param name="modeName">Mode name</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the setting mode model
    /// </returns>
    public virtual async Task<SettingModeModel> PrepareSettingModeModelAsync(string modeName)
    {
        var model = new SettingModeModel
        {
            ModeName = modeName,
            Enabled = await _genericAttributeService.GetAttributeAsync<bool>(await _workContext.GetCurrentCustomerAsync(), modeName)
        };

        return model;
    }

    /// <summary>
    /// Prepare site scope configuration model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the site scope configuration model
    /// </returns>
    public virtual async Task<SiteScopeConfigurationModel> PrepareSiteScopeConfigurationModelAsync()
    {
        var model = new SiteScopeConfigurationModel
        {
            Sites = (await _siteService.GetAllSitesAsync()).Select(site => site.ToModel<SiteModel>()).ToList(),
            SiteId = await _siteContext.GetActiveSiteScopeConfigurationAsync()
        };

        return model;
    }

    #endregion
}