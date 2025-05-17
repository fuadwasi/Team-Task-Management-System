using AssetForge.Core.Domain.Customers;
using AssetForge.Services.Caching;

namespace AssetForge.Services.Customers.Caching;

/// <summary>
/// Represents a customer password cache event consumer
/// </summary>
public partial class CustomerPasswordCacheEventConsumer : CacheEventConsumer<CustomerPassword>
{
}