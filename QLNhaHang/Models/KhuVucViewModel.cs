using PagedList;
using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNhaHang.Models
{
    public class KhuVucViewModel
    {
        public KhuVuc KhuVuc { get; set; }
        public IPagedList<KhuVuc> KhuVucs { get; set; }
        public IEnumerable<VanPhong> VanPhongs { get; set; }
        public List<LoaiThucDonListViewModel> LoaiViewModels { get; set; }
        public List<NhanVien> NhanViens { get; set; }
        public List<Ban> Bans { get; set; }
        public string StrUrl { get; set; }
    }
}