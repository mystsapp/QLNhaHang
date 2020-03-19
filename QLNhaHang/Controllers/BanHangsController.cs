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
            BanHangVM = new BanHangViewModel();
        }
        // GET: BanHangs
        public ActionResult Index(string maBan = null)
        {
            //BanHangVM.StrUrl = UriHelper.GetDisplayUrl(Request);
            ViewBag.strUrl = Request.Url.AbsoluteUri.ToString();
            BanHangVM.Bans = _unitOfWork.banRepository.GetAll().ToList();
            
            return View(BanHangVM);
        }

        public ActionResult creat()
        {
            return View();
        }

        public ActionResult edit()
        {
            return View();
        }

        public ActionResult DetailsRedirect(string strUrl)
        {
            return Redirect(strUrl);
        }
    }
}