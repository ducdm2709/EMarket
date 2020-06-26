using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMarket.Areas.Admin.Models
{
    public partial class ThongTinTaiKhoan
    {
        [Display(Name ="Mã Thông Tin Tài Khoản")]
        [Required]
        public int ThongTinTaiKhoanId { get; set; }
        [Display(Name = "Họ Và Tên")]
        [Required]
        public string HoVaTen { get; set; }
        [Display(Name = "Ngày Sinh")]
        [Required]
        public DateTime NgaySinh { get; set; }
        [Display(Name = "Số điện thoại")]
        [Required]
        public string Sdt { get; set; }
        [Display(Name = "Địa chỉ")]
        [Required]
        public string DiaChi { get; set; }
       
        public int TaiKhoanId { get; set; }

        public TaiKhoan TaiKhoan { get; set; }
    }
}
