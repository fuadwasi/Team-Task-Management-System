using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents an installation configuration model
/// </summary>
public partial record InstallationConfigModel : BaseModel, IConfigModel
{
    #region Properties

    [ResourceDisplayName("Admin.Configuration.AppSettings.Installation.DisableSampleData")]
    public bool DisableSampleData { get; set; }

    [ResourceDisplayName("Admin.Configuration.AppSettings.Installation.DisabledPlugins")]
    public string DisabledPlugins { get; set; }

    [ResourceDisplayName("Admin.Configuration.AppSettings.Installation.InstallRegionalResources")]
    public bool InstallRegionalResources { get; set; }

    #endregion
}