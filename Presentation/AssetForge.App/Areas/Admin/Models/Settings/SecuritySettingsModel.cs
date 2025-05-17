using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents a security settings model
/// </summary>
public partial record SecuritySettingsModel : BaseModel, ISettingsModel
{
    #region Properties

    public int ActiveSiteScopeConfiguration { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.EncryptionKey")]
    public string EncryptionKey { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.AdminAreaAllowedIpAddresses")]
    public string AdminAreaAllowedIpAddresses { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.HoneypotEnabled")]
    public bool HoneypotEnabled { get; set; }

    #endregion
}