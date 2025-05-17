using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Models.Customer;

public partial record MultiFactorAuthenticationModel : BaseModel
{
    public MultiFactorAuthenticationModel()
    {
        Providers = new List<MultiFactorAuthenticationProviderModel>();
    }

    [ResourceDisplayName("Account.MultiFactorAuthentication.Fields.IsEnabled")]
    public bool IsEnabled { get; set; }

    public List<MultiFactorAuthenticationProviderModel> Providers { get; set; }

    public string Message { get; set; }

}