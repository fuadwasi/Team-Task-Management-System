using AssetForge.Core;
using AssetForge.Core.Domain;
using AssetForge.Core.Domain.Common;
using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Messages;
using AssetForge.Core.Domain.Sites;
using AssetForge.Core.Domain.Vendors;
using AssetForge.Core.Events;
using AssetForge.Core.Infrastructure;
using AssetForge.Services.Attributes;
using AssetForge.Services.Common;
using AssetForge.Services.Customers;
using AssetForge.Services.Directory;
using AssetForge.Services.Helpers;
using AssetForge.Services.Html;
using AssetForge.Services.Localization;
using AssetForge.Services.Logging;
using AssetForge.Services.Seo;
using AssetForge.Services.Sites;
using AssetForge.Services.Vendors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace AssetForge.Services.Messages;

/// <summary>
/// Message token provider
/// </summary>
public partial class MessageTokenProvider : IMessageTokenProvider
{
    #region Fields

    protected readonly IActionContextAccessor _actionContextAccessor;
    protected readonly IAddressService _addressService;
    protected readonly IAttributeFormatter<AddressAttribute, AddressAttributeValue> _addressAttributeFormatter;
    protected readonly IAttributeFormatter<CustomerAttribute, CustomerAttributeValue> _customerAttributeFormatter;
    private readonly IAttributeFormatter<VendorAttribute, VendorAttributeValue> _vendorAttributeFormatter;
    protected readonly ICountryService _countryService;
    protected readonly ICustomerService _customerService;
    protected readonly IDateTimeHelper _dateTimeHelper;
    protected readonly IEventPublisher _eventPublisher;
    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly IHtmlFormatter _htmlFormatter;
    protected readonly ILanguageService _languageService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILogger _logger;
    protected readonly IStateProvinceService _stateProvinceService;
    protected readonly ISiteContext _siteContext;
    protected readonly ISiteService _siteService;
    protected readonly IUrlHelperFactory _urlHelperFactory;
    protected readonly IUrlRecordService _urlRecordService;
    protected readonly IWorkContext _workContext;
    protected readonly MessageTemplatesSettings _templatesSettings;
    protected readonly SiteInformationSettings _siteInformationSettings;

    protected Dictionary<string, IEnumerable<string>> _allowedTokens;

    #endregion Fields

    #region Ctor

    public MessageTokenProvider(
        IActionContextAccessor actionContextAccessor,
        IAddressService addressService,
        IAttributeFormatter<AddressAttribute, AddressAttributeValue> addressAttributeFormatter,
        IAttributeFormatter<CustomerAttribute, CustomerAttributeValue> customerAttributeFormatter,
        IAttributeFormatter<VendorAttribute, VendorAttributeValue> vendorAttributeFormatter,
        ICountryService countryService,
        ICustomerService customerService,
        IDateTimeHelper dateTimeHelper,
        IEventPublisher eventPublisher,
        IGenericAttributeService genericAttributeService,
        IHtmlFormatter htmlFormatter,
        ILanguageService languageService,
        ILocalizationService localizationService,
        ILogger logger,
        IStateProvinceService stateProvinceService,
        ISiteContext siteContext,
        ISiteService siteService,
        IUrlHelperFactory urlHelperFactory,
        IUrlRecordService urlRecordService,
        IWorkContext workContext,
        MessageTemplatesSettings templatesSettings,
        SiteInformationSettings siteInformationSettings
        )
    {
        _actionContextAccessor = actionContextAccessor;
        _addressService = addressService;
        _addressAttributeFormatter = addressAttributeFormatter;
        _customerAttributeFormatter = customerAttributeFormatter;
        _vendorAttributeFormatter = vendorAttributeFormatter;
        _countryService = countryService;
        _customerService = customerService;
        _dateTimeHelper = dateTimeHelper;
        _eventPublisher = eventPublisher;
        _genericAttributeService = genericAttributeService;
        _htmlFormatter = htmlFormatter;
        _languageService = languageService;
        _localizationService = localizationService;
        _logger = logger;
        _stateProvinceService = stateProvinceService;
        _siteContext = siteContext;
        _siteService = siteService;
        _urlHelperFactory = urlHelperFactory;
        _urlRecordService = urlRecordService;
        _workContext = workContext;
        _templatesSettings = templatesSettings;
        _siteInformationSettings = siteInformationSettings;
    }

    #endregion Ctor

    #region Allowed tokens

    /// <summary>
    /// Get all available tokens by token groups
    /// </summary>
    protected Dictionary<string, IEnumerable<string>> AllowedTokens
    {
        get
        {
            if (_allowedTokens != null)
                return _allowedTokens;

            _allowedTokens = new Dictionary<string, IEnumerable<string>>
            {
                //site tokens
                {
                    TokenGroupNames.SiteTokens,
                    new[]
                    {
                        "%Site.Name%",
                        "%Site.URL%",
                        "%Site.Email%",
                        "%Site.CompanyName%",
                        "%Site.CompanyAddress%",
                        "%Site.CompanyPhoneNumber%",
                        "%Site.CompanyVat%",
                        "%Facebook.URL%",
                        "%Twitter.URL%",
                        "%YouTube.URL%",
                        "%Instagram.URL%"
                    }
                },

                //customer tokens
                {
                    TokenGroupNames.CustomerTokens,
                    new[]
                    {
                        "%Customer.Email%",
                        "%Customer.Username%",
                        "%Customer.FullName%",
                        "%Customer.FirstName%",
                        "%Customer.LastName%",
                        "%Customer.CustomAttributes%",
                        "%Customer.PasswordRecoveryURL%",
                        "%Customer.AccountActivationURL%",
                        "%Customer.EmailRevalidationURL%",
                        "%Wishlist.URLForCustomer%"
                    }
                },

                //newsletter subscription tokens
                {
                    TokenGroupNames.SubscriptionTokens,
                    new[]
                    {
                        "%NewsLetterSubscription.Email%",
                        "%NewsLetterSubscription.ActivationUrl%",
                        "%NewsLetterSubscription.DeactivationUrl%"
                    }
                },

                //forum tokens
                {
                    TokenGroupNames.ForumTokens,
                    new[]
                    {
                        "%Forums.ForumURL%",
                        "%Forums.ForumName%"
                    }
                },

                //forum topic tokens
                {
                    TokenGroupNames.ForumTopicTokens,
                    new[]
                    {
                        "%Forums.TopicURL%",
                        "%Forums.TopicName%"
                    }
                },

                //forum post tokens
                {
                    TokenGroupNames.ForumPostTokens,
                    new[]
                    {
                        "%Forums.PostAuthor%",
                        "%Forums.PostBody%"
                    }
                },

                //private message tokens
                {
                    TokenGroupNames.PrivateMessageTokens,
                    new[]
                    {
                        "%PrivateMessage.Subject%",
                        "%PrivateMessage.Text%"
                    }
                },

                //attribute combination tokens
                {
                    TokenGroupNames.AttributeCombinationTokens,
                    new[]
                    {
                        "%AttributeCombination.Formatted%",
                        "%AttributeCombination.SKU%",
                        "%AttributeCombination.StockQuantity%"
                    }
                },

                //blog comment tokens
                {
                    TokenGroupNames.BlogCommentTokens,
                    new[]
                    {
                        "%BlogComment.BlogPostTitle%"
                    }
                },

                //news comment tokens
                {
                    TokenGroupNames.NewsCommentTokens,
                    new[]
                    {
                        "%NewsComment.NewsTitle%"
                    }
                },

                //email a friend tokens
                {
                    TokenGroupNames.EmailAFriendTokens,
                    new[]
                    {
                        "%EmailAFriend.PersonalMessage%",
                        "%EmailAFriend.Email%"
                    }
                },

                //contact us tokens
                {
                    TokenGroupNames.ContactUs,
                    new[]
                    {
                        "%ContactUs.SenderEmail%",
                        "%ContactUs.SenderName%",
                        "%ContactUs.Body%"
                    }
                },
            };

            return _allowedTokens;
        }
    }

    #endregion Allowed tokens

    #region Utilities

    /// <summary>
    /// Generates an absolute URL for the specified site, routeName and route values
    /// </summary>
    /// <param name="siteId">Site identifier; Pass 0 to load URL of the current site</param>
    /// <param name="routeName">The name of the route that is used to generate URL</param>
    /// <param name="routeValues">An object that contains route values</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the generated URL
    /// </returns>
    protected virtual async Task<string> RouteUrlAsync(int siteId = 0, string routeName = null, object routeValues = null)
    {
        try
        {
            //try to get a site by the passed identifier
            var site = await _siteService.GetSiteByIdAsync(siteId) ?? await _siteContext.GetCurrentSiteAsync()
                ?? throw new Exception("No site could be loaded");

            //ensure that the site URL is specified
            if (string.IsNullOrEmpty(site.Url))
                throw new Exception("Site URL cannot be empty");

            //generate the relative URL
            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
            var url = urlHelper.RouteUrl(routeName, routeValues);

            //compose the result
            return new Uri(new Uri(site.Url), url).AbsoluteUri;
        }
        catch (Exception exception)
        {
            var warning = $"When sending a notification, an error occurred while creating a link for '{routeName}', ensure that URL of the site #{siteId} is correct.";
            await _logger.WarningAsync(warning, exception);

            return string.Empty;
        }
    }

    #endregion Utilities

    #region Methods

    /// <summary>
    /// Add site tokens
    /// </summary>
    /// <param name="tokens">List of already added tokens</param>
    /// <param name="site">Site</param>
    /// <param name="emailAccount">Email account</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task AddSiteTokensAsync(IList<Token> tokens, Site site, EmailAccount emailAccount)
    {
        ArgumentNullException.ThrowIfNull(emailAccount);

        tokens.Add(new Token("Site.Name", await _localizationService.GetLocalizedAsync(site, x => x.Name)));
        tokens.Add(new Token("Site.URL", site.Url, true));
        tokens.Add(new Token("Site.Email", emailAccount.Email));
        tokens.Add(new Token("Site.CompanyName", site.CompanyName));
        tokens.Add(new Token("Site.CompanyAddress", site.CompanyAddress));
        tokens.Add(new Token("Site.CompanyPhoneNumber", site.CompanyPhoneNumber));
        tokens.Add(new Token("Site.CompanyVat", site.CompanyVat));

        tokens.Add(new Token("Facebook.URL", _siteInformationSettings.FacebookLink));
        tokens.Add(new Token("Twitter.URL", _siteInformationSettings.TwitterLink));
        tokens.Add(new Token("YouTube.URL", _siteInformationSettings.YoutubeLink));
        tokens.Add(new Token("Instagram.URL", _siteInformationSettings.InstagramLink));

        //event notification
        await _eventPublisher.EntityTokensAddedAsync(site, tokens);
    }

    /// <summary>
    /// Add vendor tokens
    /// </summary>
    /// <param name="tokens">List of already added tokens</param>
    /// <param name="vendor">Vendor</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task AddVendorTokensAsync(IList<Token> tokens, Vendor vendor)
    {
        tokens.Add(new Token("Vendor.Name", vendor.Name));
        tokens.Add(new Token("Vendor.Email", vendor.Email));

        var vendorAttributesXml = await _genericAttributeService.GetAttributeAsync<string>(vendor, VendorDefaults.VendorAttributes);
        tokens.Add(new Token("Vendor.VendorAttributes", await _vendorAttributeFormatter.FormatAttributesAsync(vendorAttributesXml), true));

        //event notification
        await _eventPublisher.EntityTokensAddedAsync(vendor, tokens);
    }

    /// <summary>
    /// Add customer tokens
    /// </summary>
    /// <param name="tokens">List of already added tokens</param>
    /// <param name="customerId">Customer identifier</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task AddCustomerTokensAsync(IList<Token> tokens, int customerId)
    {
        if (customerId <= 0)
            throw new ArgumentOutOfRangeException(nameof(customerId));

        var customer = await _customerService.GetCustomerByIdAsync(customerId);

        await AddCustomerTokensAsync(tokens, customer);
    }

    /// <summary>
    /// Add customer tokens
    /// </summary>
    /// <param name="tokens">List of already added tokens</param>
    /// <param name="customer">Customer</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task AddCustomerTokensAsync(IList<Token> tokens, Customer customer)
    {
        tokens.Add(new Token("Customer.Email", customer.Email));
        tokens.Add(new Token("Customer.Username", customer.Username));
        tokens.Add(new Token("Customer.FullName", await _customerService.GetCustomerFullNameAsync(customer)));
        tokens.Add(new Token("Customer.FirstName", customer.FirstName));
        tokens.Add(new Token("Customer.LastName", customer.LastName));

        var customAttributesXml = customer.CustomCustomerAttributesXML;
        tokens.Add(new Token("Customer.CustomAttributes", await _customerAttributeFormatter.FormatAttributesAsync(customAttributesXml), true));

        //note: we do not use SEO friendly URLS for these links because we can get errors caused by having .(dot) in the URL (from the email address)
        var passwordRecoveryUrl = await RouteUrlAsync(routeName: "PasswordRecoveryConfirm", routeValues: new { token = await _genericAttributeService.GetAttributeAsync<string>(customer, CustomerDefaults.PasswordRecoveryTokenAttribute), guid = customer.CustomerGuid });
        var accountActivationUrl = await RouteUrlAsync(routeName: "AccountActivation", routeValues: new { token = await _genericAttributeService.GetAttributeAsync<string>(customer, CustomerDefaults.AccountActivationTokenAttribute), guid = customer.CustomerGuid });
        var emailRevalidationUrl = await RouteUrlAsync(routeName: "EmailRevalidation", routeValues: new { token = await _genericAttributeService.GetAttributeAsync<string>(customer, CustomerDefaults.EmailRevalidationTokenAttribute), guid = customer.CustomerGuid });
        var wishlistUrl = await RouteUrlAsync(routeName: "Wishlist", routeValues: new { customerGuid = customer.CustomerGuid });
        tokens.Add(new Token("Customer.PasswordRecoveryURL", passwordRecoveryUrl, true));
        tokens.Add(new Token("Customer.AccountActivationURL", accountActivationUrl, true));
        tokens.Add(new Token("Customer.EmailRevalidationURL", emailRevalidationUrl, true));
        tokens.Add(new Token("Wishlist.URLForCustomer", wishlistUrl, true));

        //event notification
        await _eventPublisher.EntityTokensAddedAsync(customer, tokens);
    }

    /// <summary>
    /// Add newsletter subscription tokens
    /// </summary>
    /// <param name="tokens">List of already added tokens</param>
    /// <param name="subscription">Newsletter subscription</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task AddNewsLetterSubscriptionTokensAsync(IList<Token> tokens, NewsLetterSubscription subscription)
    {
        tokens.Add(new Token("NewsLetterSubscription.Email", subscription.Email));

        var activationUrl = await RouteUrlAsync(routeName: "NewsletterActivation", routeValues: new { token = subscription.NewsLetterSubscriptionGuid, active = "true" });
        tokens.Add(new Token("NewsLetterSubscription.ActivationUrl", activationUrl, true));

        var deactivationUrl = await RouteUrlAsync(routeName: "NewsletterActivation", routeValues: new { token = subscription.NewsLetterSubscriptionGuid, active = "false" });
        tokens.Add(new Token("NewsLetterSubscription.DeactivationUrl", deactivationUrl, true));

        //event notification
        await _eventPublisher.EntityTokensAddedAsync(subscription, tokens);
    }

    /// <summary>
    /// Get collection of allowed (supported) message tokens
    /// </summary>
    /// <param name="tokenGroups">Collection of token groups; pass null to get all available tokens</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the collection of allowed message tokens
    /// </returns>
    public virtual async Task<IEnumerable<string>> GetListOfAllowedTokensAsync(IEnumerable<string> tokenGroups = null)
    {
        var additionalTokens = new AdditionalTokensAddedEvent
        {
            TokenGroups = tokenGroups
        };
        await _eventPublisher.PublishAsync(additionalTokens);

        var allowedTokens = AllowedTokens.Where(x => tokenGroups == null || tokenGroups.Contains(x.Key))
            .SelectMany(x => x.Value).ToList();

        allowedTokens.AddRange(additionalTokens.AdditionalTokens);

        return allowedTokens.Distinct();
    }

    /// <summary>
    /// Get token groups of message template
    /// </summary>
    /// <param name="messageTemplate">Message template</param>
    /// <returns>Collection of token group names</returns>
    public virtual IEnumerable<string> GetTokenGroups(MessageTemplate messageTemplate)
    {
        //groups depend on which tokens are added at the appropriate methods in IWorkflowMessageService
        return messageTemplate.Name switch
        {
            MessageTemplateSystemNames.CUSTOMER_REGISTERED_SITE_OWNER_NOTIFICATION or
                MessageTemplateSystemNames.CUSTOMER_WELCOME_MESSAGE or
                MessageTemplateSystemNames.CUSTOMER_EMAIL_VALIDATION_MESSAGE or
                MessageTemplateSystemNames.CUSTOMER_EMAIL_REVALIDATION_MESSAGE or
                MessageTemplateSystemNames.CUSTOMER_PASSWORD_RECOVERY_MESSAGE or
                MessageTemplateSystemNames.DELETE_CUSTOMER_REQUEST_SITE_OWNER_NOTIFICATION => new[] { TokenGroupNames.SiteTokens, TokenGroupNames.CustomerTokens },

            MessageTemplateSystemNames.NEWSLETTER_SUBSCRIPTION_ACTIVATION_MESSAGE or
            MessageTemplateSystemNames.NEWSLETTER_SUBSCRIPTION_DEACTIVATION_MESSAGE => [TokenGroupNames.SiteTokens, TokenGroupNames.SubscriptionTokens],

            MessageTemplateSystemNames.NEW_FORUM_TOPIC_MESSAGE => [TokenGroupNames.SiteTokens, TokenGroupNames.ForumTopicTokens, TokenGroupNames.ForumTokens, TokenGroupNames.CustomerTokens],
            MessageTemplateSystemNames.NEW_FORUM_POST_MESSAGE => [TokenGroupNames.SiteTokens, TokenGroupNames.ForumPostTokens, TokenGroupNames.ForumTopicTokens, TokenGroupNames.ForumTokens, TokenGroupNames.CustomerTokens],
            MessageTemplateSystemNames.PRIVATE_MESSAGE_NOTIFICATION => [TokenGroupNames.SiteTokens, TokenGroupNames.PrivateMessageTokens, TokenGroupNames.CustomerTokens],

            MessageTemplateSystemNames.BLOG_COMMENT_SITE_OWNER_NOTIFICATION => [TokenGroupNames.SiteTokens, TokenGroupNames.BlogCommentTokens, TokenGroupNames.CustomerTokens],
            MessageTemplateSystemNames.NEWS_COMMENT_SITE_OWNER_NOTIFICATION => [TokenGroupNames.SiteTokens, TokenGroupNames.NewsCommentTokens, TokenGroupNames.CustomerTokens],
            MessageTemplateSystemNames.CONTACT_US_MESSAGE => [TokenGroupNames.SiteTokens, TokenGroupNames.ContactUs],

            _ => [],
        };
    }

    #endregion Methods
}