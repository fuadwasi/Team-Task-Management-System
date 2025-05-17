using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents a site information settings model
/// </summary>
public partial record SiteInformationSettingsModel : BaseModel, ISettingsModel
{
    #region Ctor

    public SiteInformationSettingsModel()
    {
        AvailableSiteThemes = new List<ThemeModel>();
    }

    #endregion

    #region Properties

    public int ActiveSiteScopeConfiguration { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.SiteClosed")]
    public bool SiteClosed { get; set; }
    public bool SiteClosed_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DefaultSiteTheme")]
    public string DefaultSiteTheme { get; set; }
    public bool DefaultSiteTheme_OverrideForSite { get; set; }
    public IList<ThemeModel> AvailableSiteThemes { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.AllowCustomerToSelectTheme")]
    public bool AllowCustomerToSelectTheme { get; set; }
    public bool AllowCustomerToSelectTheme_OverrideForSite { get; set; }

    [UIHint("Picture")]
    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.Logo")]
    public int LogoPictureId { get; set; }
    public bool LogoPictureId_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayEuCookieLawWarning")]
    public bool DisplayEuCookieLawWarning { get; set; }
    public bool DisplayEuCookieLawWarning_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.FacebookLink")]
    public string FacebookLink { get; set; }
    public bool FacebookLink_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.TwitterLink")]
    public string TwitterLink { get; set; }
    public bool TwitterLink_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.YoutubeLink")]
    public string YoutubeLink { get; set; }
    public bool YoutubeLink_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.InstagramLink")]
    public string InstagramLink { get; set; }
    public bool InstagramLink_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.SubjectFieldOnContactUsForm")]
    public bool SubjectFieldOnContactUsForm { get; set; }
    public bool SubjectFieldOnContactUsForm_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.UseSystemEmailForContactUsForm")]
    public bool UseSystemEmailForContactUsForm { get; set; }
    public bool UseSystemEmailForContactUsForm_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.PopupForTermsOfServiceLinks")]
    public bool PopupForTermsOfServiceLinks { get; set; }
    public bool PopupForTermsOfServiceLinks_OverrideForSite { get; set; }

    #endregion

    #region Nested classes

    public partial record ThemeModel
    {
        public string SystemName { get; set; }
        public string FriendlyName { get; set; }
        public string PreviewImageUrl { get; set; }
        public string PreviewText { get; set; }
        public bool SupportRtl { get; set; }
        public bool Selected { get; set; }
    }

    #endregion
}