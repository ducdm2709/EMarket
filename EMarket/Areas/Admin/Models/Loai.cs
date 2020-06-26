using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMarket.Areas.Admin.Models
{
    public partial class Loai
    {
        public Loai()
        {
            HangHoa = new HashSet<HangHoa>();
        }

        [Display(Name = "Mã loại")]
        [Required]
        public int LoaiId { get; set; }

        [Display(Name = "Tên loại")]
        [Required]
        public string TenLoai { get; set; }

        [Display(Name = "Mô Tả")]        
        public string MoTa { get; set; }

        public ICollection<HangHoa> HangHoa { get; set; }
    }
}
