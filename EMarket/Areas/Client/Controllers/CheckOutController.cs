using System;
using System.Collections.Generic;
using System.Linq;
using EMarket.Areas.Admin.Models;
using EMarket.Areas.Client.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EMarket.Areas.Client.Controllers
{
    [Area("Client")]
    public class CheckOutController : Controller
    {
        private readonly EMarketContext _context;

        public CheckOutController(EMarketContext context)
        {
            _context = context;
        }

        public IActionResult Index(string danhsach)
        {
            //if (String.IsNullOrEmpty(danhsach))
            //{
            //    TempData["status"] = "Không có sản phẩm nào được chọn";
            //    return RedirectToAction("Index", "HangHoa");
            //}
            List<GioHang> danhsachhang = JsonConvert.DeserializeObject<List<GioHang>>(danhsach);
            if (danhsachhang.Count() == 0)
            {
                TempData["status"] = "Không có sản phẩm nào được chọn";
                return RedirectToAction("Index", "HangHoa");
            }
            if (HttpContext.Session.GetString("User") == null || HttpContext.Session.GetString("User") == "")
            {
                TempData["status"] = "Bạn Cần Phải Đăng Nhập";
                return RedirectToAction("Index", "HangHoa");
            }
            GioHangViewModel list = new GioHangViewModel { GioHang = danhsachhang };
            return View(list);
        }
        
    }
}