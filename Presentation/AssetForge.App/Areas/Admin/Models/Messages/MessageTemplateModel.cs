using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AssetForge.App.Areas.Admin.Models.Messages;

/// <summary>
/// Represents a message template model
/// </summary>
public partial record MessageTemplateModel : BaseEntityModel, ILocalizedModel<MessageTemplateLocalizedModel>, ISiteMappingSupportedModel
{
    #region Ctor

    public MessageTemplateModel()
    {
        Locales = new List<MessageTemplateLocalizedModel>();
        AvailableEmailAccounts = new List<SelectListItem>();

        SelectedSiteIds = new List<int>();
        AvailableSites = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    [ResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.AllowedTokens")]
    public string AllowedTokens { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.Name")]
    public string Name { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.BccEmailAddresses")]
    public string BccEmailAddresses { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.Subject")]
    public string Subject { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.Body")]
    public string Body { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.IsActive")]
    public bool IsActive { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.SendImmediately")]
    public bool SendImmediately { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.DelayBeforeSend")]
    [UIHint("Int32Nullable")]
    public int? DelayBeforeSend { get; set; }

    public int DelayPeriodId { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.AllowDirectReply")]
    public bool AllowDirectReply { get; set; }

    public bool HasAttachedDownload { get; set; }
    [ResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.AttachedDownload")]
    [UIHint("Download")]
    public int AttachedDownloadId { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.EmailAccount")]
    public int EmailAccountId { get; set; }

    public IList<SelectListItem> AvailableEmailAccounts { get; set; }

    //site mapping
    [ResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.LimitedToSites")]
    public IList<int> SelectedSiteIds { get; set; }

    public IList<SelectListItem> AvailableSites { get; set; }

    //comma-separated list of sites used on the list page
    [ResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.LimitedToSites")]
    public string ListOfSites { get; set; }

    public IList<MessageTemplateLocalizedModel> Locales { get; set; }

    #endregion
}

public partial record MessageTemplateLocalizedModel : ILocalizedLocaleModel
{
    public MessageTemplateLocalizedModel()
    {
        AvailableEmailAccounts = new List<SelectListItem>();
    }

    public int LanguageId { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.BccEmailAddresses")]
    public string BccEmailAddresses { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.Subject")]
    public string Subject { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.Body")]
    public string Body { get; set; }

    [ResourceDisplayName("Admin.ContentManagement.MessageTemplates.Fields.EmailAccount")]
    public int EmailAccountId { get; set; }
    public IList<SelectListItem> AvailableEmailAccounts { get; set; }
}