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
            BanHangVM = new BanHangViewModel() {
                Ban = new Data.Models.Ban(),
                MonDaGoi = new Data.Models.MonDaGoi()
            };
        }
        // GET: BanHangs
        public ActionResult Index(string maBan = null)
        {
            ///////////////////// load Ban by flag /////////////////////
            //var banByFlags = _unitOfWork.banRepository.Find(x => x.Flag);
            ///////////////////// load Ban by flag /////////////////////
            //BanHangVM.StrUrl = UriHelper.GetDisplayUrl(Request);
            ViewBag.strUrl = Request.Url.AbsoluteUri.ToString();
            BanHangVM.Bans = _unitOfWork.banRepository.GetAll().ToList();            

            if (maBan != null)
            {
                BanHangVM.Ban = _unitOfWork.banRepository.GetByStringId(maBan);
                BanHangVM.MonDaGois = _unitOfWork.monDaGoiRepository
                                                  .FindIncludeTwo(x => x.Ban, y => y.ThucDon, z => z.MaBan.Equals(maBan))
                                                  .OrderBy(x => x.ThucDon.TenMon)
                                                  .ToList();

                var listDecimal = BanHangVM.MonDaGois.Select(x => x.ThanhTien).ToList();
                BanHangVM.TongTien = listDecimal.Sum();
            }
            
            return View(BanHangVM);
        }

        public JsonResult MonInBan(string maBan)
        {
            var mons = _unitOfWork.monDaGoiRepository.Find(x => x.MaBan.Equals(maBan));
            if (mons.Count() > 0)
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
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