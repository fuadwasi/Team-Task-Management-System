using AssetForge.App.Areas.Admin.Models.Customers;
using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Models.Customer
{
    public record CustomerListTeamModel : BaseModel
    {
        public CustomerListTeamModel()
        {
            Customers = new List<CustomerModel>();
        }
        public IList<CustomerModel> Customers { get; set; }
    }
}
