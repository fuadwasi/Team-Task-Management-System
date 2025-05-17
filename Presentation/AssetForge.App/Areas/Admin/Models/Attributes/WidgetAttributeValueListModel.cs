using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Attributes
{
    /// <summary>
    /// Represents a widget attribute value list model
    /// </summary>
    public partial record WidgetAttributeValueListModel : BasePagedListModel<WidgetAttributeValueModel>
    {
    }
}