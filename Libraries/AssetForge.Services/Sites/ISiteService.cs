using AssetForge.Core.Domain.Sites;

namespace AssetForge.Services.Sites
{
    /// <summary>
    /// Site service interface
    /// </summary>
    public partial interface ISiteService
    {
        /// <summary>
        /// Deletes a site
        /// </summary>
        /// <param name="site">Site</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteSiteAsync(Site site);

        /// <summary>
        /// Gets all sites
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the sites
        /// </returns>
        Task<IList<Site>> GetAllSitesAsync();

        /// <summary>
        /// Gets all sites
        /// </summary>
        /// <returns>
        /// The sites
        /// </returns>
        IList<Site> GetAllSites();

        /// <summary>
        /// Gets a site 
        /// </summary>
        /// <param name="siteId">Site identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the site
        /// </returns>
        Task<Site> GetSiteByIdAsync(int siteId);

        /// <summary>
        /// Inserts a site
        /// </summary>
        /// <param name="site">Site</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertSiteAsync(Site site);

        /// <summary>
        /// Updates the site
        /// </summary>
        /// <param name="site">Site</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateSiteAsync(Site site);

        /// <summary>
        /// Updates the site
        /// </summary>
        /// <param name="site">Site</param>
        void UpdateSite(Site site);

        /// <summary>
        /// Indicates whether a site contains a specified host
        /// </summary>
        /// <param name="site">Site</param>
        /// <param name="host">Host</param>
        /// <returns>true - contains, false - no</returns>
        bool ContainsHostValue(Site site, string host);

        /// <summary>
        /// Returns a list of names of not existing sites
        /// </summary>
        /// <param name="siteIdsNames">The names and/or IDs of the site to check</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the list of names and/or IDs not existing sites
        /// </returns>
        Task<string[]> GetNotExistingSitesAsync(string[] siteIdsNames);
    }
}