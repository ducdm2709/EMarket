using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EMarket.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;

namespace EMarket.ViewComponents
{
    public class TheLoaiHangHoaViewComponent : ViewComponent
    {
        private readonly EMarketContext db;
        public TheLoaiHangHoaViewComponent(EMarketContext context) {
            db = context;
        }
        public async Task<IViewComponentResult> InvokeAsync() {
            var rows = await GetLoaiAsync();
            return View(rows);
        }
        public Task<List<Loai>> GetLoaiAsync(){
            return db.Loai.ToListAsync();
        }
    }
}
