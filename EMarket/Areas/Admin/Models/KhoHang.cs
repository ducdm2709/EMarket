using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMarket.Areas.Admin.Models
{
    public partial class KhoHang
    {

        [Display(Name = "Mã kho hàng")]
        [Required]
        public int KhoHangId { get; set; }


        [Display(Name = "Số lượng tồn kho")]
        [Required]
        public int SoLuong { get; set; }
        public int HangHoaId { get; set; }

        public HangHoa HangHoa { get; set; }
    }
}
