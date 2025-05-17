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
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

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


        public virtual async Task<CustomerInfoModel> PrepareCustomerInfoModelAsync(CustomerInfoModel model, Customer customer,
            bool excludeProperties, string overrideCustomCustomerAttributesXml = "")
        {
            ArgumentNullException.ThrowIfNull(model);

            ArgumentNullException.ThrowIfNull(customer);

            model.AllowCustomersToSetTimeZone = _dateTimeSettings.AllowCustomersToSetTimeZone;
            foreach (var tzi in _dateTimeHelper.GetSystemTimeZones())
                model.AvailableTimeZones.Add(new SelectListItem { Text = tzi.DisplayName, Value = tzi.Id, Selected = (excludeProperties ? tzi.Id == model.TimeZoneId : tzi.Id == (await _dateTimeHelper.GetCurrentTimeZoneAsync()).Id) });

            var site = await _siteContext.GetCurrentSiteAsync();
            if (!excludeProperties)
            {
                model.FirstName = customer.FirstName;
                model.LastName = customer.LastName;
                model.Gender = customer.Gender;
                var dateOfBirth = customer.DateOfBirth;
                if (dateOfBirth.HasValue)
                {
                    var currentCalendar = CultureInfo.CurrentCulture.Calendar;

                    model.DateOfBirthDay = currentCalendar.GetDayOfMonth(dateOfBirth.Value);
                    model.DateOfBirthMonth = currentCalendar.GetMonth(dateOfBirth.Value);
                    model.DateOfBirthYear = currentCalendar.GetYear(dateOfBirth.Value);
                }
                model.Company = customer.Company;
                model.StreetAddress = customer.StreetAddress;
                model.StreetAddress2 = customer.StreetAddress2;
                model.ZipPostalCode = customer.ZipPostalCode;
                model.City = customer.City;
                model.County = customer.County;
                model.CountryId = customer.CountryId;
                model.StateProvinceId = customer.StateProvinceId;
                model.Phone = customer.Phone;
                model.Fax = customer.Fax;


                model.Signature = await _genericAttributeService.GetAttributeAsync<string>(customer, CustomerDefaults.SignatureAttribute);

                model.Email = customer.Email;
                model.Username = customer.Username;
            }
            else
            {
                if (_customerSettings.UsernamesEnabled && !_customerSettings.AllowUsersToChangeUsernames)
                    model.Username = customer.Username;
            }

            var currentLanguage = await _workContext.GetWorkingLanguageAsync();
            //countries and states
            if (_customerSettings.CountryEnabled)
            {
                if (model.CountryId == 0)
                    model.CountryId = _customerSettings.DefaultCountryId ?? 0;

                model.AvailableCountries.Add(new SelectListItem { Text = await _localizationService.GetResourceAsync("Address.SelectCountry"), Value = "0" });
                foreach (var c in await _countryService.GetAllCountriesAsync(currentLanguage.Id))
                {
                    model.AvailableCountries.Add(new SelectListItem
                    {
                        Text = await _localizationService.GetLocalizedAsync(c, x => x.Name),
                        Value = c.Id.ToString(),
                        Selected = c.Id == model.CountryId
                    });
                }

                if (_customerSettings.StateProvinceEnabled)
                {
                    //states
                    var states = (await _stateProvinceService.GetStateProvincesByCountryIdAsync(model.CountryId, currentLanguage.Id)).ToList();
                    if (states.Any())
                    {
                        model.AvailableStates.Add(new SelectListItem { Text = await _localizationService.GetResourceAsync("Address.SelectState"), Value = "0" });

                        foreach (var s in states)
                        {
                            model.AvailableStates.Add(new SelectListItem { Text = await _localizationService.GetLocalizedAsync(s, x => x.Name), Value = s.Id.ToString(), Selected = (s.Id == model.StateProvinceId) });
                        }
                    }
                    else
                    {
                        var anyCountrySelected = model.AvailableCountries.Any(x => x.Selected);

                        model.AvailableStates.Add(new SelectListItem
                        {
                            Text = await _localizationService.GetResourceAsync(anyCountrySelected ? "Address.Other" : "Address.SelectState"),
                            Value = "0"
                        });
                    }

                }
            }

            model.FirstNameEnabled = _customerSettings.FirstNameEnabled;
            model.LastNameEnabled = _customerSettings.LastNameEnabled;
            model.FirstNameRequired = _customerSettings.FirstNameRequired;
            model.LastNameRequired = _customerSettings.LastNameRequired;
            model.GenderEnabled = _customerSettings.GenderEnabled;
            model.NeutralGenderEnabled = _customerSettings.NeutralGenderEnabled;
            model.DateOfBirthEnabled = _customerSettings.DateOfBirthEnabled;
            model.DateOfBirthRequired = _customerSettings.DateOfBirthRequired;
            model.CompanyEnabled = _customerSettings.CompanyEnabled;
            model.CompanyRequired = _customerSettings.CompanyRequired;
            model.StreetAddressEnabled = _customerSettings.StreetAddressEnabled;
            model.StreetAddressRequired = _customerSettings.StreetAddressRequired;
            model.StreetAddress2Enabled = _customerSettings.StreetAddress2Enabled;
            model.StreetAddress2Required = _customerSettings.StreetAddress2Required;
            model.ZipPostalCodeEnabled = _customerSettings.ZipPostalCodeEnabled;
            model.ZipPostalCodeRequired = _customerSettings.ZipPostalCodeRequired;
            model.CityEnabled = _customerSettings.CityEnabled;
            model.CityRequired = _customerSettings.CityRequired;
            model.CountyEnabled = _customerSettings.CountyEnabled;
            model.CountyRequired = _customerSettings.CountyRequired;
            model.CountryEnabled = _customerSettings.CountryEnabled;
            model.CountryRequired = _customerSettings.CountryRequired;
            model.StateProvinceEnabled = _customerSettings.StateProvinceEnabled;
            model.StateProvinceRequired = _customerSettings.StateProvinceRequired;
            model.PhoneEnabled = _customerSettings.PhoneEnabled;
            model.PhoneRequired = _customerSettings.PhoneRequired;
            model.FaxEnabled = _customerSettings.FaxEnabled;
            model.FaxRequired = _customerSettings.FaxRequired;
            model.NewsletterEnabled = _customerSettings.NewsletterEnabled;
            model.UsernamesEnabled = _customerSettings.UsernamesEnabled;
            model.AllowUsersToChangeUsernames = _customerSettings.AllowUsersToChangeUsernames;
            model.CheckUsernameAvailabilityEnabled = _customerSettings.CheckUsernameAvailabilityEnabled;

            //external authentication
            var currentCustomer = await _workContext.GetCurrentCustomerAsync();


            return model;
        }


        public virtual async Task<RegisterModel> PrepareRegisterModelAsync(RegisterModel model, bool excludeProperties,
            string overrideCustomCustomerAttributesXml = "", bool setDefaultValues = false)
        {
            ArgumentNullException.ThrowIfNull(model);

            var customer = await _workContext.GetCurrentCustomerAsync();

            model.AllowCustomersToSetTimeZone = _dateTimeSettings.AllowCustomersToSetTimeZone;
            foreach (var tzi in _dateTimeHelper.GetSystemTimeZones())
                model.AvailableTimeZones.Add(new SelectListItem { Text = tzi.DisplayName, Value = tzi.Id, Selected = (excludeProperties ? tzi.Id == model.TimeZoneId : tzi.Id == (await _dateTimeHelper.GetCurrentTimeZoneAsync()).Id) });

            //form fields
            model.FirstNameEnabled = _customerSettings.FirstNameEnabled;
            model.LastNameEnabled = _customerSettings.LastNameEnabled;
            model.FirstNameRequired = _customerSettings.FirstNameRequired;
            model.LastNameRequired = _customerSettings.LastNameRequired;
            model.GenderEnabled = _customerSettings.GenderEnabled;
            model.NeutralGenderEnabled = _customerSettings.NeutralGenderEnabled;
            model.DateOfBirthEnabled = _customerSettings.DateOfBirthEnabled;
            model.DateOfBirthRequired = _customerSettings.DateOfBirthRequired;
            model.CompanyEnabled = _customerSettings.CompanyEnabled;
            model.CompanyRequired = _customerSettings.CompanyRequired;
            model.StreetAddressEnabled = _customerSettings.StreetAddressEnabled;
            model.StreetAddressRequired = _customerSettings.StreetAddressRequired;
            model.StreetAddress2Enabled = _customerSettings.StreetAddress2Enabled;
            model.StreetAddress2Required = _customerSettings.StreetAddress2Required;
            model.ZipPostalCodeEnabled = _customerSettings.ZipPostalCodeEnabled;
            model.ZipPostalCodeRequired = _customerSettings.ZipPostalCodeRequired;
            model.CityEnabled = _customerSettings.CityEnabled;
            model.CityRequired = _customerSettings.CityRequired;
            model.CountyEnabled = _customerSettings.CountyEnabled;
            model.CountyRequired = _customerSettings.CountyRequired;
            model.CountryEnabled = _customerSettings.CountryEnabled;
            model.CountryRequired = _customerSettings.CountryRequired;
            model.StateProvinceEnabled = _customerSettings.StateProvinceEnabled;
            model.StateProvinceRequired = _customerSettings.StateProvinceRequired;
            model.PhoneEnabled = _customerSettings.PhoneEnabled;
            model.PhoneRequired = _customerSettings.PhoneRequired;
            model.FaxEnabled = _customerSettings.FaxEnabled;
            model.FaxRequired = _customerSettings.FaxRequired;
            model.NewsletterEnabled = _customerSettings.NewsletterEnabled;
            model.AcceptPrivacyPolicyEnabled = _customerSettings.AcceptPrivacyPolicyEnabled;
            model.AcceptPrivacyPolicyPopup = _commonSettings.PopupForTermsOfServiceLinks;
            model.UsernamesEnabled = _customerSettings.UsernamesEnabled;
            model.CheckUsernameAvailabilityEnabled = _customerSettings.CheckUsernameAvailabilityEnabled;
            model.HoneypotEnabled = _securitySettings.HoneypotEnabled;
            model.DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnRegistrationPage;
            model.EnteringEmailTwice = _customerSettings.EnteringEmailTwice;
            if (setDefaultValues)
            {
                //enable newsletter by default
                model.Newsletter = _customerSettings.NewsletterTickedByDefault;
            }

            //countries and states
            if (_customerSettings.CountryEnabled)
            {
                model.AvailableCountries.Add(new SelectListItem { Text = await _localizationService.GetResourceAsync("Address.SelectCountry"), Value = "0" });
                model.CountryId = _customerSettings.DefaultCountryId ?? 0;
                var currentLanguage = await _workContext.GetWorkingLanguageAsync();
                foreach (var c in await _countryService.GetAllCountriesAsync(currentLanguage.Id))
                {
                    model.AvailableCountries.Add(new SelectListItem
                    {
                        Text = await _localizationService.GetLocalizedAsync(c, x => x.Name),
                        Value = c.Id.ToString(),
                        Selected = c.Id == model.CountryId
                    });
                }

                if (_customerSettings.StateProvinceEnabled)
                {
                    //states
                    var states = (await _stateProvinceService.GetStateProvincesByCountryIdAsync(model.CountryId, currentLanguage.Id)).ToList();
                    if (states.Any())
                    {
                        model.AvailableStates.Add(new SelectListItem { Text = await _localizationService.GetResourceAsync("Address.SelectState"), Value = "0" });

                        foreach (var s in states)
                        {
                            model.AvailableStates.Add(new SelectListItem { Text = await _localizationService.GetLocalizedAsync(s, x => x.Name), Value = s.Id.ToString(), Selected = (s.Id == model.StateProvinceId) });
                        }
                    }
                    else
                    {
                        var anyCountrySelected = model.AvailableCountries.Any(x => x.Selected);

                        model.AvailableStates.Add(new SelectListItem
                        {
                            Text = await _localizationService.GetResourceAsync(anyCountrySelected ? "Address.Other" : "Address.SelectState"),
                            Value = "0"
                        });
                    }

                }
            }

            return model;
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
