using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMarket.Areas.Admin.Models
{
    [JsonObject(IsReference = true)]
    public partial class HangHoa
    {
        public HangHoa()
        {
            ChiTietHoaDon = new HashSet<ChiTietHoaDon>();
            KhoHang = new HashSet<KhoHang>();
            TopSelling = new HashSet<TopSelling>();
        }

        [Display(Name="Mã Hàng Hóa")]
        [Required]
        public int HangHoaId { get; set; }

        [Display(Name = "Tên Hàng Hóa")]
        [Required]
        public string TenHangHoa { get; set; }
        public int NhaCungCapId { get; set; }       
        public int LoaiId { get; set; }

        [Display(Name = "Giá")]
        [Required]
        public double Gia { get; set; }
        [Display(Name = "Hình")]
        [Required]
        public string Hinh { get; set; }
        [Display(Name = "Mô Tả")]       
        public string MoTa { get; set; }

        public Loai Loai { get; set; }
        public NhaCungCap NhaCungCap { get; set; }
        public ICollection<ChiTietHoaDon> ChiTietHoaDon { get; set; }
        public ICollection<KhoHang> KhoHang { get; set; }
        public ICollection<TopSelling> TopSelling { get; set; }
        public string TenHhSeoUrl => TenHangHoa.ToUrlFriendly();
        public string TenLoaiSeoUrl => Loai.TenLoai.ToUrlFriendly();
    }
}
