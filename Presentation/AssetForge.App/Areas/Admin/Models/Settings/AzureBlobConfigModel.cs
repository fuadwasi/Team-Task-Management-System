using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents Azure Blob storage configuration model
/// </summary>
public partial record AzureBlobConfigModel : BaseModel, IConfigModel
{
    #region Properties

    [ResourceDisplayName("Admin.Configuration.AppSettings.AzureBlob.ConnectionString")]
    public string ConnectionString { get; set; }

    [ResourceDisplayName("Admin.Configuration.AppSettings.AzureBlob.ContainerName")]
    public string ContainerName { get; set; }

    [ResourceDisplayName("Admin.Configuration.AppSettings.AzureBlob.EndPoint")]
    public string EndPoint { get; set; }

    [ResourceDisplayName("Admin.Configuration.AppSettings.AzureBlob.AppendContainerName")]
    public bool AppendContainerName { get; set; }

    [ResourceDisplayName("Admin.Configuration.AppSettings.AzureBlob.SiteDataProtectionKeys")]
    public bool SiteDataProtectionKeys { get; set; }

    [ResourceDisplayName("Admin.Configuration.AppSettings.AzureBlob.DataProtectionKeysContainerName")]
    public string DataProtectionKeysContainerName { get; set; }

    [ResourceDisplayName("Admin.Configuration.AppSettings.AzureBlob.DataProtectionKeysVaultId")]
    public string DataProtectionKeysVaultId { get; set; }

    #endregion
}