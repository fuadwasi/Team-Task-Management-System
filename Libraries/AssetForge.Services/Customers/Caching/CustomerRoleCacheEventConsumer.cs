using AssetForge.Core.Domain.Customers;
using AssetForge.Services.Caching;

namespace AssetForge.Services.Customers.Caching;

/// <summary>
/// Represents a customer role cache event consumer
/// </summary>
public partial class CustomerRoleCacheEventConsumer : CacheEventConsumer<CustomerRole>
{
    /// <summary>
    /// Clear cache by entity event type
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <param name="entityEventType">Entity event type</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    protected override async Task ClearCacheAsync(CustomerRole entity, EntityEventType entityEventType)
    {
        switch (entityEventType)
        {
            case EntityEventType.Update:
                await RemoveByPrefixAsync(AssetForgeCustomerServicesDefaults.CustomerRolesBySystemNamePrefix);
                break;
            case EntityEventType.Delete:
                await RemoveAsync(AssetForgeCustomerServicesDefaults.CustomerRolesBySystemNameCacheKey, entity.SystemName);
                break;
        }

        if (entityEventType != EntityEventType.Insert)
            await RemoveByPrefixAsync(AssetForgeCustomerServicesDefaults.CustomerCustomerRolesPrefix);

        await base.ClearCacheAsync(entity, entityEventType);
    }
}