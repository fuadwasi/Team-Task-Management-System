using AssetForge.Core.Domain.Sites;
using AssetForge.Services.Caching;
using AssetForge.Services.Localization;

namespace AssetForge.Services.Sites.Caching
{
    /// <summary>
    /// Represents a site cache event consumer
    /// </summary>
    public partial class SiteCacheEventConsumer : CacheEventConsumer<Site>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(Site entity)
        {
            await RemoveByPrefixAsync(LocalizationDefaults.LanguagesBySitePrefix, entity);
        }
    }
}