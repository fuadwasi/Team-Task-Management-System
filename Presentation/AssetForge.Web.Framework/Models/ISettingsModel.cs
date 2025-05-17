namespace AssetForge.Web.Framework.Models;

/// <summary>
/// Represents a settings model
/// </summary>
public partial interface ISettingsModel
{
    /// <summary>
    /// Gets or sets an active site scope configuration (site identifier)
    /// </summary>
    int ActiveSiteScopeConfiguration { get; set; }
}