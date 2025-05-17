using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Logging;

/// <summary>
/// Represents a log list model
/// </summary>
public partial record LogListModel : BasePagedListModel<LogModel>
{
}