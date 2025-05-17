using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents a CAPTCHA settings model
/// </summary>
public partial record CaptchaSettingsModel : BaseModel, ISettingsModel
{
    #region Properties

    public int ActiveSiteScopeConfiguration { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.CaptchaEnabled")]
    public bool Enabled { get; set; }
    public bool Enabled_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.CaptchaShowOnLoginPage")]
    public bool ShowOnLoginPage { get; set; }
    public bool ShowOnLoginPage_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.CaptchaShowOnRegistrationPage")]
    public bool ShowOnRegistrationPage { get; set; }
    public bool ShowOnRegistrationPage_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.CaptchaShowOnContactUsPage")]
    public bool ShowOnContactUsPage { get; set; }
    public bool ShowOnContactUsPage_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.CaptchaShowOnBlogCommentPage")]
    public bool ShowOnBlogCommentPage { get; set; }
    public bool ShowOnBlogCommentPage_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.CaptchaShowOnNewsCommentPage")]
    public bool ShowOnNewsCommentPage { get; set; }
    public bool ShowOnNewsCommentPage_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.CaptchaShowOnNewsletterPage")]
    public bool ShowOnNewsletterPage { get; set; }
    public bool ShowOnNewsletterPage_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.CaptchaShowOnForgotPasswordPage")]
    public bool ShowOnForgotPasswordPage { get; set; }
    public bool ShowOnForgotPasswordPage_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.CaptchaShowOnForum")]
    public bool ShowOnForum { get; set; }
    public bool ShowOnForum_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.reCaptchaPublicKey")]
    public string ReCaptchaPublicKey { get; set; }
    public bool ReCaptchaPublicKey_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.reCaptchaPrivateKey")]
    public string ReCaptchaPrivateKey { get; set; }
    public bool ReCaptchaPrivateKey_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.CaptchaType")]
    public int CaptchaType { get; set; }
    public bool CaptchaType_OverrideForSite { get; set; }
    public SelectList CaptchaTypeValues { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.reCaptchaV3ScoreThreshold")]
    public decimal ReCaptchaV3ScoreThreshold { get; set; }
    public bool ReCaptchaV3ScoreThreshold_OverrideForSite { get; set; }

    #endregion
}