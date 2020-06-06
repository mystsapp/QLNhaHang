using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNhaHang.Models
{
    public class ThongKeViewModel
    {
        [Display(Name = "Từ ngày")]
        [RegularExpression(@"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$", ErrorMessage = "Chưa đúng định dạng dd/MM/yyyy.")]
        public string TuNgay { get; set; }
        [Display(Name = "Đến ngày")]
        [RegularExpression(@"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$", ErrorMessage = "Chưa đúng định dạng dd/MM/yyyy.")]
        public string DenNgay { get; set; }
        [Display(Name = "Tình trạng")]
        public string DaIn { get; set; }
        public List<DaInModel> DaInModels { get; set; }
        public List<VanPhong> VanPhongs { get; set; }
        public List<KhuVuc> KhuVucs { get; set; }
        public List<ThucDon> ThucDons { get; set; }
        public List<NhanVien> NhanViens { get; set; }
        public int VanPhongId { get; set; }
        public int KhuVucId { get; set; }
        public int ThucDonId { get; set; }
        public string NhanVienId { get; set; }
    }
}