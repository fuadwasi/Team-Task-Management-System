using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssetForge.App.Areas.Admin.Models.Cms;

public partial record LayoutSearchModel : BaseSearchModel
{
    public LayoutSearchModel()
    {
        AvailableActiveOptions = new List<SelectListItem>();
    }

    [ResourceDisplayName("Admin.ContentManagement.Layouts.Search.Fields.SearchName")]
    public string SearchName { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Layouts.Search.Fields.IsActive")]
    public int IsActive { get; set; }

    public IList<SelectListItem> AvailableActiveOptions { get; set; }
}