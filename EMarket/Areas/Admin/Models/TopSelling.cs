using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMarket.Areas.Admin.Models
{
    public partial class TopSelling
    {
        public int TopSellingId { get; set; }
        public int HangHoaId { get; set; }
        [Display(Name="Số Lần Đặt Hàng")]
        public int? SoLan { get; set; }
        public int? DanhGia { get; set; }

        public HangHoa HangHoa { get; set; }
    }
}
