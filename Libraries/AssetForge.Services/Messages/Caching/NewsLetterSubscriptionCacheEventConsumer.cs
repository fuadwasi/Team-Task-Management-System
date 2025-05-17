using AssetForge.Core.Domain.Messages;
using AssetForge.Services.Caching;

namespace AssetForge.Services.Messages.Caching;

/// <summary>
/// Represents news letter subscription cache event consumer
/// </summary>
public partial class NewsLetterSubscriptionCacheEventConsumer : CacheEventConsumer<NewsLetterSubscription>
{
}