using AssetForge.Core;

namespace AssetForge.Services.Common;

/// <summary>
/// Represents the HTTP client to request current site
/// </summary>
public partial class SiteHttpClient
{
    #region Fields

    protected readonly HttpClient _httpClient;

    #endregion

    #region Ctor

    public SiteHttpClient(HttpClient client,
        IWebHelper webHelper)
    {
        //configure client
        client.BaseAddress = new Uri(webHelper.GetSiteLocation());

        _httpClient = client;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Keep the current site site alive
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the asynchronous task whose result determines that request completed
    /// </returns>
    public virtual async Task KeepAliveAsync()
    {
        await _httpClient.GetStringAsync(CommonDefaults.KeepAlivePath);
    }

    #endregion
}