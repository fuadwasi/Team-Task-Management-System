using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Attributes
{
    /// <summary>
    /// Represents a widget attribute mapping list model
    /// </summary>
    public partial record WidgetAttributeMappingListModel : BasePagedListModel<WidgetAttributeMappingModel>
    {
    }
}