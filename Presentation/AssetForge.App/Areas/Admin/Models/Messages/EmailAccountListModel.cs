using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Messages;

/// <summary>
/// Represents an email account list model
/// </summary>
public partial record EmailAccountListModel : BasePagedListModel<EmailAccountModel>
{
}