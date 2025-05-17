using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssetForge.App.Areas.Admin.Models.Topics;

/// <summary>
/// Represents a topic search model
/// </summary>
public partial record TopicSearchModel : BaseSearchModel
{
    #region Ctor

    public TopicSearchModel()
    {
        AvailableSites = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    [ResourceDisplayName("Admin.ContentManagement.Topics.List.SearchSite")]
    public int SearchSiteId { get; set; }

    public IList<SelectListItem> AvailableSites { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Topics.List.SearchKeywords")]
    public string SearchKeywords { get; set; }

    public bool HideSitesList { get; set; }

    #endregion
}