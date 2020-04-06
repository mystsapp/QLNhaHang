using PagedList;
using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNhaHang.Models
{
    public class VanPhongViewModel
    {
        public IPagedList<VanPhong> VanPhongs { get; set; }
        public VanPhong VanPhong { get; set; }
        public List<Role> Roles { get; set; }
        public List<NhanVien> NhanViens { get; set; }
        public string StrUrl { get; set; }
        [Remote("IsStringNameAvailable", "VanPhongs", ErrorMessage = "Văn phòng đã tồn tại")]
        public string TenVPCreate { get; set; }
    }
}