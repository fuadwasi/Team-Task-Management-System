using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Areas.Admin.Models.Common;

public partial record CommonStatisticsModel : BaseModel
{
    public int NumberOfOrders { get; set; }

    public int NumberOfCustomers { get; set; }

    public int NumberOfPendingReturnRequests { get; set; }

    public int NumberOfLowStockProducts { get; set; }
}