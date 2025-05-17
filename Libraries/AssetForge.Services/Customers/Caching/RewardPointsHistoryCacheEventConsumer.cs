using AssetForge.Core.Domain.Customers;
using AssetForge.Services.Caching;

namespace AssetForge.Services.Customers.Caching;

/// <summary>
/// Represents a reward point history cache event consumer
/// </summary>
public partial class RewardPointsHistoryCacheEventConsumer : CacheEventConsumer<RewardPointsHistory>
{
}