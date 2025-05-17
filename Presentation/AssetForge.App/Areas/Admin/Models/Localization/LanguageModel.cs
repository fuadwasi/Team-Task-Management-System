using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssetForge.App.Areas.Admin.Models.Localization;

/// <summary>
/// Represents a language model
/// </summary>
public partial record LanguageModel : BaseEntityModel, ISiteMappingSupportedModel
{
    #region Ctor

    public LanguageModel()
    {
        //AvailableCurrencies = new List<SelectListItem>();
        AvailableFlagImages = new List<SelectListItem>();
        SelectedSiteIds = new List<int>();
        AvailableSites = new List<SelectListItem>();
        LocaleResourceSearchModel = new LocaleResourceSearchModel();
    }

    #endregion

    #region Properties

    [ResourceDisplayName("Admin.Configuration.Languages.Fields.Name")]
    public string Name { get; set; }

    [ResourceDisplayName("Admin.Configuration.Languages.Fields.LanguageCulture")]
    public string LanguageCulture { get; set; }

    [ResourceDisplayName("Admin.Configuration.Languages.Fields.UniqueSeoCode")]
    public string UniqueSeoCode { get; set; }

    //flags
    [ResourceDisplayName("Admin.Configuration.Languages.Fields.FlagImageFileName")]
    public string FlagImageFileName { get; set; }

    [ResourceDisplayName("Admin.Configuration.Languages.Fields.Rtl")]
    public bool Rtl { get; set; }

    ////default currency
    //[ResourceDisplayName("Admin.Configuration.Languages.Fields.DefaultCurrency")]
    //public int DefaultCurrencyId { get; set; }

    //public IList<SelectListItem> AvailableCurrencies { get; set; }

    [ResourceDisplayName("Admin.Configuration.Languages.Fields.Published")]
    public bool Published { get; set; }

    [ResourceDisplayName("Admin.Configuration.Languages.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    //site mapping
    [ResourceDisplayName("Admin.Configuration.Languages.Fields.LimitedToSites")]
    public IList<int> SelectedSiteIds { get; set; }

    public IList<SelectListItem> AvailableFlagImages { get; set; }

    public IList<SelectListItem> AvailableSites { get; set; }

    // search
    public LocaleResourceSearchModel LocaleResourceSearchModel { get; set; }

    #endregion
}