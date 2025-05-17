using Microsoft.AspNetCore.Authentication;

namespace AssetForge.Services.Authentication.External;

/// <summary>
/// Interface to register (configure) an external authentication service (plugin)
/// </summary>
public partial interface IExternalAuthenticationRegistrar
{
    /// <summary>
    /// Configure
    /// </summary>
    /// <param name="builder">Authentication builder</param>
    void Configure(AuthenticationBuilder builder);
}