using AssetForge.App.Areas.Admin.Factories;
using AssetForge.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace AssetForge.App.Areas.Admin.Components;

/// <summary>
/// Represents a view component that displays the Site scope configuration
/// </summary>
public partial class SiteScopeConfigurationViewComponent : AssetForgeViewComponent
{
    #region Fields

    protected readonly ISettingModelFactory _settingModelFactory;

    #endregion

    #region Ctor

    public SiteScopeConfigurationViewComponent(ISettingModelFactory settingModelFactory)
    {
        _settingModelFactory = settingModelFactory;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Invoke view component
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the view component result
    /// </returns>
    public async Task<IViewComponentResult> InvokeAsync()
    {
        //prepare model
        var model = await _settingModelFactory.PrepareSiteScopeConfigurationModelAsync();

        if (model.Sites.Count < 2)
            return Content(string.Empty);

        return View(model);
    }

    #endregion
}