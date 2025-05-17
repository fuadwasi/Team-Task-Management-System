using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents a hosting configuration model
/// </summary>
public partial record HostingConfigModel : BaseModel, IConfigModel
{
    #region Properties

    [ResourceDisplayName("Admin.Configuration.AppSettings.Hosting.UseProxy")]
    public bool UseProxy { get; set; }

    [ResourceDisplayName("Admin.Configuration.AppSettings.Hosting.ForwardedForHeaderName")]
    public string ForwardedForHeaderName { get; set; }

    [ResourceDisplayName("Admin.Configuration.AppSettings.Hosting.ForwardedProtoHeaderName")]
    public string ForwardedProtoHeaderName { get; set; }

    [ResourceDisplayName("Admin.Configuration.AppSettings.Hosting.KnownProxies")]
    public string KnownProxies { get; set; }

    [ResourceDisplayName("Admin.Configuration.AppSettings.Hosting.KnownNetworks")]
    public string KnownNetworks { get; set; }
    #endregion
}