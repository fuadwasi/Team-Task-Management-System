using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Settings;

/// <summary>
/// Represents a setting list model
/// </summary>
public partial record SettingListModel : BasePagedListModel<SettingModel>
{
}