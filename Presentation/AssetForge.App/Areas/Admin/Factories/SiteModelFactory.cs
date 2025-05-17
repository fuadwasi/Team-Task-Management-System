using AssetForge.App.Areas.Admin.Infrastructure.Mapper.Extensions;
using AssetForge.App.Areas.Admin.Models.Sites;
using AssetForge.Core.Domain.Sites;
using AssetForge.Services.Localization;
using AssetForge.Services.Sites;
using AssetForge.Web.Framework.Models.Extensions;

namespace AssetForge.App.Areas.Admin.Factories;

/// <summary>
/// Represents the site model factory implementation
/// </summary>
public partial class SiteModelFactory : ISiteModelFactory
{
    #region Fields

    protected readonly IBaseAdminModelFactory _baseAdminModelFactory;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedModelFactory _localizedModelFactory;
    protected readonly ISiteService _siteService;

    #endregion

    #region Ctor

    public SiteModelFactory(IBaseAdminModelFactory baseAdminModelFactory,
        ILocalizationService localizationService,
        ILocalizedModelFactory localizedModelFactory,
        ISiteService siteService)
    {
        _baseAdminModelFactory = baseAdminModelFactory;
        _localizationService = localizationService;
        _localizedModelFactory = localizedModelFactory;
        _siteService = siteService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Prepare site search model
    /// </summary>
    /// <param name="searchModel">Site search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the site search model
    /// </returns>
    public virtual Task<SiteSearchModel> PrepareSiteSearchModelAsync(SiteSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //prepare page parameters
        searchModel.SetGridPageSize();

        return Task.FromResult(searchModel);
    }

    /// <summary>
    /// Prepare paged site list model
    /// </summary>
    /// <param name="searchModel">Site search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the site list model
    /// </returns>
    public virtual async Task<SiteListModel> PrepareSiteListModelAsync(SiteSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //get sites
        var sites = (await _siteService.GetAllSitesAsync()).ToPagedList(searchModel);

        //prepare list model
        var model = new SiteListModel().PrepareToGrid(searchModel, sites, () =>
        {
            //fill in model values from the entity
            return sites.Select(site => site.ToModel<SiteModel>());
        });

        return model;
    }

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
    public virtual async Task<SiteModel> PrepareSiteModelAsync(SiteModel model, Site site, bool excludeProperties = false)
    {
        Func<SiteLocalizedModel, int, Task> localizedModelConfiguration = null;

        if (site != null)
        {
            //fill in model values from the entity
            model ??= site.ToModel<SiteModel>();

            //define localized model configuration action
            localizedModelConfiguration = async (locale, languageId) =>
            {
                locale.Name = await _localizationService.GetLocalizedAsync(site, entity => entity.Name, languageId, false, false);
                locale.DefaultTitle = await _localizationService.GetLocalizedAsync(site, entity => entity.DefaultTitle, languageId, false, false);
                locale.DefaultMetaDescription = await _localizationService.GetLocalizedAsync(site, entity => entity.DefaultMetaDescription, languageId, false, false);
                locale.DefaultMetaKeywords = await _localizationService.GetLocalizedAsync(site, entity => entity.DefaultMetaKeywords, languageId, false, false);
                locale.HomepageDescription = await _localizationService.GetLocalizedAsync(site, entity => entity.HomepageDescription, languageId, false, false);
                locale.HomepageTitle = await _localizationService.GetLocalizedAsync(site, entity => entity.HomepageTitle, languageId, false, false);
            };
        }

        //prepare available languages
        await _baseAdminModelFactory.PrepareLanguagesAsync(model.AvailableLanguages,
            defaultItemText: await _localizationService.GetResourceAsync("Admin.Common.EmptyItemText"));

        //prepare localized models
        if (!excludeProperties)
            model.Locales = await _localizedModelFactory.PrepareLocalizedModelsAsync(localizedModelConfiguration);

        return model;
    }

    #endregion
}