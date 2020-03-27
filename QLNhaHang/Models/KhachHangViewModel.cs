using PagedList;
using QLNhaHang.Data.Models;
using System.Web.Mvc;

namespace QLNhaHang.Models
{
    public class KhachHangViewModel
    {
        public KhachHang KhachHang { get; set; }
        public IPagedList<KhachHang> KhachHangs { get; set; }
        public string StrUrl { get; set; }
        [Remote("IsStringNameAvailable", "KhachHangs", ErrorMessage = "Tên KH đã tồn tại")]
        public string TenKHCreate { get; set; }
        //[Remote("IsStringNameEditAvailable", "KhachHangs", ErrorMessage = "Tên KH đã tồn tại")]
        //public string TenKHEdit { get; set; }
    }
}