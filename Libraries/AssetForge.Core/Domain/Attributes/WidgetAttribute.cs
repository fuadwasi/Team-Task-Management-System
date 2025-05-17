using AssetForge.Core.Domain.Localization;

namespace AssetForge.Core.Domain.Attributes;

/// <summary>
/// Represents a widget attribute
/// </summary>
public partial class WidgetAttribute : BaseEntity, ILocalizedEntity
{
    /// <summary>
    /// Gets or sets the name
    /// </summary>
    public string Name { get; set; }


    public string SystemName { get; set; }

    /// <summary>
    /// Gets or sets the description
    /// </summary>
    public string Description { get; set; }
}