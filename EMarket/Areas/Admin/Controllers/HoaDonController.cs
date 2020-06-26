using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EMarket.Areas.Admin.Models;
using EMarket.Areas.Admin.Filters;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit;

namespace EMarket.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ServiceFilter(typeof(SessionFilter))]
    public class HoaDonController : Controller
    {
        private readonly EMarketContext _context;

        public HoaDonController(EMarketContext context)
        {
            _context = context;
        }

        // GET: Admin/HoaDons
        public async Task<IActionResult> Index()
        {
            return View(await _context.HoaDon.ToListAsync());
        }

        // GET: Admin/HoaDons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chitiet = await _context.ChiTietHoaDon.Where(p => p.HoaDonId == id).Include(p=>p.HangHoa).ToListAsync();


            if (chitiet == null)
            {
                return NotFound();
            }

            return View(chitiet);
        }

        // GET: Admin/HoaDons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/HoaDons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HoaDonId,NgayLapHoaDon,TinhTrang,TenKhachHang,DiaChi,Sdt,Email")] HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hoaDon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hoaDon);
        }

        public async Task<IActionResult> Check(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDon.FindAsync(id);

            var chitiet = await _context.ChiTietHoaDon.Include(p=>p.HangHoa).Where(p => p.HoaDonId == id).ToListAsync();
            if (hoaDon == null)
            {
                return NotFound();
            }

            if (hoaDon.TinhTrang == false)
            {
                SendingInvoice(hoaDon, chitiet);
            }
            hoaDon.TinhTrang = !hoaDon.TinhTrang;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Admin/HoaDons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDon.FindAsync(id);
            if (hoaDon == null)
            {
                return NotFound();
            }
            return View(hoaDon);
        }

        // POST: Admin/HoaDons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HoaDonId,NgayLapHoaDon,TinhTrang,TenKhachHang,DiaChi,Sdt,Email")] HoaDon hoaDon)
        {
            if (id != hoaDon.HoaDonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hoaDon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoaDonExists(hoaDon.HoaDonId))
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
            return View(hoaDon);
        }

        // GET: Admin/HoaDons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDon
                .FirstOrDefaultAsync(m => m.HoaDonId == id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(hoaDon);
        }

        // POST: Admin/HoaDons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hoaDon = await _context.HoaDon.FindAsync(id);
            _context.HoaDon.Remove(hoaDon);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HoaDonExists(int id)
        {
            return _context.HoaDon.Any(e => e.HoaDonId == id);
        }


        private void SendingInvoice(HoaDon hoadon,List<ChiTietHoaDon> chitiet)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Emarket-Dukes", "damnhatphong671998@gmail.com"));
            message.To.Add(new MailboxAddress("Mr(Mrs) Đàm Nhật Phong", "damnhatphong671998@gmail.com"));
            message.Subject = "EmarketInvoice";
            var bodyBuilder = new BodyBuilder();

            bodyBuilder.HtmlBody = "<html><body><table style='border: 1px solid black;border-collapse: collapse; width:100%;'>"+
                                        "<thead>"+
                                        "<tr>"+
                                            "<th style = 'border: 1px solid black;border-collapse: collapse;' >"+
                                                "Mã hóa đơn:"+
                                            "</th>"+
                                            "<th style = 'border: 1px solid black;border-collapse: collapse;' >"+
                                                "Tên hàng hóa:"+
                                            "</th>"+
                                            "<th style = 'border: 1px solid black;border-collapse: collapse;' >"+
                                                 "Số lượng:"+
                                            "</th>"+
                                            "<th style = 'border: 1px solid black;border-collapse: collapse;' >"+
                                                 "Tổng Tiền:"+
                                            "</th>"+
                                        "</tr>"+
                                   "</thead>"+
                                    "<tbody>";
            double total = 0;
            foreach (var item in chitiet) {                
                bodyBuilder.HtmlBody+= "<tr>"+
                                            "<td style = 'border: 1px solid black;border-collapse: collapse;'>"+
                                                 item.HoaDonId +
                                             "</td>"+ 
                                             "<td style = 'border: 1px solid black;border-collapse: collapse;'>"+
                                                  item.HangHoa.TenHangHoa +
                                              "</td>"+  
                                              "<td style = 'border: 1px solid black;border-collapse: collapse;'>"+
                                                   item.SoLuong +
                                               "</td>" +   
                                               "<td style = 'border: 1px solid black;border-collapse: collapse;'>"+
                                                   item.TongTien.ToString() +"$"+
                                            "</td>"+
                                        "</tr>";
                total += item.TongTien;
            }

            bodyBuilder.HtmlBody += "</tbody>" +
                                  "</table>";

            bodyBuilder.HtmlBody += "<tr>" +
                                            "<td>" +
                                             "</td>" +
                                             "<td>" +
                                              "</td>" +
                                              "<td style='float:right; margin-right: 10px;'>" +
                                                  "Tổng:" +
                                               "</td>" +
                                               "<td style = 'border: 1px solid black;border-collapse: collapse;'>" +
                                                   total.ToString() + "$"+
                                                "</td>" +
                                        "</tr></body></html>";

            message.Body = bodyBuilder.ToMessageBody();
            

            using (var client = new SmtpClient(new ProtocolLogger("smtp.txt")))
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect("smtp.gmail.com", 587, false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("damnhatphong671998", "nhatphong671998");
                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                                ex.ToString());
                }
                client.Disconnect(true);
            }
        }
    }
}
