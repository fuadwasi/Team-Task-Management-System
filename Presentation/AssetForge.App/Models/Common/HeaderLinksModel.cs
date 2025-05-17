using AssetForge.Core.Domain.Customers;
using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Models.Common;

public partial record HeaderLinksModel : BaseModel
{
    public bool IsAuthenticated { get; set; }
    public string CustomerName { get; set; }

    //public bool ShoppingCartEnabled { get; set; }
    //public int ShoppingCartItems { get; set; }

    //public bool WishlistEnabled { get; set; }
    //public int WishlistItems { get; set; }

    public bool AllowPrivateMessages { get; set; }
    public string UnreadPrivateMessages { get; set; }
    public string AlertMessage { get; set; }
    public UserRegistrationType RegistrationType { get; set; }
}