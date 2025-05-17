using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Attributes
{
    /// <summary>
    /// Represents a list model of widgets that use the widget attribute
    /// </summary>
    public partial record WidgetAttributeWidgetListModel : BasePagedListModel<WidgetAttributeWidgetModel>
    {
    }
}