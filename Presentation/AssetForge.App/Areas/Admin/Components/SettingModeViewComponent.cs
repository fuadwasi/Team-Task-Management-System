using AssetForge.App.Areas.Admin.Factories;
using AssetForge.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace AssetForge.App.Areas.Admin.Components;

/// <summary>
/// Represents a view component that displays the setting mode
/// </summary>
public partial class SettingModeViewComponent : AssetForgeViewComponent
{
    #region Fields

    protected readonly ISettingModelFactory _settingModelFactory;

    #endregion

    #region Ctor

    public SettingModeViewComponent(ISettingModelFactory settingModelFactory)
    {
        _settingModelFactory = settingModelFactory;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Invoke view component
    /// </summary>
    /// <param name="modeName">Setting mode name</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the view component result
    /// </returns>
    public async Task<IViewComponentResult> InvokeAsync(string modeName = "settings-advanced-mode")
    {
        //prepare model
        var model = await _settingModelFactory.PrepareSettingModeModelAsync(modeName);

        return View(model);
    }

    #endregion
}