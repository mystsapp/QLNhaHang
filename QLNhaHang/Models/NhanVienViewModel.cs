using PagedList;
using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNhaHang.Models
{
    public class NhanVienViewModel
    {
        public NhanVien NhanVien { get; set; }
        public IPagedList<NhanVien> NhanViens { get; set; }
        public List<Role> Roles { get; set; }
        public List<VanPhong> VanPhongs { get; set; }
        public List<GioiTinhViewModel> GioiTinhs { get; set; }
        public string StrUrl { get; set; }
        public string UsernameCreate { get; set; }
        public string OldPass { get; set; }
    }
}