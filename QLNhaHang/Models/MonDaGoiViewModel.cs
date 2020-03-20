using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNhaHang.Models
{
    public class MonDaGoiViewModel
    {
        public List<ThucDon> ThucDons { get; set; }
        public List<MonDaGoi> MonDaGois { get; set; }
        public MonDaGoi MonDaGoi { get; set; }
        public ThucDon ThucDon { get; set; }
        public Ban Ban { get; set; }
        public string StrUrl { get; set; }
        public string MaBan { get; set; }
        public decimal PhuPhi { get; set; }
        public decimal? TongTien { get; set; }

    }
}