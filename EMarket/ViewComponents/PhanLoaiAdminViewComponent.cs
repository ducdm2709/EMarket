using EMarket.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace EMarket.ViewComponents
{
    public class PhanLoaiAdminViewComponent: ViewComponent
    {
        private readonly EMarketContext db;
        public PhanLoaiAdminViewComponent(EMarketContext context)
        {
            db = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = await GetLoaiAsync();
            return View(list);
        }
        private Task<List<Loai>> GetLoaiAsync()
        {
            var rows = db.Loai.ToListAsync();
            return rows;
        }
    }
}
