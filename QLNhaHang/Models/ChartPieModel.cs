using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNhaHang.Models
{
    public class ChartPieModel
    {
        [Display(Name = "Ngày bán")]
        public string NgayBan { get; set; }
        [Display(Name = "Tổng tiền")]
        public decimal? TongTien { get; set; }
    }
}