using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMarket.Areas.Admin.Filters;
using EMarket.Areas.Admin.Models;
using EMarket.Areas.Admin.Models.ThongKe;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMarket.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ServiceFilter(typeof(SessionFilter))]
    public class ThongKeController : Controller
    {
        private readonly EMarketContext _context;
        public ThongKeController(EMarketContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var report = await _context.HoaDon
                            .Join(_context.ChiTietHoaDon, hd => hd.HoaDonId, ct => ct.HoaDonId, (hd, ct) => new ThongKeTheoThang { Thang = hd.NgayLapHoaDon.Month, TongTien = ct.TongTien })
                            .GroupBy(p => p.Thang)
                            .Select(p => p.First())
                            .ToListAsync();
            return View(report);
        }
        public async Task<IActionResult> ThongKeTheoThang()
        {
            var report = await _context.HoaDon
                            .Join(_context.ChiTietHoaDon, hd => hd.HoaDonId, ct => ct.HoaDonId, (hd, ct) => new ThongKeTheoThang { Thang = hd.NgayLapHoaDon.Month, TongTien = ct.TongTien })
                            .GroupBy(p=>p.Thang)
                            .Select(p=>p.First())
                            .ToListAsync();
            return View(report);
        }
    }
}