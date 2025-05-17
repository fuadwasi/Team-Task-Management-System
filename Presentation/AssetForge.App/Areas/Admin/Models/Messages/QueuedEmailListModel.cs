using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Messages;

/// <summary>
/// Represents a queued email list model
/// </summary>
public partial record QueuedEmailListModel : BasePagedListModel<QueuedEmailModel>
{
}