using QLNhaHang.Data.Repositories;
using QLNhaHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNhaHang.Controllers
{
    public class ThucDonsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ThucDonViewModel ThucDonVM { get; set; }

        public ThucDonsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            ThucDonVM = new ThucDonViewModel()
            {
                ThucDon = new Data.Models.ThucDon(),
                LoaiThucDons = _unitOfWork.loaiThucDonRepository.GetAll().ToList()
            };
        }
        // GET: KhachHangs
        public ActionResult Index(int id = 0, string searchString = null, int page = 1)
        {
            ThucDonVM.StrUrl = Request.Url.AbsoluteUri.ToString();
            ViewBag.searchString = searchString;
            if (id != 0)
            {

                var thucDon = _unitOfWork.thucDonRepository.GetById(id);
                if (thucDon == null)
                {
                    var lastId = _unitOfWork.thucDonRepository
                                            .GetAll().OrderByDescending(x => x.Id)
                                            .FirstOrDefault().Id;
                    id = lastId;

                }
                ThucDonVM.ThucDon = _unitOfWork.thucDonRepository.GetById(id);

            }

            ThucDonVM.ThucDons = _unitOfWork.thucDonRepository.ListThucDon(searchString, page);
            return View(ThucDonVM);
        }

        public ActionResult Create(string strUrl)
        {
            ThucDonVM.StrUrl = strUrl;
            return View(ThucDonVM);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePost(ThucDonViewModel model)
        {
            model.ThucDon.GiaTien = decimal.Parse(model.GiaTien);
            model.ThucDon.NgayTao = DateTime.Now;
            model.ThucDon.NguoiTao = "Admin";
            model.ThucDon.TenMon = model.TenMonCreate;
            _unitOfWork.thucDonRepository.Create(model.ThucDon);
            _unitOfWork.Complete();
            SetAlert("Thêm mới thành công.", "success");
            return Redirect(model.StrUrl);
        }

        public JsonResult IsStringNameAvailable(string TenMonCreate)
        {
            var boolName = _unitOfWork.thucDonRepository.Find(x => x.TenMon.Trim().ToLower() == TenMonCreate.Trim().ToLower()).FirstOrDefault();
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
            
            ThucDonVM.ThucDon = _unitOfWork.thucDonRepository.GetById(id);
            if (ThucDonVM.ThucDon == null)
            {
                ViewBag.ErrorMessage = "Món này không tồn tại";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            ThucDonVM.StrUrl = strUrl;
            ThucDonVM.GiaTien = ThucDonVM.ThucDon.GiaTien.ToString().Split('.')[0];

            return View(ThucDonVM);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(string strUrl, int id, ThucDonViewModel model)
        {
            model.ThucDon.GiaTien = decimal.Parse(model.GiaTien);
            if (id != model.ThucDon.Id)
            {
                ViewBag.ErrorMessage = "Món này không tồn tại";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            //model.KhachHang.TenKH = model.TenKHEdit;
            _unitOfWork.thucDonRepository.Update(model.ThucDon);
            _unitOfWork.Complete();
            SetAlert("Cập nhật thành công", "success");
            return Redirect(strUrl);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePost(string strUrl, int id)
        {
            var thucDon = _unitOfWork.thucDonRepository.GetById(id);
            _unitOfWork.thucDonRepository.Delete(thucDon);
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