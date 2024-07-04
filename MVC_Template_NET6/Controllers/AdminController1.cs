using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC_Template_NET6.Controllers
{
    [Authorize]
    public class AdminController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
