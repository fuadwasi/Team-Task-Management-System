using AssetForge.Core.Domain.Customers;
using AssetForge.Services.Customers;
using AssetForge.Services.Plugins;

namespace AssetForge.Services.Authentication.MultiFactor;

/// <summary>
/// Represents an multi-factor authentication plugin manager implementation
/// </summary>
public partial class MultiFactorAuthenticationPluginManager : PluginManager<IMultiFactorAuthenticationMethod>, IMultiFactorAuthenticationPluginManager
{
    #region Fields

    protected readonly MultiFactorAuthenticationSettings _multiFactorAuthenticationSettings;

    #endregion

    #region Ctor

    public MultiFactorAuthenticationPluginManager(MultiFactorAuthenticationSettings multiFactorAuthenticationSettings,
        ICustomerService customerService,
        IPluginService pluginService) : base(customerService, pluginService)
    {
        _multiFactorAuthenticationSettings = multiFactorAuthenticationSettings;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Check is active multi-factor authentication methods
    /// </summary>
    /// <param name="customer">Filter by customer; pass null to load all plugins</param>
    /// <param name="siteId">Filter by site; pass 0 to load all plugins</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains true if we have active multi-factor authentication methods
    /// </returns>
    public virtual async Task<bool> HasActivePluginsAsync(Customer customer = null, int siteId = 0)
    {
        return (await LoadActivePluginsAsync(_multiFactorAuthenticationSettings.ActiveAuthenticationMethodSystemNames, customer, siteId)).Any();
    }

    /// <summary>
    /// Load active multi-factor authentication methods
    /// </summary>
    /// <param name="customer">Filter by customer; pass null to load all plugins</param>
    /// <param name="siteId">Filter by site; pass 0 to load all plugins</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of active multi-factor authentication methods
    /// </returns>
    public virtual async Task<IList<IMultiFactorAuthenticationMethod>> LoadActivePluginsAsync(Customer customer = null, int siteId = 0)
    {
        return await LoadActivePluginsAsync(_multiFactorAuthenticationSettings.ActiveAuthenticationMethodSystemNames, customer, siteId);
    }

    /// <summary>
    /// Check whether the passed multi-factor authentication method is active
    /// </summary>
    /// <param name="authenticationMethod">Authentication method to check</param>
    /// <returns>Result</returns>
    public virtual bool IsPluginActive(IMultiFactorAuthenticationMethod authenticationMethod)
    {
        return IsPluginActive(authenticationMethod, _multiFactorAuthenticationSettings.ActiveAuthenticationMethodSystemNames);
    }

    /// <summary>
    /// Check whether the multi-factor authentication method with the passed system name is active
    /// </summary>
    /// <param name="systemName">System name of authentication method to check</param>
    /// <param name="customer">Filter by customer; pass null to load all plugins</param>
    /// <param name="siteId">Filter by site; pass 0 to load all plugins</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the result
    /// </returns>
    public virtual async Task<bool> IsPluginActiveAsync(string systemName, Customer customer = null, int siteId = 0)
    {
        var authenticationMethod = await LoadPluginBySystemNameAsync(systemName, customer, siteId);
        return IsPluginActive(authenticationMethod);
    }

    #endregion
}