using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents a news settings model
/// </summary>
public partial record NewsSettingsModel : BaseModel, ISettingsModel
{
    #region Properties

    public int ActiveSiteScopeConfiguration { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.News.Enabled")]
    public bool Enabled { get; set; }
    public bool Enabled_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.News.AllowNotRegisteredUsersToLeaveComments")]
    public bool AllowNotRegisteredUsersToLeaveComments { get; set; }
    public bool AllowNotRegisteredUsersToLeaveComments_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.News.NotifyAboutNewNewsComments")]
    public bool NotifyAboutNewNewsComments { get; set; }
    public bool NotifyAboutNewNewsComments_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.News.ShowNewsOnMainPage")]
    public bool ShowNewsOnMainPage { get; set; }
    public bool ShowNewsOnMainPage_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.News.MainPageNewsCount")]
    public int MainPageNewsCount { get; set; }
    public bool MainPageNewsCount_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.News.NewsArchivePageSize")]
    public int NewsArchivePageSize { get; set; }
    public bool NewsArchivePageSize_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.News.ShowHeaderRSSUrl")]
    public bool ShowHeaderRssUrl { get; set; }
    public bool ShowHeaderRssUrl_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.News.NewsCommentsMustBeApproved")]
    public bool NewsCommentsMustBeApproved { get; set; }
    public bool NewsCommentsMustBeApproved_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.News.ShowNewsCommentsPerSite")]
    public bool ShowNewsCommentsPerSite { get; set; }

    #endregion
}