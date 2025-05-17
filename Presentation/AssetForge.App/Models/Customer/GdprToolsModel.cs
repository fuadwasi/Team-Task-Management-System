using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Models.Customer;

public partial record GdprToolsModel : BaseModel
{
    public string Result { get; set; }
}