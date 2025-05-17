using AssetForge.Core.Domain.Customers;
using AssetForge.Services.Attributes;
using AssetForge.Services.Caching;

namespace AssetForge.Services.Customers.Caching;

/// <summary>
/// Represents a customer attribute value cache event consumer
/// </summary>
public partial class CustomerAttributeValueCacheEventConsumer : CacheEventConsumer<CustomerAttributeValue>
{
    /// <summary>
    /// Clear cache data
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    protected override async Task ClearCacheAsync(CustomerAttributeValue entity)
    {
        await RemoveAsync(AttributeDefaults.AttributeValuesByAttributeCacheKey, nameof(CustomerAttribute), entity.AttributeId);
    }
}