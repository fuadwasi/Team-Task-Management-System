using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Directory;

/// <summary>
/// Represents a state and province list model
/// </summary>
public partial record StateProvinceListModel : BasePagedListModel<StateProvinceModel>
{
}