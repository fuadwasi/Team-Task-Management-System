using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Models.Customer;

public partial record MultiFactorAuthenticationProviderModel : BaseEntityModel
{
    public bool Selected { get; set; }

    public string Name { get; set; }

    public string SystemName { get; set; }

    public string LogoUrl { get; set; }

    public string Description { get; set; }

    public Type ViewComponent { get; set; }
}