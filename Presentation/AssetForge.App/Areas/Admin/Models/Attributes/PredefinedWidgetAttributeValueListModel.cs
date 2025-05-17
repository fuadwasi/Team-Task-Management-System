using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Attributes
{
    /// <summary>
    /// Represents a predefined widget attribute value list model
    /// </summary>
    public partial record PredefinedWidgetAttributeValueListModel : BasePagedListModel<PredefinedWidgetAttributeValueModel>
    {
    }
}