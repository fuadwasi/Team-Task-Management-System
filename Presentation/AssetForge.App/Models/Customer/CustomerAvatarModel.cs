using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Models.Customer;

public partial record CustomerAvatarModel : BaseModel
{
    public string AvatarUrl { get; set; }
}