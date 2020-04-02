using QLNhaHang.Data.Repositories;
using QLNhaHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNhaHang.Controllers
{
    public class VanPhongsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public VanPhongViewModel VanPhongVM { get; set; }

        public VanPhongsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            VanPhongVM = new VanPhongViewModel()
            {
                VanPhong = new Data.Models.VanPhong(),
                Roles = _unitOfWork.roleRepository.GetAll().ToList()
            };
        }
        // GET: VanPhongs
        public ActionResult Index(int id = 0, string searchString = null, int page = 1)
        {
            VanPhongVM.StrUrl = Request.Url.AbsoluteUri.ToString();
            ViewBag.searchString = searchString;
            if (id != 0)
            {

                var vanPhong = _unitOfWork.vanPhongRepository.GetById(id);
                if (vanPhong == null)
                {
                    var lastId = _unitOfWork.vanPhongRepository
                                            .GetAll().OrderByDescending(x => x.Id)
                                            .FirstOrDefault().Id;
                    id = lastId;

                }
                VanPhongVM.VanPhong = _unitOfWork.vanPhongRepository.GetById(id);

            }

            VanPhongVM.VanPhongs = _unitOfWork.vanPhongRepository.ListVanPhong(searchString, page);
            return View(VanPhongVM);
        }

        public ActionResult Create(string strUrl)
        {
            VanPhongVM.StrUrl = strUrl;
            return View(VanPhongVM);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePost(VanPhongViewModel model)
        {
            
            model.VanPhong.NgayTao = DateTime.Now;
            model.VanPhong.NguoiTao = "Admin";
            model.VanPhong.Name = model.TenVPCreate;
            _unitOfWork.vanPhongRepository.Create(model.VanPhong);
            _unitOfWork.Complete();
            SetAlert("Thêm mới thành công.", "success");
            return Redirect(model.StrUrl);
        }

        public JsonResult IsStringNameAvailable(string TenVPCreate)
        {
            var boolName = _unitOfWork.vanPhongRepository.Find(x => x.Name.Trim().ToLower() == TenVPCreate.Trim().ToLower()).FirstOrDefault();
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

            VanPhongVM.VanPhong = _unitOfWork.vanPhongRepository.GetById(id);
            if (VanPhongVM.VanPhong == null)
            {
                ViewBag.ErrorMessage = "Tên này không tồn tại";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            VanPhongVM.StrUrl = strUrl;

            return View(VanPhongVM);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(string strUrl, int id, VanPhongViewModel model)
        {
            if (id != model.VanPhong.Id)
            {
                ViewBag.ErrorMessage = "Tên này không tồn tại";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            //model.KhachHang.TenKH = model.TenKHEdit;
            _unitOfWork.vanPhongRepository.Update(model.VanPhong);
            _unitOfWork.Complete();
            SetAlert("Cập nhật thành công", "success");
            return Redirect(strUrl);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePost(string strUrl, int id)
        {
            var vanPhong = _unitOfWork.vanPhongRepository.GetById(id);
            _unitOfWork.vanPhongRepository.Delete(vanPhong);
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