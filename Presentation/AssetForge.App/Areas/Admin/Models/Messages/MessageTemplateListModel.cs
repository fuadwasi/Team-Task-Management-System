using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Messages;

/// <summary>
/// Represents a message template list model
/// </summary>
public partial record MessageTemplateListModel : BasePagedListModel<MessageTemplateModel>
{
}