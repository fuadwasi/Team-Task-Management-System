using AssetForge.Core.Domain.Vendors;
using AssetForge.Services.Attributes;
using AssetForge.Services.Caching;

namespace AssetForge.Services.Vendors.Caching;

/// <summary>
/// Represents a vendor attribute value cache event consumer
/// </summary>
public partial class VendorAttributeValueCacheEventConsumer : CacheEventConsumer<VendorAttributeValue>
{
    /// <summary>
    /// Clear cache data
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    protected override async Task ClearCacheAsync(VendorAttributeValue entity)
    {
        await RemoveAsync(AttributeDefaults.AttributeValuesByAttributeCacheKey, nameof(VendorAttribute), entity.AttributeId);
    }
}