using AssetForge.Core.Configuration;

namespace AssetForge.Core.Domain;

/// <summary>
/// Site information settings
/// </summary>
public partial class SiteInformationSettings : ISettings
{
    /// <summary>
    /// Gets or sets a value indicating whether "powered by AssetForgeCms" text should be displayed.
    /// </summary>
    public bool HidePoweredByAssetForgeCms { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether Site is closed
    /// </summary>
    public bool SiteClosed { get; set; }

    /// <summary>
    /// Gets or sets a picture identifier of the logo. If 0, then the default one will be used
    /// </summary>
    public int LogoPictureId { get; set; }

    /// <summary>
    /// Gets or sets a default Site theme
    /// </summary>
    public string DefaultSiteTheme { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether customers are allowed to select a theme
    /// </summary>
    public bool AllowCustomerToSelectTheme { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether we should display warnings about the new EU cookie law
    /// </summary>
    public bool DisplayEuCookieLawWarning { get; set; }

    /// <summary>
    /// Gets or sets a value of Facebook page URL of the site
    /// </summary>
    public string FacebookLink { get; set; }

    /// <summary>
    /// Gets or sets a value of Twitter page URL of the site
    /// </summary>
    public string TwitterLink { get; set; }

    /// <summary>
    /// Gets or sets a value of YouTube channel URL of the site
    /// </summary>
    public string YoutubeLink { get; set; }

    /// <summary>
    /// Gets or sets a value of Instagram account URL of the site
    /// </summary>
    public string InstagramLink { get; set; }
}