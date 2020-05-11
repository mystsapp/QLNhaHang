using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
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
    public class KhuVucsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public KhuVucViewModel KhuVucVM { get; set; }
        public KhuVucsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            KhuVucVM = new KhuVucViewModel()
            {
                KhuVuc = new Data.Models.KhuVuc(),
                VanPhongs = _unitOfWork.vanPhongRepository.GetAll(),
                LoaiViewModels = new List<LoaiThucDonListViewModel>() { new LoaiThucDonListViewModel() { Id = 0, Name = "-- select --" } },

            };
        }
        // GET: KhuVucs
        public ActionResult Index(int id = 0, string searchString = null, string strUrl = null, int ddlVP = 0, int page = 1)
        {
            ////// moi load vao
            var user = (NhanVien)Session["UserSession"];
            if (user.Role == "Users")
            {
                return View("~/Views/Shared/AccessDeny.cshtml");
            }

            //if (ddlVP == 0)
            //{
            //    ViewBag.ddlVP = 0;
            //}
            //else
            //{
            ViewBag.ddlVP = ddlVP;
            //}
            

            KhuVucVM.StrUrl = Request.Url.AbsoluteUri.ToString();
            ViewBag.searchString = searchString;
            if (ddlVP != 0)
            {
                ViewBag.idVP = ddlVP;
            }

            if (user.Role != "Admins")
            {
                List<KhuVuc> khuVucs = new List<KhuVuc>();
                if (user.Role != "Users")
                {
                    KhuVucVM.VanPhongs = KhuVucVM.VanPhongs.Where(x => x.Role == user.Role);
                    foreach (var vanPhong in KhuVucVM.VanPhongs)
                    {
                        KhuVucVM.LoaiViewModels.Add(new LoaiThucDonListViewModel() { Id = vanPhong.Id, Name = vanPhong.Name });
                    }

                }
                // user ko co quyen quan tri

                //else
                //{
                //    KhuVucVM.VanPhongs = KhuVucVM.VanPhongs.Where(x => x.Id == user.KhuVuc.VanPhongId);
                //    foreach (var vanPhong in KhuVucVM.VanPhongs)
                //    {
                //        KhuVucVM.LoaiViewModels.Add(new LoaiThucDonListViewModel() { Id = vanPhong.Id, Name = vanPhong.Name });
                //    }
                //}

            }
            else
            {
                foreach (var vanPhong in KhuVucVM.VanPhongs)
                {
                    KhuVucVM.LoaiViewModels.Add(new LoaiThucDonListViewModel() { Id = vanPhong.Id, Name = vanPhong.Name });
                }
            }

            ///// for delete
            if (id != 0)
            {

                var khuVuc = _unitOfWork.khuVucRepository.GetById(id);
                if (khuVuc == null)
                {
                    var lastKhuVucId = _unitOfWork.khuVucRepository
                                              .GetAll().OrderByDescending(x => x.Id)
                                              .FirstOrDefault().Id;
                    id = lastKhuVucId;

                }
                KhuVucVM.KhuVuc = _unitOfWork.khuVucRepository.GetById(id);
                // DS NV
                KhuVucVM.NhanViens = _unitOfWork.nhanVienRepository.Find(x => x.KhuVucId == id).ToList();
                // DS Ban
                KhuVucVM.Bans = _unitOfWork.banRepository.Find(x => x.KhuVucId == id).ToList();

            }
            ///// for delete
            var vanPhongByRoles = JsonConvert.SerializeObject(_unitOfWork.vanPhongRepository.Find(x => x.Role == user.Role));
            KhuVucVM.KhuVucs = _unitOfWork.khuVucRepository.ListKhuVuc(user.Role, user.KhuVuc.VanPhongId, searchString, vanPhongByRoles, ddlVP, page);
            return View(KhuVucVM);
        }

        public ActionResult Create(string strUrl)
        {
            var user = (NhanVien)Session["UserSession"];
            if (user.Role.Equals("Users"))
            {
                return View("~/Views/Shared/AccessDeny.cshtml");
            }
            KhuVucVM.StrUrl = strUrl;

            if (user.Role != "Admins")
            {
                KhuVucVM.VanPhongs = KhuVucVM.VanPhongs.Where(x => x.Role == user.Role);

            }

            return View(KhuVucVM);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePost(KhuVucViewModel model)
        {
            _unitOfWork.khuVucRepository.Create(model.KhuVuc);
            _unitOfWork.Complete();
            SetAlert("Thêm mới thành công.", "success");
            return Redirect(model.StrUrl);
        }


        public ActionResult Edit(string strUrl, int id = 0)
        {

            var user = (NhanVien)Session["UserSession"];
            if (user.Role.Equals("Users"))
            {
                return View("~/Views/Shared/AccessDeny.cshtml");
            }
            if (user.Role != "Admins")
            {
                KhuVucVM.VanPhongs = KhuVucVM.VanPhongs.Where(x => x.Role == user.Role);

            }
            KhuVucVM.KhuVuc = _unitOfWork.khuVucRepository.GetById(id);
            if (KhuVucVM.KhuVuc == null || id == 0)
            {
                ViewBag.ErrorMessage = "Khu vực này không tồn tại";
                return View("~/Views/Shared/NotFound.cshtml");
            }

            KhuVucVM.StrUrl = strUrl;

            return View(KhuVucVM);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(string strUrl, int id, KhuVucViewModel model)
        {
            if (id != model.KhuVuc.Id)
            {
                ViewBag.ErrorMessage = "Khu vực này không tồn tại";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            //model.KhachHang.TenKH = model.TenKHEdit;
            _unitOfWork.khuVucRepository.Update(model.KhuVuc);
            _unitOfWork.Complete();
            SetAlert("Cập nhật thành công", "success");
            return Redirect(strUrl);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePost(string strUrl, int id)
        {
            var khuVuc = _unitOfWork.khuVucRepository.GetById(id);
            _unitOfWork.khuVucRepository.Delete(khuVuc);
            _unitOfWork.Complete();
            SetAlert("Xóa thành công.", "success");
            return Redirect(strUrl);
        }
    }
}