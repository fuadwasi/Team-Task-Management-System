using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents a display default menu item settings model
/// </summary>
public partial record DisplayDefaultMenuItemSettingsModel : BaseModel, ISettingsModel
{
    #region Properties

    public int ActiveSiteScopeConfiguration { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultMenuItemSettings.DisplayHomepageMenuItem")]
    public bool DisplayHomepageMenuItem { get; set; }
    public bool DisplayHomepageMenuItem_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultMenuItemSettings.DisplayCustomerInfoMenuItem")]
    public bool DisplayCustomerInfoMenuItem { get; set; }
    public bool DisplayCustomerInfoMenuItem_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultMenuItemSettings.DisplayBlogMenuItem")]
    public bool DisplayBlogMenuItem { get; set; }
    public bool DisplayBlogMenuItem_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultMenuItemSettings.DisplayForumsMenuItem")]
    public bool DisplayForumsMenuItem { get; set; }
    public bool DisplayForumsMenuItem_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultMenuItemSettings.DisplayContactUsMenuItem")]
    public bool DisplayContactUsMenuItem { get; set; }
    public bool DisplayContactUsMenuItem_OverrideForSite { get; set; }

    #endregion
}