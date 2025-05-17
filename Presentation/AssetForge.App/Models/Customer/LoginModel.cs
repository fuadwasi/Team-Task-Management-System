using AssetForge.Core.Domain.Customers;
using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AssetForge.App.Models.Customer;

public partial record LoginModel : BaseModel
{
    public bool CheckoutAsGuest { get; set; }

    [DataType(DataType.EmailAddress)]
    [ResourceDisplayName("Account.Login.Fields.Email")]
    public string Email { get; set; }

    public bool UsernamesEnabled { get; set; }

    public UserRegistrationType RegistrationType { get; set; }

    [ResourceDisplayName("Account.Login.Fields.Username")]
    public string Username { get; set; }

    [DataType(DataType.Password)]
    [NoTrim]
    [ResourceDisplayName("Account.Login.Fields.Password")]
    public string Password { get; set; }

    [ResourceDisplayName("Account.Login.Fields.RememberMe")]
    public bool RememberMe { get; set; }

    public bool DisplayCaptcha { get; set; }
}