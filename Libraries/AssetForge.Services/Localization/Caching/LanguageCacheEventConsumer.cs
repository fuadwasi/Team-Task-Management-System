using AssetForge.Core.Domain.Localization;
using AssetForge.Services.Caching;

namespace AssetForge.Services.Localization.Caching
{
    /// <summary>
    /// Represents a language cache event consumer
    /// </summary>
    public partial class LanguageCacheEventConsumer : CacheEventConsumer<Language>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(Language entity)
        {
            await RemoveAsync(LocalizationDefaults.LocaleStringResourcesAllPublicCacheKey, entity);
            await RemoveAsync(LocalizationDefaults.LocaleStringResourcesAllAdminCacheKey, entity);
            await RemoveAsync(LocalizationDefaults.LocaleStringResourcesAllCacheKey, entity);
            await RemoveByPrefixAsync(LocalizationDefaults.LocaleStringResourcesByNamePrefix, entity);
        }
    }
}