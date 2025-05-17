using AssetForge.Core;
using AssetForge.Core.Domain.Sites;
using AssetForge.Services.Sites;
using AssetForge.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssetForge.Web.Framework.Factories;

/// <summary>
/// Represents the base site mapping supported model factory implementation
/// </summary>
public partial class SiteMappingSupportedModelFactory : ISiteMappingSupportedModelFactory
{
    #region Fields

    protected readonly ISiteMappingService _siteMappingService;
    protected readonly ISiteService _siteService;

    #endregion

    #region Ctor

    public SiteMappingSupportedModelFactory(ISiteMappingService siteMappingService,
        ISiteService siteService)
    {
        _siteMappingService = siteMappingService;
        _siteService = siteService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Prepare selected and all available sites for the passed model
    /// </summary>
    /// <typeparam name="TModel">Site mapping supported model type</typeparam>
    /// <param name="model">Model</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task PrepareModelSitesAsync<TModel>(TModel model) where TModel : ISiteMappingSupportedModel
    {
        ArgumentNullException.ThrowIfNull(model);

        //prepare available sites
        var availableSites = await _siteService.GetAllSitesAsync();
        model.AvailableSites = availableSites.Select(site => new SelectListItem
        {
            Text = site.Name,
            Value = site.Id.ToString(),
            Selected = model.SelectedSiteIds.Contains(site.Id)
        }).ToList();
    }

    /// <summary>
    /// Prepare selected and all available sites for the passed model by site mappings
    /// </summary>
    /// <typeparam name="TModel">Site mapping supported model type</typeparam>
    /// <typeparam name="TEntity">Site mapping supported entity type</typeparam>
    /// <param name="model">Model</param>
    /// <param name="entity">Entity</param>
    /// <param name="ignoreSiteMappings">Whether to ignore existing site mappings</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task PrepareModelSitesAsync<TModel, TEntity>(TModel model, TEntity entity, bool ignoreSiteMappings)
        where TModel : ISiteMappingSupportedModel where TEntity : BaseEntity, ISiteMappingSupported
    {
        ArgumentNullException.ThrowIfNull(model);

        //prepare sites with granted access
        if (!ignoreSiteMappings && entity != null)
            model.SelectedSiteIds = (await _siteMappingService.GetSitesIdsWithAccessAsync(entity)).ToList();

        await PrepareModelSitesAsync(model);
    }

    #endregion
}