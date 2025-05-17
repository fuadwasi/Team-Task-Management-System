using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssetForge.App.Areas.Admin.Models.Common;

/// <summary>
/// Represents an URL record search model
/// </summary>
public partial record UrlRecordSearchModel : BaseSearchModel
{
    #region Ctor

    public UrlRecordSearchModel()
    {
        AvailableLanguages = new List<SelectListItem>();
        AvailableActiveOptions = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    [ResourceDisplayName("Admin.System.SeNames.List.Name")]
    public string SeName { get; set; }

    [ResourceDisplayName("Admin.System.SeNames.List.Language")]
    public int LanguageId { get; set; }

    [ResourceDisplayName("Admin.System.SeNames.List.IsActive")]
    public int IsActiveId { get; set; }

    public IList<SelectListItem> AvailableLanguages { get; set; }

    public IList<SelectListItem> AvailableActiveOptions { get; set; }

    #endregion
}