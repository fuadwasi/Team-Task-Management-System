using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents a setting mode model
/// </summary>
public partial record SettingModeModel : BaseModel
{
    #region Properties

    public string ModeName { get; set; }

    public bool Enabled { get; set; }

    #endregion
}