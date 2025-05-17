namespace AssetForge.Core.Domain.Sites;

/// <summary>
/// Represents a Site mapping record
/// </summary>
public partial class SiteMapping : BaseEntity
{
    /// <summary>
    /// Gets or sets the entity identifier
    /// </summary>
    public int EntityId { get; set; }

    /// <summary>
    /// Gets or sets the entity name
    /// </summary>
    public string EntityName { get; set; }

    /// <summary>
    /// Gets or sets the Site identifier
    /// </summary>
    public int SiteId { get; set; }
}