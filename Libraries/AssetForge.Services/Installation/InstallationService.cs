using AssetForge.Core;
using AssetForge.Core.Domain;
using AssetForge.Core.Domain.Common;
using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Directory;
using AssetForge.Core.Domain.Localization;
using AssetForge.Core.Domain.Logging;
using AssetForge.Core.Domain.Media;
using AssetForge.Core.Domain.ScheduleTasks;
using AssetForge.Core.Domain.Security;
using AssetForge.Core.Domain.Seo;
using AssetForge.Core.Domain.Sites;
using AssetForge.Core.Domain.Teams;
using AssetForge.Core.Http;
using AssetForge.Core.Infrastructure;
using AssetForge.Core.Security;
using AssetForge.Data;
using AssetForge.Services.Common;
using AssetForge.Services.Configuration;
using AssetForge.Services.Customers;
using AssetForge.Services.ExportImport;
using AssetForge.Services.Helpers;
using AssetForge.Services.Localization;
using AssetForge.Services.Seo;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Text;

namespace AssetForge.Services.Installation
{
    /// <summary>
    /// Installation service
    /// </summary>
    public partial class InstallationService : IInstallationService
    {
        #region Fields

        protected readonly IDataProvider _dataProvider;
        protected readonly IAssetForgeFileProvider _fileProvider;
        protected readonly IRepository<ActivityLogType> _activityLogTypeRepository;
        protected readonly IRepository<Address> _addressRepository;
        protected readonly IRepository<Country> _countryRepository;
        protected readonly IRepository<Customer> _customerRepository;
        protected readonly IRepository<CustomerRole> _customerRoleRepository;
        protected readonly IRepository<Language> _languageRepository;
        protected readonly IRepository<StateProvince> _stateProvinceRepository;
        protected readonly IRepository<Site> _siteRepository;
        protected readonly IRepository<UrlRecord> _urlRecordRepository;
        protected readonly IWebHelper _webHelper;

        #endregion Fields

        #region Ctor

        public InstallationService(IDataProvider dataProvider,
            IAssetForgeFileProvider fileProvider,
            IRepository<ActivityLogType> activityLogTypeRepository,
            IRepository<Address> addressRepository,
            IRepository<Country> countryRepository,
            IRepository<Customer> customerRepository,
            IRepository<CustomerRole> customerRoleRepository,
            IRepository<Language> languageRepository,
            IRepository<StateProvince> stateProvinceRepository,
            IRepository<Site> siteRepository,
            IRepository<UrlRecord> urlRecordRepository,
            IWebHelper webHelper)
        {
            _dataProvider = dataProvider;
            _fileProvider = fileProvider;
            _activityLogTypeRepository = activityLogTypeRepository;
            _addressRepository = addressRepository;
            _countryRepository = countryRepository;
            _customerRepository = customerRepository;
            _customerRoleRepository = customerRoleRepository;
            _languageRepository = languageRepository;
            _stateProvinceRepository = stateProvinceRepository;
            _siteRepository = siteRepository;
            _urlRecordRepository = urlRecordRepository;
            _webHelper = webHelper;
        }

        #endregion Ctor

        #region Utilities

        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task<T> InsertInstallationDataAsync<T>(T entity) where T : BaseEntity
        {
            return await _dataProvider.InsertEntityAsync(entity);
        }

        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task InsertInstallationDataAsync<T>(params T[] entities) where T : BaseEntity
        {
            await _dataProvider.BulkInsertEntitiesAsync(entities);
        }

        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task InsertInstallationDataAsync<T>(IList<T> entities) where T : BaseEntity
        {
            if (!entities.Any())
                return;

            await InsertInstallationDataAsync(entities.ToArray());
        }

        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task UpdateInstallationDataAsync<T>(T entity) where T : BaseEntity
        {
            await _dataProvider.UpdateEntityAsync(entity);
        }

        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task UpdateInstallationDataAsync<T>(IList<T> entities) where T : BaseEntity
        {
            if (!entities.Any())
                return;

            foreach (var entity in entities)
                await _dataProvider.UpdateEntityAsync(entity);
        }

        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task<string> ValidateSeNameAsync<T>(T entity, string seName) where T : BaseEntity
        {
            //duplicate of ValidateSeName method of \AssetForge.Services\Seo\UrlRecordService.cs (we cannot inject it here)
            ArgumentNullException.ThrowIfNull(entity);

            //validation
            var okChars = "abcdefghijklmnopqrstuvwxyzEmployee123!7890 _-";
            seName = seName.Trim().ToLowerInvariant();

            var sb = new StringBuilder();
            foreach (var c in seName.ToCharArray())
            {
                var c2 = c.ToString();
                if (okChars.Contains(c2))
                    sb.Append(c2);
            }

            seName = sb.ToString();
            seName = seName.Replace(" ", "-");
            while (seName.Contains("--"))
                seName = seName.Replace("--", "-");
            while (seName.Contains("__"))
                seName = seName.Replace("__", "_");

            //max length
            seName = CommonHelper.EnsureMaximumLength(seName, SeoDefaults.SearchEngineNameLength);

            //ensure this seName is not reserved yet
            var i = 2;
            var tempSeName = seName;
            while (true)
            {
                //check whether such slug already exists (and that is not the current entity)

                var query = from ur in _urlRecordRepository.Table
                            where tempSeName != null && ur.Slug == tempSeName
                            select ur;
                var urlRecord = await query.FirstOrDefaultAsync();

                var entityName = entity.GetType().Name;
                var reserved = urlRecord != null && !(urlRecord.EntityId == entity.Id && urlRecord.EntityName.Equals(entityName, StringComparison.InvariantCultureIgnoreCase));
                if (!reserved)
                    break;

                tempSeName = $"{seName}-{i}";
                i++;
            }

            seName = tempSeName;

            return seName;
        }

        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task InstallSitesAsync()
        {
            var siteUrl = _webHelper.GetSiteLocation();
            var sites = new List<Site>
            {
                new() {
                    Name = "Your CMS site name",
                    DefaultTitle = "Your CMS",
                    DefaultMetaKeywords = string.Empty,
                    DefaultMetaDescription = string.Empty,
                    HomepageTitle = "Home page title",
                    HomepageDescription = "Home page description",
                    Url = siteUrl,
                    SslEnabled = _webHelper.IsCurrentConnectionSecured(),
                    Hosts = "yourcms.com,www.yourcms.com",
                    DisplayOrder = 0,
                    //should we set some default company info?
                    CompanyName = "Your company name",
                    CompanyAddress = "your company country, state, zip, street, etc",
                    CompanyPhoneNumber = "(123) 456-78901",
                    CompanyVat = null
                },
            };

            await InsertInstallationDataAsync(sites);
        }

        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task InstallAdditionalSitesAsync()
        {
            var siteUrl = _webHelper.GetSiteLocation();
            var sites = new List<Site>
            {
                new() {
                    Name = "Your CMS 2",
                    DefaultTitle = "Your CMS",
                    DefaultMetaKeywords = string.Empty,
                    DefaultMetaDescription = string.Empty,
                    HomepageTitle = "Home page title",
                    HomepageDescription = "Home page description",
                    Url = siteUrl,
                    SslEnabled = _webHelper.IsCurrentConnectionSecured(),
                    Hosts = "yourcms.com,www.yourcms.com",
                    DisplayOrder = 2,
                    //should we set some default company info?
                    CompanyName = "Your company name",
                    CompanyAddress = "your company country, state, zip, street, etc",
                    CompanyPhoneNumber = "(123) 456-78901",
                    CompanyVat = null
                },
                new() {
                    Name = "Your CMS 3",
                    DefaultTitle = "Your CMS",
                    DefaultMetaKeywords = string.Empty,
                    DefaultMetaDescription = string.Empty,
                    HomepageTitle = "Home page title",
                    HomepageDescription = "Home page description",
                    Url = siteUrl,
                    SslEnabled = _webHelper.IsCurrentConnectionSecured(),
                    Hosts = "yourcms.com,www.yourcms.com",
                    DisplayOrder = 3,
                    //should we set some default company info?
                    CompanyName = "Your company name",
                    CompanyAddress = "your company country, state, zip, street, etc",
                    CompanyPhoneNumber = "(123) 456-78901",
                    CompanyVat = null
                },
                new() {
                    Name = "Your CMS 4",
                    DefaultTitle = "Your CMS",
                    DefaultMetaKeywords = string.Empty,
                    DefaultMetaDescription = string.Empty,
                    HomepageTitle = "Home page title",
                    HomepageDescription = "Home page description",
                    Url = siteUrl,
                    SslEnabled = _webHelper.IsCurrentConnectionSecured(),
                    Hosts = "yourcms.com,www.yourcms.com",
                    DisplayOrder = 4,
                    //should we set some default company info?
                    CompanyName = "Your company name",
                    CompanyAddress = "your company country, state, zip, street, etc",
                    CompanyPhoneNumber = "(123) 456-78901",
                    CompanyVat = null
                },
            };

            await InsertInstallationDataAsync(sites);
        }

        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task InstallLanguagesAsync((string languagePackDownloadLink, int languagePackProgress) languagePackInfo, CultureInfo cultureInfo, RegionInfo regionInfo)
        {
            var localizationService = EngineContext.Current.Resolve<ILocalizationService>();

            var defaultCulture = new CultureInfo(CommonDefaults.DefaultLanguageCulture);
            var defaultLanguage = new Language
            {
                Name = defaultCulture.TwoLetterISOLanguageName.ToUpperInvariant(),
                LanguageCulture = defaultCulture.Name,
                UniqueSeoCode = defaultCulture.TwoLetterISOLanguageName,
                FlagImageFileName = $"{defaultCulture.Name.ToLowerInvariant()[^2..]}.png",
                Rtl = defaultCulture.TextInfo.IsRightToLeft,
                Published = true,
                DisplayOrder = 1
            };
            await InsertInstallationDataAsync(defaultLanguage);

            //Install locale resources for default culture
            var directoryPath = _fileProvider.MapPath(AssetForgeInstallationDefaults.LocalizationResourcesPath);
            var pattern = $"*.{AssetForgeInstallationDefaults.LocalizationResourcesFileExtension}";
            foreach (var filePath in _fileProvider.EnumerateFiles(directoryPath, pattern))
            {
                using var streamReader = new StreamReader(filePath);
                await localizationService.ImportResourcesFromXmlAsync(defaultLanguage, streamReader);
            }

            if (cultureInfo == null || regionInfo == null || cultureInfo.Name == CommonDefaults.DefaultLanguageCulture)
                return;

            var language = new Language
            {
                Name = cultureInfo.TwoLetterISOLanguageName.ToUpperInvariant(),
                LanguageCulture = cultureInfo.Name,
                UniqueSeoCode = cultureInfo.TwoLetterISOLanguageName,
                FlagImageFileName = $"{regionInfo.TwoLetterISORegionName.ToLowerInvariant()}.png",
                Rtl = cultureInfo.TextInfo.IsRightToLeft,
                Published = true,
                DisplayOrder = 2
            };
            await InsertInstallationDataAsync(language);

            if (string.IsNullOrEmpty(languagePackInfo.languagePackDownloadLink))
                return;

            //download and import language pack
            try
            {
                var httpClientFactory = EngineContext.Current.Resolve<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient(HttpDefaults.DefaultHttpClient);
                await using var stream = await httpClient.GetStreamAsync(languagePackInfo.languagePackDownloadLink);
                using var streamReader = new StreamReader(stream);
                await localizationService.ImportResourcesFromXmlAsync(language, streamReader);

                //set this language as default
                language.DisplayOrder = 0;
                await UpdateInstallationDataAsync(language);

                //save progress for showing in admin panel (only for first start)
                await InsertInstallationDataAsync(new GenericAttribute
                {
                    EntityId = language.Id,
                    Key = CommonDefaults.LanguagePackProgressAttribute,
                    KeyGroup = nameof(Language),
                    Value = languagePackInfo.languagePackProgress.ToString(),
                    SiteId = 0,
                    CreatedOrUpdatedDateUTC = DateTime.UtcNow
                });
            }
            catch { }
        }

        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task InstallCountriesAndStatesAsync()
        {
            var countries = ISO3166.GetCollection().Select(country => new Country
            {
                Name = country.Name,
                AllowsBilling = true,
                AllowsShipping = true,
                TwoLetterIsoCode = country.Alpha2,
                ThreeLetterIsoCode = country.Alpha3,
                NumericIsoCode = country.NumericCode,
                SubjectToVat = country.SubjectToVat,
                DisplayOrder = country.NumericCode == 50 ? 1 : 100,
                Published = true
            }).ToList();

            await InsertInstallationDataAsync(countries.ToArray());

            //Import states for all countries
            var directoryPath = _fileProvider.MapPath(AssetForgeInstallationDefaults.LocalizationResourcesPath);
            var pattern = "*.txt";

            //we use different scope to prevent creating wrong settings in DI, because the settings data not exists yet
            var serviceScopeFactory = EngineContext.Current.Resolve<IServiceScopeFactory>();
            using var scope = serviceScopeFactory.CreateScope();
            var importManager = EngineContext.Current.Resolve<IImportManager>(scope);
            foreach (var filePath in _fileProvider.EnumerateFiles(directoryPath, pattern))
            {
                await using var stream = new FileStream(filePath, FileMode.Open);
                await importManager.ImportStatesFromTxtAsync(stream, false);
            }
        }

        protected virtual async Task InstallSampleCustomersAsync()
        {
            var crRegistered = await _customerRoleRepository.Table
                .FirstOrDefaultAsync(customerRole => customerRole.SystemName == CustomerDefaults.RegisteredRoleName);

            ArgumentNullException.ThrowIfNull(crRegistered);

            //default site
            var defaultSite = await _siteRepository.Table.FirstOrDefaultAsync() ?? throw new Exception("No default site could be loaded");

            var siteId = defaultSite.Id;

            //second user
            var secondUserEmail = "manager@demo.com";
            var secondUser = new Customer
            {
                CustomerGuid = Guid.NewGuid(),
                Email = secondUserEmail,
                Username = secondUserEmail,
                Active = true,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow,
                RegisteredInSiteId = siteId
            };
            var defaultSecondUserAddress = await InsertInstallationDataAsync(
                new Address
                {
                    FirstName = "Steve",
                    LastName = "Gates",
                    PhoneNumber = "87654321",
                    Email = secondUserEmail,
                    FaxNumber = string.Empty,
                    Company = "Steve Company",
                    Address1 = "750 Bel Air Rd.",
                    Address2 = string.Empty,
                    City = "Los Angeles",
                    StateProvinceId = _stateProvinceRepository.Table.FirstOrDefault(sp => sp.Name == "California")?.Id,
                    CountryId = _countryRepository.Table.FirstOrDefault(c => c.ThreeLetterIsoCode == "USA")?.Id,
                    ZipPostalCode = "90077",
                    CreatedOnUtc = DateTime.UtcNow
                });

            secondUser.FirstName = defaultSecondUserAddress.FirstName;
            secondUser.LastName = defaultSecondUserAddress.LastName;

            await InsertInstallationDataAsync(secondUser);

            await InsertInstallationDataAsync(new CustomerAddressMapping { CustomerId = secondUser.Id, AddressId = defaultSecondUserAddress.Id });
            await InsertInstallationDataAsync(new CustomerCustomerRoleMapping { CustomerId = secondUser.Id, CustomerRoleId = crRegistered.Id });

            //set customer password
            await InsertInstallationDataAsync(new CustomerPassword
            {
                CustomerId = secondUser.Id,
                Password = "Manager123!",
                PasswordFormat = PasswordFormat.Clear,
                PasswordSalt = string.Empty,
                CreatedOnUtc = DateTime.UtcNow
            });

            var team = new Team
            {
                Name = "Team Demo",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged."
            };
            //set customer password
            await InsertInstallationDataAsync(team);
            var teamManagerMap = new TeamMemberMap()
            {
                CustomerId = secondUser.Id,
                TeamId = team.Id,
                RoleTypeId = (int)RoleType.Manager
            };
            await InsertInstallationDataAsync(teamManagerMap);

            var crTeamManager = await _customerRoleRepository.Table
                .FirstOrDefaultAsync(customerRole => customerRole.SystemName == CustomerDefaults.ManagerRoleName);
            if(crTeamManager != null){
                await InsertInstallationDataAsync(new CustomerCustomerRoleMapping { CustomerId = secondUser.Id, CustomerRoleId = crTeamManager.Id });
            }

            //third user
            var thirdUserEmail = "arthur_holmes@demo.com";
            var thirdUser = new Customer
            {
                CustomerGuid = Guid.NewGuid(),
                Email = thirdUserEmail,
                Username = thirdUserEmail,
                Active = true,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow,
                RegisteredInSiteId = siteId
            };

            var defaultThirdUserAddress = await InsertInstallationDataAsync(
                new Address
                {
                    FirstName = "Arthur",
                    LastName = "Holmes",
                    PhoneNumber = "111222333",
                    Email = thirdUserEmail,
                    FaxNumber = string.Empty,
                    Company = "Holmes Company",
                    Address1 = "221B Baker Street",
                    Address2 = string.Empty,
                    City = "London",
                    CountryId = _countryRepository.Table.FirstOrDefault(c => c.ThreeLetterIsoCode == "GBR")?.Id,
                    ZipPostalCode = "NW1 6XE",
                    CreatedOnUtc = DateTime.UtcNow
                });

            thirdUser.FirstName = defaultThirdUserAddress.FirstName;
            thirdUser.LastName = defaultThirdUserAddress.LastName;

            await InsertInstallationDataAsync(thirdUser);

            await InsertInstallationDataAsync(new CustomerAddressMapping { CustomerId = thirdUser.Id, AddressId = defaultThirdUserAddress.Id });
            await InsertInstallationDataAsync(new CustomerCustomerRoleMapping { CustomerId = thirdUser.Id, CustomerRoleId = crRegistered.Id });

            var teamManagerMap2 = new TeamMemberMap()
            {
                CustomerId = secondUser.Id,
                TeamId = team.Id,
                RoleTypeId = (int)RoleType.Employee
            };
            await InsertInstallationDataAsync(teamManagerMap2);

            var teamMemberTask = new TeamTask()
            {
                Title = "Task Demo",
                Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.",
                AssignedToTeamMemberMapId = teamManagerMap2.Id,
                TaskStatusId = (int) TeamTaskStatus.Todo,
                CreatedByUserId = secondUser.Id,
                DueDateUtc = DateTime.UtcNow.AddDays(7),
                CreatedDateUtc = DateTime.UtcNow
            };
            await InsertInstallationDataAsync(teamMemberTask);

            //set customer password
            await InsertInstallationDataAsync(new CustomerPassword
            {
                CustomerId = thirdUser.Id,
                Password = "Employee123!",
                PasswordFormat = PasswordFormat.Clear,
                PasswordSalt = string.Empty,
                CreatedOnUtc = DateTime.UtcNow
            });

            //fourth user
            var fourthUserEmail = "james_pan@demo.com";
            var fourthUser = new Customer
            {
                CustomerGuid = Guid.NewGuid(),
                Email = fourthUserEmail,
                Username = fourthUserEmail,
                Active = true,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow,
                RegisteredInSiteId = siteId
            };
            var defaultFourthUserAddress = await InsertInstallationDataAsync(
                new Address
                {
                    FirstName = "James",
                    LastName = "Pan",
                    PhoneNumber = "369258147",
                    Email = fourthUserEmail,
                    FaxNumber = string.Empty,
                    Company = "Pan Company",
                    Address1 = "St Katharine’s West 16",
                    Address2 = string.Empty,
                    City = "St Andrews",
                    CountryId = _countryRepository.Table.FirstOrDefault(c => c.ThreeLetterIsoCode == "GBR")?.Id,
                    ZipPostalCode = "KY16 9AX",
                    CreatedOnUtc = DateTime.UtcNow
                });

            fourthUser.FirstName = defaultFourthUserAddress.FirstName;
            fourthUser.LastName = defaultFourthUserAddress.LastName;

            await InsertInstallationDataAsync(fourthUser);

            await InsertInstallationDataAsync(new CustomerAddressMapping { CustomerId = fourthUser.Id, AddressId = defaultFourthUserAddress.Id });
            await InsertInstallationDataAsync(new CustomerCustomerRoleMapping { CustomerId = fourthUser.Id, CustomerRoleId = crRegistered.Id });

            //set customer password
            await InsertInstallationDataAsync(new CustomerPassword
            {
                CustomerId = fourthUser.Id,
                Password = "Employee123!",
                PasswordFormat = PasswordFormat.Clear,
                PasswordSalt = string.Empty,
                CreatedOnUtc = DateTime.UtcNow
            });

            //fifth user
            var fifthUserEmail = "brenda_lindgren@demo.com";
            var fifthUser = new Customer
            {
                CustomerGuid = Guid.NewGuid(),
                Email = fifthUserEmail,
                Username = fifthUserEmail,
                Active = true,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow,
                RegisteredInSiteId = siteId
            };
            var defaultFifthUserAddress = await InsertInstallationDataAsync(
                new Address
                {
                    FirstName = "Brenda",
                    LastName = "Lindgren",
                    PhoneNumber = "14785236",
                    Email = fifthUserEmail,
                    FaxNumber = string.Empty,
                    Company = "Brenda Company",
                    Address1 = "1249 Tongass Avenue, Suite B",
                    Address2 = string.Empty,
                    City = "Ketchikan",
                    StateProvinceId = _stateProvinceRepository.Table.FirstOrDefault(sp => sp.Name == "Alaska")?.Id,
                    CountryId = _countryRepository.Table.FirstOrDefault(c => c.ThreeLetterIsoCode == "USA")?.Id,
                    ZipPostalCode = "99901",
                    CreatedOnUtc = DateTime.UtcNow
                });

            fifthUser.FirstName = defaultFifthUserAddress.FirstName;
            fifthUser.LastName = defaultFifthUserAddress.LastName;

            await InsertInstallationDataAsync(fifthUser);

            await InsertInstallationDataAsync(new CustomerAddressMapping { CustomerId = fifthUser.Id, AddressId = defaultFifthUserAddress.Id });
            await InsertInstallationDataAsync(new CustomerCustomerRoleMapping { CustomerId = fifthUser.Id, CustomerRoleId = crRegistered.Id });

            //set customer password
            await InsertInstallationDataAsync(new CustomerPassword
            {
                CustomerId = fifthUser.Id,
                Password = "Employee123!",
                PasswordFormat = PasswordFormat.Clear,
                PasswordSalt = string.Empty,
                CreatedOnUtc = DateTime.UtcNow
            });

            //sixth user
            var sixthUserEmail = "victoria_victoria@demo.com";
            var sixthUser = new Customer
            {
                CustomerGuid = Guid.NewGuid(),
                Email = sixthUserEmail,
                Username = sixthUserEmail,
                Active = true,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow,
                RegisteredInSiteId = siteId
            };
            var defaultSixthUserAddress = await InsertInstallationDataAsync(
                new Address
                {
                    FirstName = "Victoria",
                    LastName = "Terces",
                    PhoneNumber = "45612378",
                    Email = sixthUserEmail,
                    FaxNumber = string.Empty,
                    Company = "Terces Company",
                    Address1 = "201 1st Avenue South",
                    Address2 = string.Empty,
                    City = "Saskatoon",
                    StateProvinceId = (await _stateProvinceRepository.Table.FirstOrDefaultAsync(sp => sp.Name == "Saskatchewan"))?.Id,
                    CountryId = (await _countryRepository.Table.FirstOrDefaultAsync(c => c.ThreeLetterIsoCode == "CAN"))?.Id,
                    ZipPostalCode = "S7K 1J9",
                    CreatedOnUtc = DateTime.UtcNow
                });

            sixthUser.FirstName = defaultSixthUserAddress.FirstName;
            sixthUser.LastName = defaultSixthUserAddress.LastName;

            await InsertInstallationDataAsync(sixthUser);

            await InsertInstallationDataAsync(new CustomerAddressMapping { CustomerId = sixthUser.Id, AddressId = defaultSixthUserAddress.Id });
            await InsertInstallationDataAsync(new CustomerCustomerRoleMapping { CustomerId = sixthUser.Id, CustomerRoleId = crRegistered.Id });

            //set customer password
            await InsertInstallationDataAsync(new CustomerPassword
            {
                CustomerId = sixthUser.Id,
                Password = "Employee123!",
                PasswordFormat = PasswordFormat.Clear,
                PasswordSalt = string.Empty,
                CreatedOnUtc = DateTime.UtcNow
            });
        }

        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task InstallCustomersAndUsersAsync(string defaultUserEmail, string defaultUserPassword)
        {
            var crAdministrators = new CustomerRole
            {
                Name = "Administrators",
                Active = true,
                IsSystemRole = true,
                SystemName = CustomerDefaults.AdministratorsRoleName
            };
            var crForumModerators = new CustomerRole
            {
                Name = "TeamManager",
                Active = true,
                IsSystemRole = true,
                SystemName = CustomerDefaults.ManagerRoleName
            };
            var crRegistered = new CustomerRole
            {
                Name = "Registered",
                Active = true,
                IsSystemRole = true,
                SystemName = CustomerDefaults.RegisteredRoleName
            };
            var crGuests = new CustomerRole
            {
                Name = "Guests",
                Active = true,
                IsSystemRole = true,
                SystemName = CustomerDefaults.GuestsRoleName
            };
            var customerRoles = new List<CustomerRole>
            {
                crAdministrators,
                crForumModerators,
                crRegistered,
                crGuests
            };

            await InsertInstallationDataAsync(customerRoles);

            //default site
            var defaultSite = await _siteRepository.Table.FirstOrDefaultAsync() ?? throw new Exception("No default site could be loaded");

            var siteId = defaultSite.Id;

            //admin user
            var adminUser = new Customer
            {
                CustomerGuid = Guid.NewGuid(),
                Email = defaultUserEmail,
                Username = defaultUserEmail,
                Active = true,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow,
                RegisteredInSiteId = siteId
            };

            var defaultAdminUserAddress = await InsertInstallationDataAsync(
                new Address
                {
                    FirstName = "John",
                    LastName = "Smith",
                    PhoneNumber = "Employee123!78",
                    Email = defaultUserEmail,
                    FaxNumber = string.Empty,
                    Company = "AssetForge Solutions Ltd",
                    Address1 = "21 West 52nd Street",
                    Address2 = string.Empty,
                    City = "New York",
                    StateProvinceId = _stateProvinceRepository.Table.FirstOrDefault(sp => sp.Name == "New York")?.Id,
                    CountryId = _countryRepository.Table.FirstOrDefault(c => c.ThreeLetterIsoCode == "USA")?.Id,
                    ZipPostalCode = "10021",
                    CreatedOnUtc = DateTime.UtcNow
                });

            adminUser.FirstName = defaultAdminUserAddress.FirstName;
            adminUser.LastName = defaultAdminUserAddress.LastName;

            await InsertInstallationDataAsync(adminUser);

            await InsertInstallationDataAsync(new CustomerAddressMapping { CustomerId = adminUser.Id, AddressId = defaultAdminUserAddress.Id });

            await InsertInstallationDataAsync(
                new CustomerCustomerRoleMapping { CustomerId = adminUser.Id, CustomerRoleId = crAdministrators.Id },
                new CustomerCustomerRoleMapping { CustomerId = adminUser.Id, CustomerRoleId = crForumModerators.Id },
                new CustomerCustomerRoleMapping { CustomerId = adminUser.Id, CustomerRoleId = crRegistered.Id });

            //set hashed admin password
            var customerRegistrationService = EngineContext.Current.Resolve<ICustomerRegistrationService>();
            await customerRegistrationService.ChangePasswordAsync(new ChangePasswordRequest(defaultUserEmail, false,
                 PasswordFormat.Hashed, defaultUserPassword, null, AssetForgeCustomerServicesDefaults.DefaultHashedPasswordFormat));

            //search engine (crawler) built-in user
            var searchEngineUser = new Customer
            {
                Email = "builtin@search_engine_record.com",
                CustomerGuid = Guid.NewGuid(),
                AdminComment = "Built-in system guest record used for requests from search engines.",
                Active = true,
                IsSystemAccount = true,
                SystemName = CustomerDefaults.SearchEngineCustomerName,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow,
                RegisteredInSiteId = siteId
            };

            await InsertInstallationDataAsync(searchEngineUser);

            await InsertInstallationDataAsync(new CustomerCustomerRoleMapping { CustomerRoleId = crGuests.Id, CustomerId = searchEngineUser.Id });

            //built-in user for background tasks
            var backgroundTaskUser = new Customer
            {
                Email = "builtin@background-task-record.com",
                CustomerGuid = Guid.NewGuid(),
                AdminComment = "Built-in system record used for background tasks.",
                Active = true,
                IsSystemAccount = true,
                SystemName = CustomerDefaults.BackgroundTaskCustomerName,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow,
                RegisteredInSiteId = siteId
            };

            await InsertInstallationDataAsync(backgroundTaskUser);

            await InsertInstallationDataAsync(new CustomerCustomerRoleMapping { CustomerId = backgroundTaskUser.Id, CustomerRoleId = crGuests.Id });
        }

        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task InstallSearchTermsAsync()
        {
            //default site
            var defaultSite = _siteRepository.Table.FirstOrDefault() ?? throw new Exception("No default site could be loaded");

            await InsertInstallationDataAsync(new SearchTerm
            {
                Count = 34,
                Keyword = "computer",
                SiteId = defaultSite.Id
            });

            await InsertInstallationDataAsync(new SearchTerm
            {
                Count = 30,
                Keyword = "camera",
                SiteId = defaultSite.Id
            });

            await InsertInstallationDataAsync(new SearchTerm
            {
                Count = 27,
                Keyword = "jewelry",
                SiteId = defaultSite.Id
            });

            await InsertInstallationDataAsync(new SearchTerm
            {
                Count = 26,
                Keyword = "shoes",
                SiteId = defaultSite.Id
            });

            await InsertInstallationDataAsync(new SearchTerm
            {
                Count = 19,
                Keyword = "jeans",
                SiteId = defaultSite.Id
            });

            await InsertInstallationDataAsync(new SearchTerm
            {
                Count = 10,
                Keyword = "gift",
                SiteId = defaultSite.Id
            });
        }

        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task InstallSettingsAsync(RegionInfo regionInfo)
        {
            var isMetric = regionInfo?.IsMetric ?? false;
            var country = regionInfo?.TwoLetterISORegionName ?? string.Empty;
            var isGermany = country == "DE";
            var isEurope = ISO3166.FromCountryCode(country)?.SubjectToVat ?? false;

            var settingService = EngineContext.Current.Resolve<ISettingService>();
            await settingService.SaveSettingAsync(new PdfSettings
            {
                LogoPictureId = 0,
                LetterPageSizeEnabled = false,
                RenderOrderNotes = true,
                FontFamily = "FreeSerif",
                PdfFooterTextColumn1 = null,
                PdfFooterTextColumn2 = null
            });

            await settingService.SaveSettingAsync(new SitemapSettings
            {
                SitemapEnabled = true,
                SitemapPageSize = 200,
                SitemapIncludeBlogPosts = true,
                SitemapIncludeNews = false,
                SitemapIncludePages = true
            });

            await settingService.SaveSettingAsync(new SitemapXmlSettings
            {
                SitemapXmlEnabled = true,
                SitemapXmlIncludeBlogPosts = true,
                SitemapXmlIncludeCategories = true,
                SitemapXmlIncludeManufacturers = true,
                SitemapXmlIncludeNews = true,
                SitemapXmlIncludeProducts = true,
                SitemapXmlIncludeProductTags = true,
                SitemapXmlIncludeCustomUrls = true,
                SitemapXmlIncludePages = true,
                RebuildSitemapXmlAfterHours = 2 * 24,
                SitemapBuildOperationDelay = 60
            });

            await settingService.SaveSettingAsync(new CommonSettings
            {
                UseSystemEmailForContactUsForm = true,
                DisplayJavaScriptDisabledWarning = false,
                Log404Errors = true,
                BreadcrumbDelimiter = "/",
                BbcodeEditorOpenLinksInNewWindow = false,
                PopupForTermsOfServiceLinks = true,
                JqueryMigrateScriptLoggingActive = false,
                UseResponseCompression = true,
                FaviconAndAppIconsHeadCode =
                    "<link rel=\"apple-touch-icon\" sizes=\"180x180\" href=\"/icons/icons_0/apple-touch-icon.png\"><link rel=\"icon\" type=\"image/png\" sizes=\"32x32\" href=\"/icons/icons_0/favicon-32x32.png\"><link rel=\"icon\" type=\"image/png\" sizes=\"192x192\" href=\"/icons/icons_0/android-chrome-192x192.png\"><link rel=\"icon\" type=\"image/png\" sizes=\"16x16\" href=\"/icons/icons_0/favicon-16x16.png\"><link rel=\"manifest\" href=\"/icons/icons_0/site.webmanifest\"><link rel=\"mask-icon\" href=\"/icons/icons_0/safari-pinned-tab.svg\" color=\"#5bbad5\"><link rel=\"shortcut icon\" href=\"/icons/icons_0/favicon.ico\"><meta name=\"msapplication-TileColor\" content=\"#2d89ef\"><meta name=\"msapplication-TileImage\" content=\"/icons/icons_0/mstile-144x144.png\"><meta name=\"msapplication-config\" content=\"/icons/icons_0/browserconfig.xml\"><meta name=\"theme-color\" content=\"#ffffff\">",
                EnableHtmlMinification = true,
                RestartTimeout = CommonDefaults.RestartTimeout,
                HeaderCustomHtml = string.Empty,
                FooterCustomHtml = string.Empty
            });

            await settingService.SaveSettingAsync(new SeoSettings
            {
                PageTitleSeparator = ". ",
                PageTitleSeoAdjustment = PageTitleSeoAdjustment.PagenameAfterSitename,
                ConvertNonWesternChars = false,
                AllowUnicodeCharsInUrls = true,
                CanonicalUrlsEnabled = false,
                QueryStringInCanonicalUrlsEnabled = false,
                WwwRequirement = WwwRequirement.NoMatter,
                TwitterMetaTags = true,
                OpenGraphMetaTags = true,
                MicrodataEnabled = true,
                ReservedUrlRecordSlugs = SeoDefaults.ReservedUrlRecordSlugs,
                CustomHeadTags = string.Empty
            });

            await settingService.SaveSettingAsync(new AdminAreaSettings
            {
                DefaultGridPageSize = 15,
                PopupGridPageSize = 7,
                GridPageSizes = "7, 15, 20, 50, 100",
                RichEditorAdditionalSettings = null,
                RichEditorAllowJavaScript = false,
                RichEditorAllowStyleTag = false,
                UseRichEditorForCustomerEmails = false,
                UseRichEditorInMessageTemplates = false,
                CheckLicense = true,
                UseIsoDateFormatInJsonResult = true,
                ShowDocumentationReferenceLinks = true
            });

            await settingService.SaveSettingAsync(new LocalizationSettings
            {
                DefaultAdminLanguageId =
                    _languageRepository.Table
                        .Single(l => l.LanguageCulture == CommonDefaults.DefaultLanguageCulture).Id,
                UseImagesForLanguageSelection = false,
                SeoFriendlyUrlsForLanguagesEnabled = false,
                AutomaticallyDetectLanguage = false,
                LoadAllLocaleRecordsOnStartup = true,
                LoadAllLocalizedPropertiesOnStartup = true,
                LoadAllUrlRecordsOnStartup = false,
                IgnoreRtlPropertyForAdminArea = false
            });

            await settingService.SaveSettingAsync(new CustomerSettings
            {
                UsernamesEnabled = false,
                CheckUsernameAvailabilityEnabled = false,
                AllowUsersToChangeUsernames = false,
                DefaultPasswordFormat = PasswordFormat.Hashed,
                HashedPasswordFormat = AssetForgeCustomerServicesDefaults.DefaultHashedPasswordFormat,
                PasswordMinLength = 6,
                PasswordMaxLength = 64,
                PasswordRequireDigit = false,
                PasswordRequireLowercase = false,
                PasswordRequireNonAlphanumeric = false,
                PasswordRequireUppercase = false,
                UnduplicatedPasswordsNumber = 4,
                PasswordRecoveryLinkDaysValid = 7,
                PasswordLifetime = 90,
                FailedPasswordAllowedAttempts = 0,
                FailedPasswordLockoutMinutes = 30,
                RequiredReLoginAfterPasswordChange = false,
                UserRegistrationType = UserRegistrationType.Standard,
                AllowCustomersToUploadAvatars = false,
                AvatarMaximumSizeBytes = 20000,
                DefaultAvatarEnabled = true,
                ShowCustomersLocation = false,
                ShowCustomersJoinDate = false,
                AllowViewingProfiles = false,
                NotifyNewCustomerRegistration = false,
                HideDownloadableProductsTab = false,
                HideBackInStockSubscriptionsTab = false,
                DownloadableProductsValidateUser = false,
                CustomerNameFormat = CustomerNameFormat.ShowFirstName,
                FirstNameEnabled = true,
                FirstNameRequired = true,
                LastNameEnabled = true,
                LastNameRequired = true,
                GenderEnabled = true,
                NeutralGenderEnabled = false,
                DateOfBirthEnabled = false,
                DateOfBirthRequired = false,
                DateOfBirthMinimumAge = null,
                CompanyEnabled = true,
                StreetAddressEnabled = false,
                StreetAddress2Enabled = false,
                ZipPostalCodeEnabled = false,
                CityEnabled = false,
                CountyEnabled = false,
                CountyRequired = false,
                CountryEnabled = false,
                CountryRequired = false,
                StateProvinceEnabled = false,
                StateProvinceRequired = false,
                PhoneEnabled = false,
                FaxEnabled = false,
                AcceptPrivacyPolicyEnabled = false,
                NewsletterEnabled = true,
                NewsletterTickedByDefault = true,
                HideNewsletterBlock = false,
                NewsletterBlockAllowToUnsubscribe = false,
                OnlineCustomerMinutes = 20,
                SiteLastVisitedPage = false,
                SiteIpAddresses = true,
                LastActivityMinutes = 15,
                SuffixDeletedCustomers = false,
                EnteringEmailTwice = false,
                DeleteGuestTaskOlderThanMinutes = 1440,
                PhoneNumberValidationEnabled = false,
                PhoneNumberValidationUseRegex = false,
                PhoneNumberValidationRule = "^[0-9]{1,14}?$",
                DefaultCountryId = _countryRepository.Table.FirstOrDefault(c => c.ThreeLetterIsoCode == regionInfo.ThreeLetterISORegionName)?.Id
            });

            await settingService.SaveSettingAsync(new MultiFactorAuthenticationSettings
            {
                ForceMultifactorAuthentication = false
            });

            await settingService.SaveSettingAsync(new AddressSettings
            {
                CompanyEnabled = true,
                StreetAddressEnabled = true,
                StreetAddressRequired = true,
                StreetAddress2Enabled = true,
                ZipPostalCodeEnabled = true,
                ZipPostalCodeRequired = true,
                CityEnabled = true,
                CityRequired = true,
                CountyEnabled = false,
                CountyRequired = false,
                CountryEnabled = true,
                StateProvinceEnabled = true,
                PhoneEnabled = true,
                PhoneRequired = true,
                FaxEnabled = true,
                DefaultCountryId = _countryRepository.Table.FirstOrDefault(c => c.ThreeLetterIsoCode == regionInfo.ThreeLetterISORegionName)?.Id
            });

            await settingService.SaveSettingAsync(new MediaSettings
            {
                AvatarPictureSize = 120,
                CatelogThumbPictureSize = 450,
                AutoCompleteSearchThumbPictureSize = 20,
                ImageSquarePictureSize = 32,
                MaximumImageSize = 1980,
                DefaultPictureZoomEnabled = false,
                AllowSVGUploads = false,
                DefaultImageQuality = 80,
                MultipleThumbDirectories = false,
                ImportProductImagesUsingHash = true,
                AzureCacheControlHeader = string.Empty,
                UseAbsoluteImagePath = true,
                VideoIframeAllow = "fullscreen",
                VideoIframeWidth = 300,
                VideoIframeHeight = 150
            });

            await settingService.SaveSettingAsync(new SiteInformationSettings
            {
                SiteClosed = false,
                DefaultSiteTheme = "DefaultClean",
                AllowCustomerToSelectTheme = false,
                DisplayEuCookieLawWarning = isEurope,
                FacebookLink = "https://www.facebook.com/radiowavehub",
                TwitterLink = "https://twitter.com/radiowavehub",
                YoutubeLink = "https://www.youtube.com/user/radiowavehub",
                InstagramLink = "https://www.instagram.com/radiowavehub_official",
            });

            await settingService.SaveSettingAsync(new SecuritySettings
            {
                EncryptionKey = CommonHelper.GenerateRandomDigitCode(16),
                AdminAreaAllowedIpAddresses = null,
                HoneypotEnabled = false,
                HoneypotInputName = "hpinput",
                AllowNonAsciiCharactersInHeaders = true,
                UseAesEncryptionAlgorithm = true,
                AllowSiteOwnerExportImportCustomersWithHashedPassword = true
            });

            await settingService.SaveSettingAsync(new DateTimeSettings
            {
                DefaultSiteTimeZoneId = string.Empty,
                AllowCustomersToSetTimeZone = false
            });

            await settingService.SaveSettingAsync(new DisplayDefaultMenuItemSettings
            {
                DisplayHomepageMenuItem = true,
                DisplayCustomerInfoMenuItem = true,
                DisplayBlogMenuItem = true,
                DisplayForumsMenuItem = true,
                DisplayContactUsMenuItem = true
            });

            await settingService.SaveSettingAsync(new DisplayDefaultFooterItemSettings
            {
                DisplaySitemapFooterItem = true,
                DisplayContactUsFooterItem = true,
                DisplayNewsFooterItem = true,
                DisplayBlogFooterItem = true,
                DisplayForumsFooterItem = true,
                DisplayCustomerInfoFooterItem = true,
                DisplayCustomerAddressesFooterItem = true,
            });

            await settingService.SaveSettingAsync(new CaptchaSettings
            {
                ReCaptchaApiUrl = "https://www.google.com/recaptcha/",
                ReCaptchaDefaultLanguage = string.Empty,
                ReCaptchaPrivateKey = string.Empty,
                ReCaptchaPublicKey = string.Empty,
                ReCaptchaRequestTimeout = 20,
                ReCaptchaTheme = string.Empty,
                AutomaticallyChooseLanguage = true,
                Enabled = false,
                CaptchaType = CaptchaType.CheckBoxReCaptchaV2,
                ReCaptchaV3ScoreThreshold = 0.5M,
                ShowOnBlogCommentPage = false,
                ShowOnContactUsPage = false,
                ShowOnForgotPasswordPage = false,
                ShowOnForum = false,
                ShowOnLoginPage = false,
                ShowOnNewsCommentPage = false,
                ShowOnNewsletterPage = false,
                ShowOnRegistrationPage = false,
            });


            await settingService.SaveSettingAsync(new ProxySettings
            {
                Enabled = false,
                Address = string.Empty,
                Port = string.Empty,
                Username = string.Empty,
                Password = string.Empty,
                BypassOnLocal = true,
                PreAuthenticate = true
            });

            await settingService.SaveSettingAsync(new CookieSettings
            {
                CompareProductsCookieExpires = 24 * 10,
                RecentlyViewedProductsCookieExpires = 24 * 10,
                CustomerCookieExpires = 24 * 365
            });

            await settingService.SaveSettingAsync(new RobotsTxtSettings
            {
                DisallowPaths =
                [
                    "/admin",
                    "/bin/",
                    "/files/",
                    "/files/exportimport/",
                    "/install",
                    "/*?*returnUrl=",
                    //AJAX urls
                    "/cart/estimateshipping",
                    "/cart/selectshippingoption",
                    "/customer/addressdelete",
                    "/customer/removeexternalassociation",
                    "/customer/checkusernameavailability",
                    "/catalog/searchtermautocomplete",
                    "/catalog/getcatalogroot",
                    "/addproducttocart/catalog/*",
                    "/addproducttocart/details/*",
                    "/compareproducts/add/*",
                    "/backinstocksubscribe/*",
                    "/subscribenewsletter",
                    "/t-popup/*",
                    "/setproductreviewhelpfulness",
                    "/poll/vote",
                    "/country/getstatesbycountryid/",
                    "/eucookielawaccept",
                    "/page/authenticate",
                    "/category/products/",
                    "/product/combinations",
                    "/uploadfileproductattribute/*",
                    "/shoppingcart/productdetails_attributechange/*",
                    "/uploadfilereturnrequest",
                    "/boards/pagewatch/*",
                    "/boards/forumwatch/*",
                    "/install/restartapplication",
                    "/boards/postvote",
                    "/product/estimateshipping/*",
                    "/shoppingcart/checkoutattributechange/*"
                ],
                LocalizableDisallowPaths =
                [
                    "/addproducttocart/catalog/",
                    "/addproducttocart/details/",
                    "/backinstocksubscriptions/manage",
                    "/boards/forumsubscriptions",
                    "/boards/forumwatch",
                    "/boards/postedit",
                    "/boards/postdelete",
                    "/boards/postcreate",
                    "/boards/pageedit",
                    "/boards/pagedelete",
                    "/boards/pagecreate",
                    "/boards/pagemove",
                    "/boards/pagewatch",
                    "/cart$",
                    "/changecurrency",
                    "/changelanguage",
                    "/changetaxtype",
                    "/checkout",
                    "/checkout/billingaddress",
                    "/checkout/completed",
                    "/checkout/confirm",
                    "/checkout/shippingaddress",
                    "/checkout/shippingmethod",
                    "/checkout/paymentinfo",
                    "/checkout/paymentmethod",
                    "/clearcomparelist",
                    "/compareproducts",
                    "/compareproducts/add/*",
                    "/customer/avatar",
                    "/customer/activation",
                    "/customer/addresses",
                    "/customer/changepassword",
                    "/customer/checkusernameavailability",
                    "/customer/downloadableproducts",
                    "/customer/info",
                    "/customer/productreviews",
                    "/deletepm",
                    "/emailwishlist",
                    "/eucookielawaccept",
                    "/inboxupdate",
                    "/newsletter/subscriptionactivation",
                    "/onepagecheckout",
                    "/order/history",
                    "/orderdetails",
                    "/passwordrecovery/confirm",
                    "/poll/vote",
                    "/privatemessages",
                    "/recentlyviewedproducts",
                    "/returnrequest",
                    "/returnrequest/history",
                    "/rewardpoints/history",
                    "/search?",
                    "/sendpm",
                    "/sentupdate",
                    "/shoppingcart/*",
                    "/siteclosed",
                    "/subscribenewsletter",
                    "/page/authenticate",
                    "/viewpm",
                    "/uploadfilecheckoutattribute",
                    "/uploadfileproductattribute",
                    "/uploadfilereturnrequest",
                    "/wishlist"
                ]
            });
        }

        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task InstallActivityLogTypesAsync()
        {
            var activityLogTypes = new List<ActivityLogType>
            {
                //admin area activities
                new() {
                    SystemKeyword = "AddNewAddressAttribute",
                    Enabled = true,
                    Name = "Add a new address attribute"
                },
                new() {
                    SystemKeyword = "AddNewAddressAttributeValue",
                    Enabled = true,
                    Name = "Add a new address attribute value"
                },
                new() {
                    SystemKeyword = "AddNewAffiliate",
                    Enabled = true,
                    Name = "Add a new affiliate"
                },
                new() {
                    SystemKeyword = "AddNewBlogPost",
                    Enabled = true,
                    Name = "Add a new blog post"
                },

                new() {
                    SystemKeyword = "AddNewCountry",
                    Enabled = true,
                    Name = "Add a new country"
                },
                new() {
                    SystemKeyword = "AddNewCustomer",
                    Enabled = true,
                    Name = "Add a new customer"
                },
                new() {
                    SystemKeyword = "AddNewCustomerAttribute",
                    Enabled = true,
                    Name = "Add a new customer attribute"
                },
                new() {
                    SystemKeyword = "AddNewCustomerAttributeValue",
                    Enabled = true,
                    Name = "Add a new customer attribute value"
                },
                new() {
                    SystemKeyword = "AddNewCustomerRole",
                    Enabled = true,
                    Name = "Add a new customer role"
                },
                new() {
                    SystemKeyword = "AddNewEmailAccount",
                    Enabled = true,
                    Name = "Add a new email account"
                },
                new() {
                    SystemKeyword = "AddNewLanguage",
                    Enabled = true,
                    Name = "Add a new language"
                },
                new() {
                    SystemKeyword = "AddNewNews",
                    Enabled = true,
                    Name = "Add a new news"
                },
                new() {
                    SystemKeyword = "AddNewSetting",
                    Enabled = true,
                    Name = "Add a new setting"
                },
                new() {
                    SystemKeyword = "AddNewSpecAttribute",
                    Enabled = true,
                    Name = "Add a new specification attribute"
                },
                new() {
                    SystemKeyword = "AddNewSpecAttributeGroup",
                    Enabled = true,
                    Name = "Add a new specification attribute group"
                },
                new() {
                    SystemKeyword = "AddNewStateProvince",
                    Enabled = true,
                    Name = "Add a new state or province"
                },
                new() {
                    SystemKeyword = "AddNewSite",
                    Enabled = true,
                    Name = "Add a new site"
                },
                new() {
                    SystemKeyword = "AddNewPage",
                    Enabled = true,
                    Name = "Add a new page"
                },
                new() {
                    SystemKeyword = "AddNewReviewType",
                    Enabled = true,
                    Name = "Add a new review type"
                },
                new() {
                    SystemKeyword = "AddNewWarehouse",
                    Enabled = true,
                    Name = "Add a new warehouse"
                },
                new() {
                    SystemKeyword = "AddNewWidget",
                    Enabled = true,
                    Name = "Add a new widget"
                },
                new() {
                    SystemKeyword = "DeleteActivityLog",
                    Enabled = true,
                    Name = "Delete activity log"
                },
                new() {
                    SystemKeyword = "DeleteAddressAttribute",
                    Enabled = true,
                    Name = "Delete an address attribute"
                },
                new() {
                    SystemKeyword = "DeleteAddressAttributeValue",
                    Enabled = true,
                    Name = "Delete an address attribute value"
                },
                new() {
                    SystemKeyword = "DeleteAffiliate",
                    Enabled = true,
                    Name = "Delete an affiliate"
                },
                new() {
                    SystemKeyword = "DeleteBlogPost",
                    Enabled = true,
                    Name = "Delete a blog post"
                },
                new() {
                    SystemKeyword = "DeleteBlogPostComment",
                    Enabled = true,
                    Name = "Delete a blog post comment"
                },
                new() {
                    SystemKeyword = "DeleteCampaign",
                    Enabled = true,
                    Name = "Delete a campaign"
                },
                new() {
                    SystemKeyword = "DeleteCategory",
                    Enabled = true,
                    Name = "Delete category"
                },
                new() {
                    SystemKeyword = "DeleteCheckoutAttribute",
                    Enabled = true,
                    Name = "Delete a checkout attribute"
                },
                new() {
                    SystemKeyword = "DeleteCountry",
                    Enabled = true,
                    Name = "Delete a country"
                },
                new() {
                    SystemKeyword = "DeleteCurrency",
                    Enabled = true,
                    Name = "Delete a currency"
                },
                new() {
                    SystemKeyword = "DeleteCustomer",
                    Enabled = true,
                    Name = "Delete a customer"
                },
                new() {
                    SystemKeyword = "DeleteCustomerAttribute",
                    Enabled = true,
                    Name = "Delete a customer attribute"
                },
                new() {
                    SystemKeyword = "DeleteCustomerAttributeValue",
                    Enabled = true,
                    Name = "Delete a customer attribute value"
                },
                new() {
                    SystemKeyword = "DeleteCustomerRole",
                    Enabled = true,
                    Name = "Delete a customer role"
                },
                new() {
                    SystemKeyword = "DeleteDiscount",
                    Enabled = true,
                    Name = "Delete a discount"
                },
                new() {
                    SystemKeyword = "DeleteEmailAccount",
                    Enabled = true,
                    Name = "Delete an email account"
                },
                new() {
                    SystemKeyword = "DeleteGiftCard",
                    Enabled = true,
                    Name = "Delete a gift card"
                },
                new() {
                    SystemKeyword = "DeleteLanguage",
                    Enabled = true,
                    Name = "Delete a language"
                },
                new() {
                    SystemKeyword = "DeleteManufacturer",
                    Enabled = true,
                    Name = "Delete a manufacturer"
                },
                new() {
                    SystemKeyword = "DeleteMeasureDimension",
                    Enabled = true,
                    Name = "Delete a measure dimension"
                },
                new() {
                    SystemKeyword = "DeleteMeasureWeight",
                    Enabled = true,
                    Name = "Delete a measure weight"
                },
                new() {
                    SystemKeyword = "DeleteMessageTemplate",
                    Enabled = true,
                    Name = "Delete a message template"
                },
                new() {
                    SystemKeyword = "DeleteNews",
                    Enabled = true,
                    Name = "Delete a news"
                },
                 new() {
                    SystemKeyword = "DeleteNewsComment",
                    Enabled = true,
                    Name = "Delete a news comment"
                },
                new() {
                    SystemKeyword = "DeleteOrder",
                    Enabled = true,
                    Name = "Delete an order"
                },
                new() {
                    SystemKeyword = "DeletePlugin",
                    Enabled = true,
                    Name = "Delete a plugin"
                },
                new() {
                    SystemKeyword = "DeleteProduct",
                    Enabled = true,
                    Name = "Delete a product"
                },
                new() {
                    SystemKeyword = "DeleteProductAttribute",
                    Enabled = true,
                    Name = "Delete a product attribute"
                },
                new() {
                    SystemKeyword = "DeleteProductReview",
                    Enabled = true,
                    Name = "Delete a product review"
                },
                new() {
                    SystemKeyword = "DeleteReturnRequest",
                    Enabled = true,
                    Name = "Delete a return request"
                },
                new() {
                    SystemKeyword = "DeleteReviewType",
                    Enabled = true,
                    Name = "Delete a review type"
                },
                new() {
                    SystemKeyword = "DeleteSetting",
                    Enabled = true,
                    Name = "Delete a setting"
                },
                new() {
                    SystemKeyword = "DeleteSpecAttribute",
                    Enabled = true,
                    Name = "Delete a specification attribute"
                },
                new() {
                    SystemKeyword = "DeleteSpecAttributeGroup",
                    Enabled = true,
                    Name = "Delete a specification attribute group"
                },
                new() {
                    SystemKeyword = "DeleteStateProvince",
                    Enabled = true,
                    Name = "Delete a state or province"
                },
                new() {
                    SystemKeyword = "DeleteSite",
                    Enabled = true,
                    Name = "Delete a site"
                },
                new() {
                    SystemKeyword = "DeleteSystemLog",
                    Enabled = true,
                    Name = "Delete system log"
                },
                new() {
                    SystemKeyword = "DeletePage",
                    Enabled = true,
                    Name = "Delete a page"
                },
                new() {
                    SystemKeyword = "DeleteWarehouse",
                    Enabled = true,
                    Name = "Delete a warehouse"
                },
                new() {
                    SystemKeyword = "DeleteWidget",
                    Enabled = true,
                    Name = "Delete a widget"
                },
                new() {
                    SystemKeyword = "EditActivityLogTypes",
                    Enabled = true,
                    Name = "Edit activity log types"
                },
                new() {
                    SystemKeyword = "EditAddressAttribute",
                    Enabled = true,
                    Name = "Edit an address attribute"
                },
                 new() {
                    SystemKeyword = "EditAddressAttributeValue",
                    Enabled = true,
                    Name = "Edit an address attribute value"
                },
                new() {
                    SystemKeyword = "EditAffiliate",
                    Enabled = true,
                    Name = "Edit an affiliate"
                },
                new() {
                    SystemKeyword = "EditBlogPost",
                    Enabled = true,
                    Name = "Edit a blog post"
                },
                new() {
                    SystemKeyword = "EditCampaign",
                    Enabled = true,
                    Name = "Edit a campaign"
                },
                new() {
                    SystemKeyword = "EditCategory",
                    Enabled = true,
                    Name = "Edit category"
                },
                new() {
                    SystemKeyword = "EditCheckoutAttribute",
                    Enabled = true,
                    Name = "Edit a checkout attribute"
                },
                new() {
                    SystemKeyword = "EditCountry",
                    Enabled = true,
                    Name = "Edit a country"
                },
                new() {
                    SystemKeyword = "EditCurrency",
                    Enabled = true,
                    Name = "Edit a currency"
                },
                new() {
                    SystemKeyword = "EditCustomer",
                    Enabled = true,
                    Name = "Edit a customer"
                },
                new() {
                    SystemKeyword = "EditCustomerAttribute",
                    Enabled = true,
                    Name = "Edit a customer attribute"
                },
                new() {
                    SystemKeyword = "EditCustomerAttributeValue",
                    Enabled = true,
                    Name = "Edit a customer attribute value"
                },
                new() {
                    SystemKeyword = "EditCustomerRole",
                    Enabled = true,
                    Name = "Edit a customer role"
                },
                new() {
                    SystemKeyword = "EditDiscount",
                    Enabled = true,
                    Name = "Edit a discount"
                },
                new() {
                    SystemKeyword = "EditEmailAccount",
                    Enabled = true,
                    Name = "Edit an email account"
                },
                new() {
                    SystemKeyword = "EditGiftCard",
                    Enabled = true,
                    Name = "Edit a gift card"
                },
                new() {
                    SystemKeyword = "EditLanguage",
                    Enabled = true,
                    Name = "Edit a language"
                },
                new() {
                    SystemKeyword = "EditManufacturer",
                    Enabled = true,
                    Name = "Edit a manufacturer"
                },
                new() {
                    SystemKeyword = "EditMeasureDimension",
                    Enabled = true,
                    Name = "Edit a measure dimension"
                },
                new() {
                    SystemKeyword = "EditMeasureWeight",
                    Enabled = true,
                    Name = "Edit a measure weight"
                },
                new() {
                    SystemKeyword = "EditMessageTemplate",
                    Enabled = true,
                    Name = "Edit a message template"
                },
                new() {
                    SystemKeyword = "EditNews",
                    Enabled = true,
                    Name = "Edit a news"
                },
                new() {
                    SystemKeyword = "EditOrder",
                    Enabled = true,
                    Name = "Edit an order"
                },
                new() {
                    SystemKeyword = "EditPlugin",
                    Enabled = true,
                    Name = "Edit a plugin"
                },
                new() {
                    SystemKeyword = "EditProduct",
                    Enabled = true,
                    Name = "Edit a product"
                },
                new() {
                    SystemKeyword = "EditProductAttribute",
                    Enabled = true,
                    Name = "Edit a product attribute"
                },
                new() {
                    SystemKeyword = "EditProductReview",
                    Enabled = true,
                    Name = "Edit a product review"
                },
                new() {
                    SystemKeyword = "EditPromotionProviders",
                    Enabled = true,
                    Name = "Edit promotion providers"
                },
                new() {
                    SystemKeyword = "EditReturnRequest",
                    Enabled = true,
                    Name = "Edit a return request"
                },
                new() {
                    SystemKeyword = "EditReviewType",
                    Enabled = true,
                    Name = "Edit a review type"
                },
                new() {
                    SystemKeyword = "EditSettings",
                    Enabled = true,
                    Name = "Edit setting(s)"
                },
                new() {
                    SystemKeyword = "EditStateProvince",
                    Enabled = true,
                    Name = "Edit a state or province"
                },
                new() {
                    SystemKeyword = "EditSite",
                    Enabled = true,
                    Name = "Edit a site"
                },
                new() {
                    SystemKeyword = "EditTask",
                    Enabled = true,
                    Name = "Edit a task"
                },
                new() {
                    SystemKeyword = "EditSpecAttribute",
                    Enabled = true,
                    Name = "Edit a specification attribute"
                },
                new() {
                    SystemKeyword = "EditSpecAttributeGroup",
                    Enabled = true,
                    Name = "Edit a specification attribute group"
                },
                new() {
                    SystemKeyword = "EditWarehouse",
                    Enabled = true,
                    Name = "Edit a warehouse"
                },
                new() {
                    SystemKeyword = "EditPage",
                    Enabled = true,
                    Name = "Edit a page"
                },
                new() {
                    SystemKeyword = "EditWidget",
                    Enabled = true,
                    Name = "Edit a widget"
                },
                new() {
                    SystemKeyword = "Impersonation.Started",
                    Enabled = true,
                    Name = "Customer impersonation session. Started"
                },
                new() {
                    SystemKeyword = "Impersonation.Finished",
                    Enabled = true,
                    Name = "Customer impersonation session. Finished"
                },
                new() {
                    SystemKeyword = "ImportCategories",
                    Enabled = true,
                    Name = "Categories were imported"
                },
                new() {
                    SystemKeyword = "ImportManufacturers",
                    Enabled = true,
                    Name = "Manufacturers were imported"
                },
                new() {
                    SystemKeyword = "ImportProducts",
                    Enabled = true,
                    Name = "Products were imported"
                },
                new() {
                    SystemKeyword = "ImportCustomers",
                    Enabled = true,
                    Name = "Customers were imported"
                },
                new() {
                    SystemKeyword = "ImportNewsLetterSubscriptions",
                    Enabled = true,
                    Name = "Newsletter subscriptions were imported"
                },
                new() {
                    SystemKeyword = "ImportStates",
                    Enabled = true,
                    Name = "States were imported"
                },
                new() {
                    SystemKeyword = "ExportCustomers",
                    Enabled = true,
                    Name = "Customers were exported"
                },
                new() {
                    SystemKeyword = "ExportCategories",
                    Enabled = true,
                    Name = "Categories were exported"
                },
                new() {
                    SystemKeyword = "ExportManufacturers",
                    Enabled = true,
                    Name = "Manufacturers were exported"
                },
                new() {
                    SystemKeyword = "ExportProducts",
                    Enabled = true,
                    Name = "Products were exported"
                },
                new() {
                    SystemKeyword = "ExportOrders",
                    Enabled = true,
                    Name = "Orders were exported"
                },
                new() {
                    SystemKeyword = "ExportStates",
                    Enabled = true,
                    Name = "States were exported"
                },
                new() {
                    SystemKeyword = "ExportNewsLetterSubscriptions",
                    Enabled = true,
                    Name = "Newsletter subscriptions were exported"
                },
                new() {
                    SystemKeyword = "InstallNewPlugin",
                    Enabled = true,
                    Name = "Install a new plugin"
                },
                new() {
                    SystemKeyword = "UninstallPlugin",
                    Enabled = true,
                    Name = "Uninstall a plugin"
                },
                new() {
                    SystemKeyword = "UpdatePlugin",
                    Enabled = true,
                    Name = "Update a plugin"
                },
                //public site activities
                new() {
                    SystemKeyword = "PublicSite.ViewCategory",
                    Enabled = false,
                    Name = "Public site. View a category"
                },
                new() {
                    SystemKeyword = "PublicSite.ViewManufacturer",
                    Enabled = false,
                    Name = "Public site. View a manufacturer"
                },
                new() {
                    SystemKeyword = "PublicSite.ViewProduct",
                    Enabled = false,
                    Name = "Public site. View a product"
                },
                new() {
                    SystemKeyword = "PublicSite.PlaceOrder",
                    Enabled = false,
                    Name = "Public site. Place an order"
                },
                new() {
                    SystemKeyword = "PublicSite.SendPM",
                    Enabled = false,
                    Name = "Public site. Send PM"
                },
                new() {
                    SystemKeyword = "PublicSite.ContactUs",
                    Enabled = false,
                    Name = "Public site. Use contact us form"
                },
                new() {
                    SystemKeyword = "PublicSite.AddToCompareList",
                    Enabled = false,
                    Name = "Public site. Add to compare list"
                },
                new() {
                    SystemKeyword = "PublicSite.AddToShoppingCart",
                    Enabled = false,
                    Name = "Public site. Add to shopping cart"
                },
                new() {
                    SystemKeyword = "PublicSite.AddToWishlist",
                    Enabled = false,
                    Name = "Public site. Add to wishlist"
                },
                new() {
                    SystemKeyword = "PublicSite.Login",
                    Enabled = false,
                    Name = "Public site. Login"
                },
                new() {
                    SystemKeyword = "PublicSite.Logout",
                    Enabled = false,
                    Name = "Public site. Logout"
                },
                new() {
                    SystemKeyword = "PublicSite.AddProductReview",
                    Enabled = false,
                    Name = "Public site. Add product review"
                },
                new() {
                    SystemKeyword = "PublicSite.AddNewsComment",
                    Enabled = false,
                    Name = "Public site. Add news comment"
                },
                new() {
                    SystemKeyword = "PublicSite.AddBlogComment",
                    Enabled = false,
                    Name = "Public site. Add blog comment"
                },
                new() {
                    SystemKeyword = "PublicSite.AddForumPage",
                    Enabled = false,
                    Name = "Public site. Add forum page"
                },
                new() {
                    SystemKeyword = "PublicSite.EditForumPage",
                    Enabled = false,
                    Name = "Public site. Edit forum page"
                },
                new() {
                    SystemKeyword = "PublicSite.DeleteForumPage",
                    Enabled = false,
                    Name = "Public site. Delete forum page"
                },
                new() {
                    SystemKeyword = "PublicSite.AddForumPost",
                    Enabled = false,
                    Name = "Public site. Add forum post"
                },
                new() {
                    SystemKeyword = "PublicSite.EditForumPost",
                    Enabled = false,
                    Name = "Public site. Edit forum post"
                },
                new() {
                    SystemKeyword = "PublicSite.DeleteForumPost",
                    Enabled = false,
                    Name = "Public site. Delete forum post"
                },
                new() {
                    SystemKeyword = "UploadNewPlugin",
                    Enabled = true,
                    Name = "Upload a plugin"
                },
                new() {
                    SystemKeyword = "UploadNewTheme",
                    Enabled = true,
                    Name = "Upload a theme"
                },
                new() {
                    SystemKeyword = "UploadIcons",
                    Enabled = true,
                    Name = "Upload a favicon and app icons"
                }
            };

            await InsertInstallationDataAsync(activityLogTypes);
        }

        /// <returns>A task that represents the asynchronous operation</returns>
        protected virtual async Task InstallScheduleTasksAsync()
        {
            var lastEnabledUtc = DateTime.UtcNow;
            var tasks = new List<ScheduleTask>
            {
                new() {
                    Name = "Send emails",
                    Seconds = 60,
                    Type = "AssetForge.Services.Messages.QueuedMessagesSendTask, AssetForge.Services",
                    Enabled = true,
                    LastEnabledUtc = lastEnabledUtc,
                    StopOnError = false
                },
                new() {
                    Name = "Keep alive",
                    Seconds = 300,
                    Type = "AssetForge.Services.Common.KeepAliveTask, AssetForge.Services",
                    Enabled = true,
                    LastEnabledUtc = lastEnabledUtc,
                    StopOnError = false
                },
                new() {
                    Name = nameof(ResetLicenseCheckTask),
                    Seconds = 2073600,
                    Type = "AssetForge.Services.Common.ResetLicenseCheckTask, AssetForge.Services",
                    Enabled = true,
                    LastEnabledUtc = lastEnabledUtc,
                    StopOnError = false
                },
                new() {
                    Name = "Delete guests",
                    Seconds = 600,
                    Type = "AssetForge.Services.Customers.DeleteGuestsTask, AssetForge.Services",
                    Enabled = true,
                    LastEnabledUtc = lastEnabledUtc,
                    StopOnError = false
                },
                new() {
                    Name = "Clear cache",
                    Seconds = 600,
                    Type = "AssetForge.Services.Caching.ClearCacheTask, AssetForge.Services",
                    Enabled = false,
                    StopOnError = false
                },
                new() {
                    Name = "Clear log",
                    //60 minutes
                    Seconds = 3600,
                    Type = "AssetForge.Services.Logging.ClearLogTask, AssetForge.Services",
                    Enabled = false,
                    StopOnError = false
                },
            };

            await InsertInstallationDataAsync(tasks);
        }

        #endregion Utilities

        #region Methods

        /// <summary>
        /// Install required data
        /// </summary>
        /// <param name="defaultUserEmail">Default user email</param>
        /// <param name="defaultUserPassword">Default user password</param>
        /// <param name="languagePackInfo">Language pack info</param>
        /// <param name="regionInfo">RegionInfo</param>
        /// <param name="cultureInfo">CultureInfo</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InstallRequiredDataAsync(string defaultUserEmail, string defaultUserPassword,
            (string languagePackDownloadLink, int languagePackProgress) languagePackInfo, RegionInfo regionInfo, CultureInfo cultureInfo)
        {
            await InstallSitesAsync();
            await InstallLanguagesAsync(languagePackInfo, cultureInfo, regionInfo);
            await InstallCountriesAndStatesAsync();
            await InstallSettingsAsync(regionInfo);
            await InstallCustomersAndUsersAsync(defaultUserEmail, defaultUserPassword);
            await InstallActivityLogTypesAsync();
            await InstallScheduleTasksAsync();
        }

        /// <summary>
        /// Install sample data
        /// </summary>
        /// <param name="defaultUserEmail">Default user email</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InstallSampleDataAsync(string defaultUserEmail)
        {
            await InstallAdditionalSitesAsync();
            await InstallSampleCustomersAsync();
            await InstallSearchTermsAsync();

            var settingService = EngineContext.Current.Resolve<ISettingService>();

            await settingService.SaveSettingAsync(new DisplayDefaultMenuItemSettings
            {
                DisplayHomepageMenuItem = false,
                DisplayCustomerInfoMenuItem = false,
                DisplayBlogMenuItem = false,
                DisplayForumsMenuItem = false,
                DisplayContactUsMenuItem = false
            });
        }

        #endregion Methods
    }
}