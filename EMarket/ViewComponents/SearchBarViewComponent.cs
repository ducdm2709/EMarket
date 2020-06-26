using EMarket.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMarket.ViewComponents
{
    public class SearchBarViewComponent:ViewComponent
    {
        private readonly EMarketContext db;
        public SearchBarViewComponent(EMarketContext context)
        {
            db = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var rows = await GetLoaiAsync();
            return View(rows);
        }
        public Task<List<Loai>> GetLoaiAsync()
        {
            return db.Loai.ToListAsync();
        }
    }
}
