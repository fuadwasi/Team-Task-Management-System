using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssetForge.App.Areas.Admin.Models.Cms;

/// <summary>
/// Represents a widget search model
/// </summary>
public partial record WidgetSearchModel : BaseSearchModel
{
    public WidgetSearchModel()
    {
        AvailableActiveOptions = new List<SelectListItem>();
    }

    [ResourceDisplayName("Admin.ContentManagement.Widgets.Search.Fields.SystemName")]
    public string SystemName { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Widgets.Search.Fields.IsActive")]
    public int IsActive { get; set; }

    public IList<SelectListItem> AvailableActiveOptions { get; set; }
}