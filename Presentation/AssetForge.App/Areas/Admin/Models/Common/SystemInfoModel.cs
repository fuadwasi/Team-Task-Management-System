using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Common;

public partial record SystemInfoModel : BaseModel
{
    public SystemInfoModel()
    {
        Headers = new List<HeaderModel>();
        LoadedAssemblies = new List<LoadedAssembly>();
    }

    [ResourceDisplayName("Admin.System.SystemInfo.ASPNETInfo")]
    public string AspNetInfo { get; set; }

    [ResourceDisplayName("Admin.System.SystemInfo.IsFullTrust")]
    public bool IsFullTrust { get; set; }

    [ResourceDisplayName("Admin.System.SystemInfo.AssetForgeVersion")]
    public string AssetForgeVersion { get; set; }

    [ResourceDisplayName("Admin.System.SystemInfo.OperatingSystem")]
    public string OperatingSystem { get; set; }

    [ResourceDisplayName("Admin.System.SystemInfo.ServerLocalTime")]
    public DateTime ServerLocalTime { get; set; }

    [ResourceDisplayName("Admin.System.SystemInfo.ServerTimeZone")]
    public string ServerTimeZone { get; set; }

    [ResourceDisplayName("Admin.System.SystemInfo.UTCTime")]
    public DateTime UtcTime { get; set; }

    [ResourceDisplayName("Admin.System.SystemInfo.CurrentUserTime")]
    public DateTime CurrentUserTime { get; set; }

    [ResourceDisplayName("Admin.System.SystemInfo.CurrentStaticCacheManager")]
    public string CurrentStaticCacheManager { get; set; }

    [ResourceDisplayName("Admin.System.SystemInfo.HTTPHOST")]
    public string HttpHost { get; set; }

    [ResourceDisplayName("Admin.System.SystemInfo.Headers")]
    public IList<HeaderModel> Headers { get; set; }

    [ResourceDisplayName("Admin.System.SystemInfo.LoadedAssemblies")]
    public IList<LoadedAssembly> LoadedAssemblies { get; set; }

    [ResourceDisplayName("Admin.System.SystemInfo.AzureBlobStorageEnabled")]
    public bool AzureBlobStorageEnabled { get; set; }

    public partial record HeaderModel : BaseModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public partial record LoadedAssembly : BaseModel
    {
        public string FullName { get; set; }
        public string Location { get; set; }
        public bool IsDebug { get; set; }
        public DateTime? BuildDate { get; set; }
    }
}