using AssetForge.Core;
using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Localization;
using AssetForge.Core.Events;
using AssetForge.Core.Http;
using AssetForge.Core.Security;
using AssetForge.Services.Authentication;
using AssetForge.Services.Common;
using AssetForge.Services.Customers;
using AssetForge.Services.Directory;
using AssetForge.Services.Helpers;
using AssetForge.Services.Localization;
using AssetForge.Services.ScheduleTasks;
using AssetForge.Services.Sites;
using AssetForge.Web.Framework.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace AssetForge.Web.Framework;

/// <summary>
/// Represents work context for web application
/// </summary>
public partial class WebWorkContext : IWorkContext
{
    #region Fields

    protected readonly CookieSettings _cookieSettings;
    protected readonly IAuthenticationService _authenticationService;
    protected readonly ICustomerService _customerService;
    protected readonly IEventPublisher _eventPublisher;
    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly ILanguageService _languageService;
    protected readonly ISiteContext _siteContext;
    protected readonly ISiteMappingService _siteMappingService;
    protected readonly IUserAgentHelper _userAgentHelper;
    protected readonly IWebHelper _webHelper;
    protected readonly LocalizationSettings _localizationSettings;

    protected Customer _cachedCustomer;
    protected Customer _originalCustomerIfImpersonated;
    protected Language _cachedLanguage;

    #endregion

    #region Ctor

    public WebWorkContext(CookieSettings cookieSettings,
        IAuthenticationService authenticationService,
        ICustomerService customerService,
        IEventPublisher eventPublisher,
        IGenericAttributeService genericAttributeService,
        IHttpContextAccessor httpContextAccessor,
        ILanguageService languageService,
        ISiteContext siteContext,
        ISiteMappingService siteMappingService,
        IUserAgentHelper userAgentHelper,
        IWebHelper webHelper,
        LocalizationSettings localizationSettings)
    {
        _cookieSettings = cookieSettings;
        _authenticationService = authenticationService;
        _customerService = customerService;
        _eventPublisher = eventPublisher;
        _genericAttributeService = genericAttributeService;
        _httpContextAccessor = httpContextAccessor;
        _languageService = languageService;
        _siteContext = siteContext;
        _siteMappingService = siteMappingService;
        _userAgentHelper = userAgentHelper;
        _webHelper = webHelper;
        _localizationSettings = localizationSettings;
    }

    #endregion

    #region Utilities

    /// <summary>
    /// Get ix customer cookie
    /// </summary>
    /// <returns>String value of cookie</returns>
    protected virtual string GetCustomerCookie()
    {
        var cookieName = $"{CookieDefaults.Prefix}{CookieDefaults.CustomerCookie}";
        return _httpContextAccessor.HttpContext?.Request?.Cookies[cookieName];
    }

    /// <summary>
    /// Set ix customer cookie
    /// </summary>
    /// <param name="customerGuid">Guid of the customer</param>
    protected virtual void SetCustomerCookie(Guid customerGuid)
    {
        if (_httpContextAccessor.HttpContext?.Response.HasStarted ?? true)
            return;

        //delete current cookie value
        var cookieName = $"{CookieDefaults.Prefix}{CookieDefaults.CustomerCookie}";
        _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieName);

        //get date of cookie expiration
        var cookieExpires = _cookieSettings.CustomerCookieExpires;
        var cookieExpiresDate = DateTime.Now.AddHours(cookieExpires);

        //if passed guid is empty set cookie as expired
        if (customerGuid == Guid.Empty)
            cookieExpiresDate = DateTime.Now.AddMonths(-1);

        //set new cookie value
        var options = new CookieOptions
        {
            HttpOnly = true,
            Expires = cookieExpiresDate,
            Secure = _webHelper.IsCurrentConnectionSecured()
        };
        _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, customerGuid.ToString(), options);
    }

    /// <summary>
    /// Set language culture cookie
    /// </summary>
    /// <param name="language">Language</param>
    protected virtual void SetLanguageCookie(Language language)
    {
        if (_httpContextAccessor.HttpContext?.Response.HasStarted ?? true)
            return;

        //delete current cookie value
        var cookieName = $"{CookieDefaults.Prefix}{CookieDefaults.CultureCookie}";
        _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieName);

        if (string.IsNullOrEmpty(language?.LanguageCulture))
            return;

        //set new cookie value
        var value = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(language.LanguageCulture));
        var options = new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) };
        _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, value, options);
    }

    /// <summary>
    /// Get language from the request
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the found language
    /// </returns>
    protected virtual async Task<Language> GetLanguageFromRequestAsync()
    {
        var requestCultureFeature = _httpContextAccessor.HttpContext?.Features.Get<IRequestCultureFeature>();
        if (requestCultureFeature is null)
            return null;

        //whether we should detect the current language by customer settings
        if (requestCultureFeature.Provider is not SeoUrlCultureProvider && !_localizationSettings.AutomaticallyDetectLanguage)
            return null;

        //get request culture
        if (requestCultureFeature.RequestCulture is null)
            return null;

        //try to get language by culture name
        var requestLanguage = (await _languageService.GetAllLanguagesAsync()).FirstOrDefault(language =>
            language.LanguageCulture.Equals(requestCultureFeature.RequestCulture.Culture.Name, StringComparison.InvariantCultureIgnoreCase));

        //check language availability
        if (requestLanguage == null || !requestLanguage.Published || !await _siteMappingService.AuthorizeAsync(requestLanguage))
            return null;

        return requestLanguage;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the current customer
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task<Customer> GetCurrentCustomerAsync()
    {
        //whether there is a cached value
        if (_cachedCustomer != null)
            return _cachedCustomer;

        await SetCurrentCustomerAsync();

        return _cachedCustomer;
    }

    /// <summary>
    /// Sets the current customer
    /// </summary>
    /// <param name="customer">Current customer</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task SetCurrentCustomerAsync(Customer customer = null)
    {
        if (customer == null)
        {
            //check whether request is made by a background (schedule) task
            if (_httpContextAccessor.HttpContext?.Request
                    ?.Path.Equals(new PathString($"/{TaskDefaults.ScheduleTaskPath}"), StringComparison.InvariantCultureIgnoreCase)
                ?? true)
            {
                //in this case return built-in customer record for background task
                customer = await _customerService.GetOrCreateBackgroundTaskUserAsync();
            }

            if (customer == null || customer.Deleted || !customer.Active || customer.RequireReLogin)
            {
                //check whether request is made by a search engine, in this case return built-in customer record for search engines
                if (_userAgentHelper.IsSearchEngine())
                    customer = await _customerService.GetOrCreateSearchEngineUserAsync();
            }

            if (customer == null || customer.Deleted || !customer.Active || customer.RequireReLogin)
            {
                //try to get registered user
                customer = await _authenticationService.GetAuthenticatedCustomerAsync();
            }

            if (customer != null && !customer.Deleted && customer.Active && !customer.RequireReLogin)
            {
                //get impersonate user if required
                var impersonatedCustomerId = await _genericAttributeService
                    .GetAttributeAsync<int?>(customer, CustomerDefaults.ImpersonatedCustomerIdAttribute);
                if (impersonatedCustomerId.HasValue && impersonatedCustomerId.Value > 0)
                {
                    var impersonatedCustomer = await _customerService.GetCustomerByIdAsync(impersonatedCustomerId.Value);
                    if (impersonatedCustomer != null && !impersonatedCustomer.Deleted &&
                        impersonatedCustomer.Active &&
                        !impersonatedCustomer.RequireReLogin)
                    {
                        //set impersonated customer
                        _originalCustomerIfImpersonated = customer;
                        customer = impersonatedCustomer;
                    }
                }
            }

            if (customer == null || customer.Deleted || !customer.Active || customer.RequireReLogin)
            {
                //get guest customer
                var customerCookie = GetCustomerCookie();
                if (Guid.TryParse(customerCookie, out var customerGuid))
                {
                    //get customer from cookie (should not be registered)
                    var customerByCookie = await _customerService.GetCustomerByGuidAsync(customerGuid);
                    if (customerByCookie != null && !await _customerService.IsRegisteredAsync(customerByCookie))
                        customer = customerByCookie;
                }
            }

            if (customer == null || customer.Deleted || !customer.Active || customer.RequireReLogin)
            {
                //create guest if not exists
                customer = await _customerService.InsertGuestCustomerAsync();
            }
        }

        if (!customer.Deleted && customer.Active && !customer.RequireReLogin)
        {
            //set customer cookie
            SetCustomerCookie(customer.CustomerGuid);

            //cache the found customer
            _cachedCustomer = customer;
        }
    }

    /// <summary>
    /// Gets the original customer (in case the current one is impersonated)
    /// </summary>
    public virtual Customer OriginalCustomerIfImpersonated => _originalCustomerIfImpersonated;

    /// <summary>
    /// Sets current user working language
    /// </summary>
    /// <param name="language">Language</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task SetWorkingLanguageAsync(Language language)
    {
        //save passed language identifier
        var customer = await GetCurrentCustomerAsync();

        if (!customer.IsSystemAccount)
        {
            customer.LanguageId = language?.Id;
            await _customerService.UpdateCustomerAsync(customer);

            //raise event
            await _eventPublisher.PublishAsync(new CustomerChangeWorkingLanguageEvent(customer));
        }

        //set cookie
        SetLanguageCookie(language);

        //then reset the cached value
        _cachedLanguage = null;
    }

    /// <summary>
    /// Gets current user working language
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task<Language> GetWorkingLanguageAsync()
    {
        //whether there is a cached value
        if (_cachedLanguage != null)
            return _cachedLanguage;

        var customer = await GetCurrentCustomerAsync();
        var site = await _siteContext.GetCurrentSiteAsync();

        //whether we should detect the language from the request
        var detectedLanguage = await GetLanguageFromRequestAsync();

        //get current saved language identifier
        var currentLanguageId = customer.LanguageId;

        //if the language is detected we need to save it
        if (detectedLanguage != null)
        {
            //save the detected language identifier if it differs from the current one
            if (detectedLanguage.Id != currentLanguageId)
                await SetWorkingLanguageAsync(detectedLanguage);
        }
        else
        {
            var allSiteLanguages = await _languageService.GetAllLanguagesAsync(siteId: site.Id);

            //check customer language availability
            detectedLanguage = allSiteLanguages.FirstOrDefault(language => language.Id == currentLanguageId);

            //it not found, then try to get the default language for the current site (if specified)
            detectedLanguage ??= allSiteLanguages.FirstOrDefault(language => language.Id == site.DefaultLanguageId);

            //if the default language for the current site not found, then try to get the first one
            detectedLanguage ??= allSiteLanguages.FirstOrDefault();

            //if there are no languages for the current site try to get the first one regardless of the site
            detectedLanguage ??= (await _languageService.GetAllLanguagesAsync()).FirstOrDefault();

            SetLanguageCookie(detectedLanguage);
        }

        //cache the found language
        _cachedLanguage = detectedLanguage;

        return _cachedLanguage;
    }

    /// <summary>
    /// Gets or sets value indicating whether we're in admin area
    /// </summary>
    public virtual bool IsAdmin { get; set; }

    #endregion
}