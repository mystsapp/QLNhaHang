using QLNhaHang.Data.Repositories;
using QLNhaHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNhaHang.Controllers
{
    public class BanHangsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BanHangViewModel BanHangVM { get; set; }
        public BanHangsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }
        // GET: BanHangs
        public ActionResult Index()
        {
            BanHangVM = new BanHangViewModel();
            BanHangVM.Bans = _unitOfWork.banRepository.GetAll().ToList();
            return View(BanHangVM);
        }
    }
}