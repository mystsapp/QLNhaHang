using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNhaHang.Models
{
    public class ChiTietHdViewModel
    {
        public string HoTen { get; set; }
        public string VPName { get; set; }
        public string KVName { get; set; }
        public DateTime? NgayTao { get; set; }
        public string TenMon { get; set; }
        public int SoLuong { get; set; }
        public decimal? DonGia { get; set; }
        public decimal? ThanhTien { get; set; }
        public decimal? TC { get; set; }
        public string TenBan { get; set; }
        public string NoiLamViec { get; set; }
    }
}