using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMarket.Areas.Admin.Models;
using EMarket.Areas.Client.Helpers;
using EMarket.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EMarket.Areas.Client.Controllers
{
    [Area("Client")]
    public class LoginController : Controller
    {

        private readonly EMarketContext _context;
        private readonly ILogger<LoginController> _logger;

        public LoginController(EMarketContext context, ILogger<LoginController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetString("User", "");
            return RedirectToAction("Index", "HangHoa");
        }
        
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel user)
        {
            string license = ValidateUserInfomation(user);

            switch (license) {
                case "passed":
                    string username = user.Username;
                    _logger.LogInformation("Logged " + user.Username);
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "User", username);
                    return RedirectToAction("Index", "HangHoa");
                case "unknown":
                    ViewData["Error"] = "Tài khoản không tồn tại";
                    return View("Index");
                case "failed":
                    ViewData["Error"] = "Thông tin đăng nhập Không Chính Xác";
                    return View("Index");
                default:
                    _logger.LogWarning("Unknown condition detected in the login function with license: " + license);
                    ViewData["Error"] = "Thông tin đăng nhập Không Chính Xác";
                    return View("Index");
            }
        }

        private string ValidateUserInfomation(LoginViewModel user)
        {
            var userInDB = _context.TaiKhoan.Where(p => p.UserName == user.Username).FirstOrDefault();
            if (userInDB != null)
            {
                return (userInDB.Password == Encryptor.MD5Hash(user.Password)) ? "passed" : "failed";                
            }
            else
            {
                return "unknown";
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("UserName,Email,Password")] TaiKhoan register, string repassword)
        {
            if (UserIsExisting(register.UserName))
            {
                ViewData["RegisterError"] = "Tài Khoản Đã Tồn Tại";
                return View("Register");
            }
            else
            {

                register.LoaiTaiKhoan = true;
                register.NgayDk = DateTime.Now;

                if (ModelState.IsValid)
                {
                    if (repassword == register.Password)
                    {
                        register.Password = Encryptor.MD5Hash(register.Password);
                        _context.Add(register);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewData["RegisterError"] = "Thông tin mật khẩu không chính xác";
                        return View("Register");
                    }
                }

                return View("Index");
            }
        }

        private bool UserIsExisting(string userName)
        {
            var userInDB = _context.TaiKhoan.Where(p => p.UserName == userName).FirstOrDefault();
            return (userInDB != null) ? true : false;
        }

        [HttpGet]
        public IActionResult ChangeInfo()
        {
            var user = HttpContext.Session.GetString("User");
            var taikhoan = _context.ThongTinTaiKhoan.Include(p => p.TaiKhoan).Where(p => p.TaiKhoan.UserName == user).FirstOrDefault();
            if (taikhoan == null) taikhoan = new ThongTinTaiKhoan();
            return View(taikhoan);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeInfo([Bind("ThongTinTaiKhoanId","TaiKhoanId","HoVaTen","NgaySinh","Sdt","DiaChi")] ThongTinTaiKhoan info)
        {
            _context.Update(info);
            _context.SaveChanges();
            return RedirectToAction("Index", "HangHoa");
        }
    
    }
}