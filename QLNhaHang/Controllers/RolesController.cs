using QLNhaHang.Data.Models;
using QLNhaHang.Data.Repositories;
using QLNhaHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNhaHang.Controllers
{
    public class RolesController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public RoleViewModel RoleVM { get; set; }

        public RolesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RoleVM = new RoleViewModel()
            {
                Role = new Data.Models.Role(),
                VanPhongs = new List<VanPhong>(),
                NhanViens = new List<NhanVien>()
            };
        }
        // GET: Roles
        public ActionResult Index(int id = 0, string searchString = null, int page = 1)
        {
            RoleVM.StrUrl = Request.Url.AbsoluteUri.ToString();
            ViewBag.searchString = searchString;
            if (id != 0)
            {

                var role = _unitOfWork.roleRepository.GetById(id);
                if (role == null)
                {
                    var lastId = _unitOfWork.roleRepository
                                            .GetAll().OrderByDescending(x => x.Id)
                                            .FirstOrDefault().Id;
                    id = lastId;

                }
                RoleVM.Role = _unitOfWork.roleRepository.GetById(id);

                RoleVM.VanPhongs = _unitOfWork.vanPhongRepository.Find(x => x.Role.Equals(RoleVM.Role.Name)).ToList();
                RoleVM.NhanViens = _unitOfWork.nhanVienRepository.Find(x => x.RoleId.Equals(id)).ToList();

            }

            RoleVM.Roles = _unitOfWork.roleRepository.ListRole(searchString, page);
            return View(RoleVM);
        }

        public ActionResult Create(string strUrl)
        {
            var user = (NhanVien)Session["UserSession"];
            if (user.Role.Name.Equals("Users"))
            {
                return View("~/Views/Shared/AccessDeny.cshtml");
            }
            
            RoleVM.StrUrl = strUrl;
            return View(RoleVM);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePost(RoleViewModel model)
        {


            model.Role.NgayTao = DateTime.Now;
            model.Role.NguoiTao = "Admin";
            model.Role.Name = model.TenRoleCreate;
            _unitOfWork.roleRepository.Create(model.Role);
            _unitOfWork.Complete();
            SetAlert("Thêm mới thành công.", "success");
            return Redirect(model.StrUrl);
        }

        public JsonResult IsStringNameAvailable(string TenRoleCreate)
        {
            var boolName = _unitOfWork.roleRepository.Find(x => x.Name.Trim().ToLower() == TenRoleCreate.Trim().ToLower()).FirstOrDefault();
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

            RoleVM.Role = _unitOfWork.roleRepository.GetById(id);
            if (RoleVM.Role == null)
            {
                ViewBag.ErrorMessage = "Tên này không tồn tại";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            RoleVM.StrUrl = strUrl;

            return View(RoleVM);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(string strUrl, int id, RoleViewModel model)
        {
            if (id != model.Role.Id)
            {
                ViewBag.ErrorMessage = "Tên này không tồn tại";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            //model.KhachHang.TenKH = model.TenKHEdit;
            _unitOfWork.roleRepository.Update(model.Role);
            _unitOfWork.Complete();
            SetAlert("Cập nhật thành công", "success");
            return Redirect(strUrl);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePost(string strUrl, int id)
        {
            var role = _unitOfWork.roleRepository.GetById(id);
            _unitOfWork.roleRepository.Delete(role);
            _unitOfWork.Complete();
            SetAlert("Xóa thành công.", "success");
            return Redirect(strUrl);
        }
    }
}