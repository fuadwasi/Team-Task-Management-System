using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Localization;

/// <summary>
/// Represents a language list model
/// </summary>
public partial record LanguageListModel : BasePagedListModel<LanguageModel>
{
}