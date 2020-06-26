using EMarket.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMarket.ViewComponents
{
    public class NewArrivalsViewComponent:ViewComponent
    {
        private readonly EMarketContext db;
        public NewArrivalsViewComponent(EMarketContext context)
        {
            db = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = await GetNhaCungCapAsync();
            ViewBag.Checkavailable = db.KhoHang.Select(p => p.HangHoaId).ToList();
            return View(list);
        }
        private Task<List<HangHoa>> GetNhaCungCapAsync()
        {
            var rows = db.HangHoa.Include(p=>p.NhaCungCap).Include(p=>p.Loai).OrderByDescending(p => p.HangHoaId).Take(5).ToListAsync();
            return rows;
        }
    }
}
