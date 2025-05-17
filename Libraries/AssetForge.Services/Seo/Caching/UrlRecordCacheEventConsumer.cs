using AssetForge.Core.Domain.Seo;
using AssetForge.Services.Caching;

namespace AssetForge.Services.Seo.Caching;

/// <summary>
/// Represents an URL record cache event consumer
/// </summary>
public partial class UrlRecordCacheEventConsumer : CacheEventConsumer<UrlRecord>
{
    /// <summary>
    /// Clear cache data
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    protected override async Task ClearCacheAsync(UrlRecord entity)
    {
        await RemoveAsync(SeoDefaults.UrlRecordCacheKey, entity.EntityId, entity.EntityName, entity.LanguageId);
        await RemoveAsync(SeoDefaults.UrlRecordBySlugCacheKey, entity.Slug);
        await RemoveAsync(SeoDefaults.UrlRecordEntityIdLookupCacheKey, entity.LanguageId);
        await RemoveAsync(SeoDefaults.UrlRecordSlugLookupCacheKey);
    }
}