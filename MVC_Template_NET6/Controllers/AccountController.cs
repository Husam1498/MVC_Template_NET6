using Microsoft.AspNetCore.Mvc;
using MVC_Template_NET6.Entity;
using MVC_Template_NET6.Models;
using NETCore.Encrypt.Extensions;

namespace MVC_Template_NET6.Controllers
{
    public class AccountController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IConfiguration _configuration;

        public AccountController(DatabaseContext databaseContext, IConfiguration configuration )
        {
            _databaseContext = databaseContext;
            _configuration = configuration;
        }

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

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {               
                User user = new()
                {
                    Fullname = model.Fullname,
                    Username= model.Username,
                    Password= MD5Hashed(model.Password),
                };
                _databaseContext.Users.Add(user);
                int ekleme_islemi= _databaseContext.SaveChanges();
                if (ekleme_islemi == 0)
                {
                    ModelState.AddModelError("", "User eklenemedi");//genel hata
                }
                else
                {
                    return RedirectToAction(nameof(Login));
                }
            }
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        public string MD5Hashed(string sifre)
        {
            string md5Salt = _configuration.GetValue<string>("AppStettings:Md5Salted");
            string saltedPassword = sifre + md5Salt;
            string hashedPassword = saltedPassword.MD5();
            return hashedPassword;
        }

    }
}
