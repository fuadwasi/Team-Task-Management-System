using AssetForge.Core.Domain.Messages;
using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AssetForge.App.Areas.Admin.Models.Messages;

/// <summary>
/// Represents an email account model
/// </summary>
public partial record EmailAccountModel : BaseEntityModel
{
    #region Properties

    [DataType(DataType.EmailAddress)]
    [ResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.Email")]
    public string Email { get; set; }

    [ResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.DisplayName")]
    public string DisplayName { get; set; }

    [ResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.Host")]
    public string Host { get; set; }

    [ResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.Port")]
    public int Port { get; set; }

    [ResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.Username")]
    public string Username { get; set; }

    [ResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.Password")]
    [NoTrim]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [ResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.EnableSsl")]
    public bool EnableSsl { get; set; }

    [ResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.IsDefaultEmailAccount")]
    public bool IsDefaultEmailAccount { get; set; }

    [ResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.SendTestEmailTo")]
    public string SendTestEmailTo { get; set; }

    [ResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.MaxNumberOfEmails")]
    public int MaxNumberOfEmails { get; set; }

    [ResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.EmailAuthenticationMethod")]
    public EmailAuthenticationMethod EmailAuthenticationMethod { get; set; }
    public List<SelectListItem> AvailableEmailAuthenticationMethods { get; set; } = new();

    [ResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.ClientId")]
    public string ClientId { get; set; }

    [ResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.ClientSecret")]
    [NoTrim]
    [DataType(DataType.Password)]
    public string ClientSecret { get; set; }

    [ResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.TenantId")]
    public string TenantId { get; set; }

    public string AuthUrl { get; set; }

    #endregion
}