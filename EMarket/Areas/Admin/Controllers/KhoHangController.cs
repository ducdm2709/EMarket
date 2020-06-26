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
    public class KhoHangController : Controller
    {
        private readonly EMarketContext _context;

        public KhoHangController(EMarketContext context)
        {
            _context = context;
        }

        // GET: Admin/KhoHang
        public async Task<IActionResult> Index()
        {
            var eMarketContext = _context.KhoHang.Include(k => k.HangHoa);
            return View(await eMarketContext.ToListAsync());
        }

        // GET: Admin/KhoHang/Create
        public IActionResult Create()
        {
            ViewData["HangHoaId"] = new SelectList(_context.HangHoa, "HangHoaId", "TenHangHoa");
            return View();
        }

        // POST: Admin/KhoHang/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KhoHangId,SoLuong,HangHoaId")] KhoHang khoHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(khoHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HangHoaId"] = new SelectList(_context.HangHoa, "HangHoaId", "Hinh", khoHang.HangHoaId);
            return View(khoHang);
        }

        // GET: Admin/KhoHang/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khoHang = await _context.KhoHang.FindAsync(id);
            if (khoHang == null)
            {
                return NotFound();
            }
            ViewData["HangHoaId"] = new SelectList(_context.HangHoa, "HangHoaId", "Hinh", khoHang.HangHoaId);
            return View(khoHang);
        }

        // POST: Admin/KhoHang/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KhoHangId,SoLuong,HangHoaId")] KhoHang khoHang)
        {
            if (id != khoHang.KhoHangId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(khoHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhoHangExists(khoHang.KhoHangId))
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
            ViewData["HangHoaId"] = new SelectList(_context.HangHoa, "HangHoaId", "Hinh", khoHang.HangHoaId);
            return View(khoHang);
        }

        // GET: Admin/KhoHang/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khoHang = await _context.KhoHang
                .Include(k => k.HangHoa)
                .FirstOrDefaultAsync(m => m.KhoHangId == id);
            if (khoHang == null)
            {
                return NotFound();
            }

            return View(khoHang);
        }

        // POST: Admin/KhoHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var khoHang = await _context.KhoHang.FindAsync(id);
            _context.KhoHang.Remove(khoHang);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KhoHangExists(int id)
        {
            return _context.KhoHang.Any(e => e.KhoHangId == id);
        }
    }
}
