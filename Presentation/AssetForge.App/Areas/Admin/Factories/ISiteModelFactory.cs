using AssetForge.App.Areas.Admin.Models.Sites;
using AssetForge.Core.Domain.Sites;

namespace AssetForge.App.Areas.Admin.Factories;

/// <summary>
/// Represents the site model factory
/// </summary>
public partial interface ISiteModelFactory
{
    /// <summary>
    /// Prepare site search model
    /// </summary>
    /// <param name="searchModel">Site search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the site search model
    /// </returns>
    Task<SiteSearchModel> PrepareSiteSearchModelAsync(SiteSearchModel searchModel);

    /// <summary>
    /// Prepare paged site list model
    /// </summary>
    /// <param name="searchModel">Site search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the site list model
    /// </returns>
    Task<SiteListModel> PrepareSiteListModelAsync(SiteSearchModel searchModel);

    /// <summary>
    /// Prepare site model
    /// </summary>
    /// <param name="model">Site model</param>
    /// <param name="site">Site</param>
    /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the site model
    /// </returns>
    Task<SiteModel> PrepareSiteModelAsync(SiteModel model, Site site, bool excludeProperties = false);
}