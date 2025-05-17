using AssetForge.Core.Domain.Customers;
using AssetForge.Services.Caching;

namespace AssetForge.Services.Customers.Caching;

/// <summary>
/// Represents a customer customer role mapping cache event consumer
/// </summary>
public partial class CustomerCustomerRoleMappingCacheEventConsumer : CacheEventConsumer<CustomerCustomerRoleMapping>
{
    /// <summary>
    /// Clear cache data
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    protected override async Task ClearCacheAsync(CustomerCustomerRoleMapping entity)
    {
        await RemoveAsync(AssetForgeCustomerServicesDefaults.CustomerRolesCacheKey, entity.CustomerId);
    }
}