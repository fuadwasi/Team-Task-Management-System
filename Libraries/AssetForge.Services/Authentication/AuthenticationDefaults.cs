using Microsoft.AspNetCore.Http;

namespace AssetForge.Services.Authentication;

/// <summary>
/// Represents default values related to authentication services
/// </summary>
public static partial class AuthenticationDefaults
{
    /// <summary>
    /// The default value used for authentication scheme
    /// </summary>
    public static string AuthenticationScheme => "Authentication";

    /// <summary>
    /// The default value used for external authentication scheme
    /// </summary>
    public static string ExternalAuthenticationScheme => "ExternalAuthentication";

    /// <summary>
    /// The issuer that should be used for any claims that are created
    /// </summary>
    public static string ClaimsIssuer => "AssetForgeCms";

    /// <summary>
    /// The default value for the login path
    /// </summary>
    public static PathString LoginPath => new("/login");

    /// <summary>
    /// The default value for the access denied path
    /// </summary>
    public static PathString AccessDeniedPath => new("/page-not-found");

    /// <summary>
    /// Gets a key to site external authentication errors to session
    /// </summary>
    public static string ExternalAuthenticationErrorsSessionKey => "ix.externalauth.errors";
}