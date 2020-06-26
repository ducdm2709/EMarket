using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EMarket.Areas.Admin.Models;
using EMarket.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
namespace EMarket.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly EMarketContext _context;

        public LoginController(EMarketContext context) {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel user)
        {
            var result = _context.TaiKhoan.Where(p => p.UserName == user.Username).SingleOrDefault();
            if (result != null)
            {
                if (result.Password == Encryptor.MD5Hash(user.Password))
                {
                    if (result.LoaiTaiKhoan == true) {
                        ViewData["Error"] = "Bạn Không Có Quyền Admin";
                        return View("Index");
                    }
                    HttpContext.Session.SetString("User",result.UserName);
                    return RedirectToAction("Index", "Base");
                }
                else
                {
                    ViewData["Error"] = "Mật Khẩu Không Chính Xác";
                    return View("Index");
                }
            }
            else
            {
                ViewData["Error"] = "Tài Khoản Không Tồn Tại";
                return View("Index");
            }
        }

        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("UserName,Email,Password")] TaiKhoan register,string repassword )
        {
            var check = _context.TaiKhoan.Where(p => p.UserName == register.UserName).FirstOrDefault();
            if (check != null) { ViewData["RegisterError"] = "Tài Khoản Đã Tồn Tại"; return View("Register"); }
            register.LoaiTaiKhoan = false;
            register.NgayDk = DateTime.Now;

            if (ModelState.IsValid) {
                if (repassword == register.Password)
                {
                    register.Password = Encryptor.MD5Hash(register.Password);
                    _context.Add(register);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["RegisterError"] = "Mật Khẩu không Khớp";
                    return View("Register");                        
                }
            }
            return View("Index");
        }
    }
}