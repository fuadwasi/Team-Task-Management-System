using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Logging;

/// <summary>
/// Represents a log model
/// </summary>
public partial record LogModel : BaseEntityModel
{
    #region Properties

    [ResourceDisplayName("Admin.System.Log.Fields.LogLevel")]
    public string LogLevel { get; set; }

    [ResourceDisplayName("Admin.System.Log.Fields.ShortMessage")]
    public string ShortMessage { get; set; }

    [ResourceDisplayName("Admin.System.Log.Fields.FullMessage")]
    public string FullMessage { get; set; }

    [ResourceDisplayName("Admin.System.Log.Fields.IPAddress")]
    public string IpAddress { get; set; }

    [ResourceDisplayName("Admin.System.Log.Fields.Customer")]
    public int? CustomerId { get; set; }

    [ResourceDisplayName("Admin.System.Log.Fields.Customer")]
    public string CustomerEmail { get; set; }

    [ResourceDisplayName("Admin.System.Log.Fields.PageURL")]
    public string PageUrl { get; set; }

    [ResourceDisplayName("Admin.System.Log.Fields.ReferrerURL")]
    public string ReferrerUrl { get; set; }

    [ResourceDisplayName("Admin.System.Log.Fields.CreatedOn")]
    public DateTime CreatedOn { get; set; }

    #endregion
}