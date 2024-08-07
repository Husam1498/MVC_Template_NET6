﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_Template_NET6.Entity;
using MVC_Template_NET6.Models;
using NETCore.Encrypt.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

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
                User user=_databaseContext.Users.SingleOrDefault(x=> x.Username.ToLower()==model.Username.ToLower() && x.Password==MD5Hashed(model.Password));
                if (user != null)
                {
                    if (user.Locked)
                    {
                        ModelState.AddModelError("", "Giriş başarısız aktif üyeliğiniz bitmiştir, müşteri temsilcisi ile göürüşünüz");
                        return View(model);
                    }

                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim("Id", user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, user.Fullname));
                    claims.Add(new Claim("Username", user.Username.ToString()));
                    claims.Add(new Claim(ClaimTypes.Role, user.Role));

                    ClaimsIdentity identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal);
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("", "Giriş başarısız bilgilerini kontrol et lütfen");
                }
            
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
                if(_databaseContext.Users.Any(x=> x.Username.ToLower()==model.Username.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.Username), "User eklenemedi kullanıcı var");
                    return View(model);

                }
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
            return View(model);
        }

        [Authorize]
        public IActionResult Profile()
        {
           Guid userId = new Guid(User.FindFirstValue("Id"));
            User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userId);
            ViewData["Fullname"] = user.Fullname;

            return View();
        }

        [HttpPost]
        public IActionResult ProfileChangeFullname([Required][StringLength(50)] string fullname)
        {

            if (ModelState.IsValid)
            {
                Guid userId = new Guid(User.FindFirstValue("Id"));
                User user= _databaseContext.Users.SingleOrDefault(x=> x.Id==userId);

                user.Fullname= fullname;
                _databaseContext.SaveChanges();
                return RedirectToAction(nameof(Profile));

            }


            return View(nameof(Profile));
        }
        [HttpPost]
        public IActionResult ProfileChangePassword([Required][MinLength(4)] [MaxLength(8)] string password)
        {

            if (ModelState.IsValid)
            {
                Guid userId = new Guid(User.FindFirstValue("Id"));
                User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userId);

                user.Password = MD5Hashed(password);
                _databaseContext.SaveChanges();

                ViewData["result"] = "PasswordChange";

            }


            return View(nameof(Profile));
        }

        [Authorize]
        public IActionResult Logout()
        {

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
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
