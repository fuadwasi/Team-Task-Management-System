using Microsoft.AspNetCore.Mvc.Rendering;
using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Topics;

/// <summary>
/// Represents a topic model
/// </summary>
public partial record TopicModel : BaseEntityModel, IAclSupportedModel, ILocalizedModel<TopicLocalizedModel>, ISiteMappingSupportedModel
{
    #region Ctor

    public TopicModel()
    {
        AvailableTopicTemplates = new List<SelectListItem>();
        Locales = new List<TopicLocalizedModel>();

        SelectedCustomerRoleIds = new List<int>();
        AvailableCustomerRoles = new List<SelectListItem>();

        SelectedSiteIds = new List<int>();
        AvailableSites = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.SystemName")]
    public string SystemName { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.IncludeInSitemap")]
    public bool IncludeInSitemap { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.IncludeInTopMenu")]
    public bool IncludeInTopMenu { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.IncludeInFooterColumn1")]
    public bool IncludeInFooterColumn1 { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.IncludeInFooterColumn2")]
    public bool IncludeInFooterColumn2 { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.IncludeInFooterColumn3")]
    public bool IncludeInFooterColumn3 { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.AccessibleWhenSiteClosed")]
    public bool AccessibleWhenSiteClosed { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.IsPasswordProtected")]
    public bool IsPasswordProtected { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.Password")]
    public string Password { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.URL")]
    public string Url { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.Title")]
    public string Title { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.Body")]
    public string Body { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.Published")]
    public bool Published { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.TopicTemplate")]
    public int TopicTemplateId { get; set; }

    public IList<SelectListItem> AvailableTopicTemplates { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.MetaKeywords")]
    public string MetaKeywords { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.MetaDescription")]
    public string MetaDescription { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.MetaTitle")]
    public string MetaTitle { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.SeName")]
    public string SeName { get; set; }

    public IList<TopicLocalizedModel> Locales { get; set; }

    //site mapping
    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.LimitedToSites")]
    public IList<int> SelectedSiteIds { get; set; }

    public IList<SelectListItem> AvailableSites { get; set; }

    //ACL (customer roles)
    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.AclCustomerRoles")]
    public IList<int> SelectedCustomerRoleIds { get; set; }

    public IList<SelectListItem> AvailableCustomerRoles { get; set; }

    public string TopicName { get; set; }

    #endregion
}

public partial record TopicLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.Title")]
    public string Title { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.Body")]
    public string Body { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.MetaKeywords")]
    public string MetaKeywords { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.MetaDescription")]
    public string MetaDescription { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.MetaTitle")]
    public string MetaTitle { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.Topics.Fields.SeName")]
    public string SeName { get; set; }
}