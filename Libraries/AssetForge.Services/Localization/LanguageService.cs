using AssetForge.Core.Caching;
using AssetForge.Core.Domain.Localization;
using AssetForge.Data;
using AssetForge.Services.Configuration;
using AssetForge.Services.Sites;
using System.Globalization;

namespace AssetForge.Services.Localization
{
    /// <summary>
    /// Language service
    /// </summary>
    public partial class LanguageService : ILanguageService
    {
        #region Fields

        private readonly IRepository<Language> _languageRepository;
        private readonly ISettingService _settingService;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ISiteMappingService _siteMappingService;
        private readonly LocalizationSettings _localizationSettings;

        #endregion

        #region Ctor

        public LanguageService(IRepository<Language> languageRepository,
            ISettingService settingService,
            IStaticCacheManager staticCacheManager,
            ISiteMappingService siteMappingService,
            LocalizationSettings localizationSettings)
        {
            _languageRepository = languageRepository;
            _settingService = settingService;
            _staticCacheManager = staticCacheManager;
            _siteMappingService = siteMappingService;
            _localizationSettings = localizationSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a language
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteLanguageAsync(Language language)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));

            //update default admin area language (if required)
            if (_localizationSettings.DefaultAdminLanguageId == language.Id)
                foreach (var activeLanguage in await GetAllLanguagesAsync())
                {
                    if (activeLanguage.Id == language.Id)
                        continue;

                    _localizationSettings.DefaultAdminLanguageId = activeLanguage.Id;
                    await _settingService.SaveSettingAsync(_localizationSettings);
                    break;
                }

            await _languageRepository.DeleteAsync(language);
        }

        /// <summary>
        /// Gets all languages
        /// </summary>
        /// <param name="siteId">Load records allowed only in a specified site; pass 0 to load all records</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the languages
        /// </returns>
        public virtual async Task<IList<Language>> GetAllLanguagesAsync(bool showHidden = false, int siteId = 0)
        {
            //cacheable copy
            var key = _staticCacheManager.PrepareKeyForDefaultCache(LocalizationDefaults.LanguagesAllCacheKey, siteId, showHidden);

            var languages = await _staticCacheManager.GetAsync(key, async () =>
            {
                var allLanguages = await _languageRepository.GetAllAsync(query =>
                {
                    if (!showHidden)
                        query = query.Where(l => l.Published);
                    query = query.OrderBy(l => l.DisplayOrder).ThenBy(l => l.Id);

                    return query;
                });

                //site mapping
                if (siteId > 0)
                    allLanguages = await allLanguages
                        .WhereAwait(async l => await _siteMappingService.AuthorizeAsync(l, siteId))
                        .ToListAsync();

                return allLanguages;
            });

            return languages;
        }

        /// <summary>
        /// Gets all languages
        /// </summary>
        /// <param name="siteId">Load records allowed only in a specified site; pass 0 to load all records</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>
        /// The languages
        /// </returns>
        public virtual IList<Language> GetAllLanguages(bool showHidden = false, int siteId = 0)
        {
            //cacheable copy
            var key = _staticCacheManager.PrepareKeyForDefaultCache(LocalizationDefaults.LanguagesAllCacheKey, siteId, showHidden);

            var languages = _staticCacheManager.Get(key, () =>
            {
                var allLanguages = _languageRepository.GetAll(query =>
                {
                    if (!showHidden)
                        query = query.Where(l => l.Published);
                    query = query.OrderBy(l => l.DisplayOrder).ThenBy(l => l.Id);

                    return query;
                });

                //site mapping
                if (siteId > 0)
                    allLanguages = allLanguages
                        .Where(l => _siteMappingService.Authorize(l, siteId))
                        .ToList();

                return allLanguages;
            });

            return languages;
        }

        /// <summary>
        /// Gets a language
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the language
        /// </returns>
        public virtual async Task<Language> GetLanguageByIdAsync(int languageId)
        {
            return await _languageRepository.GetByIdAsync(languageId, cache => default);
        }

        /// <summary>
        /// Inserts a language
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertLanguageAsync(Language language)
        {
            await _languageRepository.InsertAsync(language);
        }

        /// <summary>
        /// Updates a language
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateLanguageAsync(Language language)
        {
            //update language
            await _languageRepository.UpdateAsync(language);
        }

        /// <summary>
        /// Get 2 letter ISO language code
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>ISO language code</returns>
        public virtual string GetTwoLetterIsoLanguageName(Language language)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));

            if (string.IsNullOrEmpty(language.LanguageCulture))
                return "en";

            var culture = new CultureInfo(language.LanguageCulture);
            var code = culture.TwoLetterISOLanguageName;

            return string.IsNullOrEmpty(code) ? "en" : code;
        }

        #endregion
    }
}