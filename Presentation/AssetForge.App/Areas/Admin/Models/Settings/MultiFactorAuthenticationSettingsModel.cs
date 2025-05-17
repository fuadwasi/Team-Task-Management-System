using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents a multi-factor authentication settings model
/// </summary>
public partial record MultiFactorAuthenticationSettingsModel : BaseModel, ISettingsModel
{
    #region Properties

    public int ActiveSiteScopeConfiguration { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.CustomerUser.ForceMultifactorAuthentication")]
    public bool ForceMultifactorAuthentication { get; set; }

    #endregion
}