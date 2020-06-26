using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EMarket.Areas.Admin.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using EMarket.Areas.Admin.Filters;

namespace EMarket.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ServiceFilter(typeof(SessionFilter))]
    public class HangHoaController : Controller
    {
        private readonly EMarketContext _context;

        public HangHoaController(EMarketContext context)
        {
            _context = context;
           
        }

        // GET: Admin/HangHoas
        public async Task<IActionResult> Index(int? page,string searchstring,int? LoaiId)
        {
            ViewData["SearchString"] = searchstring;
            ViewData["LoaiId"] = LoaiId;
            var eMarketContext = _context.HangHoa.Include(h => h.Loai).Include(h => h.NhaCungCap).OrderBy(h=>h.HangHoaId);
            if (!String.IsNullOrEmpty(searchstring))
            {
                eMarketContext = eMarketContext.Where(s => s.TenHangHoa.Contains(searchstring) || Convert.ToString(s.HangHoaId) == searchstring).OrderBy(h=>h.HangHoaId);
            }
            if (LoaiId != null)
            {
                eMarketContext = eMarketContext.Where(p => p.LoaiId == LoaiId).OrderBy(p => p.HangHoaId);
            }
            int pageSize = 5;
            return View(await PaginatedList<HangHoa>.CreateAsync(eMarketContext.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Admin/HangHoas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hangHoa = await _context.HangHoa
                .Include(h => h.Loai)
                .Include(h => h.NhaCungCap)
                .FirstOrDefaultAsync(m => m.HangHoaId == id);
            if (hangHoa == null)
            {
                return NotFound();
            }

            return View(hangHoa);
        }

        // GET: Admin/HangHoas/Create
        public IActionResult Create()
        {
            ViewData["LoaiId"] = new SelectList(_context.Loai, "LoaiId", "TenLoai");
            ViewData["NhaCungCapId"] = new SelectList(_context.NhaCungCap, "NhaCungCapId", "TenNhaCungCap");
            return View();
        }

        // POST: Admin/HangHoas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HangHoaId,TenHangHoa,NhaCungCapId,LoaiId,Gia,MoTa")] HangHoa hangHoa, IFormFile Hinh)
        {
            if (ModelState.IsValid)
            {
                if (Hinh != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", Hinh.FileName);
                    using (var file = new FileStream(path, FileMode.Create))
                    {
                        Hinh.CopyTo(file);
                    }
                    hangHoa.Hinh = Hinh.FileName;
                }
                _context.Add(hangHoa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LoaiId"] = new SelectList(_context.Loai, "LoaiId", "TenLoai", hangHoa.LoaiId);
            ViewData["NhaCungCapId"] = new SelectList(_context.NhaCungCap, "NhaCungCapId", "TenNhaCungCap", hangHoa.NhaCungCapId);
            return View(hangHoa);
        }

        // GET: Admin/HangHoas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hangHoa = await _context.HangHoa.FindAsync(id);
            if (hangHoa == null)
            {
                return NotFound();
            }
            ViewData["LoaiId"] = new SelectList(_context.Loai, "LoaiId", "TenLoai", hangHoa.LoaiId);
            ViewData["NhaCungCapId"] = new SelectList(_context.NhaCungCap, "NhaCungCapId", "TenNhaCungCap", hangHoa.NhaCungCapId);
            return View(hangHoa);
        }

        // POST: Admin/HangHoas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HangHoaId,TenHangHoa,NhaCungCapId,LoaiId,Gia,MoTa")] HangHoa hangHoa,IFormFile Hinh)
        {
            
            if (id != hangHoa.HangHoaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
                try
                {
                    if (Hinh != null)
                    {
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", Hinh.FileName);
                        using (var file = new FileStream(path, FileMode.Create))
                        {
                            Hinh.CopyTo(file);
                        }
                        hangHoa.Hinh = Hinh.FileName;
                    }
                    _context.Update(hangHoa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HangHoaExists(hangHoa.HangHoaId))
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
            ViewData["LoaiId"] = new SelectList(_context.Loai, "LoaiId", "TenLoai", hangHoa.LoaiId);
            ViewData["NhaCungCapId"] = new SelectList(_context.NhaCungCap, "NhaCungCapId", "TenNhaCungCap", hangHoa.NhaCungCapId);
            return View(hangHoa);
        }

        // GET: Admin/HangHoas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hangHoa = await _context.HangHoa
                .Include(h => h.Loai)
                .Include(h => h.NhaCungCap)
                .FirstOrDefaultAsync(m => m.HangHoaId == id);
            if (hangHoa == null)
            {
                return NotFound();
            }

            return View(hangHoa);
        }

        // POST: Admin/HangHoas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hangHoa = await _context.HangHoa.FindAsync(id);
            _context.HangHoa.Remove(hangHoa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HangHoaExists(int id)
        {
            return _context.HangHoa.Any(e => e.HangHoaId == id);
        }
    }
}
