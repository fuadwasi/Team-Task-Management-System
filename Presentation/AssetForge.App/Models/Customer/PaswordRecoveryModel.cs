using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AssetForge.App.Models.Customer;

public partial record PasswordRecoveryModel : BaseModel
{
    [DataType(DataType.EmailAddress)]
    [ResourceDisplayName("Account.PasswordRecovery.Email")]
    public string Email { get; set; }

    public bool DisplayCaptcha { get; set; }
}