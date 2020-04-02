using PagedList;
using QLNhaHang.Data.Models;
using System.Web.Mvc;

namespace QLNhaHang.Models
{
    public class BanViewModel
    {
        public Ban Ban { get; set; }
        public IPagedList<Ban> Bans { get; set; }
        public string StrUrl { get; set; }
        [Remote("IsStringNameAvailable", "Bans", ErrorMessage = "Bàn này đã tồn tại")]
        public string TenBanCreate { get; set; }
        //[Remote("IsStringNameEditAvailable", "KhachHangs", ErrorMessage = "Tên KH đã tồn tại")]
        //public string TenKHEdit { get; set; }
    }
}