using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents a PDF settings model
/// </summary>
public partial record PdfSettingsModel : BaseModel, ISettingsModel
{
    #region Properties

    public int ActiveSiteScopeConfiguration { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.PdfLetterPageSizeEnabled")]
    public bool LetterPageSizeEnabled { get; set; }
    public bool LetterPageSizeEnabled_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.PdfLogo")]
    [UIHint("Picture")]
    public int LogoPictureId { get; set; }
    public bool LogoPictureId_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.PdfFooterTextColumn1")]
    public string PdfFooterTextColumn1 { get; set; }
    public bool PdfFooterTextColumn1_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.PdfFooterTextColumn2")]
    public string PdfFooterTextColumn2 { get; set; }
    public bool PdfFooterTextColumn2_OverrideForSite { get; set; }

    #endregion
}