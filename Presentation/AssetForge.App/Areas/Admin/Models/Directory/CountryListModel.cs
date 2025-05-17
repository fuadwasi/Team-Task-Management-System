using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Directory;

/// <summary>
/// Represents a country list model
/// </summary>
public partial record CountryListModel : BasePagedListModel<CountryModel>
{
}