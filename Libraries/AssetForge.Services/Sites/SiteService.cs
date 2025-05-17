using AssetForge.Core.Domain.Sites;
using AssetForge.Data;

namespace AssetForge.Services.Sites
{
    /// <summary>
    /// Site service
    /// </summary>
    public partial class SiteService : ISiteService
    {
        #region Fields

        private readonly IRepository<Site> _siteRepository;

        #endregion

        #region Ctor

        public SiteService(IRepository<Site> siteRepository)
        {
            _siteRepository = siteRepository;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Parse comma-separated Hosts
        /// </summary>
        /// <param name="site">Site</param>
        /// <returns>Comma-separated hosts</returns>
        protected virtual string[] ParseHostValues(Site site)
        {
            if (site == null)
                throw new ArgumentNullException(nameof(site));

            var parsedValues = new List<string>();
            if (string.IsNullOrEmpty(site.Hosts))
                return parsedValues.ToArray();

            var hosts = site.Hosts.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var host in hosts)
            {
                var tmp = host.Trim();
                if (!string.IsNullOrEmpty(tmp))
                    parsedValues.Add(tmp);
            }

            return parsedValues.ToArray();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a site
        /// </summary>
        /// <param name="site">Site</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteSiteAsync(Site site)
        {
            if (site == null)
                throw new ArgumentNullException(nameof(site));

            var allSites = await GetAllSitesAsync();
            if (allSites.Count == 1)
                throw new Exception("You cannot delete the only configured site");

            await _siteRepository.DeleteAsync(site);
        }

        /// <summary>
        /// Gets all sites
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the sites
        /// </returns>
        public virtual async Task<IList<Site>> GetAllSitesAsync()
        {
            return await _siteRepository.GetAllAsync(query =>
            {
                return from s in query orderby s.DisplayOrder, s.Id select s;
            }, _ => default, includeDeleted: false);
        }

        /// <summary>
        /// Gets all sites
        /// </summary>
        /// <returns>
        /// The sites
        /// </returns>
        public virtual IList<Site> GetAllSites()
        {
            return _siteRepository.GetAll(query =>
            {
                return from s in query orderby s.DisplayOrder, s.Id select s;
            }, _ => default, includeDeleted: false);
        }

        /// <summary>
        /// Gets a site 
        /// </summary>
        /// <param name="siteId">Site identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the site
        /// </returns>
        public virtual async Task<Site> GetSiteByIdAsync(int siteId)
        {
            return await _siteRepository.GetByIdAsync(siteId, cache => default, false);
        }

        /// <summary>
        /// Inserts a site
        /// </summary>
        /// <param name="site">Site</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertSiteAsync(Site site)
        {
            await _siteRepository.InsertAsync(site);
        }

        /// <summary>
        /// Updates the site
        /// </summary>
        /// <param name="site">Site</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateSiteAsync(Site site)
        {
            await _siteRepository.UpdateAsync(site);
        }

        /// <summary>
        /// Updates the site
        /// </summary>
        /// <param name="site">Site</param>
        public virtual void UpdateSite(Site site)
        {
            _siteRepository.Update(site);
        }

        /// <summary>
        /// Indicates whether a site contains a specified host
        /// </summary>
        /// <param name="site">Site</param>
        /// <param name="host">Host</param>
        /// <returns>true - contains, false - no</returns>
        public virtual bool ContainsHostValue(Site site, string host)
        {
            if (site == null)
                throw new ArgumentNullException(nameof(site));

            if (string.IsNullOrEmpty(host))
                return false;

            var contains = ParseHostValues(site).Any(x => x.Equals(host, StringComparison.InvariantCultureIgnoreCase));

            return contains;
        }

        /// <summary>
        /// Returns a list of names of not existing sites
        /// </summary>
        /// <param name="siteIdsNames">The names and/or IDs of the site to check</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the list of names and/or IDs not existing sites
        /// </returns>
        public async Task<string[]> GetNotExistingSitesAsync(string[] siteIdsNames)
        {
            if (siteIdsNames == null)
                throw new ArgumentNullException(nameof(siteIdsNames));

            var query = _siteRepository.Table;
            var queryFilter = siteIdsNames.Distinct().ToArray();
            //filtering by name
            var filter = await query.Select(site => site.Name)
                .Where(site => queryFilter.Contains(site))
                .ToListAsync();
            queryFilter = queryFilter.Except(filter).ToArray();

            //if some names not found
            if (!queryFilter.Any())
                return queryFilter.ToArray();

            //filtering by IDs
            filter = await query.Select(site => site.Id.ToString())
                .Where(site => queryFilter.Contains(site))
                .ToListAsync();
            queryFilter = queryFilter.Except(filter).ToArray();

            return queryFilter.ToArray();
        }

        #endregion
    }
}