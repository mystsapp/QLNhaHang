using PagedList;
using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNhaHang.Models
{
    public class ThongTinHDViewModel
    {
        public ThongTinHD ThongTinHD { get; set; }
        public IPagedList<ThongTinHD> ThongTinHDs { get; set; }
        public string StrUrl { get; set; }
    }
}