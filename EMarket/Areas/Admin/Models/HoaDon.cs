using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMarket.Areas.Admin.Models
{
    public partial class HoaDon
    {
        public HoaDon()
        {
            ChiTietHoaDon = new HashSet<ChiTietHoaDon>();
        }

        [Display(Name ="Mã hóa đơn")]
        [Required]
        public int HoaDonId { get; set; }

        [Display(Name = "Ngày lập hóa đơn")]
        [Required]
        public DateTime NgayLapHoaDon { get; set; }

        [Display(Name = "Phê duyệt")]
        [Required]
        public bool TinhTrang { get; set; }

        [Display(Name = "Tên khách hàng")]
        [Required]
        public string TenKhachHang { get; set; }

        [Display(Name = "Địa chỉ")]
        [Required]
        public string DiaChi { get; set; }

        [Display(Name = "Số điện thoại")]
        [Required]
        public string Sdt { get; set; }

        [Display(Name = "Email")]
        [Required]
        public string Email { get; set; }

        public ICollection<ChiTietHoaDon> ChiTietHoaDon { get; set; }
    }
}
