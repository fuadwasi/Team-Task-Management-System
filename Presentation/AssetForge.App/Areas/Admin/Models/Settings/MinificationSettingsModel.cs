using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents a minification settings model
/// </summary>
public partial record MinificationSettingsModel : BaseModel, ISettingsModel
{
    #region Properties

    public int ActiveSiteScopeConfiguration { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.EnableHtmlMinification")]
    public bool EnableHtmlMinification { get; set; }
    public bool EnableHtmlMinification_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.UseResponseCompression")]
    public bool UseResponseCompression { get; set; }
    public bool UseResponseCompression_OverrideForSite { get; set; }

    #endregion

}