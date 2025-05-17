using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Common;

/// <summary>
/// Represents a backup file list model
/// </summary>
public partial record BackupFileListModel : BasePagedListModel<BackupFileModel>
{
}