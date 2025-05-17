using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Common;

/// <summary>
/// Represents a popular search term model
/// </summary>
public partial record PopularSearchTermModel : BaseModel
{
    #region Properties

    [ResourceDisplayName("Admin.SearchTermReport.Keyword")]
    public string Keyword { get; set; }

    [ResourceDisplayName("Admin.SearchTermReport.Count")]
    public int Count { get; set; }

    #endregion
}