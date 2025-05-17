using AssetForge.Core.Domain.Messages;
using AssetForge.Services.Caching;

namespace AssetForge.Services.Messages.Caching;

/// <summary>
/// Represents a message template cache event consumer
/// </summary>
public partial class MessageTemplateCacheEventConsumer : CacheEventConsumer<MessageTemplate>
{
    /// <summary>
    /// Clear cache data
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    protected override async Task ClearCacheAsync(MessageTemplate entity)
    {
        await RemoveByPrefixAsync(AssetForgeMessageDefaults.MessageTemplatesByNamePrefix, entity.Name);
    }
}