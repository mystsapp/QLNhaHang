using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNhaHang.Models
{
    public class BanHangViewModel
    {
        public List<Ban> Bans { get; set; }
        public BanHangViewModel()
        {
            Bans = new List<Ban>();
        }
    }
}