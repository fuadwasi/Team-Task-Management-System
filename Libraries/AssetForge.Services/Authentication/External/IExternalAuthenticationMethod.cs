using AssetForge.Services.Plugins;

namespace AssetForge.Services.Authentication.External;

/// <summary>
/// Represents method for the external authentication
/// </summary>
public partial interface IExternalAuthenticationMethod : IPlugin
{
    /// <summary>
    /// Gets a type of a view component for displaying plugin in public site
    /// </summary>
    /// <returns>View component type</returns>
    Type GetPublicViewComponent();
}