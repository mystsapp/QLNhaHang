using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNhaHang.Models
{
    public class PhaCheViewModel
    {
        public int Id { get; set; }
        public List<MonDaGoi> MonDaGois { get; set; }
    }
}