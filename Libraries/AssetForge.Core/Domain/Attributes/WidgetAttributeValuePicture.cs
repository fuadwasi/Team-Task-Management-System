using AssetForge.Core.Domain.Localization;

namespace AssetForge.Core.Domain.Attributes;

/// <summary>
/// Represents a widget attribute value picture
/// </summary>
public partial class WidgetAttributeValuePicture : BaseEntity, ILocalizedEntity
{
    /// <summary>
    /// Gets or sets the widget attribute value id
    /// </summary>
    public int WidgetAttributeValueId { get; set; }

    /// <summary>
    /// Gets or sets the picture (identifier) associated with this value. This picture should replace a widget main picture once clicked (selected).
    /// </summary>
    public int PictureId { get; set; }
}