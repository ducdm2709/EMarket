using EMarket.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMarket.ViewComponents
{
    public class TopSellingViewComponent:ViewComponent
    {
        private readonly EMarketContext db;
        public TopSellingViewComponent(EMarketContext context)
        {
            db = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var rows = await GetTop3Async();
            return View(rows);
        }

        public Task<List<HangHoa>> GetTop3Async()
        {
            //var list = db.TopSelling.Take(3).ToListAsync();
            var list = db.TopSelling.Join(db.HangHoa, top => top.HangHoaId, inner => inner.HangHoaId, (top, inner) => inner).Take(3).ToListAsync();         
            return list;
        }
    }
}
