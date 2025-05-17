using AssetForge.App.Areas.Admin.Models.Localization;
using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Common;

/// <summary>
/// Represents an admin language selector model
/// </summary>
public partial record LanguageSelectorModel : BaseModel
{
    #region Ctor

    public LanguageSelectorModel()
    {
        AvailableLanguages = new List<LanguageModel>();
    }

    #endregion

    #region Properties

    public IList<LanguageModel> AvailableLanguages { get; set; }

    public LanguageModel CurrentLanguage { get; set; }

    #endregion
}