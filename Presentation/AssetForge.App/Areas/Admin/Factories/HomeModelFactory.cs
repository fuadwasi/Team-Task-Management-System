using AssetForge.App.Areas.Admin.Models.Common;
using AssetForge.App.Areas.Admin.Models.Home;
using AssetForge.Core;
using AssetForge.Core.Caching;
using AssetForge.Core.Domain.Common;
using AssetForge.Services.Common;
using AssetForge.Services.Configuration;
using AssetForge.Services.Localization;
using AssetForge.Web.Framework.Models.DataTables;
using ILogger = AssetForge.Services.Logging.ILogger;

namespace AssetForge.App.Areas.Admin.Factories;

/// <summary>
/// Represents the home models factory implementation
/// </summary>
public partial class HomeModelFactory : IHomeModelFactory
{
    #region Fields

    protected readonly AdminAreaSettings _adminAreaSettings;
    protected readonly ICommonModelFactory _commonModelFactory;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILogger _logger;
    //protected readonly IOrderModelFactory _orderModelFactory;
    protected readonly ISettingService _settingService;
    protected readonly IStaticCacheManager _staticCacheManager;
    protected readonly IWorkContext _workContext;
    protected readonly AssetForgeHttpClient _ixHttpClient;

    #endregion

    #region Ctor

    public HomeModelFactory(AdminAreaSettings adminAreaSettings,
        ICommonModelFactory commonModelFactory,
        ILocalizationService localizationService,
        ILogger logger,
        //IOrderModelFactory orderModelFactory,
        ISettingService settingService,
        IStaticCacheManager staticCacheManager,
        IWorkContext workContext,
        AssetForgeHttpClient ixHttpClient)
    {
        _adminAreaSettings = adminAreaSettings;
        _commonModelFactory = commonModelFactory;
        _localizationService = localizationService;
        _logger = logger;
        //_orderModelFactory = orderModelFactory;
        _settingService = settingService;
        _staticCacheManager = staticCacheManager;
        _workContext = workContext;
        _ixHttpClient = ixHttpClient;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Prepare dashboard model
    /// </summary>
    /// <param name="model">Dashboard model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the dashboard model
    /// </returns>
    public virtual async Task<DashboardModel> PrepareDashboardModelAsync(DashboardModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        return model;
    }

    /// <summary>
    /// Prepare popular search term report model
    /// </summary>
    /// <param name="model">DataTables model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the dashboard model
    /// </returns>
    public virtual async Task<DataTablesModel> PreparePopularSearchTermReportModelAsync(DataTablesModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var searchModel = new PopularSearchTermSearchModel();
        searchModel = await _commonModelFactory.PreparePopularSearchTermSearchModelAsync(searchModel);

        model.Name = "search-term-report-grid";
        model.UrlRead = new DataUrl("PopularSearchTermsReport", "Common", null);
        model.Length = searchModel.Length;
        model.LengthMenu = searchModel.AvailablePageSizes;
        model.Dom = "<'row'<'col-md-12't>>" +
                    "<'row margin-t-5'" +
                    "<'col-lg-10 col-xs-12'<'float-lg-left'p>>" +
                    "<'col-lg-2 col-xs-12'<'float-lg-right text-center'i>>" +
                    ">";
        model.ColumnCollection = new List<ColumnProperty>
        {
            new(nameof(PopularSearchTermModel.Keyword))
            {
                Title = await _localizationService.GetResourceAsync("Admin.SearchTermReport.Keyword")
            },
            new(nameof(PopularSearchTermModel.Count))
            {
                Title = await _localizationService.GetResourceAsync("Admin.SearchTermReport.Count")
            }
        };

        return model;
    }

    /// <summary>
    /// Prepare bestsellers brief by amount report model
    /// </summary>
    /// <param name="model">DataTables model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the dashboard model
    /// </returns>
    //public virtual async Task<DataTablesModel> PrepareBestsellersBriefReportByAmountModelAsync(DataTablesModel model)
    //{
    //    ArgumentNullException.ThrowIfNull(model);

    //    var searchModel = new BestsellerBriefSearchModel();
    //    searchModel = await _orderModelFactory.PrepareBestsellerBriefSearchModelAsync(searchModel);

    //    model.Name = "bestsellers-byamount-grid";
    //    model.UrlRead = new DataUrl("BestsellersBriefReportByAmountList", "Order", new RouteValueDictionary { [nameof(searchModel.OrderBy)] = OrderByEnum.OrderByTotalAmount });
    //    model.Length = searchModel.PageSize;
    //    model.Dom = "<'row'<'col-md-12't>>" +
    //                "<'row margin-t-5'" +
    //                "<'col-lg-10 col-xs-12'<'float-lg-left'p>>" +
    //                "<'col-lg-2 col-xs-12'<'float-lg-right text-center'i>>" +
    //                ">";
    //    model.ColumnCollection = new List<ColumnProperty>
    //    {
    //        new(nameof(BestsellerModel.ProductName))
    //        {
    //            Title = await _localizationService.GetResourceAsync("Admin.Reports.Sales.Bestsellers.Fields.Name")
    //        },
    //        new(nameof(BestsellerModel.TotalQuantity))
    //        {
    //            Title = await _localizationService.GetResourceAsync("Admin.Reports.Sales.Bestsellers.Fields.TotalQuantity")
    //        },
    //        new(nameof(BestsellerModel.TotalAmount))
    //        {
    //            Title = await _localizationService.GetResourceAsync("Admin.Reports.Sales.Bestsellers.Fields.TotalAmount")
    //        },
    //        new(nameof(BestsellerModel.ProductId))
    //        {
    //            Title = await _localizationService.GetResourceAsync("Admin.Common.View"),
    //            Width = "80",
    //            ClassName = AssetForgeColumnClassDefaults.Button,
    //            Render = new RenderButtonView(new DataUrl("~/Admin/Product/Edit/"))
    //        }
    //    };

    //    return model;
    //}

    /// <summary>
    /// Prepare bestsellers brief by quantity report model
    /// </summary>
    /// <param name="model">DataTables model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the dashboard model
    /// </returns>
    //public virtual async Task<DataTablesModel> PrepareBestsellersBriefReportByQuantityModelAsync(DataTablesModel model)
    //{
    //    ArgumentNullException.ThrowIfNull(model);

    //    var searchModel = new BestsellerBriefSearchModel();
    //    searchModel = await _orderModelFactory.PrepareBestsellerBriefSearchModelAsync(searchModel);

    //    model.Name = "bestsellers-byquantity-grid";
    //    model.UrlRead = new DataUrl("BestsellersBriefReportByQuantityList", "Order", new RouteValueDictionary { [nameof(searchModel.OrderBy)] = OrderByEnum.OrderByQuantity });
    //    model.Length = searchModel.PageSize;
    //    model.Dom = "<'row'<'col-md-12't>>" +
    //                "<'row margin-t-5'" +
    //                "<'col-lg-10 col-xs-12'<'float-lg-left'p>>" +
    //                "<'col-lg-2 col-xs-12'<'float-lg-right text-center'i>>" +
    //                ">";
    //    model.ColumnCollection = new List<ColumnProperty>
    //    {
    //        new(nameof(BestsellerModel.ProductName))
    //        {
    //            Title = await _localizationService.GetResourceAsync("Admin.Reports.Sales.Bestsellers.Fields.Name")
    //        },
    //        new(nameof(BestsellerModel.TotalQuantity))
    //        {
    //            Title = await _localizationService.GetResourceAsync("Admin.Reports.Sales.Bestsellers.Fields.TotalQuantity")
    //        },
    //        new(nameof(BestsellerModel.TotalAmount))
    //        {
    //            Title = await _localizationService.GetResourceAsync("Admin.Reports.Sales.Bestsellers.Fields.TotalAmount")
    //        },
    //        new(nameof(BestsellerModel.ProductId))
    //        {
    //            Title = await _localizationService.GetResourceAsync("Admin.Common.View"),
    //            Width = "80",
    //            ClassName = AssetForgeColumnClassDefaults.Button,
    //            Render = new RenderButtonView(new DataUrl("~/Admin/Product/Edit/"))
    //        }
    //    };

    //    return model;
    //}


    /// <summary>
    /// Prepare latest orders model
    /// </summary>
    /// <param name="model">DataTables model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the dashboard model
    /// </returns>
    //public virtual async Task<DataTablesModel> PrepareLatestOrdersModelAsync(DataTablesModel model)
    //{
    //    ArgumentNullException.ThrowIfNull(model);

    //    model.Name = "orders-grid";
    //    model.UrlRead = new DataUrl("OrderList", "Order", null);
    //    model.Length = 5;
    //    model.Dom = "<'row'<'col-md-12't>>" +
    //                "<'row margin-t-5'" +
    //                "<'col-lg-10 col-xs-12'<'float-lg-left'p>>" +
    //                "<'col-lg-2 col-xs-12'<'float-lg-right text-center'i>>" +
    //                ">";
    //    model.ColumnCollection = new List<ColumnProperty>
    //    {
    //        new(nameof(OrderModel.CustomOrderNumber))
    //        {
    //            Title = await _localizationService.GetResourceAsync("Admin.Orders.Fields.CustomOrderNumber"),
    //            Width = "80"
    //        },
    //        new(nameof(OrderModel.OrderStatus))
    //        {
    //            Title = await _localizationService.GetResourceAsync("Admin.Orders.Fields.OrderStatus"),
    //            Width = "100",
    //            Render = new RenderCustom("renderColumnOrderStatus")
    //        },
    //        new(nameof(OrderModel.CustomerEmail))
    //        {
    //            Title = await _localizationService.GetResourceAsync("Admin.Orders.Fields.Customer"),
    //            Width = "250",
    //            Render = new RenderCustom("renderColumnCustomerEmail")
    //        },
    //        new(nameof(OrderModel.CreatedOn))
    //        {
    //            Title = await _localizationService.GetResourceAsync("Admin.Orders.Fields.CreatedOn"),
    //            Width = "100",
    //            Render = new RenderDate()
    //        },
    //        new(nameof(OrderModel.Id))
    //        {
    //            Title = await _localizationService.GetResourceAsync("Admin.Common.View"),
    //            Width = "50",
    //            ClassName = AssetForgeColumnClassDefaults.Button,
    //            Render = new RenderButtonView(new DataUrl("~/Admin/Order/Edit/"))
    //        }
    //    };

    //    return model;
    //}

    /// <summary>
    /// Prepare incomplete orders report model
    /// </summary>
    /// <param name="model">DataTables model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the dashboard model
    /// </returns>
    //public virtual async Task<DataTablesModel> PrepareOrderIncompleteModelAsync(DataTablesModel model)
    //{
    //    ArgumentNullException.ThrowIfNull(model);

    //    model.Name = "incomplete-order-report-grid";
    //    model.UrlRead = new DataUrl("OrderIncompleteReportList", "Order", null);
    //    model.Length = int.MaxValue;
    //    model.Paging = false;
    //    model.Info = false;
    //    model.ColumnCollection = new List<ColumnProperty>
    //    {
    //        new(nameof(OrderIncompleteReportModel.Item))
    //        {
    //            Title = await _localizationService.GetResourceAsync("Admin.SalesReport.Incomplete.Item")
    //        },
    //        new(nameof(OrderIncompleteReportModel.Total))
    //        {
    //            Title = await _localizationService.GetResourceAsync("Admin.SalesReport.Incomplete.Total"),
    //            Width = "50"
    //        },
    //        new(nameof(OrderIncompleteReportModel.Count))
    //        {
    //            Title = await _localizationService.GetResourceAsync("Admin.SalesReport.Incomplete.Count"),
    //            Width = "120",
    //            ClassName =  AssetForgeColumnClassDefaults.Button,
    //            Render = new RenderCustom("renderColumnOrderIncompleteReportCount")
    //        }
    //    };

    //    return model;
    //}

    /// <summary>
    /// Prepare order average report model
    /// </summary>
    /// <param name="model">DataTables model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the dashboard model
    /// </returns>
    //public virtual async Task<DataTablesModel> PrepareOrderAverageModelAsync(DataTablesModel model)
    //{
    //    ArgumentNullException.ThrowIfNull(model);

    //    model.Name = "average-order-report-grid";
    //    model.UrlRead = new DataUrl("OrderAverageReportList", "Order", null);
    //    model.Length = int.MaxValue;
    //    model.Paging = false;
    //    model.Info = false;
    //    model.ColumnCollection = new List<ColumnProperty>
    //    {
    //        new(nameof(OrderAverageReportModel.OrderStatus))
    //        {
    //            Title = await _localizationService.GetResourceAsync("Admin.SalesReport.Average.OrderStatus")
    //        },
    //        new(nameof(OrderAverageReportModel.SumTodayOrders))
    //        {
    //            Title = await _localizationService.GetResourceAsync("Admin.SalesReport.Average.SumTodayOrders")
    //        },
    //        new(nameof(OrderAverageReportModel.SumThisWeekOrders))
    //        {
    //            Title = await _localizationService.GetResourceAsync("Admin.SalesReport.Average.SumThisWeekOrders")
    //        },
    //        new(nameof(OrderAverageReportModel.SumThisMonthOrders))
    //        {
    //            Title = await _localizationService.GetResourceAsync("Admin.SalesReport.Average.SumThisMonthOrders")
    //        },
    //        new(nameof(OrderAverageReportModel.SumThisYearOrders))
    //        {
    //            Title = await _localizationService.GetResourceAsync("Admin.SalesReport.Average.SumThisYearOrders")
    //        },
    //        new(nameof(OrderAverageReportModel.SumAllTimeOrders))
    //        {
    //            Title = await _localizationService.GetResourceAsync("Admin.SalesReport.Average.SumAllTimeOrders")
    //        }
    //    };

    //    return model;
    //}

    /// <summary>
    /// Prepare ixCms news model
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the ixCms news model
    /// </returns>
    //public virtual async Task<AssetForgeCommerceNewsModel> PrepareAssetForgeCommerceNewsModelAsync()
    //{
    //    var model = new AssetForgeCommerceNewsModel
    //    {
    //        HideAdvertisements = _adminAreaSettings.HideAdvertisementsOnAdminArea
    //    };

    //    try
    //    {
    //        //try to get news RSS feed
    //        var rssData = await _staticCacheManager.GetAsync(_staticCacheManager.PrepareKeyForDefaultCache(AssetForgeModelCacheDefaults.OfficialNewsModelKey), async () =>
    //        {
    //            try
    //            {
    //                return await _ixHttpClient.GetNewsRssAsync();
    //            }
    //            catch (AggregateException exception)
    //            {
    //                //rethrow actual excepion
    //                throw exception.InnerException;
    //            }
    //        });

    //        for (var i = 0; i < rssData.Items.Count; i++)
    //        {
    //            var item = rssData.Items.ElementAt(i);
    //            var newsItem = new AssetForgeCommerceNewsDetailsModel
    //            {
    //                Title = item.TitleText,
    //                Summary = XmlHelper.XmlDecode(item.Content?.Value ?? string.Empty),
    //                Url = item.Url.OriginalString,
    //                PublishDate = item.PublishDate
    //            };
    //            model.Items.Add(newsItem);

    //            //has new items?
    //            if (i != 0)
    //                continue;

    //            var firstRequest = string.IsNullOrEmpty(_adminAreaSettings.LastNewsTitleAdminArea);
    //            if (_adminAreaSettings.LastNewsTitleAdminArea == newsItem.Title)
    //                continue;

    //            _adminAreaSettings.LastNewsTitleAdminArea = newsItem.Title;
    //            await _settingService.SaveSettingAsync(_adminAreaSettings);

    //            //new item
    //            if (!firstRequest)
    //                model.HasNewItems = true;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        await _logger.ErrorAsync("No access to the news. Website www.radiowavehub.store is not available.", ex);
    //    }

    //    return model;
    //}

    #endregion
}