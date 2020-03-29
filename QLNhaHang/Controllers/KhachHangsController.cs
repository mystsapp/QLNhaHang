using QLNhaHang.Data.Repositories;
using QLNhaHang.Models;
using QLNhaHang.Utilities;
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
            KhachHangVM = new KhachHangViewModel()
            {
                KhachHang = new Data.Models.KhachHang()
            };
        }
        // GET: KhachHangs
        public ActionResult Index(string maKH = null, string searchString = null, string strUrl = null, int page = 1)
        {
            KhachHangVM.StrUrl = Request.Url.AbsoluteUri.ToString();
            if (!string.IsNullOrEmpty(maKH))
            {

                var khachHang = _unitOfWork.khachHangRepository.GetByStringId(maKH);
                if(khachHang == null)
                {
                    var lastMaKH = _unitOfWork.khachHangRepository
                                              .GetAll().OrderByDescending(x => x.MaKH)
                                              .FirstOrDefault().MaKH;
                    maKH = lastMaKH;

                }
                KhachHangVM.KhachHang = _unitOfWork.khachHangRepository.GetByStringId(maKH);

            }
            
            KhachHangVM.KhachHangs = _unitOfWork.khachHangRepository.ListKhachHang(searchString, page);
            return View(KhachHangVM);
        }

        public ActionResult Create(string strUrl)
        {
            var khachHang = _unitOfWork.khachHangRepository.GetAll()
                                                            .OrderByDescending(x => x.MaKH)
                                                            .FirstOrDefault();
            if (khachHang != null)
            {
                KhachHangVM.KhachHang.MaKH = GetNextId.NextID(khachHang.MaKH, "00120");
            }
            else
            {
                KhachHangVM.KhachHang.MaKH = GetNextId.NextID("", "00120");
            }
            TempData["gioiTinh"] = ListGioiTinh();
            KhachHangVM.StrUrl = strUrl;
            return View(KhachHangVM);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePost(KhachHangViewModel model)
        {
            model.KhachHang.NgayTao = DateTime.Now;
            model.KhachHang.NguoiTao = "Admin";
            model.KhachHang.TenKH = model.TenKHCreate;
            _unitOfWork.khachHangRepository.Create(model.KhachHang);
            _unitOfWork.Complete();
            SetAlert("Thêm mới thành công.", "success");
            return Redirect(model.StrUrl);
        }

        public JsonResult IsStringNameAvailable(string TenKHCreate)
        {
            var boolName = _unitOfWork.khachHangRepository.Find(x => x.TenKH.ToLower() == TenKHCreate.ToLower()).FirstOrDefault();
            if (boolName == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        
        public JsonResult IsStringNameEditAvailable(string TenKHEdit)
        {
            var boolName = _unitOfWork.khachHangRepository.Find(x => x.TenKH.ToLower() == TenKHEdit.ToLower()).FirstOrDefault();
            if (boolName == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit(string strUrl, string maKH)
        {
            KhachHangVM.KhachHang = _unitOfWork.khachHangRepository.GetByStringId(maKH);
            if(KhachHangVM.KhachHang == null)
            {
                ViewBag.ErrorMessage = "Khách hàng này không tồn tại";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            TempData["gioiTinh"] = ListGioiTinh();
            KhachHangVM.StrUrl = strUrl;
            //KhachHangVM.TenKHEdit = KhachHangVM.KhachHang.TenKH;
            return View(KhachHangVM);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(string strUrl, string maKH, KhachHangViewModel model)
        {
            if(maKH != model.KhachHang.MaKH)
            {
                ViewBag.ErrorMessage = "Khách hàng này không tồn tại";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            //model.KhachHang.TenKH = model.TenKHEdit;
            _unitOfWork.khachHangRepository.Update(model.KhachHang);
            _unitOfWork.Complete();
            SetAlert("Cập nhật thành công", "success");
            return Redirect(strUrl);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePost(string strUrl, string maKH)
        {
            var khachHang = _unitOfWork.khachHangRepository.GetByStringId(maKH);
            _unitOfWork.khachHangRepository.Delete(khachHang);
            _unitOfWork.Complete();
            SetAlert("Xóa thành công.", "success");
            return Redirect(strUrl);
        }
        private List<GioiTinhViewModel> ListGioiTinh()
        {
            return new List<GioiTinhViewModel>()
            {
                new GioiTinhViewModel() { Id = "None", Name = "--None--" },
                new GioiTinhViewModel() { Id = "Nam", Name = "Nam" },
                new GioiTinhViewModel() { Id = "Nử", Name = "Nử" }
            };
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