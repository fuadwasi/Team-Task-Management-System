using AssetForge.Services.Plugins;

namespace AssetForge.Services.Authentication.MultiFactor;

/// <summary>
/// Represents method for the multi-factor authentication
/// </summary>
public partial interface IMultiFactorAuthenticationMethod : IPlugin
{
    #region Methods

    /// <summary>
    ///  Gets a multi-factor authentication type
    /// </summary>
    MultiFactorAuthenticationType Type { get; }

    /// <summary>
    /// Gets a type of a view component for displaying plugin in public site
    /// </summary>
    /// <returns>View component type</returns>
    Type GetPublicViewComponent();

    /// <summary>
    /// Gets a type of a view component for displaying verification page
    /// </summary>
    /// <returns>View component type</returns>
    Type GetVerificationViewComponent();

    /// <summary>
    /// Gets a multi-factor authentication method description that will be displayed on customer info pages in the public site
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task<string> GetDescriptionAsync();

    #endregion
}