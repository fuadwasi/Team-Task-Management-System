using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Tasks;

/// <summary>
/// Represents a schedule task list model
/// </summary>
public partial record ScheduleTaskListModel : BasePagedListModel<ScheduleTaskModel>
{
}