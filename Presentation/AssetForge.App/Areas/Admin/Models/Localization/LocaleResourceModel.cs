using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Localization;

/// <summary>
/// Represents a locale resource model
/// </summary>
public partial record LocaleResourceModel : BaseEntityModel
{
    #region Properties

    [ResourceDisplayName("Admin.Configuration.Languages.Resources.Fields.Name")]
    public string ResourceName { get; set; }

    [ResourceDisplayName("Admin.Configuration.Languages.Resources.Fields.Value")]
    public string ResourceValue { get; set; }

    public int LanguageId { get; set; }

    #endregion
}