using AssetForge.Web.Framework.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AssetForge.Web.Framework.TagHelpers;

public static class TagHelperExtensions
{
    #region Methods

    public static async Task<string> GetAttributeValueAsync(this TagHelperOutput output, string attributeName)
    {

        if (string.IsNullOrEmpty(attributeName) || !output.Attributes.TryGetAttribute(attributeName, out var attr))
            return null;

        if (attr.Value is string stringValue)
            return stringValue;

        return attr.Value switch
        {
            HtmlString htmlString => htmlString.ToString(),
            IHtmlContent content => await content.RenderHtmlContentAsync(),
            _ => default
        };
    }
    public static async Task<IDictionary<string, string>> GetAttributeDictionaryAsync(this TagHelperOutput output)
    {
        ArgumentNullException.ThrowIfNull(output);

        var result = new Dictionary<string, string>();

        if (!output.Attributes.Any())
            return result;

        foreach (var attrName in output.Attributes.Select(x => x.Name).Distinct())
        {
            result.Add(attrName, await output.GetAttributeValueAsync(attrName));
        }

        return result;
    }

    #endregion
}