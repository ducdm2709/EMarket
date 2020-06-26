using EMarket.Areas.Admin.Models;
using EMarket.Areas.Client.Helpers;
using EMarket.Areas.Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMarket.ViewComponents
{
    public class GioHangViewComponent:ViewComponent
    {
        private readonly EMarketContext db;
        public GioHangViewComponent(EMarketContext context)
        {
            db = context;
        }
        public Task<IViewComponentResult> InvokeAsync()
        {
            List<GioHang> list = GetGioHangAsync();
            if (list == null) { list = new List<GioHang>(); }
            GioHangViewModel giohang = new GioHangViewModel() { GioHang = list };
            return Task.FromResult<IViewComponentResult>(View(giohang));
        }
        private List<GioHang> GetGioHangAsync()
        {
            List<GioHang> giohang = new List<GioHang>();
            giohang = SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "cart");
            return giohang;
        }
    }
}
