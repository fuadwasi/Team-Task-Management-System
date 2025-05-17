namespace AssetForge.Core.Security;

/// <summary>
/// Represents default values related to data protection
/// </summary>
public static partial class DataProtectionDefaults
{
    /// <summary>
    /// Gets the name of the key file used to Site the protection key list to Azure (used with the UseAzureBlobStorageToSiteDataProtectionKeys option enabled)
    /// </summary>
    public static string AzureDataProtectionKeyFile => "DataProtectionKeys.xml";

    /// <summary>
    /// Gets the name of the key path used to Site the protection key list to local file system (used when UseAzureBlobStorageToSiteDataProtectionKeys option not enabled)
    /// </summary>
    public static string DataProtectionKeysPath => "~/App_Data/DataProtectionKeys";
}