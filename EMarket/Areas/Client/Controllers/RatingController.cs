using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMarket.Areas.Admin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMarket.Areas.Client.Controllers
{
    [Area("Client")]
    public class RatingController : Controller
    {
        private readonly EMarketContext eMarketContext;

        public RatingController(EMarketContext context)
        {
            eMarketContext = context;
        }
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult AddNewRating(int id,string comment,int rating)
        {
            var username = HttpContext.Session.GetString("User");
            int ratingid = id;
            if (username == null || username == "") {
                TempData["loginError"] = "Bạn cần đăng nhập để sử dụng chức năng này";                
                return RedirectToAction("Details", "HangHoa", new { id = id });
            }
            var user = eMarketContext.TaiKhoan.Where(p => p.UserName == username).FirstOrDefault();
            var newRating = new DanhGia();
            newRating.Comment = comment;
            newRating.HangHoaId = id;
            newRating.TaiKhoanId = user.TaiKhoanId;
            newRating.Rating = rating;
            eMarketContext.Add(newRating);
            eMarketContext.SaveChanges();
            return RedirectToAction("Index", "HangHoa");
        }
    }
}