using AssetForge.Core.Domain.Security;
using AssetForge.Services.Caching;

namespace AssetForge.Services.Security.Caching;

/// <summary>
/// Represents a ACL record cache event consumer
/// </summary>
public partial class AclRecordCacheEventConsumer : CacheEventConsumer<AclRecord>
{
    /// <summary>
    /// Clear cache data
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    protected override async Task ClearCacheAsync(AclRecord entity)
    {
        await RemoveAsync(SecurityDefaults.AclRecordCacheKey, entity.EntityId, entity.EntityName);
        await RemoveAsync(SecurityDefaults.EntityAclRecordExistsCacheKey, entity.EntityName);
    }
}