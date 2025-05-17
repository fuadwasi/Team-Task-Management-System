using AssetForge.Core;
using AssetForge.Core.Domain.Common;
using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Directory;
using AssetForge.Core.Domain.Localization;
using AssetForge.Core.Domain.Security;
using AssetForge.Core.Domain.Seo;
using AssetForge.Services.Attributes;
using AssetForge.Services.Common;
using AssetForge.Services.Customers;
using AssetForge.Services.Directory;
using AssetForge.Services.ExportImport.Help;
using AssetForge.Services.Helpers;
using AssetForge.Services.Localization;
using AssetForge.Services.Logging;
using AssetForge.Services.Media;
using AssetForge.Services.Seo;
using AssetForge.Services.Sites;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml;

namespace AssetForge.Services.ExportImport;

/// <summary>
/// Export manager
/// </summary>
public partial class ExportManager : IExportManager
{
    #region Fields

    protected readonly AddressSettings _addressSettings;
    protected readonly SecuritySettings _securitySettings;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly CustomerSettings _customerSettings;
    protected readonly DateTimeSettings _dateTimeSettings;
    protected readonly IAddressService _addressService;
    protected readonly ICountryService _countryService;
    protected readonly IAttributeFormatter<CustomerAttribute, CustomerAttributeValue> _customerAttributeFormatter;
    protected readonly ICustomerService _customerService;
    protected readonly IDateTimeHelper _dateTimeHelper;
    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly ILanguageService _languageService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedEntityService _localizedEntityService;
    protected readonly IPictureService _pictureService;
    protected readonly IStateProvinceService _stateProvinceService;
    protected readonly ISiteMappingService _siteMappingService;
    protected readonly ISiteService _siteService;
    protected readonly IUrlRecordService _urlRecordService;
    protected readonly IWorkContext _workContext;

    #endregion

    #region Ctor

    public ExportManager(AddressSettings addressSettings,
        SecuritySettings securitySettings,
        CustomerSettings customerSettings,
        DateTimeSettings dateTimeSettings,
        IAddressService addressService,
        IAttributeFormatter<CustomerAttribute, CustomerAttributeValue> customerAttributeFormatter,
        ICountryService countryService,
        ICustomerActivityService customerActivityService,
        ICustomerService customerService,
        IDateTimeHelper dateTimeHelper,
        IGenericAttributeService genericAttributeService,
        ILanguageService languageService,
        ILocalizationService localizationService,
        ILocalizedEntityService localizedEntityService,
        IPictureService pictureService,
        IStateProvinceService stateProvinceService,
        ISiteMappingService siteMappingService,
        ISiteService siteService,
        IUrlRecordService urlRecordService,
        IWorkContext workContext)
    {
        _addressSettings = addressSettings;
        _securitySettings = securitySettings;
        _customerSettings = customerSettings;
        _dateTimeSettings = dateTimeSettings;
        _addressService = addressService;
        _customerAttributeFormatter = customerAttributeFormatter;
        _countryService = countryService;
        _customerActivityService = customerActivityService;
        _customerService = customerService;
        _dateTimeHelper = dateTimeHelper;
        _genericAttributeService = genericAttributeService;
        _languageService = languageService;
        _localizationService = localizationService;
        _localizedEntityService = localizedEntityService;
        _pictureService = pictureService;
        _stateProvinceService = stateProvinceService;
        _siteMappingService = siteMappingService;
        _siteService = siteService;
        _urlRecordService = urlRecordService;
        _workContext = workContext;
    }

    #endregion

    #region Utilities

    /// <summary>
    /// Returns the path to the image file by ID
    /// </summary>
    /// <param name="pictureId">Picture ID</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the path to the image file
    /// </returns>
    protected virtual async Task<string> GetPicturesAsync(int pictureId)
    {
        var picture = await _pictureService.GetPictureByIdAsync(pictureId);

        return await _pictureService.GetThumbLocalPathAsync(picture);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    protected virtual async Task<bool> IgnoreExportCategoryPropertyAsync()
    {
        try
        {
            return !await _genericAttributeService.GetAttributeAsync<bool>(await _workContext.GetCurrentCustomerAsync(), "category-advanced-mode");
        }
        catch (ArgumentNullException)
        {
            return false;
        }
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    protected virtual async Task<bool> IgnoreExportManufacturerPropertyAsync()
    {
        try
        {
            return !await _genericAttributeService.GetAttributeAsync<bool>(await _workContext.GetCurrentCustomerAsync(), "manufacturer-advanced-mode");
        }
        catch (ArgumentNullException)
        {
            return false;
        }
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    protected virtual async Task<TProperty> GetLocalizedAsync<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> keySelector,
        Language language) where TEntity : BaseEntity, ILocalizedEntity
    {
        if (entity == null)
            return default;

        return await _localizationService.GetLocalizedAsync(entity, keySelector, language.Id, false);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    protected virtual async Task<object> GetCustomCustomerAttributesAsync(Customer customer)
    {
        return await _customerAttributeFormatter.FormatAttributesAsync(customer.CustomCustomerAttributesXML, ";");
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    protected virtual async Task WriteLocalizedPropertyXmlAsync<TEntity, TPropType>(TEntity entity, Expression<Func<TEntity, TPropType>> keySelector,
        XmlWriter xmlWriter, IList<Language> languages, bool ignore = false, string overriddenNodeName = null)
        where TEntity : BaseEntity, ILocalizedEntity
    {
        if (ignore)
            return;

        ArgumentNullException.ThrowIfNull(entity);

        if (keySelector.Body is not MemberExpression member)
            throw new ArgumentException($"Expression '{keySelector}' refers to a method, not a property.");

        if (member.Member is not PropertyInfo propInfo)
            throw new ArgumentException($"Expression '{keySelector}' refers to a field, not a property.");

        var localeKeyGroup = entity.GetType().Name;
        var localeKey = propInfo.Name;

        var nodeName = localeKey;
        if (!string.IsNullOrWhiteSpace(overriddenNodeName))
            nodeName = overriddenNodeName;

        await xmlWriter.WriteStartElementAsync(nodeName);
        await xmlWriter.WriteStringAsync("Standard", propInfo.GetValue(entity));

        if (languages.Count >= 2)
        {
            await xmlWriter.WriteStartElementAsync("Locales");

            var properties = await _localizedEntityService.GetEntityLocalizedPropertiesAsync(entity.Id, localeKeyGroup, localeKey);
            foreach (var language in languages)
                if (properties.FirstOrDefault(lp => lp.LanguageId == language.Id) is LocalizedProperty localizedProperty)
                    await xmlWriter.WriteStringAsync(language.UniqueSeoCode, localizedProperty.LocaleValue);

            await xmlWriter.WriteEndElementAsync();
        }

        await xmlWriter.WriteEndElementAsync();
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    protected virtual async Task WriteLocalizedSeNameXmlAsync<TEntity>(TEntity entity, XmlWriter xmlWriter, IList<Language> languages,
        bool ignore = false, string overriddenNodeName = null)
        where TEntity : BaseEntity, ISlugSupported
    {
        if (ignore)
            return;

        ArgumentNullException.ThrowIfNull(entity);

        var nodeName = "SEName";
        if (!string.IsNullOrWhiteSpace(overriddenNodeName))
            nodeName = overriddenNodeName;

        await xmlWriter.WriteStartElementAsync(nodeName);
        await xmlWriter.WriteStringAsync("Standard", await _urlRecordService.GetSeNameAsync(entity, 0));

        if (languages.Count >= 2)
        {
            await xmlWriter.WriteStartElementAsync("Locales");

            foreach (var language in languages)
                if (await _urlRecordService.GetSeNameAsync(entity, language.Id, returnDefaultValue: false) is string seName && !string.IsNullOrWhiteSpace(seName))
                    await xmlWriter.WriteStringAsync(language.UniqueSeoCode, seName);

            await xmlWriter.WriteEndElementAsync();
        }

        await xmlWriter.WriteEndElementAsync();
    }

    #endregion

    #region Methods

    public virtual async Task<string> ExportCustomersToXmlAsync(IList<Customer> customers)
    {
        var settings = new XmlWriterSettings
        {
            Async = true,
            ConformanceLevel = ConformanceLevel.Auto
        };

        await using var stringWriter = new StringWriter();
        await using var xmlWriter = XmlWriter.Create(stringWriter, settings);

        await xmlWriter.WriteStartDocumentAsync();
        await xmlWriter.WriteStartElementAsync("Customers");
        await xmlWriter.WriteAttributeStringAsync("Version", AssetForgeVersion.CURRENT_VERSION);

        foreach (var customer in customers)
        {
            await xmlWriter.WriteStartElementAsync("Customer");
            await xmlWriter.WriteElementStringAsync("CustomerId", null, customer.Id.ToString());
            await xmlWriter.WriteElementStringAsync("CustomerGuid", null, customer.CustomerGuid.ToString());
            await xmlWriter.WriteElementStringAsync("Email", null, customer.Email);
            await xmlWriter.WriteElementStringAsync("Username", null, customer.Username);

            await xmlWriter.WriteElementStringAsync("Active", null, customer.Active.ToString());

            await xmlWriter.WriteElementStringAsync("IsGuest", null, (await _customerService.IsGuestAsync(customer)).ToString());
            await xmlWriter.WriteElementStringAsync("IsRegistered", null, (await _customerService.IsRegisteredAsync(customer)).ToString());
            await xmlWriter.WriteElementStringAsync("IsAdministrator", null, (await _customerService.IsAdminAsync(customer)).ToString());
            //await xmlWriter.WriteElementStringAsync("IsForumModerator", null, (await _customerService.IsForumModeratorAsync(customer)).ToString());
            await xmlWriter.WriteElementStringAsync("CreatedOnUtc", null, customer.CreatedOnUtc.ToString(CultureInfo.InvariantCulture));

            await xmlWriter.WriteElementStringAsync("FirstName", null, customer.FirstName);
            await xmlWriter.WriteElementStringAsync("LastName", null, customer.LastName);
            await xmlWriter.WriteElementStringAsync("Gender", null, customer.Gender);
            await xmlWriter.WriteElementStringAsync("Company", null, customer.Company);

            await xmlWriter.WriteElementStringAsync("CountryId", null, customer.CountryId.ToString());
            await xmlWriter.WriteElementStringAsync("StreetAddress", null, customer.StreetAddress);
            await xmlWriter.WriteElementStringAsync("StreetAddress2", null, customer.StreetAddress2);
            await xmlWriter.WriteElementStringAsync("ZipPostalCode", null, customer.ZipPostalCode);
            await xmlWriter.WriteElementStringAsync("City", null, customer.City);
            await xmlWriter.WriteElementStringAsync("County", null, customer.County);
            await xmlWriter.WriteElementStringAsync("StateProvinceId", null, customer.StateProvinceId.ToString());
            await xmlWriter.WriteElementStringAsync("Phone", null, customer.Phone);
            await xmlWriter.WriteElementStringAsync("Fax", null, customer.Fax);
            await xmlWriter.WriteElementStringAsync("TimeZoneId", null, customer.TimeZoneId);

            await xmlWriter.WriteElementStringAsync("AvatarPictureId", null, (await _genericAttributeService.GetAttributeAsync<int>(customer, CustomerDefaults.AvatarPictureIdAttribute)).ToString());
            await xmlWriter.WriteElementStringAsync("ForumPostCount", null, (await _genericAttributeService.GetAttributeAsync<int>(customer, CustomerDefaults.ForumPostCountAttribute)).ToString());
            await xmlWriter.WriteElementStringAsync("Signature", null, await _genericAttributeService.GetAttributeAsync<string>(customer, CustomerDefaults.SignatureAttribute));

            if (!string.IsNullOrEmpty(customer.CustomCustomerAttributesXML))
            {
                var selectedCustomerAttributes = new StringReader(customer.CustomCustomerAttributesXML);
                var selectedCustomerAttributesXmlReader = XmlReader.Create(selectedCustomerAttributes);
                await xmlWriter.WriteNodeAsync(selectedCustomerAttributesXmlReader, false);
            }

            await xmlWriter.WriteEndElementAsync();
        }

        await xmlWriter.WriteEndElementAsync();
        await xmlWriter.WriteEndDocumentAsync();
        await xmlWriter.FlushAsync();

        //activity log
        await _customerActivityService.InsertActivityAsync("ExportCustomers",
            string.Format(await _localizationService.GetResourceAsync("ActivityLog.ExportCustomers"), customers.Count));

        return stringWriter.ToString();
    }

    public virtual async Task<string> ExportStatesToTxtAsync(IList<StateProvince> states)
    {
        ArgumentNullException.ThrowIfNull(states);

        const char separator = ',';
        var sb = new StringBuilder();
        foreach (var state in states)
        {
            sb.Append((await _countryService.GetCountryByIdAsync(state.CountryId)).TwoLetterIsoCode);
            sb.Append(separator);
            sb.Append(state.Name);
            sb.Append(separator);
            sb.Append(state.Abbreviation);
            sb.Append(separator);
            sb.Append(state.Published);
            sb.Append(separator);
            sb.Append(state.DisplayOrder);
            sb.Append(Environment.NewLine); //new line
        }

        //activity log
        await _customerActivityService.InsertActivityAsync("ExportStates",
            string.Format(await _localizationService.GetResourceAsync("ActivityLog.ExportStates"), states.Count));

        return sb.ToString();
    }

    public virtual async Task<byte[]> ExportCustomersToXlsxAsync(IList<Customer> customers)
    {
        async Task<object> getCountry(Customer customer)
        {
            var countryId = customer.CountryId;

            //if (!_catalogSettings.ExportImportRelatedEntitiesByName)
                return countryId;

            var country = await _countryService.GetCountryByIdAsync(countryId);

            return country?.Name ?? string.Empty;
        }

        async Task<object> getStateProvince(Customer customer)
        {
            var stateProvinceId = customer.StateProvinceId;

            //if (!_catalogSettings.ExportImportRelatedEntitiesByName)
                return stateProvinceId;

            var stateProvince = await _stateProvinceService.GetStateProvinceByIdAsync(stateProvinceId);

            return stateProvince?.Name ?? string.Empty;
        }

        //property manager 
        var manager = new PropertyManager<Customer, Language>(new[]
        {
            new PropertyByName<Customer, Language>("CustomerId", (p, l) => p.Id),
            new PropertyByName<Customer, Language>("CustomerGuid", (p, l) => p.CustomerGuid),
            new PropertyByName<Customer, Language>("Email", (p, l) => p.Email),
            new PropertyByName<Customer, Language>("Username", (p, l) => p.Username),
            new PropertyByName<Customer, Language>("Active", (p, l) => p.Active),
            new PropertyByName<Customer, Language>("CustomerRoles",  async (p, l) =>  string.Join(", ",
                (await _customerService.GetCustomerRolesAsync(p)).Select(role => role.Name))),
            new PropertyByName<Customer, Language>("IsGuest", async (p, l) => await _customerService.IsGuestAsync(p)),
            new PropertyByName<Customer, Language>("IsRegistered", async (p, l) => await _customerService.IsRegisteredAsync(p)),
            new PropertyByName<Customer, Language>("IsAdministrator", async (p, l) => await _customerService.IsAdminAsync(p)),
            //new PropertyByName<Customer, Language>("IsForumModerator", async (p, l) => await _customerService.IsForumModeratorAsync(p)),
            new PropertyByName<Customer, Language>("CreatedOnUtc", (p, l) => p.CreatedOnUtc),
            //attributes
            new PropertyByName<Customer, Language>("FirstName", (p, l) => p.FirstName, !_customerSettings.FirstNameEnabled),
            new PropertyByName<Customer, Language>("LastName", (p, l) => p.LastName, !_customerSettings.LastNameEnabled),
            new PropertyByName<Customer, Language>("Gender", (p, l) => p.Gender, !_customerSettings.GenderEnabled),
            new PropertyByName<Customer, Language>("Company", (p, l) => p.Company, !_customerSettings.CompanyEnabled),
            new PropertyByName<Customer, Language>("StreetAddress", (p, l) => p.StreetAddress, !_customerSettings.StreetAddressEnabled),
            new PropertyByName<Customer, Language>("StreetAddress2", (p, l) => p.StreetAddress2, !_customerSettings.StreetAddress2Enabled),
            new PropertyByName<Customer, Language>("ZipPostalCode", (p, l) => p.ZipPostalCode, !_customerSettings.ZipPostalCodeEnabled),
            new PropertyByName<Customer, Language>("City", (p, l) => p.City, !_customerSettings.CityEnabled),
            new PropertyByName<Customer, Language>("County", (p, l) => p.County, !_customerSettings.CountyEnabled),
            new PropertyByName<Customer, Language>("Country",  async (p, l) => await getCountry(p), !_customerSettings.CountryEnabled),
            new PropertyByName<Customer, Language>("StateProvince",  async (p, l) => await getStateProvince(p), !_customerSettings.StateProvinceEnabled),
            new PropertyByName<Customer, Language>("Phone", (p, l) => p.Phone, !_customerSettings.PhoneEnabled),
            new PropertyByName<Customer, Language>("Fax", (p, l) => p.Fax, !_customerSettings.FaxEnabled),
            new PropertyByName<Customer, Language>("TimeZone", (p, l) => p.TimeZoneId, !_dateTimeSettings.AllowCustomersToSetTimeZone),
            new PropertyByName<Customer, Language>("AvatarPictureId", async (p, l) => await _genericAttributeService.GetAttributeAsync<int>(p, CustomerDefaults.AvatarPictureIdAttribute), !_customerSettings.AllowCustomersToUploadAvatars),
            new PropertyByName<Customer, Language>("ForumPostCount", async (p, l) => await _genericAttributeService.GetAttributeAsync<int>(p, CustomerDefaults.ForumPostCountAttribute)),
            new PropertyByName<Customer, Language>("Signature", async (p, l) => await _genericAttributeService.GetAttributeAsync<string>(p, CustomerDefaults.SignatureAttribute)),
            new PropertyByName<Customer, Language>("CustomCustomerAttributes", async (p, l) => await GetCustomCustomerAttributesAsync(p)),
            new PropertyByName<Customer, Language>("CustomCustomerAttributesXML", (p, l) => p.CustomCustomerAttributesXML),
            new PropertyByName<Customer, Language>("Password", async (p, l) =>
            {
                if (!_securitySettings.AllowSiteOwnerExportImportCustomersWithHashedPassword)
                    return string.Empty;

                var password = await _customerService.GetCurrentPasswordAsync(p.Id);

                if(password == null)
                    return string.Empty;

                if (password.PasswordFormat == PasswordFormat.Hashed)
                    return password.Password;

                return string.Empty;
            },  !_securitySettings.AllowSiteOwnerExportImportCustomersWithHashedPassword),
            new PropertyByName<Customer, Language>("PasswordSalt", async (p, l) =>
            {
                if (!_securitySettings.AllowSiteOwnerExportImportCustomersWithHashedPassword)
                    return string.Empty;

                var password = await _customerService.GetCurrentPasswordAsync(p.Id);

                if(password == null)
                    return string.Empty;

                if (password.PasswordFormat == PasswordFormat.Hashed)
                    return password.PasswordSalt;

                return string.Empty;

            }, !_securitySettings.AllowSiteOwnerExportImportCustomersWithHashedPassword),

        });

        //activity log
        await _customerActivityService.InsertActivityAsync("ExportCustomers",
            string.Format(await _localizationService.GetResourceAsync("ActivityLog.ExportCustomers"), customers.Count));

        return await manager.ExportToXlsxAsync(customers);
    }

    #endregion
}