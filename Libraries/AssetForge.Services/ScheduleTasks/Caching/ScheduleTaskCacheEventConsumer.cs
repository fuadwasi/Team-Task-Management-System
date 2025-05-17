using AssetForge.Core.Domain.ScheduleTasks;
using AssetForge.Services.Caching;

namespace AssetForge.Services.ScheduleTasks.Caching
{
    /// <summary>
    /// Represents a schedule task cache event consumer
    /// </summary>
    public partial class ScheduleTaskCacheEventConsumer : CacheEventConsumer<ScheduleTask>
    {
    }
}
