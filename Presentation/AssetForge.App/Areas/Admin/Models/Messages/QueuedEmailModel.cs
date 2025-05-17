using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AssetForge.App.Areas.Admin.Models.Messages;

/// <summary>
/// Represents a queued email model
/// </summary>
public partial record QueuedEmailModel : BaseEntityModel
{
    #region Properties

    [ResourceDisplayName("Admin.System.QueuedEmails.Fields.Id")]
    public override int Id { get; set; }

    [ResourceDisplayName("Admin.System.QueuedEmails.Fields.Priority")]
    public string PriorityName { get; set; }

    [ResourceDisplayName("Admin.System.QueuedEmails.Fields.From")]
    public string From { get; set; }

    [ResourceDisplayName("Admin.System.QueuedEmails.Fields.FromName")]
    public string FromName { get; set; }

    [ResourceDisplayName("Admin.System.QueuedEmails.Fields.To")]
    public string To { get; set; }

    [ResourceDisplayName("Admin.System.QueuedEmails.Fields.ToName")]
    public string ToName { get; set; }

    [ResourceDisplayName("Admin.System.QueuedEmails.Fields.ReplyTo")]
    public string ReplyTo { get; set; }

    [ResourceDisplayName("Admin.System.QueuedEmails.Fields.ReplyToName")]
    public string ReplyToName { get; set; }

    [ResourceDisplayName("Admin.System.QueuedEmails.Fields.CC")]
    public string CC { get; set; }

    [ResourceDisplayName("Admin.System.QueuedEmails.Fields.Bcc")]
    public string Bcc { get; set; }

    [ResourceDisplayName("Admin.System.QueuedEmails.Fields.Subject")]
    public string Subject { get; set; }

    [ResourceDisplayName("Admin.System.QueuedEmails.Fields.Body")]
    public string Body { get; set; }

    [ResourceDisplayName("Admin.System.QueuedEmails.Fields.AttachmentFilePath")]
    public string AttachmentFilePath { get; set; }

    [ResourceDisplayName("Admin.System.QueuedEmails.Fields.AttachedDownload")]
    [UIHint("Download")]
    public int AttachedDownloadId { get; set; }

    [ResourceDisplayName("Admin.System.QueuedEmails.Fields.CreatedOn")]
    public DateTime CreatedOn { get; set; }

    [ResourceDisplayName("Admin.System.QueuedEmails.Fields.SendImmediately")]
    public bool SendImmediately { get; set; }

    [ResourceDisplayName("Admin.System.QueuedEmails.Fields.DontSendBeforeDate")]
    [UIHint("DateTimeNullable")]
    public DateTime? DontSendBeforeDate { get; set; }

    [ResourceDisplayName("Admin.System.QueuedEmails.Fields.SentTries")]
    public int SentTries { get; set; }

    [ResourceDisplayName("Admin.System.QueuedEmails.Fields.SentOn")]
    public DateTime? SentOn { get; set; }

    [ResourceDisplayName("Admin.System.QueuedEmails.Fields.EmailAccountName")]
    public string EmailAccountName { get; set; }

    #endregion
}