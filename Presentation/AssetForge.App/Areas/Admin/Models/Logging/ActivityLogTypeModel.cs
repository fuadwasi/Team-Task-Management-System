using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Logging;

/// <summary>
/// Represents an activity log type model
/// </summary>
public partial record ActivityLogTypeModel : BaseEntityModel
{
    #region Properties

    [ResourceDisplayName("Admin.Customers.ActivityLogType.Fields.Name")]
    public string Name { get; set; }

    [ResourceDisplayName("Admin.Customers.ActivityLogType.Fields.Enabled")]
    public bool Enabled { get; set; }

    #endregion
}