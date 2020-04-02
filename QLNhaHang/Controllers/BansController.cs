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
                Ban = new Data.Models.Ban(),
                VanPhongs = _unitOfWork.vanPhongRepository.GetAll().ToList()
            };
        }
        // GET: Bans
        public ActionResult Index(string maBan = null, string searchString = null, int page = 1)
        {
            BanVM.StrUrl = Request.Url.AbsoluteUri.ToString();
            ViewBag.searchString = searchString;
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
            
            BanVM.StrUrl = strUrl;
            return View(BanVM);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePost(BanViewModel model)
        {
            model.Ban.NgayTao = DateTime.Now;
            model.Ban.NguoiTao = "Admin";
            model.Ban.TenBan = model.TenBanCreate;
            _unitOfWork.banRepository.Create(model.Ban);
            _unitOfWork.Complete();
            SetAlert("Thêm mới thành công.", "success");
            return Redirect(model.StrUrl);
        }

        public JsonResult IsStringNameAvailable(string TenBanCreate)
        {
            var boolName = _unitOfWork.banRepository.Find(x => x.TenBan.Trim().ToLower() == TenBanCreate.Trim().ToLower()).FirstOrDefault();
            if (boolName == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Edit(string strUrl, string maBan)
        {
            BanVM.Ban = _unitOfWork.banRepository.GetByStringId(maBan);
            if (BanVM.Ban == null)
            {
                ViewBag.ErrorMessage = "Bàn này không tồn tại";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            BanVM.StrUrl = strUrl;
            
            return View(BanVM);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(string strUrl, string maBan, BanViewModel model)
        {
            if (maBan != model.Ban.MaBan)
            {
                ViewBag.ErrorMessage = "Bàn này không tồn tại";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            
            _unitOfWork.banRepository.Update(model.Ban);
            _unitOfWork.Complete();
            SetAlert("Cập nhật thành công", "success");
            return Redirect(strUrl);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePost(string strUrl, string maBan)
        {
            var ban = _unitOfWork.banRepository.GetByStringId(maBan);
            _unitOfWork.banRepository.Delete(ban);
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