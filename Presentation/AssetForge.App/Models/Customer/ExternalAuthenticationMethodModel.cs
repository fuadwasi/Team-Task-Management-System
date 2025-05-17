using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Models.Customer;

public partial record ExternalAuthenticationMethodModel : BaseModel
{
    public Type ViewComponent { get; set; }
}