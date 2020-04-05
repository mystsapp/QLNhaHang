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
    public class LoginsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoginsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET: Logins
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _unitOfWork.nhanVienRepository.Login(model.Username, model.Password);
                if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản này không tồn tại");
                }
                else if (result == 1)
                {
                    var userInfo = _unitOfWork.nhanVienRepository
                                              .FindIncludeTwo(a => a.VanPhong,
                                                              b => b.Role,
                                                              x => x.Username.ToLower()
                                                              .Equals(model.Username.ToLower()))
                                              .FirstOrDefault();
                    Session["UserSession"] = userInfo;

                    Session["username"] = userInfo.Username;
                    Session["hoten"] = userInfo.HoTen;
                    Session["chinhanh"] = userInfo.VanPhong.Name;
                    Session["role"] = userInfo.Role.Name;

                    return RedirectToAction("Index", "Home");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản này đã bị khóa");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng.");
                }
            }
            return View("Login");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return Redirect("/");
        }

        [HttpGet]
        public ActionResult ChangePass(string strUrl)
        {
            var entity = new ChangePassModel();
            var user = (NhanVien)Session["UserSession"];
            entity.Username = user.Username;
            entity.strUrl = strUrl;

            return View(entity);
        }
        [HttpPost, ActionName("ChangePass")]
        public ActionResult ChangePass(ChangePassModel model)
        {
            if (ModelState.IsValid)
            {
                var user = (NhanVien)Session["UserSession"];
                string oldPass = user.Password;
                string modelPass = MaHoaSHA1.EncodeSHA1(model.Password);
                if (oldPass != modelPass)
                {
                    ModelState.AddModelError("", "Mật khẩu cũ không đúng");
                }

                else
                {
                    int result = _unitOfWork.nhanVienRepository.Changepass(model.Username, MaHoaSHA1.EncodeSHA1(model.NewPassword));
                    if (result > 0)
                    {
                        SetAlert("Đổi mật khẩu thành công.", "success");
                        return Redirect(model.strUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Không thể đổi mật khẩu.");
                    }
                }

            }

            return View("ChangePass");
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