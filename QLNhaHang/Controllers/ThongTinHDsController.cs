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
    public class ThongTinHDsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ThongTinHDViewModel ThongTinHDVM { get; set; }

        public ThongTinHDsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            ThongTinHDVM = new ThongTinHDViewModel()
            {
                ThongTinHD = new Data.Models.ThongTinHD()
            };
        }
        // GET: KhachHangs
        public ActionResult Index(int id = 0, string searchString = null, string strUrl = null, int page = 1)
        {
            ThongTinHDVM.StrUrl = Request.Url.AbsoluteUri.ToString();
            ///// for delete
            if (id != 0)
            {

                var thongTinHD = _unitOfWork.thongTinHDRepository.GetById(id);
                if (thongTinHD == null)
                {
                    var lastMaTTHD = _unitOfWork.thongTinHDRepository
                                              .GetAll().OrderByDescending(x => x.Id)
                                              .FirstOrDefault().Id;
                    id = lastMaTTHD;

                }
                ThongTinHDVM.ThongTinHD = _unitOfWork.thongTinHDRepository.GetById(id);

            }
            ///// for delete

            ThongTinHDVM.ThongTinHDs = _unitOfWork.thongTinHDRepository.ListThongTinHD(searchString, page);
            return View(ThongTinHDVM);
        }

        public ActionResult Create(string strUrl)
        {

            ThongTinHDVM.StrUrl = strUrl;
            return View(ThongTinHDVM);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePost(ThongTinHDViewModel model)
        {
            _unitOfWork.thongTinHDRepository.Create(model.ThongTinHD);
            _unitOfWork.Complete();
            SetAlert("Thêm mới thành công.", "success");
            return Redirect(model.StrUrl);
        }

        
        public ActionResult Edit(string strUrl, int id)
        {
            ThongTinHDVM.ThongTinHD = _unitOfWork.thongTinHDRepository.GetById(id);
            if (ThongTinHDVM.ThongTinHD == null)
            {
                ViewBag.ErrorMessage = "Thông tin này không tồn tại";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            ThongTinHDVM.StrUrl = strUrl;
            //KhachHangVM.TenKHEdit = KhachHangVM.KhachHang.TenKH;
            return View(ThongTinHDVM);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(string strUrl, int id, ThongTinHDViewModel model)
        {
            if (id != model.ThongTinHD.Id)
            {
                ViewBag.ErrorMessage = "Thông tin này không tồn tại";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            //model.KhachHang.TenKH = model.TenKHEdit;
            _unitOfWork.thongTinHDRepository.Update(model.ThongTinHD);
            _unitOfWork.Complete();
            SetAlert("Cập nhật thành công", "success");
            return Redirect(strUrl);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePost(string strUrl, int id)
        {
            var thongTin = _unitOfWork.thongTinHDRepository.GetById(id);
            _unitOfWork.thongTinHDRepository.Delete(thongTin);
            _unitOfWork.Complete();
            SetAlert("Xóa thành công.", "success");
            return Redirect(strUrl);
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