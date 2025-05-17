using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AssetForge.App.Areas.Admin.Models.Messages;

/// <summary>
/// Represents a queued email search model
/// </summary>
public partial record QueuedEmailSearchModel : BaseSearchModel
{
    #region Properties

    [ResourceDisplayName("Admin.System.QueuedEmails.List.StartDate")]
    [UIHint("DateNullable")]
    public DateTime? SearchStartDate { get; set; }

    [ResourceDisplayName("Admin.System.QueuedEmails.List.EndDate")]
    [UIHint("DateNullable")]
    public DateTime? SearchEndDate { get; set; }

    [DataType(DataType.EmailAddress)]
    [ResourceDisplayName("Admin.System.QueuedEmails.List.FromEmail")]
    public string SearchFromEmail { get; set; }

    [DataType(DataType.EmailAddress)]
    [ResourceDisplayName("Admin.System.QueuedEmails.List.ToEmail")]
    public string SearchToEmail { get; set; }

    [ResourceDisplayName("Admin.System.QueuedEmails.List.LoadNotSent")]
    public bool SearchLoadNotSent { get; set; }

    [ResourceDisplayName("Admin.System.QueuedEmails.List.MaxSentTries")]
    public int SearchMaxSentTries { get; set; }

    [ResourceDisplayName("Admin.System.QueuedEmails.List.GoDirectlyToNumber")]
    public int GoDirectlyToNumber { get; set; }

    #endregion
}