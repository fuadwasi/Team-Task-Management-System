using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Localization;

/// <summary>
/// Represents a locale resource list model
/// </summary>
public partial record LocaleResourceListModel : BasePagedListModel<LocaleResourceModel>
{
}