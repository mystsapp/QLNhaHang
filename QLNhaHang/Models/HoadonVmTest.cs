using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNhaHang.Models
{
    public class HoadonVmTest
    {
        public string MaHD { get; set; }
        public List<ChiTietHdViewModel> ChiTietHdViewModels { get; set; }
        public HoadonVmTest()
        {
            ChiTietHdViewModels = new List<ChiTietHdViewModel>();
        }
    }
}