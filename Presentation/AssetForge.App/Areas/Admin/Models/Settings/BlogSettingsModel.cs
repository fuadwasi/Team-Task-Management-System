using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents a blog settings model
/// </summary>
public partial record BlogSettingsModel : BaseModel, ISettingsModel
{
    #region Properties

    public int ActiveSiteScopeConfiguration { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.Blog.Enabled")]
    public bool Enabled { get; set; }
    public bool Enabled_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.Blog.PostsPageSize")]
    public int PostsPageSize { get; set; }
    public bool PostsPageSize_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.Blog.AllowNotRegisteredUsersToLeaveComments")]
    public bool AllowNotRegisteredUsersToLeaveComments { get; set; }
    public bool AllowNotRegisteredUsersToLeaveComments_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.Blog.NotifyAboutNewBlogComments")]
    public bool NotifyAboutNewBlogComments { get; set; }
    public bool NotifyAboutNewBlogComments_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.Blog.NumberOfTags")]
    public int NumberOfTags { get; set; }
    public bool NumberOfTags_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.Blog.ShowHeaderRSSUrl")]
    public bool ShowHeaderRssUrl { get; set; }
    public bool ShowHeaderRssUrl_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.Blog.BlogCommentsMustBeApproved")]
    public bool BlogCommentsMustBeApproved { get; set; }
    public bool BlogCommentsMustBeApproved_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.Blog.ShowBlogCommentsPerSite")]
    public bool ShowBlogCommentsPerSite { get; set; }

    #endregion
}