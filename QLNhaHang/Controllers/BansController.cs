using QLNhaHang.Data.Models;
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
    public class BansController : BaseController
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
            var user = (NhanVien)Session["UserSession"];

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

            BanVM.Bans = _unitOfWork.banRepository.ListBan(user.Role.Name, user.VanPhong.Name, searchString, page);

            return View(BanVM);
        }

        public ActionResult Create(string strUrl)
        {
            var user = (NhanVien)Session["UserSession"];
            if (user.Role.Name != "Admins")
            {
                BanVM.VanPhongs = _unitOfWork.vanPhongRepository.Find(x => x.Role.Equals(user.Role.Name)).ToList();
            }
            if (user.Role.Name.Equals("Users"))
            {
                return View("~/Views/Shared/AccessDeny.cshtml");
            }
            var ban = _unitOfWork.banRepository.GetAllIncludeOne(x => x.VanPhong)
                                                .OrderByDescending(x => x.MaBan)
                                                .FirstOrDefault();
            //if (ban != null)
            //{
            //    BanVM.Ban.MaBan = GetNextId.NextID(ban.MaBan, "00120");
            //}
            //else
            //{
            //    BanVM.Ban.MaBan = GetNextId.NextID("", "00120");
            //}

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
            var user = (NhanVien)Session["UserSession"];
            if (user.Role.Name.Equals("Users"))
            {
                return View("~/Views/Shared/AccessDeny.cshtml");
            }
            if (user.Role.Name != "Admins")
            {
                BanVM.VanPhongs = _unitOfWork.vanPhongRepository.Find(x => x.Role.Equals(user.Role.Name)).ToList();
            }
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
            var user = (NhanVien)Session["UserSession"];
            if (user.Role.Name.Equals("Users"))
            {
                return View("~/Views/Shared/AccessDeny.cshtml");
            }
            var ban = _unitOfWork.banRepository.GetByStringId(maBan);
            _unitOfWork.banRepository.Delete(ban);
            _unitOfWork.Complete();
            SetAlert("Xóa thành công.", "success");
            return Redirect(strUrl);
        }

        public JsonResult GetNextMaBan(int vanPhongId)
        {
            var yearPrefix = DateTime.Now.Year.ToString().Substring(2, 2);
            var currentPrefix = _unitOfWork.vanPhongRepository.GetById(vanPhongId).MaVP + yearPrefix;

            var bans = _unitOfWork.banRepository.GetAll().OrderByDescending(x => x.MaBan);
            var listOldBanTrung = new List<Ban>();
            foreach (var ban in bans)
            {
                var oldPrefix = ban.MaBan.Substring(0, 5);
                if (currentPrefix == oldPrefix)
                {
                    listOldBanTrung.Add(ban);
                }
            }
            int a = 1;
            if (listOldBanTrung.Count() != 0)
            {
                var lastMaBan = listOldBanTrung.OrderByDescending(x => x.MaBan).FirstOrDefault();
                var maBan = GetNextId.NextID(lastMaBan.MaBan.Substring(5, 4), currentPrefix);
                var lastMaSo = _unitOfWork.banRepository.GetAll().OrderByDescending(x => x.MaSo).FirstOrDefault();

                var maSo = GetNextId.NextID(lastMaSo.MaSo, "");

                return Json(new
                {
                    status = true,
                    data = maBan,
                    maSo = maSo
                }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var maBan = GetNextId.NextID("", currentPrefix);
                var lastMaSo = _unitOfWork.banRepository.GetAll().OrderByDescending(x => x.MaSo).FirstOrDefault();
                string maSo;
                if (lastMaSo == null)
                {
                    maSo = GetNextId.NextID("", "");
                }
                else
                {
                    maSo = GetNextId.NextID(lastMaSo.MaSo, "");
                }
                return Json(new
                {
                    status = true,
                    data = maBan,
                    maSo = maSo
                }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}