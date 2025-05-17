using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssetForge.App.Areas.Admin.Models.Messages;

/// <summary>
/// Represents a message template search model
/// </summary>
public partial record MessageTemplateSearchModel : BaseSearchModel
{
    #region Ctor

    public MessageTemplateSearchModel()
    {
        AvailableSites = new List<SelectListItem>();
        AvailableActiveOptions = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    [ResourceDisplayName("Admin.ContentManagement.MessageTemplates.List.SearchKeywords")]
    public string SearchKeywords { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.MessageTemplates.List.SearchSite")]
    public int SearchSiteId { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.MessageTemplates.List.IsActive")]
    public int IsActiveId { get; set; }

    public IList<SelectListItem> AvailableSites { get; set; }

    public IList<SelectListItem> AvailableActiveOptions { get; set; }

    public bool HideSitesList { get; set; }

    #endregion
}