using AssetForge.Core.Domain.Localization;

namespace AssetForge.Core.Domain.Attributes;

/// <summary>
/// Represents a predefined (default) widget attribute value
/// </summary>
public partial class PredefinedWidgetAttributeValue : BaseEntity, ILocalizedEntity
{
    /// <summary>
    /// Gets or sets the widget attribute identifier
    /// </summary>
    public int WidgetAttributeId { get; set; }

    /// <summary>
    /// Gets or sets the widget attribute name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the value is pre-selected
    /// </summary>
    public bool IsPreSelected { get; set; }

    /// <summary>
    /// Gets or sets the display order
    /// </summary>
    public int DisplayOrder { get; set; }
}