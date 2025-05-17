using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AssetForge.App.Models.Customer;

public partial record ChangePasswordModel : BaseModel
{
    [DataType(DataType.Password)]
    [NoTrim]
    [ResourceDisplayName("Account.ChangePassword.Fields.OldPassword")]
    public string OldPassword { get; set; }

    [DataType(DataType.Password)]
    [NoTrim]
    [ResourceDisplayName("Account.ChangePassword.Fields.NewPassword")]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [NoTrim]
    [ResourceDisplayName("Account.ChangePassword.Fields.ConfirmNewPassword")]
    public string ConfirmNewPassword { get; set; }
}