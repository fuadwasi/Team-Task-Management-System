using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AssetForge.Web.Framework.TagHelpers.Admin;

/// <summary>
/// "ix-cards" tag helper
/// </summary>
[HtmlTargetElement("ix-cards", Attributes = ID_ATTRIBUTE_NAME)]
public partial class AssetForgeCardsTagHelper : TagHelper
{
    #region Constants

    protected const string ID_ATTRIBUTE_NAME = "id";

    #endregion

    #region Properties

    /// <summary>
    /// ViewContext
    /// </summary>
    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; }

    #endregion
}