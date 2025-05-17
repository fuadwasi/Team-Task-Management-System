using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Models.Customer;

public partial record GdprConsentModel : BaseEntityModel
{
    public string Message { get; set; }

    public bool IsRequired { get; set; }

    public string RequiredMessage { get; set; }

    public bool Accepted { get; set; }
}