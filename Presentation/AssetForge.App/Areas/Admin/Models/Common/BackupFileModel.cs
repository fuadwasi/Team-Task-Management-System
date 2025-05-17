using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Common;

/// <summary>
/// Represents a backup file model
/// </summary>
public partial record BackupFileModel : BaseModel
{
    #region Properties

    public string Name { get; set; }

    public string Length { get; set; }

    public string Link { get; set; }

    #endregion
}