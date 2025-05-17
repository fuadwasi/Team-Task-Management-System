using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Security;

/// <summary>
/// Represents a permission record model
/// </summary>
public partial record PermissionRecordModel : BaseModel
{
    #region Properties

    public string Name { get; set; }

    public string SystemName { get; set; }

    #endregion
}