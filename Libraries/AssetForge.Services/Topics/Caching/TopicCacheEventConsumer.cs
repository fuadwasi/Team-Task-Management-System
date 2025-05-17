using AssetForge.Core.Domain.Topics;
using AssetForge.Services.Caching;

namespace AssetForge.Services.Topics.Caching;

/// <summary>
/// Represents a topic cache event consumer
/// </summary>
public partial class TopicCacheEventConsumer : CacheEventConsumer<Topic>
{
    /// <summary>
    /// Clear cache data
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    protected override async Task ClearCacheAsync(Topic entity)
    {
        await RemoveByPrefixAsync(AssetForgeTopicDefaults.TopicBySystemNamePrefix, entity.SystemName);
    }
}