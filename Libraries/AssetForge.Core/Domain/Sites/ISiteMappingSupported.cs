namespace AssetForge.Core.Domain.Sites;

/// <summary>
/// Represents an entity which supports Site mapping
/// </summary>
public partial interface ISiteMappingSupported
{
    /// <summary>
    /// Gets or sets a value indicating whether the entity is limited/restricted to certain Sites
    /// </summary>
    bool LimitedToSites { get; set; }
}