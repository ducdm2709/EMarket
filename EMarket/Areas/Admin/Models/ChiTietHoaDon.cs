using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMarket.Areas.Admin.Models
{
    public partial class ChiTietHoaDon
    {
        [Display(Name = "Mã Chi Tiết Hóa Đơn")]
        [Required]
        public int ChiTietHoaDonId { get; set; }
        public int HoaDonId { get; set; }
        public int HangHoaId { get; set; }
        [Display(Name = "Số Lượng")]
        [Required]
        public int SoLuong { get; set; }
        [Display(Name = "Tổng Tiền")]
        [Required]
        public double TongTien { get; set; }

        public HangHoa HangHoa { get; set; }
        public HoaDon HoaDon { get; set; }
    }
}
