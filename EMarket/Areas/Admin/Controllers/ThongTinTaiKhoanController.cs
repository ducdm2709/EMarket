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
    public class ThongTinTaiKhoanController : Controller
    {
        private readonly EMarketContext _context;

        public ThongTinTaiKhoanController(EMarketContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> SelectAccount() {
            var eMarketContext = _context.TaiKhoan.Include(t=>t.ThongTinTaiKhoan);
            return View(await eMarketContext.ToListAsync());
        }

        // GET: Admin/ThongTinTaiKhoan
        public async Task<IActionResult> Index()
        {
            var eMarketContext = _context.ThongTinTaiKhoan.Include(t => t.TaiKhoan);
            return View(await eMarketContext.ToListAsync());
        }

        // GET: Admin/ThongTinTaiKhoan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thongTinTaiKhoan = await _context.ThongTinTaiKhoan
                .Include(t => t.TaiKhoan)
                .FirstOrDefaultAsync(m => m.ThongTinTaiKhoanId == id);
            if (thongTinTaiKhoan == null)
            {
                return NotFound();
            }

            return View(thongTinTaiKhoan);
        }

        // GET: Admin/ThongTinTaiKhoan/Create
        public IActionResult Create()
        {
            ViewData["TaiKhoanId"] = new SelectList(_context.TaiKhoan, "TaiKhoanId", "UserName");
            return View();
        }

        // POST: Admin/ThongTinTaiKhoan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ThongTinTaiKhoanId,HoVaTen,NgaySinh,Sdt,DiaChi,TaiKhoanId")] ThongTinTaiKhoan thongTinTaiKhoan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(thongTinTaiKhoan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TaiKhoanId"] = new SelectList(_context.TaiKhoan, "TaiKhoanId", "UserName", thongTinTaiKhoan.TaiKhoanId);
            return View(thongTinTaiKhoan);
        }

        // GET: Admin/ThongTinTaiKhoan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thongTinTaiKhoan = await _context.ThongTinTaiKhoan.FindAsync(id);
            if (thongTinTaiKhoan == null)
            {
                return NotFound();
            }
            ViewData["TaiKhoanId"] = _context.ThongTinTaiKhoan.Where(p => p.ThongTinTaiKhoanId == id).FirstOrDefault().TaiKhoanId;
            return View(thongTinTaiKhoan);
        }

        // POST: Admin/ThongTinTaiKhoan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ThongTinTaiKhoanId,HoVaTen,NgaySinh,Sdt,DiaChi,TaiKhoanId")] ThongTinTaiKhoan thongTinTaiKhoan)
        {
            if (id != thongTinTaiKhoan.ThongTinTaiKhoanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thongTinTaiKhoan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThongTinTaiKhoanExists(thongTinTaiKhoan.ThongTinTaiKhoanId))
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
            ViewData["TaiKhoanId"] = new SelectList(_context.TaiKhoan, "TaiKhoanId", "Password", thongTinTaiKhoan.TaiKhoanId);
            return View(thongTinTaiKhoan);
        }

        // GET: Admin/ThongTinTaiKhoan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thongTinTaiKhoan = await _context.ThongTinTaiKhoan
                .Include(t => t.TaiKhoan)
                .FirstOrDefaultAsync(m => m.ThongTinTaiKhoanId == id);
            if (thongTinTaiKhoan == null)
            {
                return NotFound();
            }

            return View(thongTinTaiKhoan);
        }

        // POST: Admin/ThongTinTaiKhoan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var thongTinTaiKhoan = await _context.ThongTinTaiKhoan.FindAsync(id);
            _context.ThongTinTaiKhoan.Remove(thongTinTaiKhoan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThongTinTaiKhoanExists(int id)
        {
            return _context.ThongTinTaiKhoan.Any(e => e.ThongTinTaiKhoanId == id);
        }
    }
}
