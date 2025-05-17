using AssetForge.App.Models.Customer;
using AssetForge.Core;
using AssetForge.Core.Domain.Common;
using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Media;
using AssetForge.Core.Domain.Security;
using AssetForge.Services.Attributes;

using AssetForge.Services.Authentication.MultiFactor;
using AssetForge.Services.Common;
using AssetForge.Services.Customers;
using AssetForge.Services.Directory;
using AssetForge.Services.Helpers;
using AssetForge.Services.Localization;
using AssetForge.Services.Media;
using AssetForge.Services.Security;
using AssetForge.Services.Seo;
using AssetForge.Services.Sites;

namespace AssetForge.App.Factories
{
    public class CustomerModelFactory : ICustomerModelFactory
    {
        #region Fields

        protected readonly AddressSettings _addressSettings;
        protected readonly CaptchaSettings _captchaSettings;
        protected readonly CommonSettings _commonSettings;
        protected readonly CustomerSettings _customerSettings;
        protected readonly DateTimeSettings _dateTimeSettings;
        protected readonly IAttributeParser<CustomerAttribute, CustomerAttributeValue> _customerAttributeParser;
        protected readonly IAttributeService<CustomerAttribute, CustomerAttributeValue> _customerAttributeService;
        protected readonly ICountryService _countryService;
        protected readonly ICustomerService _customerService;
        protected readonly IDateTimeHelper _dateTimeHelper;
        protected readonly IGenericAttributeService _genericAttributeService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IMultiFactorAuthenticationPluginManager _multiFactorAuthenticationPluginManager;
        protected readonly IPermissionService _permissionService;
        protected readonly IPictureService _pictureService;
        protected readonly IStateProvinceService _stateProvinceService;
        protected readonly ISiteContext _siteContext;
        protected readonly ISiteMappingService _siteMappingService;
        protected readonly IUrlRecordService _urlRecordService;
        protected readonly IWorkContext _workContext;
        protected readonly MediaSettings _mediaSettings;
        protected readonly SecuritySettings _securitySettings;

        #endregion

        #region Ctor

        public CustomerModelFactory(AddressSettings addressSettings,
            CaptchaSettings captchaSettings,
            CommonSettings commonSettings,
            CustomerSettings customerSettings,
            DateTimeSettings dateTimeSettings,
            IAttributeParser<CustomerAttribute, CustomerAttributeValue> customerAttributeParser,
            IAttributeService<CustomerAttribute, CustomerAttributeValue> customerAttributeService,
            ICountryService countryService,
            ICustomerService customerService,
            IDateTimeHelper dateTimeHelper,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            IMultiFactorAuthenticationPluginManager multiFactorAuthenticationPluginManager,
            IPermissionService permissionService,
            IPictureService pictureService,
            IStateProvinceService stateProvinceService,
            ISiteContext siteContext,
            ISiteMappingService siteMappingService,
            IUrlRecordService urlRecordService,
            IWorkContext workContext,
            MediaSettings mediaSettings,
            SecuritySettings securitySettings
            )
        {
            _addressSettings = addressSettings;
            _captchaSettings = captchaSettings;
            _commonSettings = commonSettings;
            _customerSettings = customerSettings;
            _dateTimeSettings = dateTimeSettings;
            _customerAttributeParser = customerAttributeParser;
            _customerAttributeService = customerAttributeService;
            _countryService = countryService;
            _customerService = customerService;
            _dateTimeHelper = dateTimeHelper;
            _genericAttributeService = genericAttributeService;
            _localizationService = localizationService;
            _multiFactorAuthenticationPluginManager = multiFactorAuthenticationPluginManager;
            _permissionService = permissionService;
            _pictureService = pictureService;
            _stateProvinceService = stateProvinceService;
            _siteContext = siteContext;
            _siteMappingService = siteMappingService;
            _urlRecordService = urlRecordService;
            _workContext = workContext;
            _mediaSettings = mediaSettings;
            _securitySettings = securitySettings;
        }

        #endregion

        #region Utilities


        #endregion

        public virtual Task<LoginModel> PrepareLoginModelAsync(bool? checkoutAsGuest)
        {
            var model = new LoginModel
            {
                UsernamesEnabled = _customerSettings.UsernamesEnabled,
                RegistrationType = _customerSettings.UserRegistrationType,
                CheckoutAsGuest = checkoutAsGuest.GetValueOrDefault(),
                DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnLoginPage
            };

            return Task.FromResult(model);
        }

        public virtual async Task<MultiFactorAuthenticationProviderModel> PrepareMultiFactorAuthenticationProviderModelAsync(MultiFactorAuthenticationProviderModel providerModel, string sysName, bool isLogin = false)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            var selectedProvider = await _genericAttributeService.GetAttributeAsync<string>(customer, CustomerDefaults.SelectedMultiFactorAuthenticationProviderAttribute);
            var site = await _siteContext.GetCurrentSiteAsync();

            var multiFactorAuthenticationProvider = (await _multiFactorAuthenticationPluginManager.LoadActivePluginsAsync(customer, site.Id))
                .FirstOrDefault(provider => provider.PluginDescriptor.SystemName == sysName);

            if (multiFactorAuthenticationProvider != null)
            {
                providerModel.Name = await _localizationService.GetLocalizedFriendlyNameAsync(multiFactorAuthenticationProvider, (await _workContext.GetWorkingLanguageAsync()).Id);
                providerModel.SystemName = sysName;
                providerModel.Description = await multiFactorAuthenticationProvider.GetDescriptionAsync();
                providerModel.LogoUrl = await _multiFactorAuthenticationPluginManager.GetPluginLogoUrlAsync(multiFactorAuthenticationProvider);
                providerModel.ViewComponent = isLogin ? multiFactorAuthenticationProvider.GetVerificationViewComponent() : multiFactorAuthenticationProvider.GetPublicViewComponent();
                providerModel.Selected = sysName == selectedProvider;
            }

            return providerModel;
        }
    }
}
