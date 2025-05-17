using AssetForge.Core.Configuration;

namespace AssetForge.Services.Helpers;

/// <summary>
/// DateTime settings
/// </summary>
public partial class DateTimeSettings : ISettings
{
    /// <summary>
    /// Gets or sets a default site time zone identifier
    /// </summary>
    public string DefaultSiteTimeZoneId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether customers are allowed to select theirs time zone
    /// </summary>
    public bool AllowCustomersToSetTimeZone { get; set; }
}