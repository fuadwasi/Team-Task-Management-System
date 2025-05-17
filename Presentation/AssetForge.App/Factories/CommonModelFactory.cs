using AssetForge.App.Infrastructure.Cache;
using AssetForge.App.Models.Common;
using AssetForge.Core;
using AssetForge.Core.Caching;
using AssetForge.Core.Domain;
using AssetForge.Core.Domain.Common;
using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Localization;
using AssetForge.Core.Domain.Media;
using AssetForge.Core.Domain.Security;
using AssetForge.Core.Infrastructure;
using AssetForge.Services.Common;
using AssetForge.Services.Customers;
using AssetForge.Services.Localization;
using AssetForge.Services.Media;
using AssetForge.Services.Security;
using AssetForge.Services.Seo;
using AssetForge.Web.Framework.Themes;
using AssetForge.Web.Framework.UI;
using System.Text;

namespace AssetForge.App.Factories
{
    public class CommonModelFactory : ICommonModelFactory
    {
        #region Fields

        private readonly CustomerSettings _customerSettings;
        private readonly ICustomerService _customerService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAssetForgeFileProvider _fileProvider;
        private readonly IAssetForgeHtmlHelper _iXHtmlHelper;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IPictureService _pictureService;
        private readonly ISiteContext _siteContext;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IThemeContext _themeContext;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly LocalizationSettings _localizationSettings;
        private readonly MediaSettings _mediaSettings;
        private readonly RobotsTxtSettings _robotsTxtSettings;
        private readonly SitemapXmlSettings _sitemapXmlSettings;
        private readonly SiteInformationSettings _siteInformationSettings;
        private readonly IUrlRecordService _urlRecordService;

        #endregion

        #region Ctor

        public CommonModelFactory(CustomerSettings customerSettings,
            ICustomerService customerService,
            IGenericAttributeService genericAttributeService,
            IHttpContextAccessor httpContextAccessor,
            IAssetForgeFileProvider fileProvider,
            IAssetForgeHtmlHelper iXHtmlHelper,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IPictureService pictureService,
            ISiteContext siteContext,
            IStaticCacheManager staticCacheManager,
            IThemeContext themeContext,
            IWebHelper webHelper,
            IWorkContext workContext,
            LocalizationSettings localizationSettings,
            MediaSettings mediaSettings,
            RobotsTxtSettings robotsTxtSettings,
            SitemapXmlSettings sitemapXmlSettings,
            SiteInformationSettings siteInformationSettings,
            IUrlRecordService urlRecordService)
        {
            _customerSettings = customerSettings;
            _customerService = customerService;
            _genericAttributeService = genericAttributeService;
            _httpContextAccessor = httpContextAccessor;
            _fileProvider = fileProvider;
            _iXHtmlHelper = iXHtmlHelper;
            _languageService = languageService;
            _localizationService = localizationService;
            _permissionService = permissionService;
            _pictureService = pictureService;
            _siteContext = siteContext;
            _staticCacheManager = staticCacheManager;
            _themeContext = themeContext;
            _webHelper = webHelper;
            _workContext = workContext;
            _localizationSettings = localizationSettings;
            _mediaSettings = mediaSettings;
            _robotsTxtSettings = robotsTxtSettings;
            _sitemapXmlSettings = sitemapXmlSettings;
            _siteInformationSettings = siteInformationSettings;
            _urlRecordService = urlRecordService;
        }

        #endregion

        #region Utilites
        protected virtual async Task<int> GetUnreadPrivateMessagesAsync()
        {
            var result = 0;
            //var customer = await _workContext.GetCurrentCustomerAsync();
            //if (_forumSettings.AllowPrivateMessages && !await _customerService.IsGuestAsync(customer))
            //{
            //    var site = await _siteContext.GetCurrentSiteAsync();
            //    var privateMessages = await _forumService.GetAllPrivateMessagesAsync(site.Id,
            //        0, customer.Id, false, null, false, string.Empty, 0, 1);

            //    if (privateMessages.TotalCount > 0)
            //    {
            //        result = privateMessages.TotalCount;
            //    }
            //}

            return result;
        }

        #endregion

        #region Methods

        public virtual async Task<LogoModel> PrepareLogoModelAsync()
        {
            var site = await _siteContext.GetCurrentSiteAsync();
            var model = new LogoModel
            {
                SiteName = await _localizationService.GetLocalizedAsync(site, x => x.Name)
            };

            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(AssetForgeModelCacheDefaults.SiteLogoPath
                , site, await _themeContext.GetWorkingThemeNameAsync(), _webHelper.IsCurrentConnectionSecured());
            model.LogoPath = await _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                var logo = string.Empty;
                var logoPictureId = _siteInformationSettings.LogoPictureId;

                if (logoPictureId > 0)
                    logo = await _pictureService.GetPictureUrlAsync(logoPictureId, showDefaultPicture: false);

                if (string.IsNullOrEmpty(logo))
                {
                    //use default logo
                    var pathBase = _httpContextAccessor.HttpContext.Request.PathBase.Value ?? string.Empty;
                    var siteLocation = _mediaSettings.UseAbsoluteImagePath ? _webHelper.GetSiteLocation() : $"{pathBase}/";
                    logo = $"{siteLocation}Themes/{await _themeContext.GetWorkingThemeNameAsync()}/Content/images/logo.png";
                }

                return logo;
            });

            return model;
        }


        public virtual async Task<HeaderLinksModel> PrepareHeaderLinksModelAsync()
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            var site = await _siteContext.GetCurrentSiteAsync();

            var unreadMessageCount = await GetUnreadPrivateMessagesAsync();
            var unreadMessage = string.Empty;
            var alertMessage = string.Empty;
            if (unreadMessageCount > 0)
            {
                unreadMessage = string.Format(await _localizationService.GetResourceAsync("PrivateMessages.TotalUnread"), unreadMessageCount);

                //notifications here
                //if (_forumSettings.ShowAlertForPM &&
                //    !await _genericAttributeService.GetAttributeAsync<bool>(customer, CustomerDefaults.NotifiedAboutNewPrivateMessagesAttribute, site.Id))
                //{
                //    await _genericAttributeService.SaveAttributeAsync(customer, CustomerDefaults.NotifiedAboutNewPrivateMessagesAttribute, true, site.Id);
                //    alertMessage = string.Format(await _localizationService.GetResourceAsync("PrivateMessages.YouHaveUnreadPM"), unreadMessageCount);
                //}
            }

            var model = new HeaderLinksModel
            {
                RegistrationType = _customerSettings.UserRegistrationType,
                IsAuthenticated = await _customerService.IsRegisteredAsync(customer),
                CustomerName = await _customerService.IsRegisteredAsync(customer) ? await _customerService.FormatUsernameAsync(customer) : string.Empty,
                AllowPrivateMessages = false,
                //AllowPrivateMessages = await _customerService.IsRegisteredAsync(customer) && _forumSettings.AllowPrivateMessages,
                UnreadPrivateMessages = unreadMessage,
                AlertMessage = alertMessage,
            };

            return model;
        }


        public virtual async Task<AdminHeaderLinksModel> PrepareAdminHeaderLinksModelAsync()
        {
            var customer = await _workContext.GetCurrentCustomerAsync();

            var model = new AdminHeaderLinksModel
            {
                ImpersonatedCustomerName = await _customerService.IsRegisteredAsync(customer) ? await _customerService.FormatUsernameAsync(customer) : string.Empty,
                IsCustomerImpersonated = _workContext.OriginalCustomerIfImpersonated != null,
                DisplayAdminLink = await _permissionService.AuthorizeAsync(StandardPermissionProvider.AccessAdminPanel),
                //DisplayAdminLink = true,
                EditPageUrl = _iXHtmlHelper.GetEditPageUrl()
            };

            return model;
        }

        public virtual async Task<string> PrepareRobotsTextFileAsync()
        {
            var sb = new StringBuilder();

            //if robots.custom.txt exists, let's use it instead of hard-coded data below
            var robotsFilePath = _fileProvider.Combine(_fileProvider.MapPath("~/wwwroot"), RobotsTxtDefaults.RobotsCustomFileName);
            if (_fileProvider.FileExists(robotsFilePath))
            {
                //the robots.txt file exists
                var robotsFileContent = await _fileProvider.ReadAllTextAsync(robotsFilePath, Encoding.UTF8);
                sb.Append(robotsFileContent);
            }
            else
            {
                sb.AppendLine("User-agent: *");

                //sitemap
                if (_sitemapXmlSettings.SitemapXmlEnabled && _robotsTxtSettings.AllowSitemapXml)
                    sb.AppendLine($"Sitemap: {_webHelper.GetSiteLocation()}sitemap.xml");
                else
                    sb.AppendLine("Disallow: /sitemap.xml");

                //host
                sb.AppendLine($"Host: {_webHelper.GetSiteLocation()}");

                //usual paths
                foreach (var path in _robotsTxtSettings.DisallowPaths)
                    sb.AppendLine($"Disallow: {path}");

                //localizable paths (without SEO code)
                foreach (var path in _robotsTxtSettings.LocalizableDisallowPaths)
                    sb.AppendLine($"Disallow: {path}");

                if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
                {
                    var site = await _siteContext.GetCurrentSiteAsync();
                    //URLs are localizable. Append SEO code
                    foreach (var language in await _languageService.GetAllLanguagesAsync(siteId: site.Id))
                        if (_robotsTxtSettings.DisallowLanguages.Contains(language.Id))
                            sb.AppendLine($"Disallow: /{language.UniqueSeoCode}*");
                        else
                            foreach (var path in _robotsTxtSettings.LocalizableDisallowPaths)
                                sb.AppendLine($"Disallow: /{language.UniqueSeoCode}{path}");
                }

                foreach (var additionsRule in _robotsTxtSettings.AdditionsRules)
                    sb.AppendLine(additionsRule);

                //load and add robots.txt additions to the end of file.
                var robotsAdditionsFile = _fileProvider.Combine(_fileProvider.MapPath("~/wwwroot"), RobotsTxtDefaults.RobotsAdditionsFileName);
                if (_fileProvider.FileExists(robotsAdditionsFile))
                {
                    sb.AppendLine();
                    var robotsFileContent = await _fileProvider.ReadAllTextAsync(robotsAdditionsFile, Encoding.UTF8);
                    sb.Append(robotsFileContent);
                }
            }

            return sb.ToString();
        }

        //public virtual async Task<List<PageMenuModel>> PreparePageMenuModelsAsync(int parentPageId, bool loadChildPages = true)
        //{
        //    var result = new List<PageMenuModel>();

        //    var site = await _siteContext.GetCurrentSiteAsync();
        //    var pages = await _pageService.GetAllPagesPagedAsync(siteId: site.Id, parentId: parentPageId, pageTypeId: (int)PageType.Website, isActive: true, published: true);

        //    foreach ( var page in pages)
        //    {
        //        var targetUrl = "";
        //        Page targetPage = null;

        //        if (page.ContentType == ContentType.ExternalUrl)
        //        {
        //            targetUrl = page.ContentUrl;
        //        }
        //        else if(page.ContentType == ContentType.ExistingPage)
        //        {
        //            targetPage = await _pageService.GetPageByIdAsync(page.ContentPageId);
        //            if(targetPage == null)
        //            {
        //                continue;
        //            }
        //        }
        //        //if(page.ContentType == ContentType.ExternalUrl)
        //        var pageMenuModel = new PageMenuModel()
        //        {
        //            Id = page.Id,
        //            Name = await _localizationService.GetLocalizedAsync(page, x => x.Title),
        //            SeName = page.ContentType == ContentType.ExistingPage? await _urlRecordService.GetSeNameAsync(targetPage) : await _urlRecordService.GetSeNameAsync(page),
        //            IsUrl = page.ContentType == ContentType.ExternalUrl,
        //            TargetUrl = page.ContentType == ContentType.ExternalUrl ? page.ContentUrl : "",
        //            OpenInNewWindow = page.TargetWindowType == TargetWindowType.NewWindow,
        //            IsFile = page.ContentType == ContentType.File,
        //            FileId = page.ContentFileId
        //        };

        //        if (loadChildPages)
        //        {
        //            var childPages = await PreparePageMenuModelsAsync(page.Id);
        //            pageMenuModel.ChildPages.AddRange(childPages);
        //        }

        //        pageMenuModel.HaveChildPages = pageMenuModel.ChildPages.Count > 0;

        //        result.Add(pageMenuModel);
        //    }

        //    return result;
        ////}

        //public virtual async Task<List<PageMenuModel>> PreparePageMenuModelsAsync()
        //{
        //    //load and cache them
        //    var language = await _workContext.GetWorkingLanguageAsync();
        //    var customer = await _workContext.GetCurrentCustomerAsync();
        //    var customerRoleIds = await _customerService.GetCustomerRoleIdsAsync(customer);
        //    var site = await _siteContext.GetCurrentSiteAsync();
        //    var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(AssetForgeModelCacheDefaults.PageMenuModelKey,
        //        language, customerRoleIds, site);

        //    return await _staticCacheManager.GetAsync(cacheKey, async () => await PreparePageMenuModelsAsync(0));
        //}

        //public virtual async Task<TopMenuModel> PrepareTopMenuModelAsync()
        //{
        //    var model = new TopMenuModel
        //    {
        //        PageMenus = await PreparePageMenuModelsAsync(),
        //    };

        //    return model;
        //}

        #endregion
    }
}
