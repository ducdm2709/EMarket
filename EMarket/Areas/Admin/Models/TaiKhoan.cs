using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMarket.Areas.Admin.Models
{
    public partial class TaiKhoan
    {
        [Display(Name = "Mã Tài Khoản")]
        [Required]
        public int TaiKhoanId { get; set; }
        [Display(Name = "Tài khoản")]
        [Required]
        public string UserName { get; set; }
        [Display(Name = "Mật Khẩu")]
        [Required]
        public string Password { get; set; }
        [Display(Name = "Ngày Đăng Ký")]
        [Required]
        public DateTime NgayDk { get; set; }
        [Display(Name = "Tài khoản khách hàng")]
        [Required]
        public bool LoaiTaiKhoan { get; set; }
        [Display(Name = "Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public ThongTinTaiKhoan ThongTinTaiKhoan { get; set; }
    }
}
