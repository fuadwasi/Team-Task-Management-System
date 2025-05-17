//using AssetForge.App.Factories;
//using AssetForge.Web.Framework.Components;
//using Microsoft.AspNetCore.Mvc;

//namespace AssetForge.App.Components
//{
//    public class TopMenuViewComponent : AssetForgeViewComponent
//    {
//        private readonly ICommonModelFactory _commonModelFactory;

//        public TopMenuViewComponent(ICommonModelFactory commonModelFactory)
//        {
//            _commonModelFactory = commonModelFactory;
//        }

//        public async Task<IViewComponentResult> InvokeAsync()
//        {
//            var model = await _commonModelFactory.PrepareTopMenuModelAsync();
//            return View(model);
//        }
//    }
//}
