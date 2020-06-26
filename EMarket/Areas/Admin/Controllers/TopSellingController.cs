using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EMarket.Areas.Admin.Models;
using EMarket.Areas.Admin.Filters;

namespace EMarket.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ServiceFilter(typeof(SessionFilter))]
    public class TopSellingController : Controller
    {
        private readonly EMarketContext _context;

        public TopSellingController(EMarketContext context)
        {
            _context = context;
        }

        // GET: Admin/TopSelling
        public async Task<IActionResult> Index()
        {
            var eMarketContext = _context.TopSelling.Include(t => t.HangHoa).OrderBy(p=>p.HangHoaId).Take(5);
            return View(await eMarketContext.ToListAsync());
        }

        // GET: Admin/TopSelling/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topSelling = await _context.DanhGia               
                .FirstOrDefaultAsync(m => m.HangHoaId == id);
            if (topSelling == null)
            {
                return NotFound();
            }

            return View(topSelling);
        }

        // GET: Admin/TopSelling/Create
        public IActionResult Create()
        {
            ViewData["HangHoaId"] = new SelectList(_context.HangHoa, "HangHoaId", "Hinh");
            return View();
        }

        // POST: Admin/TopSelling/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TopSellingId,HangHoaId,SoLan,DanhGia")] TopSelling topSelling)
        {
            if (ModelState.IsValid)
            {
                _context.Add(topSelling);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HangHoaId"] = new SelectList(_context.HangHoa, "HangHoaId", "Hinh", topSelling.HangHoaId);
            return View(topSelling);
        }

        // GET: Admin/TopSelling/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topSelling = await _context.TopSelling.FindAsync(id);
            if (topSelling == null)
            {
                return NotFound();
            }
            ViewData["HangHoaId"] = new SelectList(_context.HangHoa, "HangHoaId", "Hinh", topSelling.HangHoaId);
            return View(topSelling);
        }

        // POST: Admin/TopSelling/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TopSellingId,HangHoaId,SoLan,DanhGia")] TopSelling topSelling)
        {
            if (id != topSelling.TopSellingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(topSelling);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopSellingExists(topSelling.TopSellingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["HangHoaId"] = new SelectList(_context.HangHoa, "HangHoaId", "Hinh", topSelling.HangHoaId);
            return View(topSelling);
        }

        // GET: Admin/TopSelling/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topSelling = await _context.TopSelling
                .Include(t => t.HangHoa)
                .FirstOrDefaultAsync(m => m.TopSellingId == id);
            if (topSelling == null)
            {
                return NotFound();
            }

            return View(topSelling);
        }

        // POST: Admin/TopSelling/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var topSelling = await _context.TopSelling.FindAsync(id);
            _context.TopSelling.Remove(topSelling);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TopSellingExists(int id)
        {
            return _context.TopSelling.Any(e => e.TopSellingId == id);
        }
    }
}
