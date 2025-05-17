using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents a localization settings model
/// </summary>
public partial record LocalizationSettingsModel : BaseModel, ISettingsModel
{
    #region Properties

    public int ActiveSiteScopeConfiguration { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.UseImagesForLanguageSelection")]
    public bool UseImagesForLanguageSelection { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.SeoFriendlyUrlsForLanguagesEnabled")]
    public bool SeoFriendlyUrlsForLanguagesEnabled { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.AutomaticallyDetectLanguage")]
    public bool AutomaticallyDetectLanguage { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.LoadAllLocaleRecordsOnStartup")]
    public bool LoadAllLocaleRecordsOnStartup { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.LoadAllLocalizedPropertiesOnStartup")]
    public bool LoadAllLocalizedPropertiesOnStartup { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.LoadAllUrlRecordsOnStartup")]
    public bool LoadAllUrlRecordsOnStartup { get; set; }

    #endregion
}