using AssetForge.Core.Domain.Logging;
using AssetForge.Services.Caching;

namespace AssetForge.Services.Logging.Caching
{
    /// <summary>
    /// Represents an activity log cache event consumer
    /// </summary>
    public partial class ActivityLogCacheEventConsumer : CacheEventConsumer<ActivityLog>
    {
    }
}