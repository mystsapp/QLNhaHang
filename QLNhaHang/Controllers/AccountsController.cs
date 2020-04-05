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
    public class AccountsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public NhanVienViewModel NhanVienVM { get; set; }

        public AccountsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            NhanVienVM = new NhanVienViewModel()
            {
                NhanVien = new Data.Models.NhanVien(),
                Roles = _unitOfWork.roleRepository.GetAll().ToList(),
                VanPhongs = _unitOfWork.vanPhongRepository.GetAll().ToList(),
                GioiTinhs = ListGioiTinh()
            };
        }
        // GET: Accounts
        public ActionResult Index(string maNV = null, string gioiTinh = null, string searchString = null, int page = 1)
        {
            NhanVienVM.StrUrl = Request.Url.AbsoluteUri.ToString();
            ViewBag.searchString = searchString;
            /////// for delete //////
            if (!string.IsNullOrEmpty(maNV))
            {

                var nhanVien = _unitOfWork.nhanVienRepository.GetByStringId(maNV);
                if (nhanVien == null)
                {
                    var lastMaNV = _unitOfWork.nhanVienRepository
                                            .GetAll().OrderByDescending(x => x.NgayTao)
                                            .FirstOrDefault().MaNV;
                    maNV = lastMaNV;

                }
                NhanVienVM.NhanVien = _unitOfWork.nhanVienRepository.GetByStringId(maNV);

            }

            NhanVienVM.NhanViens = _unitOfWork.nhanVienRepository.ListNhanVien(gioiTinh, searchString, page);
            /////// for delete //////
            return View(NhanVienVM);
        }

        public ActionResult Create(string strUrl)
        {
            NhanVienVM.StrUrl = strUrl;
            var nhanVien = _unitOfWork.nhanVienRepository.GetAll().OrderByDescending(x => x.MaNV).FirstOrDefault();
            if(nhanVien != null)
            {
                NhanVienVM.NhanVien.MaNV = GetNextId.NextID(nhanVien.MaNV, "00120");
            }
            else
            {
                NhanVienVM.NhanVien.MaNV = GetNextId.NextID("", "00120");
            }
            return View(NhanVienVM);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePost(NhanVienViewModel model)
        {
            var user = (NhanVien)Session["UserSession"];

            model.NhanVien.NgayTao = DateTime.Now;
            model.NhanVien.NguoiTao = "Admin";
            model.NhanVien.Password = MaHoaSHA1.EncodeSHA1(model.NhanVien.Password);
            model.NhanVien.Username = model.UsernameCreate;
            //model.NhanVien.NguoiTao = user.Username;            
            model.NhanVien.NguoiTao = "Admin";            
            model.NhanVien.NgaySinh = DateTime.Parse(model.NgaySinh);            

            _unitOfWork.nhanVienRepository.Create(model.NhanVien);
            _unitOfWork.Complete();
            SetAlert("Thêm mới thành công.", "success");
            return Redirect(model.StrUrl);
        }

        public JsonResult IsStringNameAvailable(string UsernameCreate)
            {
            var boolName = _unitOfWork.nhanVienRepository.Find(x => x.Username.Trim().ToLower() == UsernameCreate.Trim().ToLower()).FirstOrDefault();
            if (boolName == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Edit(string strUrl, string maNV)
        {

            NhanVienVM.NhanVien = _unitOfWork.nhanVienRepository.GetByStringId(maNV);
            
            if (NhanVienVM.NhanVien == null)
            {
                ViewBag.ErrorMessage = "Nhân viên này không tồn tại";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            NhanVienVM.NgaySinh = NhanVienVM.NhanVien.NgaySinh.ToString();
            NhanVienVM.OldPass = NhanVienVM.NhanVien.Password;
            NhanVienVM.NhanVien.Password = "";
            NhanVienVM.StrUrl = strUrl;

            return View(NhanVienVM);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(string strUrl, string maNV, NhanVienViewModel model)
        {
            if (maNV != model.NhanVien.MaNV)
            {
                ViewBag.ErrorMessage = "Nhân viên này không tồn tại";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            if (string.IsNullOrEmpty(model.EditPassword))
            {
                model.NhanVien.Password = model.OldPass;
            }
            else
            {
                model.NhanVien.Password = MaHoaSHA1.EncodeSHA1(model.EditPassword);
            }
            model.NhanVien.NgayCapNhat = DateTime.Now;
            model.NhanVien.NguoiCapNhat = "Admin";
            if (!string.IsNullOrEmpty(model.NgaySinh))
            {
                model.NhanVien.NgaySinh = DateTime.Parse(model.NgaySinh);
            }

            _unitOfWork.nhanVienRepository.Update(model.NhanVien);
            _unitOfWork.Complete();
            SetAlert("Cập nhật thành công", "success");
            return Redirect(strUrl);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePost(string strUrl, string maNV)
        {
            var nhanVien = _unitOfWork.nhanVienRepository.GetByStringId(maNV);
            _unitOfWork.nhanVienRepository.Delete(nhanVien);
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

       
    }
}