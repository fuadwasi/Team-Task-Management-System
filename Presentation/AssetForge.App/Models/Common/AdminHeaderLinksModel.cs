using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Models.Common;

public partial record AdminHeaderLinksModel : BaseModel
{
    public string ImpersonatedCustomerName { get; set; }
    public bool IsCustomerImpersonated { get; set; }
    public bool DisplayAdminLink { get; set; }
    public string EditPageUrl { get; set; }
}