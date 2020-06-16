using QLNhaHang.Data.Repositories;
using QLNhaHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNhaHang.Controllers
{
    public class ChiTietHDsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChiTietHoaDonViewModel ChiTietHDVM { get; set; }
        public ChiTietHDsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            ChiTietHDVM = new ChiTietHoaDonViewModel()
            {
                ChiTietHD = new Data.Models.ChiTietHD(),
                ChiTietHDs = _unitOfWork.chiTietHDRepository.GetAll().ToList()
            };
        }
        // GET: ChiTietHDs
        public ActionResult Index()
        {
            return View(ChiTietHDVM);
        }
    }
}