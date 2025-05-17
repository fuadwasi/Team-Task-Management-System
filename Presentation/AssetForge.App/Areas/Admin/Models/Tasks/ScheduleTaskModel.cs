using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Tasks;

/// <summary>
/// Represents a schedule task model
/// </summary>
public partial record ScheduleTaskModel : BaseEntityModel
{
    #region Properties

    [ResourceDisplayName("Admin.System.ScheduleTasks.Name")]
    public string Name { get; set; }

    [ResourceDisplayName("Admin.System.ScheduleTasks.Seconds")]
    public int Seconds { get; set; }

    [ResourceDisplayName("Admin.System.ScheduleTasks.Enabled")]
    public bool Enabled { get; set; }

    [ResourceDisplayName("Admin.System.ScheduleTasks.StopOnError")]
    public bool StopOnError { get; set; }

    [ResourceDisplayName("Admin.System.ScheduleTasks.LastStart")]
    public string LastStartUtc { get; set; }

    [ResourceDisplayName("Admin.System.ScheduleTasks.LastEnd")]
    public string LastEndUtc { get; set; }

    [ResourceDisplayName("Admin.System.ScheduleTasks.LastSuccess")]
    public string LastSuccessUtc { get; set; }

    #endregion
}