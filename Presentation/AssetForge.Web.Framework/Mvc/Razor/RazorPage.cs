using AssetForge.Core.Infrastructure;
using AssetForge.Services.Localization;
using AssetForge.Web.Framework.Localization;

namespace AssetForge.Web.Framework.Mvc.Razor;

/// <summary>
/// Web view page
/// </summary>
/// <typeparam name="TModel">Model</typeparam>
public abstract partial class RazorPage<TModel> : Microsoft.AspNetCore.Mvc.Razor.RazorPage<TModel>
{
    protected ILocalizationService _localizationService;
    protected Localizer _localizer;

    /// <summary>
    /// Get a localized resources
    /// </summary>
    public Localizer T
    {
        get
        {
            _localizationService ??= EngineContext.Current.Resolve<ILocalizationService>();

            _localizer ??= (format, args) =>
            {
                var resFormat = _localizationService.GetResourceAsync(format).Result;
                if (string.IsNullOrEmpty(resFormat))
                {
                    return new LocalizedString(format);
                }
                return new LocalizedString((args == null || args.Length == 0)
                    ? resFormat
                    : string.Format(resFormat, args));
            };
            return _localizer;
        }
    }
}

/// <summary>
/// Web view page
/// </summary>
/// TODO: Doesn't seem to be used anywhere
public abstract partial class AssetForgeRazorPage : RazorPage<dynamic>
{
}