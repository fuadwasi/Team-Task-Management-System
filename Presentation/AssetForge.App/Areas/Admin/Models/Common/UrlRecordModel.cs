using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Common;

/// <summary>
/// Represents an URL record model
/// </summary>
public partial record UrlRecordModel : BaseEntityModel
{
    #region Properties

    [ResourceDisplayName("Admin.System.SeNames.Name")]
    public string Name { get; set; }

    [ResourceDisplayName("Admin.System.SeNames.EntityId")]
    public int EntityId { get; set; }

    [ResourceDisplayName("Admin.System.SeNames.EntityName")]
    public string EntityName { get; set; }

    [ResourceDisplayName("Admin.System.SeNames.IsActive")]
    public bool IsActive { get; set; }

    [ResourceDisplayName("Admin.System.SeNames.Language")]
    public string Language { get; set; }

    [ResourceDisplayName("Admin.System.SeNames.Details")]
    public string DetailsUrl { get; set; }

    #endregion
}