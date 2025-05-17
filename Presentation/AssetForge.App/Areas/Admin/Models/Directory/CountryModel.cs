using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssetForge.App.Areas.Admin.Models.Directory;

/// <summary>
/// Represents a country model
/// </summary>
public partial record CountryModel : BaseEntityModel, ILocalizedModel<CountryLocalizedModel>, ISiteMappingSupportedModel
{
    #region Ctor

    public CountryModel()
    {
        Locales = new List<CountryLocalizedModel>();
        SelectedSiteIds = new List<int>();
        AvailableSites = new List<SelectListItem>();
        StateProvinceSearchModel = new StateProvinceSearchModel();
    }

    #endregion

    #region Properties

    [ResourceDisplayName("Admin.Configuration.Countries.Fields.Name")]
    public string Name { get; set; }

    [ResourceDisplayName("Admin.Configuration.Countries.Fields.AllowsBilling")]
    public bool AllowsBilling { get; set; }

    [ResourceDisplayName("Admin.Configuration.Countries.Fields.AllowsShipping")]
    public bool AllowsShipping { get; set; }

    [ResourceDisplayName("Admin.Configuration.Countries.Fields.TwoLetterIsoCode")]
    public string TwoLetterIsoCode { get; set; }

    [ResourceDisplayName("Admin.Configuration.Countries.Fields.ThreeLetterIsoCode")]
    public string ThreeLetterIsoCode { get; set; }

    [ResourceDisplayName("Admin.Configuration.Countries.Fields.NumericIsoCode")]
    public int NumericIsoCode { get; set; }

    [ResourceDisplayName("Admin.Configuration.Countries.Fields.SubjectToVat")]
    public bool SubjectToVat { get; set; }

    [ResourceDisplayName("Admin.Configuration.Countries.Fields.Published")]
    public bool Published { get; set; }

    [ResourceDisplayName("Admin.Configuration.Countries.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    [ResourceDisplayName("Admin.Configuration.Countries.Fields.NumberOfStates")]
    public int NumberOfStates { get; set; }

    public IList<CountryLocalizedModel> Locales { get; set; }

    //store mapping
    [ResourceDisplayName("Admin.Configuration.Countries.Fields.LimitedToSites")]
    public IList<int> SelectedSiteIds { get; set; }
    public IList<SelectListItem> AvailableSites { get; set; }

    public StateProvinceSearchModel StateProvinceSearchModel { get; set; }

    #endregion
}

public partial record CountryLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [ResourceDisplayName("Admin.Configuration.Countries.Fields.Name")]
    public string Name { get; set; }
}