using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EMarket.Areas.Admin.Models;
using EMarket.Areas.Client.Helpers;
using EMarket.Areas.Client.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EMarket.Services.PayPal;
using PayPal.v1.Payments;
using EMarket.Services.OnePay;

namespace EMarket.Areas.Client.Controllers
{
    [Area("Client")]
    public class GioHangController : Controller
    {
        private readonly EMarketContext eMarketContext;
        private readonly ILogger<GioHangController> _logger;
        private readonly IPayPalPayment _payPal;
        private IHttpContextAccessor _accessor;

        public GioHangController(EMarketContext context, ILogger<GioHangController> logger,IPayPalPayment payPal, IHttpContextAccessor accessor)
        {
            eMarketContext = context;
            _logger = logger;
            _payPal = payPal;
            _accessor = accessor;
        }


        public IActionResult Index()
        {
            return RedirectToAction("Index", "HangHoa");
        }


        public IActionResult Them(int id, int soLuong = 0)
        {
            if (SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "cart") == null)
            {
                List<GioHang> cart = new List<GioHang>();
                cart.Add(new GioHang { HangHoa = eMarketContext.HangHoa.Include(h => h.Loai)
                    .Include(h => h.NhaCungCap).FirstOrDefault(p => p.HangHoaId == id), SoLuong = soLuong });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<GioHang> cart = SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "cart");
                int index = IsExist(id);
                if (index != -1)
                {
                    cart[index].SoLuong += soLuong;
                }
                else
                {
                    cart.Add(new GioHang { HangHoa = eMarketContext.HangHoa.Include(h => h.Loai)
                    .Include(h => h.NhaCungCap).FirstOrDefault(p => p.HangHoaId == id), SoLuong = soLuong });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Xoa(int id)
        {
            List<GioHang> cart = SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "cart");
            int index = IsExist(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }

        private int IsExist(int id)
        {
            List<GioHang> cart = SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].HangHoa.HangHoaId.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

        [HttpPost]
        public IActionResult ThanhToan(string name, string email, string address, string tel)
        {
            List<GioHang> danhsachhang = SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "cart");

            CreateInvoice(danhsachhang, name, email, address, tel);
            HttpContext.Session.SetString("cart", "");           

            foreach (var item in danhsachhang)
            {
                var topselling = eMarketContext.TopSelling.Where(p => p.HangHoaId == item.HangHoa.HangHoaId).FirstOrDefault();
                if (topselling == null)
                {
                    var newcolumn = new TopSelling();
                    newcolumn.HangHoaId = item.HangHoa.HangHoaId;
                    newcolumn.SoLan = 1;
                    eMarketContext.Add(newcolumn);
                    eMarketContext.SaveChanges();
                }
                else
                {
                    topselling.SoLan += 1;
                    eMarketContext.Update(topselling);
                    eMarketContext.SaveChanges();
                }
            }
           
            TempData["status"] = "Đặt Hàng Thành Công";
            return RedirectToAction("Index", "HangHoa");
        }


        private HoaDon CreateInvoice(List<GioHang> danhsachhang, string name, string email, string address, string tel)
        {
            string value = SessionHelper.GetObjectFromJson<string>(HttpContext.Session,"User");
            var user = eMarketContext.TaiKhoan.Include(p => p.ThongTinTaiKhoan).Where(p => p.UserName == value).FirstOrDefault();

            HoaDon hoadon = new HoaDon();
            hoadon.TenKhachHang = name ?? user.UserName;
            hoadon.Sdt = tel ?? user.ThongTinTaiKhoan.Sdt;
            hoadon.DiaChi = address ?? user.ThongTinTaiKhoan.DiaChi;
            hoadon.Email = email ?? user.Email;
            hoadon.NgayLapHoaDon = DateTime.Now;
            hoadon.TinhTrang = false;

            eMarketContext.Add(hoadon);
            eMarketContext.SaveChanges();

            foreach (var item in danhsachhang)
            {
                ChiTietHoaDon chitethoadon = new ChiTietHoaDon();
                chitethoadon.HangHoaId = item.HangHoa.HangHoaId;
                chitethoadon.SoLuong = item.SoLuong;
                chitethoadon.TongTien = item.SoLuong * item.HangHoa.Gia;
                chitethoadon.HoaDonId = hoadon.HoaDonId;
                eMarketContext.Add(chitethoadon);
                eMarketContext.SaveChanges();
            }
            return hoadon;
        }


        [HttpPost]
        public async Task<IActionResult> PaypalPayment()
        {
            List<GioHang> danhsachhang = SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "cart");
            List<Item> items = new List<Item>();
            double total = 0;

            foreach (var x in danhsachhang)
            {   
                items.Add(new Item() {
                    Name = x.HangHoa.TenHangHoa,
                    Currency = "USD",
                    Price = x.HangHoa.Gia.ToString(),
                    Quantity = x.SoLuong.ToString(),
                    Sku = "sku",
                    Tax = "0"
                });
                total += x.HangHoa.Gia * x.SoLuong;
            }

            Payment payment = _payPal.CreatePayment(total, @"https://localhost:44336/Client/GioHang/Success", @"https://localhost:44336/Client/GioHang/Fail", "sale",items);
            string paypalRedirectUrl = await _payPal.ExecutePayment(payment);
            if (paypalRedirectUrl == "fail") {
                return RedirectToAction("Fail");
            }
            return Redirect(paypalRedirectUrl);
        }

        public IActionResult OnePayPayment(string amount)
        {
            return View();
        }

        [HttpPost]
        public IActionResult OnePayPayment([Bind("vpc_Customer_Phone,vpc_Customer_Email,vpc_Customer_Id,vpc_Customer_Name")] VPC vpc)
        {
            string value = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "User");
            var user = eMarketContext.TaiKhoan.Include(p => p.ThongTinTaiKhoan).Where(p => p.UserName == value).FirstOrDefault();

            List<GioHang> danhsachhang = SessionHelper.GetObjectFromJson<List<GioHang>>(HttpContext.Session, "cart");
            double total = 0;
            
            foreach (var x in danhsachhang)
            {
                total += x.HangHoa.Gia * x.SoLuong;
            }
            total = VPCRequest.USD_VND * total;
            var current_invoice = CreateInvoice(danhsachhang, vpc.vpc_Customer_Name, vpc.vpc_Customer_Email, vpc.vpc_Customer_Address, vpc.vpc_Customer_Phone);

            HttpContext.Session.SetString("cart", "");

            foreach (var item in danhsachhang)
            {
                var topselling = eMarketContext.TopSelling.Where(p => p.HangHoaId == item.HangHoa.HangHoaId).FirstOrDefault();
                if (topselling == null)
                {
                    var newcolumn = new TopSelling();
                    newcolumn.HangHoaId = item.HangHoa.HangHoaId;
                    newcolumn.SoLan = 1;
                    eMarketContext.Add(newcolumn);
                    eMarketContext.SaveChanges();
                }
                else
                {
                    topselling.SoLan += 1;
                    eMarketContext.Update(topselling);
                    eMarketContext.SaveChanges();
                }
            }

            //Send request to OnePay
            string returnURL = Url.Action("OnePayResult", "GioHang",null, Request.Scheme); ;
            VPCRequest conn = new VPCRequest();
            conn.SetSecureSecret(VPCRequest.SECURE_SECRET);
            conn.AddDigitalOrderField("Title", "onepay paygate");
            conn.AddDigitalOrderField("vpc_Locale", "vn");//Chon ngon ngu hien thi tren cong thanh toan (vn/en)
            conn.AddDigitalOrderField("vpc_Version", "2");
            conn.AddDigitalOrderField("vpc_Command", "pay");
            conn.AddDigitalOrderField("vpc_Merchant", VPCRequest.MERCHANT_ID);
            conn.AddDigitalOrderField("vpc_AccessCode", VPCRequest.ACCESS_CODE);
            conn.AddDigitalOrderField("vpc_MerchTxnRef", "HoaDon_"+ current_invoice.HoaDonId);
            conn.AddDigitalOrderField("vpc_OrderInfo", "HoaDon_" + current_invoice.HoaDonId);
            conn.AddDigitalOrderField("vpc_Amount", total+"00");
            conn.AddDigitalOrderField("vpc_Currency", "VND");
            conn.AddDigitalOrderField("vpc_ReturnURL", returnURL);

            // Thong tin them ve khach hang. De trong neu khong co thong tin
            conn.AddDigitalOrderField("vpc_Customer_Phone", vpc.vpc_Customer_Phone);
            conn.AddDigitalOrderField("vpc_Customer_Email", vpc.vpc_Customer_Email);
            conn.AddDigitalOrderField("vpc_Customer_Id", ""+user.TaiKhoanId);

            // Dia chi IP cua khach hang
            string ipAddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            conn.AddDigitalOrderField("vpc_TicketNo", ipAddress);

            // Chuyen huong trinh duyet sang cong thanh toan
            string url = conn.Create3PartyQueryString();
            return Redirect(url);
        }
        
        public IActionResult OnePayResult(string vpc_TxnResponseCode)
        {
            VPCRequest conn = new VPCRequest("http://onepay.vn");
            conn.SetSecureSecret(VPCRequest.SECURE_SECRET);
            // Xu ly tham so tra ve va kiem tra chuoi du lieu ma hoa
            var hashvalidateResult = conn.Process3PartyResponse(HttpContext.Request.Query);
            string result = "";
            if (hashvalidateResult == "CORRECTED" && vpc_TxnResponseCode.Trim() == "0")
            {
                result = "Giao dịch thành công";
            }
            else if (hashvalidateResult == "INVALIDATED" && vpc_TxnResponseCode.Trim() == "0")
            {
                result = "Giao dịch đang chờ xử lý";
            }
            else
            {
                result = "Giao dịch không thành công";
            }
            ViewBag.Result = result;
            return View();
        }

        public IActionResult Success()
        {
            //Tạo đơn hàng trong CSDL với trạng thái : Đã thanh toán, phương thức: Paypal
            HttpContext.Session.SetString("cart", "");
            TempData["status"] = "Đã Thanh Toán";
            return RedirectToAction("Index", "HangHoa");
        }

        public IActionResult Fail()
        {
            //Tạo đơn hàng trong CSDL với trạng thái : Chưa thanh toán, phương thức: 
            TempData["status"] = "Thanh toán đơn hàng thất bại xin vui lòng thử lại";
            return RedirectToAction("Index", "HangHoa");
        }
    }
}