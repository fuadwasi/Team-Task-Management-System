using AssetForge.Core.Domain.Vendors;
using AssetForge.Services.Attributes;
using AssetForge.Services.Caching;

namespace AssetForge.Services.Vendors.Caching;

/// <summary>
/// Represents a vendor attribute cache event consumer
/// </summary>
public partial class VendorAttributeCacheEventConsumer : CacheEventConsumer<VendorAttribute>
{
    /// <summary>
    /// Clear cache data
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    protected override async Task ClearCacheAsync(VendorAttribute entity)
    {
        await RemoveAsync(AttributeDefaults.AttributeValuesByAttributeCacheKey, nameof(VendorAttribute), entity);
    }
}