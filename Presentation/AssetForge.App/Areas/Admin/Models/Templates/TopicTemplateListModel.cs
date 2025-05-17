using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Templates;

/// <summary>
/// Represents a topic template list model
/// </summary>
public partial record TopicTemplateListModel : BasePagedListModel<TopicTemplateModel>
{
}