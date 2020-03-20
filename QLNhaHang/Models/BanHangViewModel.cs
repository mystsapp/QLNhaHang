using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLNhaHang.Models
{
    public class BanHangViewModel
    {
        public List<Ban> Bans { get; set; }
        public List<MonDaGoi> MonDaGois { get; set; }
        public List<ThucDon> ThucDons { get; set; }
        public ThucDon ThucDon { get; set; }
        public MonDaGoi MonDaGoi { get; set; }
        public Ban Ban { get; set; }
        public string StrUrl { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? TongTien { get; set; }

    }
}