using AssetForge.Core.Domain.Localization;
using AssetForge.Core.Domain.Security;
using AssetForge.Core.Domain.Seo;
using AssetForge.Core.Domain.Sites;

namespace AssetForge.Core.Domain.Topics;

/// <summary>
/// Represents a topic
/// </summary>
public partial class Topic : BaseEntity, ILocalizedEntity, ISlugSupported, ISiteMappingSupported, IAclSupported
{
    public string SystemName { get; set; }
    public bool IncludeInSitemap { get; set; }
    public bool IncludeInTopMenu { get; set; }
    public bool IncludeInFooterColumn1 { get; set; }
    public bool IncludeInFooterColumn2 { get; set; }
    public bool IncludeInFooterColumn3 { get; set; }
    public int DisplayOrder { get; set; }
    public bool AccessibleWhenSiteClosed { get; set; }
    public bool IsPasswordProtected { get; set; }
    public string Password { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public bool Published { get; set; }
    public int TopicTemplateId { get; set; }
    public string MetaKeywords { get; set; }
    public string MetaDescription { get; set; }
    public string MetaTitle { get; set; }
    public bool SubjectToAcl { get; set; }
    public bool LimitedToSites { get; set; }
}
