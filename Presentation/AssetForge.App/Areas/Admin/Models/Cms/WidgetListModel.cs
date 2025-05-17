using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Cms;

/// <summary>
/// Represents a widget list model
/// </summary>
public partial record WidgetListModel : BasePagedListModel<WidgetModel>
{
}