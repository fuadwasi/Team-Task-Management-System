using AssetForge.App.Extensions;
using AssetForge.App.Factories;
using AssetForge.App.Filters;
using AssetForge.App.Infrastructure;
using AssetForge.App.Models.Api;
using AssetForge.App.Models.Customer;
using AssetForge.Core;
using AssetForge.Core.Domain;
using AssetForge.Core.Domain.Catalog;
using AssetForge.Core.Domain.Common;
using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Localization;
using AssetForge.Core.Domain.Media;
using AssetForge.Core.Domain.Security;
using AssetForge.Core.Events;
using AssetForge.Core.Infrastructure;
using AssetForge.Services.Attributes;
using AssetForge.Services.Authentication;
using AssetForge.Services.Authentication.MultiFactor;
using AssetForge.Services.Common;
using AssetForge.Services.Configuration;
using AssetForge.Services.Customers;
using AssetForge.Services.Directory;
using AssetForge.Services.ExportImport;
using AssetForge.Services.Helpers;
using AssetForge.Services.Localization;
using AssetForge.Services.Logging;
using AssetForge.Services.Media;
using AssetForge.Services.Security;
using AssetForge.Web.Framework.Validators;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.Text.Encodings.Web;

namespace AssetForge.App.Controllers.API
{
    [ApiController]
    [Route("api/customer")]
    public partial class CustomerApiController : BaseApiController
    {
        #region Fields

        protected readonly AddressSettings _addressSettings;
        protected readonly CaptchaSettings _captchaSettings;
        protected readonly CustomerSettings _customerSettings;
        protected readonly DateTimeSettings _dateTimeSettings;
        protected readonly HtmlEncoder _htmlEncoder;
        protected readonly IAddressService _addressService;
        protected readonly IAttributeParser<AddressAttribute, AddressAttributeValue> _addressAttributeParser;
        private readonly IAttributeService<AddressAttribute, AddressAttributeValue> _addressAttributeService;
        protected readonly IAttributeParser<CustomerAttribute, CustomerAttributeValue> _customerAttributeParser;
        protected readonly IAttributeService<CustomerAttribute, CustomerAttributeValue> _customerAttributeService;
        protected readonly IAuthenticationService _authenticationService;
        protected readonly ICountryService _countryService;
        private readonly ICustomerModelFactory _customerModelFactory;
        protected readonly ICustomerActivityService _customerActivityService;
        protected readonly ICustomerRegistrationService _customerRegistrationService;
        protected readonly ICustomerService _customerService;
        protected readonly IDownloadService _downloadService;
        protected readonly IEventPublisher _eventPublisher;
        protected readonly IExportManager _exportManager;
        protected readonly IGenericAttributeService _genericAttributeService;
        protected readonly ILocalizationService _localizationService;
        protected readonly Services.Logging.ILogger _logger;
        protected readonly IMultiFactorAuthenticationPluginManager _multiFactorAuthenticationPluginManager;
        private readonly IAssetForgeFileProvider _fileProvider;
        protected readonly IPermissionService _permissionService;
        protected readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        protected readonly IStateProvinceService _stateProvinceService;
        protected readonly ISiteContext _siteContext;
        protected readonly IWorkContext _workContext;
        protected readonly LocalizationSettings _localizationSettings;
        protected readonly MediaSettings _mediaSettings;
        protected readonly MultiFactorAuthenticationSettings _multiFactorAuthenticationSettings;
        protected readonly SiteInformationSettings _siteInformationSettings;

        private static readonly char[] _separator = [','];

        #endregion

        #region Ctor

        public CustomerApiController(AddressSettings addressSettings,
            CaptchaSettings captchaSettings,
            CustomerSettings customerSettings,
            DateTimeSettings dateTimeSettings,
            HtmlEncoder htmlEncoder,
            IAddressService addressService,
            IAttributeParser<AddressAttribute, AddressAttributeValue> addressAttributeParser,
            IAttributeService<AddressAttribute, AddressAttributeValue> addressAttributeService,
            IAttributeParser<CustomerAttribute, CustomerAttributeValue> customerAttributeParser,
            IAttributeService<CustomerAttribute, CustomerAttributeValue> customerAttributeService,
            IAuthenticationService authenticationService,
            ICountryService countryService,
            ICustomerModelFactory customerModelFactory,
            ICustomerActivityService customerActivityService,
            ICustomerRegistrationService customerRegistrationService,
            ICustomerService customerService,
            IDownloadService downloadService,
            IEventPublisher eventPublisher,
            IExportManager exportManager,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            Services.Logging.ILogger logger,
            IMultiFactorAuthenticationPluginManager multiFactorAuthenticationPluginManager,
            IAssetForgeFileProvider fileProvider,
            IPermissionService permissionService,
            IPictureService pictureService,
            ISettingService settingService,
            IStateProvinceService stateProvinceService,
            IWorkContext workContext,
            LocalizationSettings localizationSettings,
            MediaSettings mediaSettings,
            ISiteContext siteContext,
            SiteInformationSettings siteInformationSettings,
            MultiFactorAuthenticationSettings multiFactorAuthenticationSettings)
        {
            _addressSettings = addressSettings;
            _captchaSettings = captchaSettings;
            _customerSettings = customerSettings;
            _dateTimeSettings = dateTimeSettings;
            _htmlEncoder = htmlEncoder;
            _addressService = addressService;
            _addressAttributeParser = addressAttributeParser;
            _addressAttributeService = addressAttributeService;
            _customerAttributeParser = customerAttributeParser;
            _customerAttributeService = customerAttributeService;
            _authenticationService = authenticationService;
            _countryService = countryService;
            _customerActivityService = customerActivityService;
            _customerModelFactory = customerModelFactory;
            _customerRegistrationService = customerRegistrationService;
            _customerService = customerService;
            _downloadService = downloadService;
            _eventPublisher = eventPublisher;
            _exportManager = exportManager;
            _genericAttributeService = genericAttributeService;
            _localizationService = localizationService;
            _logger = logger;
            _multiFactorAuthenticationPluginManager = multiFactorAuthenticationPluginManager;
            _fileProvider = fileProvider;
            _permissionService = permissionService;
            _pictureService = pictureService;
            _settingService = settingService;
            _stateProvinceService = stateProvinceService;
            _siteContext = siteContext;
            _workContext = workContext;
            _localizationSettings = localizationSettings;
            _mediaSettings = mediaSettings;
            _multiFactorAuthenticationSettings = multiFactorAuthenticationSettings;
            _siteInformationSettings = siteInformationSettings;
        }

        #endregion

        #region Utilities

        protected async Task<string> GetToken(Customer customer)
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var now = Math.Round((DateTime.UtcNow.AddDays(180) - unixEpoch).TotalSeconds);

            var payload = new Dictionary<string, object>()
                {
                    { WebApiCustomerDefaults.CustomerId, customer.Id },
                    { "exp", now }
                };

            return JwtHelper.JwtEncoder.Encode(payload, WebApiCustomerDefaults.JwtSecretKey);
        }

        protected string GetDeviceIdFromHeader()
        {
            _ = Request.Headers.TryGetValue(WebApiCustomerDefaults.DeviceId, out var headerValues);
            if (headerValues.Count > 0)
            {
                var device = headerValues.FirstOrDefault();
                if (device != null)
                    return device;
            }
            return string.Empty;
        }

        protected virtual async Task<string> ParseCustomCustomerAttributes(NameValueCollection form)
        {
            ArgumentNullException.ThrowIfNull(form);

            return string.Empty;
        }

        private async Task<bool> SecondAdminAccountExistsAsync(Customer customer)
        {
            var customers = await _customerService.GetAllCustomersAsync(customerRoleIds: new[] { (await _customerService.GetCustomerRoleBySystemNameAsync(CustomerDefaults.AdministratorsRoleName)).Id });

            return customers.Any(c => c.Active && c.Id != customer.Id);
        }

        #endregion

        #region Methods

        #region Login / logout

        [HttpGet("login")]
        public virtual async Task<IActionResult> Login(bool? checkoutAsGuest)
        {
            var model = await _customerModelFactory.PrepareLoginModelAsync(checkoutAsGuest);
            return OkWrap(model);
        }

        [HttpPost("login")]
        public virtual async Task<IActionResult> Login([FromBody] BaseQueryModel<LoginModel> queryModel)
        {
            var model = queryModel.Data;
            var response = new GenericResponseModel<LogInResponseModel>();
            var responseData = new LogInResponseModel();

            if (ModelState.IsValid)
            {
                var customerUserName = model.Username;
                var customerEmail = model.Email;
                var userNameOrEmail = _customerSettings.UsernamesEnabled ? customerUserName : customerEmail;

                var loginResult = await _customerRegistrationService.ValidateCustomerAsync(userNameOrEmail, model.Password);

                switch (loginResult)
                {
                    case CustomerLoginResults.Successful:
                        {
                            var customer = _customerSettings.UsernamesEnabled
                                ? await _customerService.GetCustomerByUsernameAsync(customerUserName)
                                : await _customerService.GetCustomerByEmailAsync(customerEmail);

                            responseData.CustomerInfo = await _customerModelFactory.PrepareCustomerInfoModelAsync(responseData.CustomerInfo, customer, false);
                            responseData.Token = await GetToken(customer);


                            //sign in new customer
                            await _authenticationService.SignInAsync(customer, true);

                            //raise event       
                            await _eventPublisher.PublishAsync(new CustomerLoggedinEvent(customer));

                            //activity log
                            await _customerActivityService.InsertActivityAsync(customer, "PublicSite.Login",
                                await _localizationService.GetResourceAsync("ActivityLog.PublicSite.Login"), customer);

                            //var deviceId = GetDeviceIdFromHeader();
                            //var device = await _deviceService.GetApiDeviceByDeviceIdAsync(deviceId, (await _siteContext.GetCurrentSiteAsync()).Id);
                            //if (device != null)
                            //{
                            //    device.CustomerId = customer.Id;
                            //    device.IsRegistered = await _customerService.IsRegisteredAsync(customer);
                            //    await _deviceService.UpdateApiDeviceAsync(device);
                            //}

                            response.Data = responseData;
                            return Ok(response);
                        }
                    case CustomerLoginResults.CustomerNotExist:
                        ModelState.AddModelError("", await _localizationService.GetResourceAsync("Account.Login.WrongCredentials.CustomerNotExist"));
                        break;
                    case CustomerLoginResults.Deleted:
                        ModelState.AddModelError("", await _localizationService.GetResourceAsync("Account.Login.WrongCredentials.Deleted"));
                        break;
                    case CustomerLoginResults.NotActive:
                        ModelState.AddModelError("", await _localizationService.GetResourceAsync("Account.Login.WrongCredentials.NotActive"));
                        break;
                    case CustomerLoginResults.NotRegistered:
                        ModelState.AddModelError("", await _localizationService.GetResourceAsync("Account.Login.WrongCredentials.NotRegistered"));
                        break;
                    case CustomerLoginResults.LockedOut:
                        ModelState.AddModelError("", await _localizationService.GetResourceAsync("Account.Login.WrongCredentials.LockedOut"));
                        break;
                    case CustomerLoginResults.WrongPassword:
                    default:
                        ModelState.AddModelError("", await _localizationService.GetResourceAsync("Account.Login.WrongCredentials"));
                        break;
                }
            }

            foreach (var modelState in ModelState.Values)
                foreach (var error in modelState.Errors)
                    response.ErrorList.Add(error.ErrorMessage);

            return BadRequest(response);
        }

        [HttpGet("logout")]
        public virtual async Task<IActionResult> Logout()
        {
            //activity log
            await _customerActivityService.InsertActivityAsync(await _workContext.GetCurrentCustomerAsync(), "PublicSite.Logout",
                await _localizationService.GetResourceAsync("ActivityLog.PublicSite.Logout"), await _workContext.GetCurrentCustomerAsync());

            //standard logout 
            await _authenticationService.SignOutAsync();

            //raise logged out event       
            await _eventPublisher.PublishAsync(new CustomerLoggedOutEvent(await _workContext.GetCurrentCustomerAsync()));

            return Ok();
        }

        #endregion


        #region Register

        [HttpGet("register")]
        public virtual async Task<IActionResult> Register()
        {
            //check whether registration is allowed
            if (_customerSettings.UserRegistrationType == UserRegistrationType.Disabled)
                return BadRequest();

            var response = new GenericResponseModel<RegisterModel>();
            var model = new RegisterModel();
            response.Data = await _customerModelFactory.PrepareRegisterModelAsync(model, false, setDefaultValues: true);
            return Ok(response);
        }


        [HttpPost("register")]
        public virtual async Task<IActionResult> Register([FromBody] BaseQueryModel<RegisterModel> queryModel)
        {
            //check whether registration is allowed
            if (_customerSettings.UserRegistrationType == UserRegistrationType.Disabled)
                return BadRequest();

            var response = new GenericResponseModel<RegisterModel>();
            var customer = await _workContext.GetCurrentCustomerAsync();
            var model = queryModel.Data;

            if (await _customerService.IsRegisteredAsync(customer))
            {
                //Already registered customer. 
                await _authenticationService.SignOutAsync();

                //raise logged out event       
                await _eventPublisher.PublishAsync(new CustomerLoggedOutEvent(customer));

                //Save a new record
                await _workContext.SetCurrentCustomerAsync(await _customerService.InsertGuestCustomerAsync());
            }

            var site = await _siteContext.GetCurrentSiteAsync();
            customer.RegisteredInSiteId = site.Id;

            var form = queryModel.FormValues == null ? new NameValueCollection() : queryModel.FormValues.ToNameValueCollection();
            //custom customer attributes
            var customerAttributesXml = await ParseCustomCustomerAttributes(form);
            var customerAttributeWarnings = await _customerAttributeParser.GetAttributeWarningsAsync(customerAttributesXml);
            foreach (var error in customerAttributeWarnings)
            {
                ModelState.AddModelError("", error);
            }

            if (ModelState.IsValid)
            {
                if (_customerSettings.UsernamesEnabled && model.Username != null)
                {
                    model.Username = model.Username.Trim();
                }

                var isApproved = _customerSettings.UserRegistrationType == UserRegistrationType.Standard;
                var registrationRequest = new CustomerRegistrationRequest(customer,
                    model.Email,
                    _customerSettings.UsernamesEnabled ? model.Username : model.Email,
                    model.Password,
                    _customerSettings.DefaultPasswordFormat,
                    site.Id,
                    isApproved);
                var registrationResult = await _customerRegistrationService.RegisterCustomerAsync(registrationRequest);
                if (registrationResult.Success)
                {
                    //properties
                    if (_dateTimeSettings.AllowCustomersToSetTimeZone)
                        customer.TimeZoneId = model.TimeZoneId;

                    //form fields
                    if (_customerSettings.GenderEnabled)
                        customer.Gender = model.Gender;
                    if (_customerSettings.FirstNameEnabled)
                        customer.FirstName = model.FirstName;
                    if (_customerSettings.LastNameEnabled)
                        customer.LastName = model.LastName;
                    if (_customerSettings.DateOfBirthEnabled)
                        customer.DateOfBirth = model.ParseDateOfBirth();
                    if (_customerSettings.CompanyEnabled)
                        customer.Company = model.Company;
                    if (_customerSettings.StreetAddressEnabled)
                        customer.StreetAddress = model.StreetAddress;
                    if (_customerSettings.StreetAddress2Enabled)
                        customer.StreetAddress2 = model.StreetAddress2;
                    if (_customerSettings.ZipPostalCodeEnabled)
                        customer.ZipPostalCode = model.ZipPostalCode;
                    if (_customerSettings.CityEnabled)
                        customer.City = model.City;
                    if (_customerSettings.CountyEnabled)
                        customer.County = model.County;
                    if (_customerSettings.CountryEnabled)
                        customer.CountryId = model.CountryId;
                    if (_customerSettings.CountryEnabled && _customerSettings.StateProvinceEnabled)
                        customer.StateProvinceId = model.StateProvinceId;
                    if (_customerSettings.PhoneEnabled)
                        customer.Phone = model.Phone;
                    if (_customerSettings.FaxEnabled)
                        customer.Fax = model.Fax;

                    //save customer attributes
                    customer.CustomCustomerAttributesXML = customerAttributesXml;
                    await _customerService.UpdateCustomerAsync(customer);


                    //insert default address (if possible)
                    var defaultAddress = new Address
                    {
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Email = customer.Email,
                        Company = customer.Company,
                        CountryId = customer.CountryId > 0
                            ? customer.CountryId
                            : null,
                        StateProvinceId = customer.StateProvinceId > 0
                            ? customer.StateProvinceId
                            : null,
                        County = customer.County,
                        City = customer.City,
                        Address1 = customer.StreetAddress,
                        Address2 = customer.StreetAddress2,
                        ZipPostalCode = customer.ZipPostalCode,
                        PhoneNumber = customer.Phone,
                        FaxNumber = customer.Fax,
                        CreatedOnUtc = customer.CreatedOnUtc
                    };
                    if (await _addressService.IsAddressValidAsync(defaultAddress))
                    {
                        //some validation
                        if (defaultAddress.CountryId == 0)
                            defaultAddress.CountryId = null;
                        if (defaultAddress.StateProvinceId == 0)
                            defaultAddress.StateProvinceId = null;
                        //set default address
                        //customer.Addresses.Add(defaultAddress);

                        //await _addressService.InsertAddressAsync(defaultAddress);

                        //await _customerService.InsertCustomerAddressAsync(customer, defaultAddress);

                        await _customerService.UpdateCustomerAsync(customer);
                    }


                    //raise event       
                    await _eventPublisher.PublishAsync(new CustomerRegisteredEvent(customer));
                    var currentLanguage = await _workContext.GetWorkingLanguageAsync();

                    switch (_customerSettings.UserRegistrationType)
                    {
                        case UserRegistrationType.EmailValidation:
                            //email validation message
                            await _genericAttributeService.SaveAttributeAsync(customer, CustomerDefaults.AccountActivationTokenAttribute, Guid.NewGuid().ToString());

                            response.Message = await _localizationService.GetResourceAsync("Account.Register.Result.EmailValidation");
                            return Ok(response);

                        case UserRegistrationType.AdminApproval:
                            response.Message = await _localizationService.GetResourceAsync("Account.Register.Result.AdminApproval");
                            return Ok(response);

                        case UserRegistrationType.Standard:
                            //send customer welcome message

                            //raise event       
                            await _eventPublisher.PublishAsync(new CustomerActivatedEvent(customer));

                            response.Message = await _localizationService.GetResourceAsync("Account.Register.Result.Standard");
                            return Ok(response);

                        default:
                            return BadRequest();
                    }
                }
                //errors
                response.ErrorList.AddRange(registrationResult.Errors);
            }

            foreach (var modelState in ModelState.Values)
                foreach (var error in modelState.Errors)
                    response.ErrorList.Add(error.ErrorMessage);

            //If we got this far, something failed, redisplay form
            response.Data = await _customerModelFactory.PrepareRegisterModelAsync(model, true, customerAttributesXml);
            return BadRequest(response);
        }

        [HttpPost("checkusernameavailability")]
        public virtual async Task<IActionResult> CheckUsernameAvailability(string username)
        {
            var usernameAvailable = false;
            var statusText = await _localizationService.GetResourceAsync("Account.CheckUsernameAvailability.NotAvailable");

            if (!UsernamePropertyValidator<string, string>.IsValid(username, _customerSettings))
            {
                statusText = await _localizationService.GetResourceAsync("Account.Fields.Username.NotValid");
            }
            else if (_customerSettings.UsernamesEnabled && !string.IsNullOrWhiteSpace(username))
            {
                var currentCustomer = await _workContext.GetCurrentCustomerAsync();
                if (currentCustomer != null &&
                    currentCustomer.Username != null &&
                    currentCustomer.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase))
                {
                    statusText = await _localizationService.GetResourceAsync("Account.CheckUsernameAvailability.CurrentUsername");
                }
                else
                {
                    var customer = await _customerService.GetCustomerByUsernameAsync(username);
                    if (customer == null)
                    {
                        statusText = await _localizationService.GetResourceAsync("Account.CheckUsernameAvailability.Available");
                        usernameAvailable = true;
                    }
                }
            }

            var response = new GenericResponseModel<bool>
            {
                Data = usernameAvailable,
                Message = statusText
            };
            return Ok(response);
        }

        [HttpGet("activation/{token}/{email}")]
        public virtual async Task<IActionResult> AccountActivation(string token, string email)
        {
            var customer = await _customerService.GetCustomerByEmailAsync(email);
            if (customer == null)
                return NotFound(await _localizationService.GetResourceAsync("NopStation.WebApi.Response.Customer.CustomerNotFound"));

            var response = new GenericResponseModel<AccountActivationModel>();
            var cToken = await _genericAttributeService.GetAttributeAsync<string>(customer, CustomerDefaults.AccountActivationTokenAttribute);
            if (string.IsNullOrEmpty(cToken))
            {
                response.ErrorList.Add(await _localizationService.GetResourceAsync("Account.AccountActivation.AlreadyActivated"));
                return BadRequest(response);
            }

            if (!cToken.Equals(token, StringComparison.InvariantCultureIgnoreCase))
                return BadRequest();

            //activate user account
            customer.Active = true;
            await _customerService.UpdateCustomerAsync(customer);
            await _genericAttributeService.SaveAttributeAsync(customer, CustomerDefaults.AccountActivationTokenAttribute, "");
            //send welcome message

            //activating newsletter if need
            var site = await _siteContext.GetCurrentSiteAsync();
            response.Message = await _localizationService.GetResourceAsync("Account.AccountActivation.Activated");
            return Ok(response);
        }

        #endregion

        #region My account / Info

        [HttpGet("info")]
        public virtual async Task<IActionResult> Info()
        {
            if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
                return Unauthorized();

            var response = new GenericResponseModel<CustomerInfoModel>();
            var model = new CustomerInfoModel();
            response.Data = await _customerModelFactory.PrepareCustomerInfoModelAsync(model, await _workContext.GetCurrentCustomerAsync(), false);
            return Ok(response);
        }


        [HttpPost("info")]
        public virtual async Task<IActionResult> Info([FromBody] BaseQueryModel<CustomerInfoModel> queryModel)
        {
            if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
                return Unauthorized();

            var model = queryModel.Data;
            var response = new GenericResponseModel<CustomerInfoModel>();
            var oldCustomerModel = new CustomerInfoModel();

            var customer = await _workContext.GetCurrentCustomerAsync();


            var form = queryModel.FormValues == null ? new NameValueCollection() : queryModel.FormValues.ToNameValueCollection();
            //custom customer attributes
            var customerAttributesXml = await ParseCustomCustomerAttributes(form);
            var customerAttributeWarnings = await _customerAttributeParser.GetAttributeWarningsAsync(customerAttributesXml);
            foreach (var error in customerAttributeWarnings)
            {
                ModelState.AddModelError("", error);
            }


            try
            {
                if (ModelState.IsValid)
                {
                    //username 
                    if (_customerSettings.UsernamesEnabled && _customerSettings.AllowUsersToChangeUsernames)
                    {
                        var userName = model.Username.Trim();
                        if (!customer.Username.Equals(userName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            //change username
                            await _customerRegistrationService.SetUsernameAsync(customer, userName);

                            //re-authenticate
                            //do not authenticate users in impersonation mode
                            if (_workContext.OriginalCustomerIfImpersonated == null)
                                await _authenticationService.SignInAsync(customer, true);
                        }
                    }
                    //email
                    var email = model.Email.Trim();
                    if (!customer.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase))
                    {
                        //change email
                        var requireValidation = _customerSettings.UserRegistrationType == UserRegistrationType.EmailValidation;
                        await _customerRegistrationService.SetEmailAsync(customer, email, requireValidation);

                        //do not authenticate users in impersonation mode
                        if (_workContext.OriginalCustomerIfImpersonated == null)
                        {
                            //re-authenticate (if usernames are disabled)
                            if (!_customerSettings.UsernamesEnabled && !requireValidation)
                                await _authenticationService.SignInAsync(customer, true);
                        }
                    }

                    //properties
                    if (_dateTimeSettings.AllowCustomersToSetTimeZone)
                        customer.TimeZoneId = model.TimeZoneId;

                    //form fields
                    if (_customerSettings.GenderEnabled)
                        customer.Gender = model.Gender;
                    if (_customerSettings.FirstNameEnabled)
                        customer.FirstName = model.FirstName;
                    if (_customerSettings.LastNameEnabled)
                        customer.LastName = model.LastName;
                    if (_customerSettings.DateOfBirthEnabled)
                        customer.DateOfBirth = model.ParseDateOfBirth();
                    if (_customerSettings.CompanyEnabled)
                        customer.Company = model.Company;
                    if (_customerSettings.StreetAddressEnabled)
                        customer.StreetAddress = model.StreetAddress;
                    if (_customerSettings.StreetAddress2Enabled)
                        customer.StreetAddress2 = model.StreetAddress2;
                    if (_customerSettings.ZipPostalCodeEnabled)
                        customer.ZipPostalCode = model.ZipPostalCode;
                    if (_customerSettings.CityEnabled)
                        customer.City = model.City;
                    if (_customerSettings.CountyEnabled)
                        customer.County = model.County;
                    if (_customerSettings.CountryEnabled)
                        customer.CountryId = model.CountryId;
                    if (_customerSettings.CountryEnabled && _customerSettings.StateProvinceEnabled)
                        customer.StateProvinceId = model.StateProvinceId;
                    if (_customerSettings.PhoneEnabled)
                        customer.Phone = model.Phone;
                    if (_customerSettings.FaxEnabled)
                        customer.Fax = model.Fax;

                    customer.CustomCustomerAttributesXML = customerAttributesXml;
                    await _customerService.UpdateCustomerAsync(customer);

                    response.Data = await _customerModelFactory.PrepareCustomerInfoModelAsync(model, await _workContext.GetCurrentCustomerAsync(), false);
                    return Ok(response);
                }
            }
            catch (Exception exc)
            {
                ModelState.AddModelError("", exc.Message);
            }

            foreach (var modelState in ModelState.Values)
                foreach (var error in modelState.Errors)
                    response.ErrorList.Add(error.ErrorMessage);

            //If we got this far, something failed, redisplay form
            response.Data = await _customerModelFactory.PrepareCustomerInfoModelAsync(model, customer, true, customerAttributesXml);
            return BadRequest(response);
        }

        #endregion

        #endregion
    }
}