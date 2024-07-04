using Microsoft.AspNetCore.Mvc;
using MVC_Template_NET6.Models;

namespace MVC_Template_NET6.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                //Login işlemleri
            }
            return View(model);
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
    }
}
