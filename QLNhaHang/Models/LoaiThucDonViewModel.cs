using PagedList;
using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNhaHang.Models
{
    public class LoaiThucDonViewModel
    {
        public IPagedList<LoaiThucDon> LoaiThucDons { get; set; }
        public LoaiThucDon LoaiThucDon { get; set; }
        public string StrUrl { get; set; }

        [Remote("IsStringNameAvailable", "LoaiThucDons", ErrorMessage = "Loại này đã tồn tại")]
        public string TenLoaiCreate { get; set; }
        public string PhuPhi { get; set; }
    }
}