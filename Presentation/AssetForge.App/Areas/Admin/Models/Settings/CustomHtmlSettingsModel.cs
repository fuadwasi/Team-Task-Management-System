using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents an custom html settings model
/// </summary>
public partial record CustomHtmlSettingsModel : BaseModel, ISettingsModel
{
    #region Properties

    public int ActiveSiteScopeConfiguration { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.HeaderCustomHtml")]
    public string HeaderCustomHtml { get; set; }
    public bool HeaderCustomHtml_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.FooterCustomHtml")]
    public string FooterCustomHtml { get; set; }
    public bool FooterCustomHtml_OverrideForSite { get; set; }

    #endregion
}