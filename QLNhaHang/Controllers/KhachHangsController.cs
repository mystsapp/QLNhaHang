using QLNhaHang.Data.Repositories;
using QLNhaHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNhaHang.Controllers
{
    public class KhachHangsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public KhachHangViewModel KhachHangVM { get; set; }

        public KhachHangsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            KhachHangVM = new KhachHangViewModel();
        }
        // GET: KhachHangs
        public ActionResult Index(string maKH = null, string searchString = null, string strUrl = null, int page = 1)
        {
            if(maKH != null)
            {
                KhachHangVM.KhachHang = _unitOfWork.khachHangRepository.GetByStringId(maKH);
            }
            KhachHangVM.KhachHangs = _unitOfWork.khachHangRepository.ListKhachHang(searchString, page);
            return View(KhachHangVM);
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}