using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents a setting search model
/// </summary>
public partial record SettingSearchModel : BaseSearchModel
{
    #region Ctor

    public SettingSearchModel()
    {
        AddSetting = new SettingModel();
    }

    #endregion

    #region Properties

    [ResourceDisplayName("Admin.Configuration.Settings.AllSettings.SearchSettingName")]
    public string SearchSettingName { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.AllSettings.SearchSettingValue")]
    public string SearchSettingValue { get; set; }

    public SettingModel AddSetting { get; set; }

    #endregion
}