using AssetForge.Services.ScheduleTasks;

namespace AssetForge.Services.Common;

/// <summary>
/// Represents a task for keeping the site alive
/// </summary>
public partial class KeepAliveTask : IScheduleTask
{
    #region Fields

    protected readonly SiteHttpClient _siteHttpClient;

    #endregion

    #region Ctor

    public KeepAliveTask(SiteHttpClient siteHttpClient)
    {
        _siteHttpClient = siteHttpClient;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Executes a task
    /// </summary>
    public async Task ExecuteAsync()
    {
        await _siteHttpClient.KeepAliveAsync();
    }

    #endregion
}