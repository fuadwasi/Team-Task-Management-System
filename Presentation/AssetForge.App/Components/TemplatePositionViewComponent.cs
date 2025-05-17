//using AssetForge.App.Factories;
//using AssetForge.App.Models.Pages;
//using AssetForge.App.Models.Widgets;
//using AssetForge.Core;
//using AssetForge.Services.Cms;
//
//using AssetForge.Web.Framework.Components;
//using Microsoft.AspNetCore.Mvc;

//namespace AssetForge.App.Components
//{
//    public class TemplatePositionViewComponent : AssetForgeViewComponent
//    {
//        private readonly ILayoutService _layoutService;
//        private readonly IPageModelFactory _pageModelFactory;
//        private readonly IPageService _pageService;
//        private readonly ISiteContext _siteContext;
//        private readonly IWidgetZoneService _widgetZoneService;

//        public TemplatePositionViewComponent(ILayoutService layoutService,
//            IPageModelFactory pageModelFactory,
//            IPageService pageService,
//            ISiteContext siteContext,
//            IWidgetZoneService widgetZoneService)
//        {
//            _layoutService = layoutService;
//            _pageModelFactory = pageModelFactory;
//            _pageService = pageService;
//            _siteContext = siteContext;
//            _widgetZoneService = widgetZoneService;
//        }

//        public async Task<IViewComponentResult> InvokeAsync(string widgetPosition, object additionalData = null)
//        {
//            var pageModel = additionalData as PageModel;
//            var site = await _siteContext.GetCurrentSiteAsync();
//            var layout = await _layoutService.GetLayoutByIdAsync(pageModel.LayoutId);
//            if (layout == null)
//                return Content("");

//            var widgetZoneInstanceIds = (await _layoutService.GetWidgetInstanceLayoutMapsByLayoutAndPositionAsync(layout, widgetPosition)).Select(lw => lw.WidgetZoneInstanceId).Distinct();
//            if (!widgetZoneInstanceIds.Any())
//                return Content("");

//            var templatePageListModel = new TemplatePageListModel();
//            foreach ( var widgetZoneInstanceId in widgetZoneInstanceIds)
//            {
//                var widget = await _widgetZoneService.GetWidgetZoneByInstanceIdAsync(widgetZoneInstanceId);
//                if(widget != null) templatePageListModel.ComponantList.Add(new WidgetComponentModel()
//                {
//                    ComponantName = widget.SystemName,
//                    WidgetZoneInstanceId = widgetZoneInstanceId,

//                });
//            }

//            return View(templatePageListModel);
//        }
//    }
//}
