using PagedList;
using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNhaHang.Models
{
    public class NhanVienViewModel
    {
        public NhanVien NhanVien { get; set; }
        public IPagedList<NhanVien> NhanViens { get; set; }
        public List<Role> Roles { get; set; }
        public List<VanPhong> VanPhongs { get; set; }
        public List<KhuVuc> KhuVucs { get; set; }
        public List<GioiTinhViewModel> GioiTinhs { get; set; }
        public List<NoiLamViecViewModel> NoiLamViecs { get; set; }
        public string StrUrl { get; set; }
        [Remote("IsStringNameAvailable", "Accounts", ErrorMessage = "Username đã tồn tại")]
        public string UsernameCreate { get; set; }
        public string OldPass { get; set; }
        [Display(Name = "Ngày sinh")]
        [RegularExpression(@"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$", ErrorMessage = "Chưa đúng định dạng dd/MM/yyyy.")]
        public string NgaySinh { get; set; }

        [Display(Name = "Password")]
        public string EditPassword { get; set; }

        public List<LoaiThucDonListViewModel> LoaiViewModels { get; set; }
    }
}