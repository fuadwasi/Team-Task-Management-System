using AssetForge.Core.Infrastructure;
using AssetForge.Services.Localization;
using System.ComponentModel;

namespace AssetForge.Web.Framework.Mvc.ModelBinding;

/// <summary>
/// Represents model property attribute that specifies the display name by passed key of the locale resource
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class ResourceDisplayNameAttribute : DisplayNameAttribute, IModelAttribute
{
    #region Fields

    private string _resourceValue = string.Empty;

    #endregion

    #region Ctor

    /// <summary>
    /// Create instance of the attribute
    /// </summary>
    /// <param name="resourceKey">Key of the locale resource</param>
    public ResourceDisplayNameAttribute(string resourceKey) : base(resourceKey)
    {
        ResourceKey = resourceKey;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets key of the locale resource 
    /// </summary>
    public string ResourceKey { get; set; }

    /// <summary>
    /// Gets the display name
    /// </summary>
    public override string DisplayName
    {
        get
        {
            //get locale resource value
            _resourceValue = EngineContext.Current.Resolve<ILocalizationService>().GetResourceAsync(ResourceKey).Result;

            return _resourceValue;
        }
    }

    /// <summary>
    /// Gets name of the attribute
    /// </summary>
    public string Name => nameof(ResourceDisplayNameAttribute);

    #endregion
}