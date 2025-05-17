using AssetForge.App.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace AssetForge.Web.Controllers;

public partial class HomeController : BasePublicController
{
    public virtual IActionResult Index()
    {
        return View();
    }
}