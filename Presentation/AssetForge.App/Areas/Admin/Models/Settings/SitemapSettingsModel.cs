using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents a Sitemap settings model
/// </summary>
public partial record SitemapSettingsModel : BaseModel, ISettingsModel
{
    public int ActiveSiteScopeConfiguration { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.SitemapEnabled")]
    public bool SitemapEnabled { get; set; }
    public bool SitemapEnabled_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.SitemapIncludeBlogPosts")]
    public bool SitemapIncludeBlogPosts { get; set; }
    public bool SitemapIncludeBlogPosts_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.SitemapIncludeNews")]
    public bool SitemapIncludeNews { get; set; }
    public bool SitemapIncludeNews_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.SitemapIncludePages")]
    public bool SitemapIncludePages { get; set; }
    public bool SitemapIncludePages_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.SitemapPageSize")]
    public int SitemapPageSize { get; set; }
    public bool SitemapPageSize_OverrideForSite { get; set; }
}