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
    public class BansController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BanViewModel BanVM { get; set; }

        public BansController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            BanVM = new BanViewModel()
            {
                Ban = new Data.Models.Ban()
            };
        }
        // GET: Bans
        public ActionResult Index(string maBan = null, string searchString = null, int page = 1)
        {
            BanVM.StrUrl = Request.Url.AbsoluteUri.ToString();
            if (!string.IsNullOrEmpty(maBan))
            {

                var ban = _unitOfWork.banRepository.GetByStringId(maBan);
                if (ban == null)
                {
                    var lastMaBan = _unitOfWork.banRepository
                                               .GetAll().OrderByDescending(x => x.MaBan)
                                               .FirstOrDefault().MaBan;
                    maBan = lastMaBan;

                }
                BanVM.Ban = _unitOfWork.banRepository.GetByStringId(maBan);

            }

            BanVM.Bans = _unitOfWork.banRepository.ListBan(searchString, page);
            return View(BanVM);
        }

        public ActionResult Create(string strUrl)
        {
            var ban = _unitOfWork.banRepository.GetAllIncludeOne(x => x.VanPhong)
                                                .OrderByDescending(x => x.MaBan)
                                                .FirstOrDefault();
            if (ban != null)
            {
                BanVM.Ban.MaBan = GetNextId.NextID(ban.MaBan, "00120");
            }
            else
            {
                BanVM.Ban.MaBan = GetNextId.NextID("", "00120");
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
            var boolName = _unitOfWork.khachHangRepository.Find(x => x.TenKH.Trim().ToLower() == TenKHCreate.Trim().ToLower()).FirstOrDefault();
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
            if (KhachHangVM.KhachHang == null)
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
            if (maKH != model.KhachHang.MaKH)
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