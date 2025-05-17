using AssetForge.Core.Domain.Messages;
using AssetForge.Services.Caching;

namespace AssetForge.Services.Messages.Caching;

/// <summary>
/// Represents an email account cache event consumer
/// </summary>
public partial class EmailAccountCacheEventConsumer : CacheEventConsumer<EmailAccount>
{
}