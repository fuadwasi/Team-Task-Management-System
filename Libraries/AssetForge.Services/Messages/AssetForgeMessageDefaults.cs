using AssetForge.Core.Caching;
using AssetForge.Core.Domain.Messages;

namespace AssetForge.Services.Messages;

/// <summary>
/// Represents default values related to messages services
/// </summary>
public static partial class AssetForgeMessageDefaults
{
    /// <summary>
    /// Gets a key for notifications list from TempDataDictionary
    /// </summary>
    public static string NotificationListKey => "NotificationList";

    /// <summary>
    /// Gets the path to directory used to site the token response
    /// </summary>
    public static string GmailAuthSitePath => "~/App_Data/Gmail/AuthSite";

    /// <summary>
    /// Gets the scopes requested to access a protected API (Gmail)
    /// </summary>
    public static string[] GmailScopes => ["https://mail.google.com/"];

    /// <summary>
    /// Gets the scopes requested to access a protected API (MSAL)
    /// </summary>
    public static string MSALTenantPattern => "https://login.microsoftonline.com/{0}/v2.0";

    /// <summary>
    /// Gets the scopes requested to access a protected API (MSAL)
    /// </summary>
    public static string[] MSALScopes => ["https://outlook.office365.com/.default"];

    #region Caching defaults

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : site ID
    /// {1} : is active?
    /// </remarks>
    public static CacheKey MessageTemplatesAllCacheKey => new("AssetForge.messagetemplate.all.{0}-{1}", EntityCacheDefaults<MessageTemplate>.AllPrefix);

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : template name
    /// {1} : site ID
    /// </remarks>
    public static CacheKey MessageTemplatesByNameCacheKey => new("AssetForge.messagetemplate.byname.{0}-{1}", MessageTemplatesByNamePrefix);

    /// <summary>
    /// Gets a key pattern to clear cache
    /// </summary>
    /// <remarks>
    /// {0} : template name
    /// </remarks>
    public static string MessageTemplatesByNamePrefix => "AssetForge.messagetemplate.byname.{0}";

    #endregion
}