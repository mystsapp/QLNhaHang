using PagedList;
using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNhaHang.Models
{
    public class RoleViewModel
    {
        public IPagedList<Role> Roles { get; set; }
        public Role Role { get; set; }
        public List<VanPhong> VanPhongs { get; set; }
        public List<NhanVien> NhanViens { get; set; }
        public IEnumerable<KhuVuc> KhuVucs { get; set; }
        public string StrUrl { get; set; }
        [Remote("IsStringNameAvailable", "Roles", ErrorMessage = "Role đã tồn tại")]
        public string TenRoleCreate { get; set; }
    }
}