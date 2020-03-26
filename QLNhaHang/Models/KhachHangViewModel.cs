using PagedList;
using QLNhaHang.Data.Models;

namespace QLNhaHang.Models
{
    public class KhachHangViewModel
    {
        public KhachHang KhachHang { get; set; }
        public IPagedList<KhachHang> KhachHangs { get; set; }
        public string StrUrl { get; set; }
    }
}