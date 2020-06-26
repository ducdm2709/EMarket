using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMarket.Areas.Admin.Models
{
    public partial class NhaCungCap
    {
        public NhaCungCap()
        {
            HangHoa = new HashSet<HangHoa>();
        }


        [Display(Name = "Mã nhà cung cấp")]
        [Required]
        public int NhaCungCapId { get; set; }

        [Display(Name = "Tên nhà cung cấp")]
        [Required]
        public string TenNhaCungCap { get; set; }

        [Display(Name = "Mô tả")]
        [Required]
        public string MoTa { get; set; }

        public ICollection<HangHoa> HangHoa { get; set; }
    }
}
