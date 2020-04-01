using PagedList;
using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNhaHang.Models
{
    public class ThucDonViewModel
    {
        public IPagedList<ThucDon> ThucDons { get; set; }
        public ThucDon ThucDon { get; set; }
        public List<LoaiThucDon> LoaiThucDons { get; set; }
        public string StrUrl { get; set; }
        [Remote("IsStringNameAvailable", "ThucDons", ErrorMessage = "Tên món đã tồn tại")]
        public string TenMonCreate { get; set; }
        public string GiaTien { get; set; }
        public decimal GiaTienD { get; set; }
    }
}