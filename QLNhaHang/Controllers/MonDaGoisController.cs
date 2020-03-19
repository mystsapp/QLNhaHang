﻿using QLNhaHang.Data.Repositories;
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
                Ban = new Data.Models.Ban(),
                MonDaGoi = new Data.Models.MonDaGoi()
            };
        }
        // GET: MonDaGois
        public ActionResult GoiMon(string maBan = null, string strUrl = null, int maTD = 0, int soLuong = 0)
        {
            MonDaGoiVM.ThucDons = _unitOfWork.thucDonRepository.GetAll().OrderBy(x => x.Id).ToList();
            TempData["thucDon"] = MonDaGoiVM.ThucDons;
            MonDaGoiVM.StrUrl = strUrl;
            MonDaGoiVM.MonDaGois = _unitOfWork.monDaGoiRepository
                                              .GetAllInclude(x => x.Ban, y => y.ThucDon)
                                              .Where(x => x.MaBan.Equals(maBan))
                                              .OrderBy(x => x.ThucDon.TenMon)
                                              .ToList();
            
            if (!string.IsNullOrEmpty(maBan))
            {
                MonDaGoiVM.MonDaGoi.MaBan = maBan;
                MonDaGoiVM.Ban = _unitOfWork.banRepository.GetByStringId(maBan);
            }
            
            if (maTD != 0)
            {
                MonDaGoiVM.MonDaGoi.ThucDonId = maTD;
                MonDaGoiVM.MonDaGoi.GiaTien = _unitOfWork.thucDonRepository.GetById(maTD).GiaTien;                
            }
            else
            {
                var tD = MonDaGoiVM.ThucDons.FirstOrDefault();
                MonDaGoiVM.MonDaGoi.GiaTien = tD.GiaTien;
                maTD = tD.Id;
            }
            MonDaGoiVM.ThucDon = _unitOfWork.thucDonRepository.GetById(maTD);
            if (soLuong == 0)
            {
                MonDaGoiVM.MonDaGoi.SoLuong = soLuong = 1;
            }
            if (soLuong != 0)
            {
                var loaiTD = _unitOfWork.thucDonRepository.GetById(maTD).LoaiThucDon;
                MonDaGoiVM.MonDaGoi.ThanhTien = MonDaGoiVM.MonDaGoi.GiaTien * soLuong;
                if(loaiTD.PhuPhi != null)
                {
                    MonDaGoiVM.MonDaGoi.PhuPhi = loaiTD.PhuPhi * soLuong;
                }
                else
                {
                    MonDaGoiVM.MonDaGoi.PhuPhi = 0;
                }
                MonDaGoiVM.MonDaGoi.SoLuong = soLuong;
            }
            return View(MonDaGoiVM);
        }

        [HttpPost]
        public ActionResult GoiMon(MonDaGoiViewModel model, string maBan = null, string strUrl = null)
        {
            var monDaGoi = _unitOfWork.monDaGoiRepository.Find(x => x.MaBan.Equals(maBan))
                                                         .Where(x => x.ThucDonId.Equals(model.MonDaGoi.ThucDonId))
                                                         .FirstOrDefault();
            if(monDaGoi != null)
            {
                model.MonDaGoi.SoLuong += monDaGoi.SoLuong;
                model.MonDaGoi.PhuPhi += monDaGoi.PhuPhi;
                model.MonDaGoi.ThanhTien += monDaGoi.ThanhTien;
            }
            _unitOfWork.monDaGoiRepository.Create(model.MonDaGoi);
            if (monDaGoi != null)
            {
                _unitOfWork.monDaGoiRepository.Delete(monDaGoi);
            }
                
            _unitOfWork.Complete();
            SetAlert("Thêm mới thành công!", "success");


            return RedirectToAction(nameof(GoiMon), new { maBan = maBan, strUrl = strUrl});
        }

        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "waring")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
    }
}