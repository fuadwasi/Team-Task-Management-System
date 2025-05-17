using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using AssetForge.Core.Domain.Localization;

namespace AssetForge.Core.Domain.Attributes;

/// <summary>
/// Represents a widget attribute value
/// </summary>
public partial class WidgetAttributeValue : BaseEntity, ILocalizedEntity
{
    /// <summary>
    /// Gets or sets the widget attribute mapping identifier
    /// </summary>
    public int WidgetAttributeMappingId { get; set; }

    /// <summary>
    /// Gets or sets the attribute value type identifier
    /// </summary>
    public int AttributeValueTypeId { get; set; }

    /// <summary>
    /// Gets or sets the associated widget identifier (used only with AttributeValueType.AssociatedToWidget)
    /// </summary>
    public int AssociatedWidgetZoneInstanceId { get; set; }

    /// <summary>
    /// Gets or sets the widget attribute name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the color RGB value (used with "Color squares" attribute type)
    /// </summary>
    public string ColorSquaresRgb { get; set; }

    /// <summary>
    /// Gets or sets the picture ID for image square (used with "Image squares" attribute type)
    /// </summary>
    public int ImageSquaresPictureId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the value is pre-selected
    /// </summary>
    public bool IsPreSelected { get; set; }

    /// <summary>
    /// Gets or sets the display order
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Gets or sets the attribute value type
    /// </summary>
    public AttributeValueType AttributeValueType
    {
        get => (AttributeValueType)AttributeValueTypeId;
        set => AttributeValueTypeId = (int)value;
    }

    /// <summary>
    /// The field is not used since 4.70 and is left only for the update process
    /// use the <see cref="WidgetAttributeValuePicture"/> instead
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    [Obsolete("The field is not used since 4.70 and is left only for the update process use the WidgetAttributeValuePicture instead")]
    public int? PictureId { get; set; }
}