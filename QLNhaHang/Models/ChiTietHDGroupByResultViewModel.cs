using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNhaHang.Models
{
    public class ChiTietHDGroupByResultViewModel
    {
        public string NoiLamViec { get; set; }
        public List<ChiTietHdViewModel> ChiTietHdViewModels { get; set; }
    }
}