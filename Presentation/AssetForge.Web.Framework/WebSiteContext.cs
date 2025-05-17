using AssetForge.Core;
using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Sites;
using AssetForge.Core.Infrastructure;
using AssetForge.Data;
using AssetForge.Services.Common;
using AssetForge.Services.Sites;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace AssetForge.Web.Framework;

/// <summary>
/// Site context for web application
/// </summary>
public partial class WebSiteContext : ISiteContext
{
    #region Fields

    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly IRepository<Site> _siteRepository;
    protected readonly ISiteService _siteService;

    protected Site _cachedSite;
    protected int? _cachedActiveSiteScopeConfiguration;

    #endregion

    #region Ctor

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="genericAttributeService">Generic attribute service</param>
    /// <param name="httpContextAccessor">HTTP context accessor</param>
    /// <param name="siteRepository">Site repository</param>
    /// <param name="siteService">Site service</param>
    public WebSiteContext(IGenericAttributeService genericAttributeService,
        IHttpContextAccessor httpContextAccessor,
        IRepository<Site> siteRepository,
        ISiteService siteService)
    {
        _genericAttributeService = genericAttributeService;
        _httpContextAccessor = httpContextAccessor;
        _siteRepository = siteRepository;
        _siteService = siteService;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the current site
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task<Site> GetCurrentSiteAsync()
    {
        if (_cachedSite != null)
            return _cachedSite;

        //try to determine the current site by HOST header
        string host = _httpContextAccessor.HttpContext?.Request.Headers[HeaderNames.Host];

        var allSites = await _siteService.GetAllSitesAsync();
        var site = allSites.FirstOrDefault(s => _siteService.ContainsHostValue(s, host)) ?? allSites.FirstOrDefault();

        _cachedSite = site ?? throw new Exception("No site could be loaded");

        return _cachedSite;
    }

    /// <summary>
    /// Gets the current site
    /// </summary>
    public virtual Site GetCurrentSite()
    {
        if (_cachedSite != null)
            return _cachedSite;

        //try to determine the current site by HOST header
        string host = _httpContextAccessor.HttpContext?.Request.Headers[HeaderNames.Host];

        //we cannot call async methods here. otherwise, an application can hang. so it's a workaround to avoid that
        var allSites = _siteRepository.GetAll(query =>
        {
            return from s in query orderby s.DisplayOrder, s.Id select s;
        }, _ => default, includeDeleted: false);

        var site = allSites.FirstOrDefault(s => _siteService.ContainsHostValue(s, host)) ?? allSites.FirstOrDefault();

        _cachedSite = site ?? throw new Exception("No site could be loaded");

        return _cachedSite;
    }

    /// <summary>
    /// Gets active site scope configuration
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task<int> GetActiveSiteScopeConfigurationAsync()
    {
        if (_cachedActiveSiteScopeConfiguration.HasValue)
            return _cachedActiveSiteScopeConfiguration.Value;

        //ensure that we have 2 (or more) sites
        if ((await _siteService.GetAllSitesAsync()).Count > 1)
        {
            //do not inject IWorkContext via constructor because it'll cause circular references
            var currentCustomer = await EngineContext.Current.Resolve<IWorkContext>().GetCurrentCustomerAsync();

            //try to get site identifier from attributes
            var siteId = await _genericAttributeService
                .GetAttributeAsync<int>(currentCustomer, CustomerDefaults.AdminAreaSiteScopeConfigurationAttribute);

            _cachedActiveSiteScopeConfiguration = (await _siteService.GetSiteByIdAsync(siteId))?.Id ?? 0;
        }
        else
            _cachedActiveSiteScopeConfiguration = 0;

        return _cachedActiveSiteScopeConfiguration ?? 0;
    }

    #endregion
}