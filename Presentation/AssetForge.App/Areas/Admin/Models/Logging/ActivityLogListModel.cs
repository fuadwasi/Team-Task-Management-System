using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Logging;

/// <summary>
/// Represents an activity log list model
/// </summary>
public partial record ActivityLogListModel : BasePagedListModel<ActivityLogModel>
{
}