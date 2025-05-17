using AssetForge.Core.Domain.Sites;

namespace AssetForge.Core
{
    /// <summary>
    /// Site context
    /// </summary>
    public interface ISiteContext
    {
        /// <summary>
        /// Gets the current Site
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task<Site> GetCurrentSiteAsync();

        /// <summary>
        /// Gets the current Site
        /// </summary>
        Site GetCurrentSite();

        /// <summary>
        /// Gets active Site scope configuration
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task<int> GetActiveSiteScopeConfigurationAsync();
    }
}
