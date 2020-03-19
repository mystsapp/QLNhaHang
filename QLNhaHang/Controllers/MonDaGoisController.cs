using QLNhaHang.Data.Repositories;
using QLNhaHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNhaHang.Controllers
{
    public class MonDaGoisController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public MonDaGoiViewModel MonDaGoiVM { get; set; }
        public MonDaGoisController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            MonDaGoiVM = new MonDaGoiViewModel()
            {
                MonDaGoi = new Data.Models.MonDaGoi(),
                Ban = new Data.Models.Ban()
            };
        }
        // GET: MonDaGois
        public ActionResult GoiMon(string maBan = null, string strUrl = null, int maTD = 0, int soLuong = 1)
        {
            MonDaGoiVM.ThucDons = _unitOfWork.thucDonRepository.GetAll().OrderBy(x => x.Id).ToList();
            TempData["thucDon"] = MonDaGoiVM.ThucDons;
            MonDaGoiVM.StrUrl = strUrl;
            MonDaGoiVM.MonDaGois = _unitOfWork.monDaGoiRepository.GetAllInclude(x => x.Ban, y => y.ThucDon).ToList();
            
            if (!string.IsNullOrEmpty(maBan))
            {
                MonDaGoiVM.Ban = _unitOfWork.banRepository.GetByStringId(maBan);
            }
            
            if (maTD != 0)
            {
                MonDaGoiVM.MonDaGoi.GiaTien = _unitOfWork.thucDonRepository.GetById(maTD).GiaTien;                
            }
            else
            {
                MonDaGoiVM.MonDaGoi.GiaTien = _unitOfWork.thucDonRepository.GetById(MonDaGoiVM.ThucDons.FirstOrDefault().Id).GiaTien;
            }

            if (soLuong != 0)
            {
                MonDaGoiVM.MonDaGoi.ThanhTien = MonDaGoiVM.MonDaGoi.GiaTien * soLuong;
                MonDaGoiVM.MonDaGoi.SoLuong = soLuong;
            }
            return View(MonDaGoiVM);
        }

        [HttpPost]
        public ActionResult GoiMon(MonDaGoiViewModel model, string maBan = null, string strUrl = null)
        {
            _unitOfWork.monDaGoiRepository.Create(model.MonDaGoi);
            _unitOfWork.Complete();
            return RedirectToAction(nameof(GoiMon), new { maBan = maBan, strUrl = strUrl});
        }
    }
}