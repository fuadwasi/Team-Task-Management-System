using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents a SEO settings model
/// </summary>
public partial record SeoSettingsModel : BaseModel, ISettingsModel
{
    #region Properties

    public int ActiveSiteScopeConfiguration { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.PageTitleSeparator")]
    [NoTrim]
    public string PageTitleSeparator { get; set; }
    public bool PageTitleSeparator_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.PageTitleSeoAdjustment")]
    public int PageTitleSeoAdjustment { get; set; }
    public bool PageTitleSeoAdjustment_OverrideForSite { get; set; }
    public SelectList PageTitleSeoAdjustmentValues { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.ConvertNonWesternChars")]
    public bool ConvertNonWesternChars { get; set; }
    public bool ConvertNonWesternChars_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.CanonicalUrlsEnabled")]
    public bool CanonicalUrlsEnabled { get; set; }
    public bool CanonicalUrlsEnabled_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.WwwRequirement")]
    public int WwwRequirement { get; set; }
    public bool WwwRequirement_OverrideForSite { get; set; }
    public SelectList WwwRequirementValues { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.TwitterMetaTags")]
    public bool TwitterMetaTags { get; set; }
    public bool TwitterMetaTags_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.OpenGraphMetaTags")]
    public bool OpenGraphMetaTags { get; set; }
    public bool OpenGraphMetaTags_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.CustomHeadTags")]
    public string CustomHeadTags { get; set; }
    public bool CustomHeadTags_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.Microdata")]
    public bool MicrodataEnabled { get; set; }
    public bool MicrodataEnabled_OverrideForSite { get; set; }
    #endregion
}