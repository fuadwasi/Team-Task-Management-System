using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AssetForge.App.Models.Customer;

public partial record PasswordRecoveryConfirmModel : BaseModel
{
    [NoTrim]
    [DataType(DataType.Password)]
    [ResourceDisplayName("Account.PasswordRecovery.NewPassword")]
    public string NewPassword { get; set; }

    [NoTrim]
    [DataType(DataType.Password)]
    [ResourceDisplayName("Account.PasswordRecovery.ConfirmNewPassword")]
    public string ConfirmNewPassword { get; set; }

    public bool DisablePasswordChanging { get; set; }
    public string Result { get; set; }

    public string ReturnUrl { get; set; }
}