using PagedList;
using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNhaHang.Models
{
    public class HoaDonViewModel
    {
        public IPagedList<HoaDon> HoaDons { get; set; }
        public IEnumerable<ChiTietHD> ChiTietHDs { get; set; }
        public HoaDon HoaDon { get; set; }
        public ChiTietHD ChiTietHD { get; set; }
        public KhachHang KhachHang { get; set; }
        public string StrUrl { get; set; }
        public string Id { get; set; }
        public decimal? TongTien { get; set; }

        ///////// in HoaDon ////
        public IEnumerable<KhachHang> KhachHangs { get; set; }
        public IEnumerable<ThongTinHD> ThongTinHDs { get; set; }
        public ThongTinHD ThongTinHD { get; set; }

        public string SoTien { get; set; }
        public string NoiDung { get; set; }

        public decimal VAT { get; set; }
        public string ThanhTienVAT { get; set; }
        
        public decimal TyLePPV { get; set; }
        public string ThanhTienSauPPV { get; set; }
    }
}