using AssetForge.Core.Domain.Messages;
using AssetForge.Services.Caching;

namespace AssetForge.Services.Messages.Caching;

/// <summary>
/// Represents an queued email cache event consumer
/// </summary>
public partial class QueuedEmailCacheEventConsumer : CacheEventConsumer<QueuedEmail>
{
}