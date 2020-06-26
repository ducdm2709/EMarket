using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMarket.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;

namespace EMarket.ViewComponents
{
    public class DanhSachNCCViewComponent:ViewComponent
    {
        private readonly EMarketContext db;
        public DanhSachNCCViewComponent(EMarketContext context) {
            db = context;
        }
        public async Task<IViewComponentResult> InvokeAsync() {
            var list = await GetNhaCungCapAsync();
            return View(list);
        }
        private Task<List<NhaCungCap>> GetNhaCungCapAsync() {
            var rows = db.NhaCungCap.ToListAsync();
            return rows;
        }
    }
}
