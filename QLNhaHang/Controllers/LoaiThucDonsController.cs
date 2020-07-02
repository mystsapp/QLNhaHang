﻿using QLNhaHang.Data.Models;
using QLNhaHang.Data.Repositories;
using QLNhaHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNhaHang.Controllers
{
    public class LoaiThucDonsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public LoaiThucDonViewModel LoaiVM { get; set; }

        public LoaiThucDonsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            LoaiVM = new LoaiThucDonViewModel()
            {
                LoaiThucDon = new Data.Models.LoaiThucDon(),
                NoiLamViecs = ListNoiLamViec(),
                ThucDons = new List<Data.Models.ThucDon>()
            };
        }
        // GET: LoaiThucDons
        public ActionResult Index(int id = 0, string searchString = null, int page = 1)
        {
            LoaiVM.StrUrl = Request.Url.AbsoluteUri.ToString();
            ViewBag.searchString = searchString;

            if (id != 0)
            {

                var loai = _unitOfWork.loaiThucDonRepository.GetById(id);
                if (loai == null)
                {
                    var lastId = _unitOfWork.loaiThucDonRepository
                                              .GetAll().OrderByDescending(x => x.Id)
                                              .FirstOrDefault().Id;
                    id = lastId;

                }
                LoaiVM.LoaiThucDon = _unitOfWork.loaiThucDonRepository.GetById(id);
                LoaiVM.ThucDons = _unitOfWork.thucDonRepository.Find(x => x.MaLoaiId.Equals(id)).ToList();
            }

            LoaiVM.LoaiThucDons = _unitOfWork.loaiThucDonRepository.ListLoai(searchString, page);
            return View(LoaiVM);
        }

        public ActionResult Create(string strUrl)
        {
            //var user = (NhanVien)Session["UserSession"];
            //if (user.Role.Name == "Users")
            //{
            //    return View("~/Views/Shared/AccessDeny.cshtml");
            //}
            LoaiVM.StrUrl = strUrl;
            return View(LoaiVM);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePost(LoaiThucDonViewModel model)
        {
            var user = (NhanVien)Session["UserSession"];
            if (!string.IsNullOrEmpty(model.PhuPhi))
            {
                model.LoaiThucDon.PhuPhi = decimal.Parse(model.PhuPhi);
            }
            
            model.LoaiThucDon.NgayTao = DateTime.Now;
            model.LoaiThucDon.NguoiTao = user.Username;
            model.LoaiThucDon.TenLoai = model.TenLoaiCreate;
            _unitOfWork.loaiThucDonRepository.Create(model.LoaiThucDon);
            _unitOfWork.Complete();
            SetAlert("Thêm mới thành công.", "success");
            return Redirect(model.StrUrl);
        }

        public JsonResult IsStringNameAvailable(string TenLoaiCreate)
        {
            var boolName = _unitOfWork.loaiThucDonRepository.Find(x => x.TenLoai.Trim().ToLower() == TenLoaiCreate.Trim().ToLower()).FirstOrDefault();
            if (boolName == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit(string strUrl, int id)
        {
            //var user = (NhanVien)Session["UserSession"];
            //if (user.Role.Name == "Users")
            //{
            //    return View("~/Views/Shared/AccessDeny.cshtml");
            //}
            LoaiVM.LoaiThucDon = _unitOfWork.loaiThucDonRepository.GetById(id);
            if (LoaiVM.LoaiThucDon == null)
            {
                ViewBag.ErrorMessage = "Loại thực đơn này không tồn tại";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            
            LoaiVM.StrUrl = strUrl;
            
            return View(LoaiVM);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(string strUrl, int id, LoaiThucDonViewModel model)
        {
            if (id != model.LoaiThucDon.Id)
            {
                ViewBag.ErrorMessage = "Loại thực đơn này không tồn tại";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            //model.KhachHang.TenKH = model.TenKHEdit;
            _unitOfWork.loaiThucDonRepository.Update(model.LoaiThucDon);
            _unitOfWork.Complete();
            SetAlert("Cập nhật thành công", "success");
            return Redirect(strUrl);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePost(string strUrl, int id)
        {
            //var user = (NhanVien)Session["UserSession"];
            //if (user.Role.Name == "Users")
            //{
            //    return View("~/Views/Shared/AccessDeny.cshtml");
            //}
            var loai = _unitOfWork.loaiThucDonRepository.GetById(id);
            _unitOfWork.loaiThucDonRepository.Delete(loai);
            _unitOfWork.Complete();
            SetAlert("Xóa thành công.", "success");
            return Redirect(strUrl);
        }

        private List<NoiLamViecViewModel> ListNoiLamViec()
        {
            return new List<NoiLamViecViewModel>()
            {
                new NoiLamViecViewModel() { Id = 0, Name = "--None--" },
                new NoiLamViecViewModel() { Id = 1, Name = "Bếp" },
                new NoiLamViecViewModel() { Id = 2, Name = "Pha chế" }
            };
        }

    }
}