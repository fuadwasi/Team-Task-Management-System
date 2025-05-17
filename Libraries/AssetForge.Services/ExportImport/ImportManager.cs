using AssetForge.Core;
using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Directory;
using AssetForge.Core.Domain.Localization;
using AssetForge.Core.Domain.Media;
using AssetForge.Core.Domain.Security;
using AssetForge.Core.Http;
using AssetForge.Core.Infrastructure;
using AssetForge.Data;
using AssetForge.Services.Common;
using AssetForge.Services.Customers;
using AssetForge.Services.Directory;
using AssetForge.Services.ExportImport.Help;
using AssetForge.Services.Localization;
using AssetForge.Services.Logging;
using AssetForge.Services.Media;
using AssetForge.Services.Seo;
using AssetForge.Services.Sites;
using ClosedXML.Excel;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;

namespace AssetForge.Services.ExportImport;

/// <summary>
/// Import manager
/// </summary>
public partial class ImportManager : IImportManager
{
    #region Fields

    protected readonly IAddressService _addressService;
    protected readonly ICountryService _countryService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ICustomerService _customerService;
    protected readonly IDataProvider _dataProvider;
    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly IHttpClientFactory _httpClientFactory;
    protected readonly ILanguageService _languageService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedEntityService _localizedEntityService;
    protected readonly ILogger _logger;
    protected readonly IAssetForgeFileProvider _fileProvider;
    protected readonly IPictureService _pictureService;
    protected readonly IServiceScopeFactory _serviceScopeFactory;
    protected readonly IStateProvinceService _stateProvinceService;
    protected readonly ISiteContext _siteContext;
    protected readonly ISiteMappingService _siteMappingService;
    protected readonly ISiteService _siteService;
    protected readonly IUrlRecordService _urlRecordService;
    protected readonly IWorkContext _workContext;
    protected readonly MediaSettings _mediaSettings;
    protected readonly SecuritySettings _securitySettings;
    private static readonly string[] _separator = [">>"];

    #endregion

    #region Ctor

    public ImportManager(
        IAddressService addressService,
        ICountryService countryService,
        ICustomerActivityService customerActivityService,
        ICustomerService customerService,
        IDataProvider dataProvider,
        IGenericAttributeService genericAttributeService,
        IHttpClientFactory httpClientFactory,
        ILanguageService languageService,
        ILocalizationService localizationService,
        ILocalizedEntityService localizedEntityService,
        ILogger logger,
        IAssetForgeFileProvider fileProvider,
        IPictureService pictureService,
        IServiceScopeFactory serviceScopeFactory,
        IStateProvinceService stateProvinceService,
        ISiteContext siteContext,
        ISiteMappingService siteMappingService,
        ISiteService siteService,
        IUrlRecordService urlRecordService,
        IWorkContext workContext,
        MediaSettings mediaSettings,
        SecuritySettings securitySettings
        )
    {
        _addressService = addressService;
        _countryService = countryService;
        _customerActivityService = customerActivityService;
        _customerService = customerService;
        _dataProvider = dataProvider;
        _genericAttributeService = genericAttributeService;
        _httpClientFactory = httpClientFactory;
        _fileProvider = fileProvider;
        _languageService = languageService;
        _localizationService = localizationService;
        _localizedEntityService = localizedEntityService;
        _logger = logger;
        _pictureService = pictureService;
        _serviceScopeFactory = serviceScopeFactory;
        _stateProvinceService = stateProvinceService;
        _siteContext = siteContext;
        _siteMappingService = siteMappingService;
        _siteService = siteService;
        _urlRecordService = urlRecordService;
        _workContext = workContext;
        _mediaSettings = mediaSettings;
        _securitySettings = securitySettings;
    }

    #endregion

    #region Utilities

    //protected virtual ExportedAttributeType GetTypeOfExportedAttribute(IXLWorksheet defaultWorksheet, List<IXLWorksheet> localizedWorksheets, PropertyManager<ExportProductAttribute, Language> productAttributeManager, PropertyManager<ExportSpecificationAttribute, Language> specificationAttributeManager, int iRow)
    //{
    //    productAttributeManager.ReadDefaultFromXlsx(defaultWorksheet, iRow, ExportProductAttribute.ProductAttributeCellOffset);

    //    if (productAttributeManager.IsCaption)
    //    {
    //        foreach (var worksheet in localizedWorksheets)
    //            productAttributeManager.ReadLocalizedFromXlsx(worksheet, iRow, ExportProductAttribute.ProductAttributeCellOffset);

    //        return ExportedAttributeType.ProductAttribute;
    //    }

    //    specificationAttributeManager.ReadDefaultFromXlsx(defaultWorksheet, iRow, ExportProductAttribute.ProductAttributeCellOffset);

    //    if (specificationAttributeManager.IsCaption)
    //    {
    //        foreach (var worksheet in localizedWorksheets)
    //            specificationAttributeManager.ReadLocalizedFromXlsx(worksheet, iRow, ExportProductAttribute.ProductAttributeCellOffset);

    //        return ExportedAttributeType.SpecificationAttribute;
    //    }

    //    return ExportedAttributeType.NotSpecified;
    //}

    ///// <returns>A task that represents the asynchronous operation</returns>
    //protected virtual async Task SetOutLineForSpecificationAttributeRowAsync(object cellValue, IXLWorksheet worksheet, int endRow)
    //{
    //    var attributeType = (cellValue ?? string.Empty).ToString();

    //    if (attributeType.Equals("AttributeType", StringComparison.InvariantCultureIgnoreCase))
    //    {
    //        worksheet.Row(endRow).OutlineLevel = 1;
    //    }
    //    else
    //    {
    //        if ((await SpecificationAttributeType.Option.ToSelectListAsync(useLocalization: false))
    //            .Any(p => p.Text.Equals(attributeType, StringComparison.InvariantCultureIgnoreCase)))
    //            worksheet.Row(endRow).OutlineLevel = 1;
    //        else if (int.TryParse(attributeType, out var attributeTypeId) && Enum.IsDefined(typeof(SpecificationAttributeType), attributeTypeId))
    //            worksheet.Row(endRow).OutlineLevel = 1;
    //    }
    //}

    //protected virtual void CopyDataToNewFile(ImportProductMetadata metadata, IXLWorksheet worksheet, string filePath, int startRow, int endRow, int endCell)
    //{
    //    using var workbook = new XLWorkbook();

    //    // get handles to the worksheets
    //    var outWorksheet = workbook.Worksheets.Add(nameof(Product));
    //    metadata.Manager.WriteDefaultCaption(outWorksheet);
    //    var outRow = 2;
    //    for (var row = startRow; row <= endRow; row++)
    //    {
    //        outWorksheet.Row(outRow).OutlineLevel = worksheet.Row(row).OutlineLevel;

    //        for (var cell = 1; cell <= endCell; cell++)
    //            outWorksheet.Row(outRow).Cell(cell).Value = worksheet.Row(row).Cell(cell).Value;

    //        outRow += 1;
    //    }

    //    workbook.SaveAs(filePath);
    //}

    protected virtual int GetColumnIndex(string[] properties, string columnName)
    {
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentNullException.ThrowIfNull(columnName);

        for (var i = 0; i < properties.Length; i++)
            if (properties[i].Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                return i + 1; //excel indexes start from 1
        return 0;
    }

    protected virtual string GetMimeTypeFromFilePath(string filePath)
    {
        new FileExtensionContentTypeProvider().TryGetContentType(filePath, out var mimeType);

        //set to jpeg in case mime type cannot be found
        return mimeType ?? _pictureService.GetPictureContentTypeByFileExtension(_fileProvider.GetFileExtension(filePath));
    }

    /// <summary>
    /// Creates or loads the image
    /// </summary>
    /// <param name="picturePath">The path to the image file</param>
    /// <param name="name">The name of the object</param>
    /// <param name="picId">Image identifier, may be null</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the image or null if the image has not changed
    /// </returns>
    protected virtual async Task<Picture> LoadPictureAsync(string picturePath, string name, int? picId = null)
    {
        if (string.IsNullOrEmpty(picturePath) || !_fileProvider.FileExists(picturePath))
            return null;

        var mimeType = GetMimeTypeFromFilePath(picturePath);
        if (string.IsNullOrEmpty(mimeType))
            return null;

        var newPictureBinary = await _fileProvider.ReadAllBytesAsync(picturePath);
        var pictureAlreadyExists = false;
        if (picId != null)
        {
            //compare with existing product pictures
            var existingPicture = await _pictureService.GetPictureByIdAsync(picId.Value);
            if (existingPicture != null)
            {
                var existingBinary = await _pictureService.LoadPictureBinaryAsync(existingPicture);
                //picture binary after validation (like in database)
                var validatedPictureBinary = await _pictureService.ValidatePictureAsync(newPictureBinary, mimeType, name);
                if (existingBinary.SequenceEqual(validatedPictureBinary) ||
                    existingBinary.SequenceEqual(newPictureBinary))
                {
                    pictureAlreadyExists = true;
                }
            }
        }

        if (pictureAlreadyExists)
            return null;

        var newPicture = await _pictureService.InsertPictureAsync(newPictureBinary, mimeType, await _pictureService.GetPictureSeNameAsync(name));
        return newPicture;
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    protected virtual async Task LogPictureInsertErrorAsync(string picturePath, Exception ex)
    {
        var extension = _fileProvider.GetFileExtension(picturePath);
        var name = _fileProvider.GetFileNameWithoutExtension(picturePath);

        var point = string.IsNullOrEmpty(extension) ? string.Empty : ".";
        var fileName = _fileProvider.FileExists(picturePath) ? $"{name}{point}{extension}" : string.Empty;

        await _logger.ErrorAsync($"Insert picture failed (file name: {fileName})", ex);
    }

    /// <returns>A task that represents the asynchronous operation</returns>
    protected virtual async Task<string> DownloadFileAsync(string urlString, IList<string> downloadedFiles)
    {
        if (string.IsNullOrEmpty(urlString))
            return string.Empty;

        if (!Uri.IsWellFormedUriString(urlString, UriKind.Absolute))
            return urlString;

        //ensure that temp directory is created
        var tempDirectory = _fileProvider.MapPath(ExportImportDefaults.UploadsTempPath);
        _fileProvider.CreateDirectory(tempDirectory);

        var fileName = _fileProvider.GetFileName(urlString);
        if (string.IsNullOrEmpty(fileName))
            return string.Empty;

        var filePath = _fileProvider.Combine(tempDirectory, fileName);
        try
        {
            var client = _httpClientFactory.CreateClient(HttpDefaults.DefaultHttpClient);
            var fileData = await client.GetByteArrayAsync(urlString);
            await using (var fs = new FileStream(filePath, FileMode.OpenOrCreate))
                fs.Write(fileData, 0, fileData.Length);

            downloadedFiles?.Add(filePath);
            return filePath;
        }
        catch (Exception ex)
        {
            await _logger.ErrorAsync("Download image failed", ex);
        }

        return string.Empty;
    }

    #endregion

    #region Methods

    public static WorkbookMetadata<T> GetWorkbookMetadata<T>(IXLWorkbook workbook, IList<Language> languages)
    {
        // get the first worksheet in the workbook
        var worksheet = workbook.Worksheets.FirstOrDefault()
                        ?? throw new AssetForgeException("No worksheet found");

        var properties = new List<PropertyByName<T, Language>>();
        var localizedProperties = new List<PropertyByName<T, Language>>();
        var localizedWorksheets = new List<IXLWorksheet>();

        var poz = 1;
        while (true)
        {
            try
            {
                var cell = worksheet.Row(1).Cell(poz);

                if (string.IsNullOrEmpty(cell?.Value.ToString()))
                    break;

                poz += 1;
                properties.Add(new PropertyByName<T, Language>(cell.Value.ToString()));
            }
            catch
            {
                break;
            }
        }

        foreach (var ws in workbook.Worksheets.Skip(1))
            if (languages.Any(l => l.UniqueSeoCode.Equals(ws.Name, StringComparison.InvariantCultureIgnoreCase)))
                localizedWorksheets.Add(ws);

        if (localizedWorksheets.Any())
        {
            // get the first worksheet in the workbook
            var localizedWorksheet = localizedWorksheets.First();

            poz = 1;
            while (true)
            {
                try
                {
                    var cell = localizedWorksheet.Row(1).Cell(poz);

                    if (string.IsNullOrEmpty(cell?.Value.ToString()))
                        break;

                    poz += 1;
                    localizedProperties.Add(new PropertyByName<T, Language>(cell.Value.ToString()));
                }
                catch
                {
                    break;
                }
            }
        }

        return new WorkbookMetadata<T>()
        {
            DefaultProperties = properties,
            LocalizedProperties = localizedProperties,
            DefaultWorksheet = worksheet,
            LocalizedWorksheets = localizedWorksheets
        };
    }

    public virtual async Task ImportCustomersFromXlsxAsync(Stream stream)
    {
        using var workbook = new XLWorkbook(stream);

        var languages = await _languageService.GetAllLanguagesAsync(showHidden: true);

        //the columns
        var metadata = GetWorkbookMetadata<Customer>(workbook, languages);
        var defaultWorksheet = metadata.DefaultWorksheet;
        var defaultProperties = metadata.DefaultProperties;

        var manager = new PropertyManager<Customer, Language>(defaultProperties);

        var iRow = 2;
        var rolesToSave = new List<int>();
        var allRoles = await _customerService.GetAllCustomerRolesAsync();
        var countries = await _countryService.GetAllCountriesAsync();
        var states = await _stateProvinceService.GetStateProvincesAsync();

        while (true)
        {
            var allColumnsAreEmpty = manager.GetDefaultProperties
                .Select(property => defaultWorksheet.Row(iRow).Cell(property.PropertyOrderPosition))
                .All(cell => cell?.Value == null || string.IsNullOrEmpty(cell.Value.ToString()));

            if (allColumnsAreEmpty)
                break;

            manager.ReadDefaultFromXlsx(defaultWorksheet, iRow);

            var customerGuid = manager.GetDefaultProperty("CustomerGuid").GuidValue;
            var customer = await _customerService.GetCustomerByGuidAsync(customerGuid) ??
                           await _customerService.GetCustomerByEmailAsync(manager.GetDefaultProperty("Email").StringValue);

            int? avatarPictureId = null;
            string signature = null;
            string password = null;
            string passwordSalt = null;

            var isNew = customer == null;

            if (isNew)
                customer = new Customer
                {
                    CustomerGuid = Guid.Empty.Equals(customerGuid) ? Guid.NewGuid() : customerGuid,
                    CreatedOnUtc = DateTime.UtcNow
                };

            foreach (var property in manager.GetDefaultProperties)
            {
                switch (property.PropertyName)
                {
                    case "Email":
                        customer.Email = property.StringValue;
                        break;
                    case "Username":
                        customer.Username = property.StringValue;
                        break;
                    case "Active":
                        customer.Active = property.BooleanValue;
                        break;
                    case "CustomerRoles":
                        var roles = property.StringValue.Split(", ");

                        foreach (var role in roles)
                            if (int.TryParse(role, out var roleId))
                                rolesToSave.Add(roleId);
                            else
                            {
                                var currentRole = allRoles.FirstOrDefault(r =>
                                    r.Name.Equals(role, StringComparison.InvariantCultureIgnoreCase));

                                if (currentRole != null)
                                    rolesToSave.Add(currentRole.Id);
                            }
                        break;
                    case "CreatedOnUtc":
                        if (DateTime.TryParse(property.StringValue, out var date))
                            customer.CreatedOnUtc = date;
                        break;
                    case "FirstName":
                        customer.FirstName = property.StringValue;
                        break;
                    case "LastName":
                        customer.LastName = property.StringValue;
                        break;
                    case "Gender":
                        customer.Gender = property.StringValue;
                        break;
                    case "Company":
                        customer.Company = property.StringValue;
                        break;
                    case "StreetAddress":
                        customer.StreetAddress = property.StringValue;
                        break;
                    case "StreetAddress2":
                        customer.StreetAddress2 = property.StringValue;
                        break;
                    case "ZipPostalCode":
                        customer.ZipPostalCode = property.StringValue;
                        break;
                    case "City":
                        customer.City = property.StringValue;
                        break;
                    case "County":
                        customer.County = property.StringValue;
                        break;
                    case "Country":
                        if (int.TryParse(property.StringValue, out var countryId))
                            customer.CountryId = countryId;
                        else
                        {
                            var country = countries.FirstOrDefault(c =>
                                c.Name.Equals(property.StringValue, StringComparison.InvariantCultureIgnoreCase));

                            if (country != null)
                                customer.CountryId = country.Id;
                        }
                        break;
                    case "StateProvince":
                        if (int.TryParse(property.StringValue, out var stateId))
                            customer.StateProvinceId = stateId;
                        else
                        {
                            var state = states.FirstOrDefault(s =>
                                s.Name.Equals(property.StringValue, StringComparison.InvariantCultureIgnoreCase));

                            if (state != null)
                                customer.StateProvinceId = state.Id;
                        }
                        break;
                    case "Phone":
                        customer.Phone = property.StringValue;
                        break;
                    case "Fax":
                        customer.Fax = property.StringValue;
                        break;
                    case "TimeZone":
                        customer.TimeZoneId = property.StringValue;
                        break;
                    case "AvatarPictureId":
                        avatarPictureId = property.IntValueNullable;
                        break;
                    case "Signature":
                        signature = property.StringValue;
                        break;
                    case "CustomCustomerAttributesXML":
                        customer.CustomCustomerAttributesXML = property.StringValue;
                        break;
                    case "Password":
                        password = property.StringValue;
                        break;
                    case "PasswordSalt":
                        passwordSalt = property.StringValue;
                        break;
                }
            }

            if (isNew)
                await _customerService.InsertCustomerAsync(customer);
            else
                await _customerService.UpdateCustomerAsync(customer);

            var customerRoles = await _customerService.GetCustomerRolesAsync(customer);

            foreach (var roleId in rolesToSave)
            {
                var role = allRoles.FirstOrDefault(r => r.Id == roleId);

                if (role == null || customerRoles.Any(cr => cr.Id == roleId))
                    continue;

                await _customerService.AddCustomerRoleMappingAsync(
                    new CustomerCustomerRoleMapping { CustomerId = customer.Id, CustomerRoleId = roleId });
            }

            if (!isNew && rolesToSave.Any())
                foreach (var customerRole in customerRoles.Where(cr => !rolesToSave.Contains(cr.Id)).ToList())
                    await _customerService.RemoveCustomerRoleMappingAsync(customer, customerRole);

            if (avatarPictureId.HasValue)
                await _genericAttributeService.SaveAttributeAsync(customer,
                    CustomerDefaults.AvatarPictureIdAttribute, avatarPictureId.Value);

            if (!string.IsNullOrEmpty(signature))
                await _genericAttributeService.SaveAttributeAsync(customer, CustomerDefaults.SignatureAttribute,
                    signature);

            if (_securitySettings.AllowSiteOwnerExportImportCustomersWithHashedPassword &&
                !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(passwordSalt))
            {
                var lastPassword = await _customerService.GetCurrentPasswordAsync(customer.Id);
                if (!lastPassword.Password.Equals(password) || lastPassword.PasswordSalt.Equals(passwordSalt))
                    await _customerService.InsertCustomerPasswordAsync(new CustomerPassword
                    {
                        CustomerId = customer.Id,
                        Password = password,
                        PasswordSalt = passwordSalt,
                        PasswordFormat = PasswordFormat.Hashed,
                        CreatedOnUtc = DateTime.UtcNow
                    });
            }

            iRow++;
        }

        //activity log
        await _customerActivityService.InsertActivityAsync("ImportCustomers",
            string.Format(await _localizationService.GetResourceAsync("ActivityLog.ImportCustomers"), iRow - 2));
    }

    public virtual async Task<int> ImportStatesFromTxtAsync(Stream stream, bool writeLog = true)
    {
        var count = 0;
        using (var reader = new StreamReader(stream))
        {
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                var tmp = line.Split(',');

                if (tmp.Length != 5)
                    throw new AssetForgeException("Wrong file format");

                //parse
                var countryTwoLetterIsoCode = tmp[0].Trim();
                var name = tmp[1].Trim();
                var abbreviation = tmp[2].Trim();
                var published = bool.Parse(tmp[3].Trim());
                var displayOrder = int.Parse(tmp[4].Trim());

                var country = await _countryService.GetCountryByTwoLetterIsoCodeAsync(countryTwoLetterIsoCode);
                if (country == null)
                {
                    //country cannot be loaded. skip
                    continue;
                }

                //import
                var states = await _stateProvinceService.GetStateProvincesByCountryIdAsync(country.Id, showHidden: true);
                var state = states.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

                if (state != null)
                {
                    state.Abbreviation = abbreviation;
                    state.Published = published;
                    state.DisplayOrder = displayOrder;
                    await _stateProvinceService.UpdateStateProvinceAsync(state);
                }
                else
                {
                    state = new StateProvince
                    {
                        CountryId = country.Id,
                        Name = name,
                        Abbreviation = abbreviation,
                        Published = published,
                        DisplayOrder = displayOrder
                    };
                    await _stateProvinceService.InsertStateProvinceAsync(state);
                }

                count++;
            }
        }

        //activity log
        if (writeLog)
        {
            await _customerActivityService.InsertActivityAsync("ImportStates",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.ImportStates"), count));
        }

        return count;
    }

    #endregion
}