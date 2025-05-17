using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Attributes
{
    /// <summary>
    /// Represents a widget attribute list model
    /// </summary>
    public partial record WidgetAttributeListModel : BasePagedListModel<WidgetAttributeModel>
    {
    }
}