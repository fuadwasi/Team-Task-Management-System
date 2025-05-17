using AssetForge.Core.Domain.Customers;
using AssetForge.Services.Caching;

namespace AssetForge.Services.Customers.Caching;

/// <summary>
/// Represents an external authentication record cache event consumer
/// </summary>
public partial class ExternalAuthenticationRecordCacheEventConsumer : CacheEventConsumer<ExternalAuthenticationRecord>
{
}