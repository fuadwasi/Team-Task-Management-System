using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents an admin area settings model
/// </summary>
public partial record AdminAreaSettingsModel : BaseModel, ISettingsModel
{
    #region Properties

    public int ActiveSiteScopeConfiguration { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.AdminArea.UseRichEditorInMessageTemplates")]
    public bool UseRichEditorInMessageTemplates { get; set; }
    public bool UseRichEditorInMessageTemplates_OverrideForSite { get; set; }

    #endregion
}