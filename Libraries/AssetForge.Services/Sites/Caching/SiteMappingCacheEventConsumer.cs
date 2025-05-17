using AssetForge.Core.Domain.Sites;
using AssetForge.Services.Caching;

namespace AssetForge.Services.Sites.Caching
{
    /// <summary>
    /// Represents a site mapping cache event consumer
    /// </summary>
    public partial class SiteMappingCacheEventConsumer : CacheEventConsumer<SiteMapping>
    {
        /// <summary>
        /// Clear cache data
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        protected override async Task ClearCacheAsync(SiteMapping entity)
        {
            await RemoveAsync(SiteDefaults.SiteMappingsCacheKey, entity.EntityId, entity.EntityName);
            await RemoveAsync(SiteDefaults.SiteMappingIdsCacheKey, entity.EntityId, entity.EntityName);
            await RemoveAsync(SiteDefaults.SiteMappingExistsCacheKey, entity.EntityName);
        }
    }
}
