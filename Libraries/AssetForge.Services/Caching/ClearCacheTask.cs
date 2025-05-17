using AssetForge.Core.Caching;
using AssetForge.Services.ScheduleTasks;

namespace AssetForge.Services.Caching;

/// <summary>
/// Clear cache scheduled task implementation
/// </summary>
public partial class ClearCacheTask : IScheduleTask
{
    #region Fields

    protected readonly IStaticCacheManager _staticCacheManager;

    #endregion

    #region Ctor

    public ClearCacheTask(IStaticCacheManager staticCacheManager)
    {
        _staticCacheManager = staticCacheManager;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Executes a task
    /// </summary>
    public async System.Threading.Tasks.Task ExecuteAsync()
    {
        await _staticCacheManager.ClearAsync();
    }

    #endregion
}