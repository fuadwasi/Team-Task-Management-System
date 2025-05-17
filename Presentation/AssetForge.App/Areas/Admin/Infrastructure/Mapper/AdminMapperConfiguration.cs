using AssetForge.App.Areas.Admin.Models.Common;
using AssetForge.App.Areas.Admin.Models.Customers;
using AssetForge.App.Areas.Admin.Models.Directory;
using AssetForge.App.Areas.Admin.Models.ExternalAuthentication;
using AssetForge.App.Areas.Admin.Models.Localization;
using AssetForge.App.Areas.Admin.Models.Logging;
using AssetForge.App.Areas.Admin.Models.MultiFactorAuthentication;
using AssetForge.App.Areas.Admin.Models.Plugins;
using AssetForge.App.Areas.Admin.Models.Settings;
using AssetForge.App.Areas.Admin.Models.Sites;
using AssetForge.App.Areas.Admin.Models.Tasks;
using AssetForge.App.Areas.Admin.Models.Templates;
using AssetForge.App.Areas.Admin.Models.Topics;
using AssetForge.Core.Configuration;
using AssetForge.Core.Domain.Common;
using AssetForge.Core.Domain.Configuration;
using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Directory;
using AssetForge.Core.Domain.Localization;
using AssetForge.Core.Domain.Logging;
using AssetForge.Core.Domain.Media;
using AssetForge.Core.Domain.ScheduleTasks;
using AssetForge.Core.Domain.Security;
using AssetForge.Core.Domain.Seo;
using AssetForge.Core.Domain.Sites;
using AssetForge.Core.Infrastructure.Mapper;
using AssetForge.Data.Configuration;
using AssetForge.Services.Authentication.MultiFactor;
using AssetForge.Services.Plugins;
using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.WebOptimizer;
using AutoMapper;
using AutoMapper.Internal;

namespace AssetForge.App.Areas.Admin.Infrastructure.Mapper;

/// <summary>
/// AutoMapper configuration for admin area models
/// </summary>
public partial class AdminMapperConfiguration : Profile, IOrderedMapperProfile
{
    #region Ctor

    public AdminMapperConfiguration()
    {
        //create specific maps
        CreateConfigMaps();
        CreateMultiFactorAuthenticationMaps();
        CreateBlogsMaps();
        CreateCommonMaps();
        CreateCustomersMaps();
        CreateDirectoryMaps();
        CreateForumsMaps();
        CreateLocalizationMaps();
        CreateLoggingMaps();
        CreateMediaMaps();
        CreatePluginsMaps();
        CreatePollsMaps();
        CreateSecurityMaps();
        CreateSeoMaps();
        CreateSitesMaps();
        CreateTasksMaps();

        //add some generic mapping rules
        this.Internal().ForAllMaps((mapConfiguration, map) =>
        {
            //exclude Form and CustomProperties from mapping BaseModel
            if (typeof(BaseModel).IsAssignableFrom(mapConfiguration.DestinationType))
            {
                //map.ForMember(nameof(BaseModel.Form), options => options.Ignore());
                map.ForMember(nameof(BaseModel.CustomProperties), options => options.Ignore());
            }

            //exclude ActiveSiteScopeConfiguration from mapping ISettingsModel
            if (typeof(ISettingsModel).IsAssignableFrom(mapConfiguration.DestinationType))
                map.ForMember(nameof(ISettingsModel.ActiveSiteScopeConfiguration), options => options.Ignore());

            //exclude some properties from mapping configuration and models
            if (typeof(IConfig).IsAssignableFrom(mapConfiguration.DestinationType))
                map.ForMember(nameof(IConfig.Name), options => options.Ignore());

            //exclude Locales from mapping ILocalizedModel
            if (typeof(ILocalizedModel).IsAssignableFrom(mapConfiguration.DestinationType))
                map.ForMember(nameof(ILocalizedModel<ILocalizedModel>.Locales), options => options.Ignore());

            //exclude some properties from mapping site mapping supported entities and models
            if (typeof(ISiteMappingSupported).IsAssignableFrom(mapConfiguration.DestinationType))
                map.ForMember(nameof(ISiteMappingSupported.LimitedToSites), options => options.Ignore());
            if (typeof(ISiteMappingSupportedModel).IsAssignableFrom(mapConfiguration.DestinationType))
            {
                map.ForMember(nameof(ISiteMappingSupportedModel.AvailableSites), options => options.Ignore());
                map.ForMember(nameof(ISiteMappingSupportedModel.SelectedSiteIds), options => options.Ignore());
            }

            //exclude some properties from mapping ACL supported entities and models
            if (typeof(IAclSupported).IsAssignableFrom(mapConfiguration.DestinationType))
                map.ForMember(nameof(IAclSupported.SubjectToAcl), options => options.Ignore());
            if (typeof(IAclSupportedModel).IsAssignableFrom(mapConfiguration.DestinationType))
            {
                map.ForMember(nameof(IAclSupportedModel.AvailableCustomerRoles), options => options.Ignore());
                map.ForMember(nameof(IAclSupportedModel.SelectedCustomerRoleIds), options => options.Ignore());
            }

            if (typeof(IPluginModel).IsAssignableFrom(mapConfiguration.DestinationType))
            {
                //exclude some properties from mapping plugin models
                map.ForMember(nameof(IPluginModel.ConfigurationUrl), options => options.Ignore());
                map.ForMember(nameof(IPluginModel.IsActive), options => options.Ignore());
                map.ForMember(nameof(IPluginModel.LogoUrl), options => options.Ignore());

                //define specific rules for mapping plugin models
                if (typeof(IPlugin).IsAssignableFrom(mapConfiguration.SourceType))
                {
                    map.ForMember(nameof(IPluginModel.DisplayOrder), options => options.MapFrom(plugin => ((IPlugin)plugin).PluginDescriptor.DisplayOrder));
                    map.ForMember(nameof(IPluginModel.FriendlyName), options => options.MapFrom(plugin => ((IPlugin)plugin).PluginDescriptor.FriendlyName));
                    map.ForMember(nameof(IPluginModel.SystemName), options => options.MapFrom(plugin => ((IPlugin)plugin).PluginDescriptor.SystemName));
                }
            }
        });
    }

    #endregion

    #region Utilities

    /// <summary>
    /// Create configuration maps 
    /// </summary>
    protected virtual void CreateConfigMaps()
    {
        CreateMap<CacheConfig, CacheConfigModel>();
        CreateMap<CacheConfigModel, CacheConfig>();

        CreateMap<HostingConfig, HostingConfigModel>();
        CreateMap<HostingConfigModel, HostingConfig>();

        CreateMap<DistributedCacheConfig, DistributedCacheConfigModel>()
            .ForMember(model => model.DistributedCacheTypeValues, options => options.Ignore());
        CreateMap<DistributedCacheConfigModel, DistributedCacheConfig>();

        CreateMap<AzureBlobConfig, AzureBlobConfigModel>();
        CreateMap<AzureBlobConfigModel, AzureBlobConfig>()
            .ForMember(entity => entity.Enabled, options => options.Ignore())
            .ForMember(entity => entity.DataProtectionKeysEncryptWithVault, options => options.Ignore());

        CreateMap<InstallationConfig, InstallationConfigModel>();
        CreateMap<InstallationConfigModel, InstallationConfig>();

        CreateMap<PluginConfig, PluginConfigModel>();
        CreateMap<PluginConfigModel, PluginConfig>();

        CreateMap<CommonConfig, CommonConfigModel>();
        CreateMap<CommonConfigModel, CommonConfig>();

        CreateMap<DataConfig, DataConfigModel>()
            .ForMember(model => model.DataProviderTypeValues, options => options.Ignore());
        CreateMap<DataConfigModel, DataConfig>();

        CreateMap<WebOptimizerConfig, WebOptimizerConfigModel>();
        CreateMap<WebOptimizerConfigModel, WebOptimizerConfig>()
            .ForMember(entity => entity.CdnUrl, options => options.Ignore())
            .ForMember(entity => entity.AllowEmptyBundle, options => options.Ignore())
            .ForMember(entity => entity.HttpsCompression, options => options.Ignore())
            .ForMember(entity => entity.EnableTagHelperBundling, options => options.Ignore())
            .ForMember(entity => entity.EnableCaching, options => options.Ignore())
            .ForMember(entity => entity.EnableMemoryCache, options => options.Ignore());
    }

    /// <summary>
    /// Create multi-factor authentication maps 
    /// </summary>
    protected virtual void CreateMultiFactorAuthenticationMaps()
    {
        CreateMap<IMultiFactorAuthenticationMethod, MultiFactorAuthenticationMethodModel>();
    }

    /// <summary>
    /// Create blogs maps 
    /// </summary>new
    protected virtual void CreateBlogsMaps()
    {
        //CreateMap<BlogComment, BlogCommentModel>()
        //    .ForMember(model => model.BlogPostTitle, options => options.Ignore())
        //    .ForMember(model => model.Comment, options => options.Ignore())
        //    .ForMember(model => model.CreatedOn, options => options.Ignore())
        //    .ForMember(model => model.CustomerInfo, options => options.Ignore())
        //    .ForMember(model => model.SiteName, options => options.Ignore());

        //CreateMap<BlogCommentModel, BlogComment>()
        //    .ForMember(entity => entity.CommentText, options => options.Ignore())
        //    .ForMember(entity => entity.CreatedOnUtc, options => options.Ignore())
        //    .ForMember(entity => entity.BlogPostId, options => options.Ignore())
        //    .ForMember(entity => entity.CustomerId, options => options.Ignore())
        //    .ForMember(entity => entity.SiteId, options => options.Ignore());

        //CreateMap<BlogPost, BlogPostModel>()
        //    .ForMember(model => model.ApprovedComments, options => options.Ignore())
        //    .ForMember(model => model.AvailableLanguages, options => options.Ignore())
        //    .ForMember(model => model.CreatedOn, options => options.Ignore())
        //    .ForMember(model => model.LanguageName, options => options.Ignore())
        //    .ForMember(model => model.NotApprovedComments, options => options.Ignore())
        //    .ForMember(model => model.SeName, options => options.Ignore())
        //    .ForMember(model => model.InitialBlogTags, options => options.Ignore());
        //CreateMap<BlogPostModel, BlogPost>()
        //    .ForMember(entity => entity.CreatedOnUtc, options => options.Ignore());

        //CreateMap<BlogSettings, BlogSettingsModel>()
        //    .ForMember(model => model.AllowNotRegisteredUsersToLeaveComments_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.BlogCommentsMustBeApproved_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.Enabled_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.NotifyAboutNewBlogComments_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.NumberOfTags_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.PostsPageSize_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.ShowHeaderRssUrl_OverrideForSite, options => options.Ignore());
        //CreateMap<BlogSettingsModel, BlogSettings>();
    }

    /// <summary>
    /// Create common maps 
    /// </summary>
    protected virtual void CreateCommonMaps()
    {
        CreateMap<Address, AddressModel>()
            .ForMember(model => model.AddressHtml, options => options.Ignore())
            .ForMember(model => model.AvailableCountries, options => options.Ignore())
            .ForMember(model => model.AvailableStates, options => options.Ignore())
            .ForMember(model => model.CountryName, options => options.Ignore())
            .ForMember(model => model.CustomAddressAttributes, options => options.Ignore())
            .ForMember(model => model.FormattedCustomAddressAttributes, options => options.Ignore())
            .ForMember(model => model.StateProvinceName, options => options.Ignore())
            .ForMember(model => model.CityRequired, options => options.Ignore())
            .ForMember(model => model.CompanyRequired, options => options.Ignore())
            .ForMember(model => model.CountryRequired, options => options.Ignore())
            .ForMember(model => model.CountyRequired, options => options.Ignore())
            .ForMember(model => model.EmailRequired, options => options.Ignore())
            .ForMember(model => model.FaxRequired, options => options.Ignore())
            .ForMember(model => model.FirstNameRequired, options => options.Ignore())
            .ForMember(model => model.LastNameRequired, options => options.Ignore())
            .ForMember(model => model.PhoneRequired, options => options.Ignore())
            .ForMember(model => model.StateProvinceName, options => options.Ignore())
            .ForMember(model => model.StreetAddress2Required, options => options.Ignore())
            .ForMember(model => model.StreetAddressRequired, options => options.Ignore())
            .ForMember(model => model.ZipPostalCodeRequired, options => options.Ignore());
        CreateMap<AddressModel, Address>()
            .ForMember(entity => entity.CreatedOnUtc, options => options.Ignore())
            .ForMember(entity => entity.CustomAttributes, options => options.Ignore());

        CreateMap<AddressAttribute, AddressAttributeModel>()
            .ForMember(model => model.AddressAttributeValueSearchModel, options => options.Ignore())
            .ForMember(model => model.AttributeControlTypeName, options => options.Ignore());
        CreateMap<AddressAttributeModel, AddressAttribute>()
            .ForMember(entity => entity.AttributeControlType, options => options.Ignore());

        CreateMap<AddressAttributeValue, AddressAttributeValueModel>();
        CreateMap<AddressAttributeValueModel, AddressAttributeValue>();

        CreateMap<AddressSettings, AddressSettingsModel>()
            .ForMember(model => model.AvailableCountries, options => options.Ignore());
        CreateMap<AddressSettingsModel, AddressSettings>()
            .ForMember(settings => settings.PreselectCountryIfOnlyOne, options => options.Ignore());

        CreateMap<Setting, SettingModel>()
            .ForMember(setting => setting.AvailableSites, options => options.Ignore())
            .ForMember(setting => setting.Site, options => options.Ignore());
    }

    /// <summary>
    /// Create customers maps 
    /// </summary>
    protected virtual void CreateCustomersMaps()
    {
        CreateMap<CustomerAttribute, CustomerAttributeModel>()
            .ForMember(model => model.AttributeControlTypeName, options => options.Ignore())
            .ForMember(model => model.CustomerAttributeValueSearchModel, options => options.Ignore());
        CreateMap<CustomerAttributeModel, CustomerAttribute>()
            .ForMember(entity => entity.AttributeControlType, options => options.Ignore());

        CreateMap<CustomerAttributeValue, CustomerAttributeValueModel>();
        CreateMap<CustomerAttributeValueModel, CustomerAttributeValue>();

        CreateMap<CustomerRole, CustomerRoleModel>();
        CreateMap<CustomerRoleModel, CustomerRole>();

        CreateMap<CustomerSettings, CustomerSettingsModel>()
            .ForMember(model => model.AvailableCountries, options => options.Ignore());
        CreateMap<CustomerSettingsModel, CustomerSettings>()
            .ForMember(settings => settings.AvatarMaximumSizeBytes, options => options.Ignore())
            .ForMember(settings => settings.DeleteGuestTaskOlderThanMinutes, options => options.Ignore())
            .ForMember(settings => settings.DownloadableProductsValidateUser, options => options.Ignore())
            .ForMember(settings => settings.HashedPasswordFormat, options => options.Ignore())
            .ForMember(settings => settings.OnlineCustomerMinutes, options => options.Ignore())
            .ForMember(settings => settings.SuffixDeletedCustomers, options => options.Ignore())
            .ForMember(settings => settings.LastActivityMinutes, options => options.Ignore())
            .ForMember(settings => settings.RequiredReLoginAfterPasswordChange, options => options.Ignore());

        CreateMap<MultiFactorAuthenticationSettings, MultiFactorAuthenticationSettingsModel>();
        CreateMap<MultiFactorAuthenticationSettingsModel, MultiFactorAuthenticationSettings>()
            .ForMember(settings => settings.ActiveAuthenticationMethodSystemNames, option => option.Ignore());

        CreateMap<ActivityLog, CustomerActivityLogModel>()
            .ForMember(model => model.CreatedOn, options => options.Ignore())
            .ForMember(model => model.ActivityLogTypeName, options => options.Ignore());

        CreateMap<Customer, CustomerModel>()
            .ForMember(model => model.Email, options => options.Ignore())
            .ForMember(model => model.FullName, options => options.Ignore())
            .ForMember(model => model.Company, options => options.Ignore())
            .ForMember(model => model.Phone, options => options.Ignore())
            .ForMember(model => model.ZipPostalCode, options => options.Ignore())
            .ForMember(model => model.CreatedOn, options => options.Ignore())
            .ForMember(model => model.LastActivityDate, options => options.Ignore())
            .ForMember(model => model.CustomerRoleNames, options => options.Ignore())
            .ForMember(model => model.AvatarUrl, options => options.Ignore())
            .ForMember(model => model.UsernamesEnabled, options => options.Ignore())
            .ForMember(model => model.Password, options => options.Ignore())
            .ForMember(model => model.GenderEnabled, options => options.Ignore())
            .ForMember(model => model.NeutralGenderEnabled, options => options.Ignore())
            .ForMember(model => model.Gender, options => options.Ignore())
            .ForMember(model => model.FirstNameEnabled, options => options.Ignore())
            .ForMember(model => model.FirstName, options => options.Ignore())
            .ForMember(model => model.LastNameEnabled, options => options.Ignore())
            .ForMember(model => model.LastName, options => options.Ignore())
            .ForMember(model => model.DateOfBirthEnabled, options => options.Ignore())
            .ForMember(model => model.DateOfBirth, options => options.Ignore())
            .ForMember(model => model.CompanyEnabled, options => options.Ignore())
            .ForMember(model => model.StreetAddressEnabled, options => options.Ignore())
            .ForMember(model => model.StreetAddress, options => options.Ignore())
            .ForMember(model => model.StreetAddress2Enabled, options => options.Ignore())
            .ForMember(model => model.StreetAddress2, options => options.Ignore())
            .ForMember(model => model.ZipPostalCodeEnabled, options => options.Ignore())
            .ForMember(model => model.CityEnabled, options => options.Ignore())
            .ForMember(model => model.City, options => options.Ignore())
            .ForMember(model => model.CountyEnabled, options => options.Ignore())
            .ForMember(model => model.County, options => options.Ignore())
            .ForMember(model => model.CountryEnabled, options => options.Ignore())
            .ForMember(model => model.CountryId, options => options.Ignore())
            .ForMember(model => model.AvailableCountries, options => options.Ignore())
            .ForMember(model => model.StateProvinceEnabled, options => options.Ignore())
            .ForMember(model => model.StateProvinceId, options => options.Ignore())
            .ForMember(model => model.AvailableStates, options => options.Ignore())
            .ForMember(model => model.PhoneEnabled, options => options.Ignore())
            .ForMember(model => model.FaxEnabled, options => options.Ignore())
            .ForMember(model => model.Fax, options => options.Ignore())
            .ForMember(model => model.CustomerAttributes, options => options.Ignore())
            .ForMember(model => model.RegisteredInSite, options => options.Ignore())
            .ForMember(model => model.DisplayRegisteredInSite, options => options.Ignore())
            .ForMember(model => model.TimeZoneId, options => options.Ignore())
            .ForMember(model => model.AllowCustomersToSetTimeZone, options => options.Ignore())
            .ForMember(model => model.AvailableTimeZones, options => options.Ignore())
            .ForMember(model => model.LastVisitedPage, options => options.Ignore())
            .ForMember(model => model.AvailableNewsletterSubscriptionSites, options => options.Ignore())
            .ForMember(model => model.SelectedNewsletterSubscriptionSiteIds, options => options.Ignore())
            .ForMember(model => model.SendEmail, options => options.Ignore())
            .ForMember(model => model.SendPm, options => options.Ignore())
            .ForMember(model => model.AllowSendingOfPrivateMessage, options => options.Ignore())
            .ForMember(model => model.AllowSendingOfWelcomeMessage, options => options.Ignore())
            .ForMember(model => model.AllowReSendingOfActivationMessage, options => options.Ignore())
            .ForMember(model => model.GdprEnabled, options => options.Ignore())
            .ForMember(model => model.MultiFactorAuthenticationProvider, options => options.Ignore())
            .ForMember(model => model.CustomerActivityLogSearchModel, options => options.Ignore());

        CreateMap<CustomerModel, Customer>()
            .ForMember(entity => entity.CustomerGuid, options => options.Ignore())
            .ForMember(entity => entity.CreatedOnUtc, options => options.Ignore())
            .ForMember(entity => entity.LastActivityDateUtc, options => options.Ignore())
            .ForMember(entity => entity.EmailToRevalidate, options => options.Ignore())
            .ForMember(entity => entity.RequireReLogin, options => options.Ignore())
            .ForMember(entity => entity.FailedLoginAttempts, options => options.Ignore())
            .ForMember(entity => entity.CannotLoginUntilDateUtc, options => options.Ignore())
            .ForMember(entity => entity.Deleted, options => options.Ignore())
            .ForMember(entity => entity.IsSystemAccount, options => options.Ignore())
            .ForMember(entity => entity.SystemName, options => options.Ignore())
            .ForMember(entity => entity.LastLoginDateUtc, options => options.Ignore())
            .ForMember(entity => entity.CustomCustomerAttributesXML, options => options.Ignore())
            .ForMember(entity => entity.CurrencyId, options => options.Ignore())
            .ForMember(entity => entity.LanguageId, options => options.Ignore())
            .ForMember(entity => entity.RegisteredInSiteId, options => options.Ignore());

        CreateMap<Customer, OnlineCustomerModel>()
            .ForMember(model => model.LastActivityDate, options => options.Ignore())
            .ForMember(model => model.CustomerInfo, options => options.Ignore())
            .ForMember(model => model.LastIpAddress, options => options.Ignore())
            .ForMember(model => model.Location, options => options.Ignore())
            .ForMember(model => model.LastVisitedPage, options => options.Ignore());
    }

    /// <summary>
    /// Create directory maps 
    /// </summary>
    protected virtual void CreateDirectoryMaps()
    {
        CreateMap<Country, CountryModel>()
            .ForMember(model => model.NumberOfStates, options => options.Ignore())
            .ForMember(model => model.StateProvinceSearchModel, options => options.Ignore());
        CreateMap<CountryModel, Country>();

        CreateMap<StateProvince, StateProvinceModel>();
        CreateMap<StateProvinceModel, StateProvince>();
    }

    /// <summary>
    /// Create forums maps 
    /// </summary>
    protected virtual void CreateForumsMaps()
    {
        //CreateMap<Forum, ForumModel>()
        //    .ForMember(model => model.CreatedOn, options => options.Ignore())
        //    .ForMember(model => model.ForumGroups, options => options.Ignore());
        //CreateMap<ForumModel, Forum>()
        //    .ForMember(entity => entity.CreatedOnUtc, options => options.Ignore())
        //    .ForMember(entity => entity.LastPostCustomerId, options => options.Ignore())
        //    .ForMember(entity => entity.LastPostId, options => options.Ignore())
        //    .ForMember(entity => entity.LastPostTime, options => options.Ignore())
        //    .ForMember(entity => entity.LastPageId, options => options.Ignore())
        //    .ForMember(entity => entity.NumPosts, options => options.Ignore())
        //    .ForMember(entity => entity.NumPages, options => options.Ignore())
        //    .ForMember(entity => entity.UpdatedOnUtc, options => options.Ignore());

        //CreateMap<ForumGroup, ForumGroupModel>()
        //    .ForMember(model => model.CreatedOn, options => options.Ignore());
        //CreateMap<ForumGroupModel, ForumGroup>()
        //    .ForMember(entity => entity.CreatedOnUtc, options => options.Ignore())
        //    .ForMember(entity => entity.UpdatedOnUtc, options => options.Ignore());

        //CreateMap<ForumSettings, ForumSettingsModel>()
        //    .ForMember(model => model.ActiveDiscussionsFeedCount_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.ActiveDiscussionsFeedEnabled_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.ActiveDiscussionsPageSize_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.AllowCustomersToDeletePosts_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.AllowCustomersToEditPosts_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.AllowCustomersToManageSubscriptions_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.AllowGuestsToCreatePosts_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.AllowGuestsToCreatePages_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.AllowPostVoting_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.AllowPrivateMessages_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.ForumEditorValues, options => options.Ignore())
        //    .ForMember(model => model.ForumEditor_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.ForumFeedCount_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.ForumFeedsEnabled_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.ForumsEnabled_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.MaxVotesPerDay_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.NotifyAboutPrivateMessages_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.PostsPageSize_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.RelativeDateTimeFormattingEnabled_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.SearchResultsPageSize_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.ShowAlertForPM_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.ShowCustomersPostCount_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.SignaturesEnabled_OverrideForSite, options => options.Ignore())
        //    .ForMember(model => model.PagesPageSize_OverrideForSite, options => options.Ignore());
        //CreateMap<ForumSettingsModel, ForumSettings>()
        //    .ForMember(settings => settings.ForumSearchTermMinimumLength, options => options.Ignore())
        //    .ForMember(settings => settings.ForumSubscriptionsPageSize, options => options.Ignore())
        //    .ForMember(settings => settings.HomepageActiveDiscussionsPageCount, options => options.Ignore())
        //    .ForMember(settings => settings.LatestCustomerPostsPageSize, options => options.Ignore())
        //    .ForMember(settings => settings.PMSubjectMaxLength, options => options.Ignore())
        //    .ForMember(settings => settings.PMTextMaxLength, options => options.Ignore())
        //    .ForMember(settings => settings.PostMaxLength, options => options.Ignore())
        //    .ForMember(settings => settings.PrivateMessagesPageSize, options => options.Ignore())
        //    .ForMember(settings => settings.StrippedPageMaxLength, options => options.Ignore())
        //    .ForMember(settings => settings.PageSubjectMaxLength, options => options.Ignore());
    }

    /// <summary>
    /// Create localization maps 
    /// </summary>
    protected virtual void CreateLocalizationMaps()
    {
        CreateMap<Language, LanguageModel>()
            //.ForMember(model => model.AvailableCurrencies, options => options.Ignore())
            .ForMember(model => model.LocaleResourceSearchModel, options => options.Ignore())
            .ForMember(model => model.AvailableFlagImages, options => options.Ignore());
        CreateMap<LanguageModel, Language>();

        CreateMap<LocaleResourceModel, LocaleStringResource>()
            .ForMember(entity => entity.LanguageId, options => options.Ignore());
    }

    /// <summary>
    /// Create logging maps 
    /// </summary>
    protected virtual void CreateLoggingMaps()
    {
        CreateMap<ActivityLog, ActivityLogModel>()
            .ForMember(model => model.ActivityLogTypeName, options => options.Ignore())
            .ForMember(model => model.CreatedOn, options => options.Ignore())
            .ForMember(model => model.CustomerEmail, options => options.Ignore());
        CreateMap<ActivityLogModel, ActivityLog>()
            .ForMember(entity => entity.ActivityLogTypeId, options => options.Ignore())
            .ForMember(entity => entity.CreatedOnUtc, options => options.Ignore())
            .ForMember(entity => entity.EntityId, options => options.Ignore())
            .ForMember(entity => entity.EntityName, options => options.Ignore());

        CreateMap<ActivityLogType, ActivityLogTypeModel>();
        CreateMap<ActivityLogTypeModel, ActivityLogType>()
            .ForMember(entity => entity.SystemKeyword, options => options.Ignore());

        CreateMap<Log, LogModel>()
            .ForMember(model => model.CreatedOn, options => options.Ignore())
            .ForMember(model => model.FullMessage, options => options.Ignore())
            .ForMember(model => model.CustomerEmail, options => options.Ignore());
        CreateMap<LogModel, Log>()
            .ForMember(entity => entity.CreatedOnUtc, options => options.Ignore())
            .ForMember(entity => entity.LogLevelId, options => options.Ignore());
    }

    /// <summary>
    /// Create media maps 
    /// </summary>
    protected virtual void CreateMediaMaps()
    {
        CreateMap<MediaSettings, MediaSettingsModel>()
            .ForMember(model => model.AvatarPictureSize_OverrideForSite, options => options.Ignore())
            //.ForMember(model => model.CatelogThumbPictureSize_OverrideForSite, options => options.Ignore())
            .ForMember(model => model.DefaultImageQuality_OverrideForSite, options => options.Ignore())
            .ForMember(model => model.DefaultPictureZoomEnabled_OverrideForSite, options => options.Ignore())
            //.ForMember(model => model.ImportProductImagesUsingHash_OverrideForSite, options => options.Ignore())
            .ForMember(model => model.MaximumImageSize_OverrideForSite, options => options.Ignore())
            .ForMember(model => model.MultipleThumbDirectories_OverrideForSite, options => options.Ignore())
            .ForMember(model => model.PicturesSitedIntoDatabase, options => options.Ignore())
            .ForMember(model => model.AllowSVGUploads_OverrideForSite, options => options.Ignore());
        CreateMap<MediaSettingsModel, MediaSettings>()
            .ForMember(settings => settings.AutoCompleteSearchThumbPictureSize, options => options.Ignore())
            .ForMember(settings => settings.AzureCacheControlHeader, options => options.Ignore())
            .ForMember(settings => settings.UseAbsoluteImagePath, options => options.Ignore())
            .ForMember(settings => settings.ImageSquarePictureSize, options => options.Ignore())
            .ForMember(settings => settings.VideoIframeAllow, options => options.Ignore())
            .ForMember(settings => settings.VideoIframeHeight, options => options.Ignore())
            .ForMember(settings => settings.VideoIframeWidth, options => options.Ignore());
    }
    protected virtual void CreatePluginsMaps()
    {
        CreateMap<PluginDescriptor, PluginModel>()
            .ForMember(model => model.CanChangeEnabled, options => options.Ignore())
            .ForMember(model => model.IsEnabled, options => options.Ignore());
    }

    /// <summary>
    /// Create polls maps 
    /// </summary>
    protected virtual void CreatePollsMaps()
    {
        //CreateMap<PollAnswer, PollAnswerModel>();
        //CreateMap<PollAnswerModel, PollAnswer>();

        //CreateMap<Poll, PollModel>()
        //    .ForMember(model => model.AvailableLanguages, options => options.Ignore())
        //    .ForMember(model => model.PollAnswerSearchModel, options => options.Ignore())
        //    .ForMember(model => model.LanguageName, options => options.Ignore());
        //CreateMap<PollModel, Poll>();
    }

    /// <summary>
    /// Create security maps 
    /// </summary>
    protected virtual void CreateSecurityMaps()
    {
        CreateMap<CaptchaSettings, CaptchaSettingsModel>()
            .ForMember(model => model.Enabled_OverrideForSite, options => options.Ignore())
            .ForMember(model => model.ReCaptchaPrivateKey_OverrideForSite, options => options.Ignore())
            .ForMember(model => model.ReCaptchaPublicKey_OverrideForSite, options => options.Ignore())
            .ForMember(model => model.ShowOnBlogCommentPage_OverrideForSite, options => options.Ignore())
            .ForMember(model => model.ShowOnContactUsPage_OverrideForSite, options => options.Ignore())
            .ForMember(model => model.ShowOnLoginPage_OverrideForSite, options => options.Ignore())
            .ForMember(model => model.ShowOnNewsCommentPage_OverrideForSite, options => options.Ignore())
            .ForMember(model => model.ShowOnNewsletterPage_OverrideForSite, options => options.Ignore())
            .ForMember(model => model.ShowOnRegistrationPage_OverrideForSite, options => options.Ignore())
            .ForMember(model => model.ShowOnForgotPasswordPage_OverrideForSite, options => options.Ignore())
            .ForMember(model => model.ShowOnForum_OverrideForSite, options => options.Ignore())
            .ForMember(model => model.CaptchaType_OverrideForSite, options => options.Ignore())
            .ForMember(model => model.ReCaptchaV3ScoreThreshold_OverrideForSite, options => options.Ignore())
            .ForMember(model => model.CaptchaTypeValues, options => options.Ignore());
        CreateMap<CaptchaSettingsModel, CaptchaSettings>()
            .ForMember(settings => settings.AutomaticallyChooseLanguage, options => options.Ignore())
            .ForMember(settings => settings.ReCaptchaDefaultLanguage, options => options.Ignore())
            .ForMember(settings => settings.ReCaptchaRequestTimeout, options => options.Ignore())
            .ForMember(settings => settings.ReCaptchaTheme, options => options.Ignore())
            .ForMember(settings => settings.ReCaptchaApiUrl, options => options.Ignore());
    }

    /// <summary>
    /// Create SEO maps 
    /// </summary>
    protected virtual void CreateSeoMaps()
    {
        CreateMap<UrlRecord, UrlRecordModel>()
            .ForMember(model => model.DetailsUrl, options => options.Ignore())
            .ForMember(model => model.Language, options => options.Ignore())
            .ForMember(model => model.Name, options => options.Ignore());
        CreateMap<UrlRecordModel, UrlRecord>()
            .ForMember(entity => entity.LanguageId, options => options.Ignore())
            .ForMember(entity => entity.Slug, options => options.Ignore());
    }

    /// <summary>
    /// Create sites maps 
    /// </summary>
    protected virtual void CreateSitesMaps()
    {
        CreateMap<Site, SiteModel>()
            .ForMember(model => model.AvailableLanguages, options => options.Ignore());
        CreateMap<SiteModel, Site>()
            .ForMember(entity => entity.SslEnabled, options => options.Ignore())
            .ForMember(entity => entity.Deleted, options => options.Ignore());
    }

    /// <summary>
    /// Create tasks maps 
    /// </summary>
    protected virtual void CreateTasksMaps()
    {
        CreateMap<ScheduleTask, ScheduleTaskModel>();
        CreateMap<ScheduleTaskModel, ScheduleTask>()
            .ForMember(entity => entity.Type, options => options.Ignore())
            .ForMember(entity => entity.LastStartUtc, options => options.Ignore())
            .ForMember(entity => entity.LastEndUtc, options => options.Ignore())
            .ForMember(entity => entity.LastSuccessUtc, options => options.Ignore())
            .ForMember(entity => entity.LastEnabledUtc, options => options.Ignore());
    }

    /// <summary>
    /// Create pages maps 
    /// </summary>
    //protected virtual void CreatePagesMaps()
    //{
    //    CreateMap<Page, PageModel>()
    //        .ForMember(model => model.SeName, options => options.Ignore())
    //        .ForMember(model => model.PageTypeStr, options => options.Ignore())
    //        .ForMember(model => model.ContentTypeStr, options => options.Ignore())
    //        .ForMember(model => model.TargetWindowTypeStr, options => options.Ignore())
    //        .ForMember(model => model.PageName, options => options.Ignore())
    //        .ForMember(model => model.Url, options => options.Ignore());
    //    CreateMap<PageModel, Page>()
    //        .ForMember(entity => entity.PageType, options => options.Ignore())
    //        .ForMember(entity => entity.TargetWindowType, options => options.Ignore());

    //    CreateMap<PageTemplate, PageTemplateModel>();
    //    CreateMap<PageTemplateModel, PageTemplate>();
    //}

    #endregion

    #region Properties

    /// <summary>
    /// Order of this mapper implementation
    /// </summary>
    public int Order => 0;

    #endregion
}