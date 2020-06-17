using Microsoft.Web.Infrastructure.DynamicValidationHelper;
using Newtonsoft.Json;
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
                var result = _unitOfWork.nhanVienRepository.Login(model.Username.Trim(), model.Password);
                if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản này không tồn tại");
                }
                else if (result == 1)
                {
                    var userInfo = _unitOfWork.nhanVienRepository
                                              .FindIncludeOne(a => a.KhuVuc,
                                                              x => x.Username.ToLower()
                                                              .Equals(model.Username.Trim().ToLower()))
                                              .FirstOrDefault();

                    Session["UserSession"] = userInfo;

                    Session["username"] = userInfo.Username;
                    Session["hoten"] = userInfo.HoTen;
                    Session["TenVP"] = userInfo.KhuVuc.VanPhong.Name;
                    Session["VPId"] = userInfo.KhuVuc.VanPhongId;
                    Session["role"] = userInfo.Role;
                    Session["noiLamViec"] = userInfo.NoiLamViec;

                    Session["listKV"] = JsonConvert.SerializeObject(_unitOfWork.khuVucRepository.Find(x => x.VanPhongId == userInfo.KhuVuc.VanPhongId));
                    if(userInfo.NoiLamViec == "Pha chế")
                    {
                        return RedirectToAction("Index", "PhaChes");
                    }
                    if(userInfo.NoiLamViec == "Bếp")
                    {
                        return RedirectToAction("Index", "Beps");
                    }
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