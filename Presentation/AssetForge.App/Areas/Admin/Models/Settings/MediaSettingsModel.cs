using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents a media settings model
/// </summary>
public partial record MediaSettingsModel : BaseModel, ISettingsModel
{
    #region Properties

    public int ActiveSiteScopeConfiguration { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.Media.PicturesSitedIntoDatabase")]
    public bool PicturesSitedIntoDatabase { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.Media.AvatarPictureSize")]
    public int AvatarPictureSize { get; set; }
    public bool AvatarPictureSize_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.Media.MaximumImageSize")]
    public int MaximumImageSize { get; set; }
    public bool MaximumImageSize_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.Media.CatelogThumbPictureSize")]
    public int CatelogThumbPictureSize { get; set; }
    public bool CatelogThumbPictureSize_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.Media.MultipleThumbDirectories")]
    public bool MultipleThumbDirectories { get; set; }
    public bool MultipleThumbDirectories_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.Media.DefaultImageQuality")]
    public int DefaultImageQuality { get; set; }
    public bool DefaultImageQuality_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.Media.DefaultPictureZoomEnabled")]
    public bool DefaultPictureZoomEnabled { get; set; }
    public bool DefaultPictureZoomEnabled_OverrideForSite { get; set; }

    [ResourceDisplayName("Admin.Configuration.Settings.Media.AllowSVGUploads")]
    public bool AllowSVGUploads { get; set; }
    public bool AllowSVGUploads_OverrideForSite { get; set; }

    #endregion
}