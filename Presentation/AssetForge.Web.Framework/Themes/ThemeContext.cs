using AssetForge.Core;
using AssetForge.Core.Domain;
using AssetForge.Core.Domain.Customers;
using AssetForge.Services.Common;
using AssetForge.Services.Themes;

namespace AssetForge.Web.Framework.Themes;

/// <summary>
/// Represents the theme context implementation
/// </summary>
public partial class ThemeContext : IThemeContext
{
    #region Fields

    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly ISiteContext _siteContext;
    protected readonly IThemeProvider _themeProvider;
    protected readonly IWorkContext _workContext;
    protected readonly SiteInformationSettings _siteInformationSettings;

    protected string _cachedThemeName;

    #endregion

    #region Ctor

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="genericAttributeService">Generic attribute service</param>
    /// <param name="siteContext">Site context</param>
    /// <param name="themeProvider">Theme provider</param>
    /// <param name="workContext">Work context</param>
    /// <param name="siteInformationSettings">Site information settings</param>
    public ThemeContext(IGenericAttributeService genericAttributeService,
        ISiteContext siteContext,
        IThemeProvider themeProvider,
        IWorkContext workContext,
        SiteInformationSettings siteInformationSettings)
    {
        _genericAttributeService = genericAttributeService;
        _siteContext = siteContext;
        _themeProvider = themeProvider;
        _workContext = workContext;
        _siteInformationSettings = siteInformationSettings;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Get or set current theme system name
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task<string> GetWorkingThemeNameAsync()
    {
        if (!string.IsNullOrEmpty(_cachedThemeName))
            return _cachedThemeName;

        var themeName = string.Empty;

        //whether customers are allowed to select a theme
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (_siteInformationSettings.AllowCustomerToSelectTheme &&
            customer != null)
        {
            var site = await _siteContext.GetCurrentSiteAsync();
            themeName = await _genericAttributeService.GetAttributeAsync<string>(customer,
                CustomerDefaults.WorkingThemeNameAttribute, site.Id);
        }

        //if not, try to get default site theme
        if (string.IsNullOrEmpty(themeName))
            themeName = _siteInformationSettings.DefaultSiteTheme;

        //ensure that this theme exists
        if (!await _themeProvider.ThemeExistsAsync(themeName))
        {
            //if it does not exist, try to get the first one
            themeName = (await _themeProvider.GetThemesAsync()).FirstOrDefault()?.SystemName
                        ?? throw new Exception("No theme could be loaded");
        }

        //cache theme system name
        _cachedThemeName = themeName;

        return themeName;
    }

    /// <summary>
    /// Set current theme system name
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task SetWorkingThemeNameAsync(string workingThemeName)
    {
        //whether customers are allowed to select a theme
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (!_siteInformationSettings.AllowCustomerToSelectTheme ||
            customer == null)
            return;

        //save selected by customer theme system name
        var site = await _siteContext.GetCurrentSiteAsync();
        await _genericAttributeService.SaveAttributeAsync(customer,
            CustomerDefaults.WorkingThemeNameAttribute, workingThemeName,
            site.Id);

        //clear cache
        _cachedThemeName = null;
    }

    #endregion
}