using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Models.Customer;

public partial record EmailRevalidationModel : BaseModel
{
    public string Result { get; set; }

    public string ReturnUrl { get; set; }
}