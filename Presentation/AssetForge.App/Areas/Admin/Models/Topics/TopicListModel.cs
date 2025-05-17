using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Topics;

/// <summary>
/// Represents a topic list model
/// </summary>
public partial record TopicListModel : BasePagedListModel<TopicModel>
{
}