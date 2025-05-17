using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Cms;

public partial record WidgetZoneInstanceSearchModel : BaseSearchModel
{
    public int WidgetZoneId { get; set; }

    public int IsActive { get; set; }
}