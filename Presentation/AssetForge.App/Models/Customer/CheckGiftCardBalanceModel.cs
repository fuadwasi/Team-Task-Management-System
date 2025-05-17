using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Models.Customer;

public partial record CheckGiftCardBalanceModel : BaseModel
{
    public string Result { get; set; }

    public string Message { get; set; }

    [ResourceDisplayName("ShoppingCart.GiftCardCouponCode.Tooltip")]
    public string GiftCardCode { get; set; }
}