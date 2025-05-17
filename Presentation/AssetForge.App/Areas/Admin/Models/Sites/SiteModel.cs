using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AssetForge.App.Areas.Admin.Models.Sites;

/// <summary>
/// Represents a site model
/// </summary>
public partial record SiteModel : BaseEntityModel, ILocalizedModel<SiteLocalizedModel>
{
    #region Ctor

    public SiteModel()
    {
        Locales = new List<SiteLocalizedModel>();
        AvailableLanguages = new List<SelectListItem>();
        DisplayOrder = 1;
        SslEnabled = true;
    }

    #endregion

    #region Properties

    [ResourceDisplayName("Admin.Configuration.Sites.Fields.Name")]
    public string Name { get; set; }

    [ResourceDisplayName("Admin.Configuration.Sites.Fields.Url")]
    public string Url { get; set; }

    [ResourceDisplayName("Admin.Configuration.Sites.Fields.SslEnabled")]
    public virtual bool SslEnabled { get; set; }

    [ResourceDisplayName("Admin.Configuration.Sites.Fields.Hosts")]
    public string Hosts { get; set; }

    //default language
    [ResourceDisplayName("Admin.Configuration.Sites.Fields.DefaultLanguage")]
    public int DefaultLanguageId { get; set; }

    public IList<SelectListItem> AvailableLanguages { get; set; }

    [ResourceDisplayName("Admin.Configuration.Sites.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    [ResourceDisplayName("Admin.Configuration.Sites.Fields.CompanyName")]
    public string CompanyName { get; set; }

    [ResourceDisplayName("Admin.Configuration.Sites.Fields.CompanyAddress")]
    public string CompanyAddress { get; set; }

    [DataType(DataType.PhoneNumber)]
    [ResourceDisplayName("Admin.Configuration.Sites.Fields.CompanyPhoneNumber")]
    public string CompanyPhoneNumber { get; set; }

    [ResourceDisplayName("Admin.Configuration.Sites.Fields.CompanyVat")]
    public string CompanyVat { get; set; }

    [ResourceDisplayName("Admin.Configuration.Sites.Fields.DefaultMetaKeywords")]
    public string DefaultMetaKeywords { get; set; }

    [ResourceDisplayName("Admin.Configuration.Sites.Fields.DefaultMetaDescription")]
    public string DefaultMetaDescription { get; set; }

    [ResourceDisplayName("Admin.Configuration.Sites.Fields.DefaultTitle")]
    public string DefaultTitle { get; set; }

    [ResourceDisplayName("Admin.Configuration.Sites.Fields.HomepageTitle")]
    public string HomepageTitle { get; set; }

    [ResourceDisplayName("Admin.Configuration.Sites.Fields.HomepageDescription")]
    public string HomepageDescription { get; set; }

    public IList<SiteLocalizedModel> Locales { get; set; }

    #endregion
}

public partial record SiteLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [ResourceDisplayName("Admin.Configuration.Sites.Fields.Name")]
    public string Name { get; set; }

    [ResourceDisplayName("Admin.Configuration.Sites.Fields.DefaultMetaKeywords")]
    public string DefaultMetaKeywords { get; set; }

    [ResourceDisplayName("Admin.Configuration.Sites.Fields.DefaultMetaDescription")]
    public string DefaultMetaDescription { get; set; }

    [ResourceDisplayName("Admin.Configuration.Sites.Fields.DefaultTitle")]
    public string DefaultTitle { get; set; }

    [ResourceDisplayName("Admin.Configuration.Sites.Fields.HomepageTitle")]
    public string HomepageTitle { get; set; }

    [ResourceDisplayName("Admin.Configuration.Sites.Fields.HomepageDescription")]
    public string HomepageDescription { get; set; }
}