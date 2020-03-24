using QLNhaHang.Data.Repositories;
using QLNhaHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNhaHang.Controllers
{
    public class HoaDonsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HoaDonViewModel HoaDonVM { get; set; }

        public HoaDonsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            HoaDonVM = new HoaDonViewModel();
        }
        // GET: HoaDons
        public ActionResult Index(string maHD = null, string searchString = null, 
                                  string searchFromDate = null, string searchToDate = null, 
                                  int page = 1)
        {
            HoaDonVM.StrUrl = Request.Url.AbsoluteUri.ToString();
            ViewBag.searchString = searchString;
            ViewBag.searchFromDate = searchFromDate;
            ViewBag.searchToDate = searchToDate;

            HoaDonVM.ChiTietHDs = _unitOfWork.chiTietHDRepository
                                             .FindIncludeTwo(x => x.HoaDon, y => y.ThucDon, z => z.MaHD.Equals(maHD));
            HoaDonVM.HoaDons = _unitOfWork.hoaDonRepository.ListHoaDon(searchString, searchFromDate, searchToDate, page);
            HoaDonVM.HoaDon = _unitOfWork.hoaDonRepository.GetByStringId(maHD);
            return View(HoaDonVM);
        }
    }
}