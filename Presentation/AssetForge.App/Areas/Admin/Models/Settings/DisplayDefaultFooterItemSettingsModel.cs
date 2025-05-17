using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents a display default footer item settings model
/// </summary>
public partial record DisplayDefaultFooterItemSettingsModel : BaseModel, ISettingsModel
{
    #region Properties

    public int ActiveSiteScopeConfiguration { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplaySitemapFooterItem")]
    public bool DisplaySitemapFooterItem { get; set; }
    public bool DisplaySitemapFooterItem_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayContactUsFooterItem")]
    public bool DisplayContactUsFooterItem { get; set; }
    public bool DisplayContactUsFooterItem_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayNewsFooterItem")]
    public bool DisplayNewsFooterItem { get; set; }
    public bool DisplayNewsFooterItem_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayBlogFooterItem")]
    public bool DisplayBlogFooterItem { get; set; }
    public bool DisplayBlogFooterItem_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayForumsFooterItem")]
    public bool DisplayForumsFooterItem { get; set; }
    public bool DisplayForumsFooterItem_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayCustomerInfoFooterItem")]
    public bool DisplayCustomerInfoFooterItem { get; set; }
    public bool DisplayCustomerInfoFooterItem_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayCustomerAddressesFooterItem")]
    public bool DisplayCustomerAddressesFooterItem { get; set; }
    public bool DisplayCustomerAddressesFooterItem_OverrideForSite { get; set; }

    #endregion
}