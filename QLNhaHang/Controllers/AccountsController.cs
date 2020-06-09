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
                GioiTinhs = ListGioiTinh(),
                NoiLamViecs = ListNoiLamViec(),
                PhongBans = ListPhongBan(),
                ChucVus = ListChucVu(),
                VanPhongs = _unitOfWork.vanPhongRepository.GetAll().ToList(),
                KhuVucs = _unitOfWork.khuVucRepository.GetAll().ToList(),
                //LoaiViewModels = new List<LoaiThucDonListViewModel>() { new LoaiThucDonListViewModel() { Id = 0, Name = "-- select --" } }
                LoaiViewModels = new List<LoaiThucDonListViewModel>()
            };
        }
        // GET: Accounts
        public ActionResult Index(string maNV = null, string gioiTinh = null, string searchString = null, int page = 1, int idVP = 0)
        {
            NhanVienVM.StrUrl = Request.Url.AbsoluteUri.ToString();
            ViewBag.searchString = searchString;

            var user = (NhanVien)Session["UserSession"];
            if (user.Role.Equals("Users"))
            {
                return View("~/Views/Shared/AccessDeny.cshtml");
            }
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

            NhanVienVM.NhanViens = _unitOfWork.nhanVienRepository.ListNhanVien(user.Role, user.KhuVuc.VanPhong.Name, gioiTinh, searchString, page, idVP);
            /////// for delete //////
            return View(NhanVienVM);
        }

        public ActionResult Create(string strUrl)
        {
            NhanVienVM.StrUrl = strUrl;
            var user = (NhanVien)Session["UserSession"];
            if (user.Role.Equals("Users"))
            {
                return View("~/Views/Shared/AccessDeny.cshtml");
            }
            else
            {
                if (user.Role.Equals("Admins"))
                {
                    foreach (var khuVuc in NhanVienVM.KhuVucs)
                    {
                        NhanVienVM.LoaiViewModels.Add(new LoaiThucDonListViewModel() { Id = khuVuc.Id, Name = khuVuc.Name + " - " + khuVuc.VanPhong.Name });
                    }

                    var yearPrefix = DateTime.Now.Year.ToString().Substring(2, 2);
                    var currentPrefix = user.KhuVuc.VanPhong.MaVP + yearPrefix;

                    var nhanViens = _unitOfWork.nhanVienRepository.GetAll().OrderByDescending(x => x.MaNV);
                    var listOldNVTrung = new List<NhanVien>();
                    foreach (var nv in nhanViens)
                    {
                        var oldPrefix = nv.MaNV.Substring(0, 5);
                        if (currentPrefix == oldPrefix)
                        {
                            listOldNVTrung.Add(nv);
                        }
                    }
                    if (listOldNVTrung.Count() != 0)
                    {
                        var lastMaNV = listOldNVTrung.OrderByDescending(x => x.MaNV).FirstOrDefault();
                        NhanVienVM.NhanVien.MaNV = GetNextId.NextID(lastMaNV.MaNV.Substring(5, 4), currentPrefix);
                    }
                    else
                    {
                        NhanVienVM.NhanVien.MaNV = GetNextId.NextID("", currentPrefix);
                    }

                }
                else
                {

                    NhanVienVM.VanPhongs = _unitOfWork.vanPhongRepository.Find(x => x.Role == user.Role).ToList();
                    List<KhuVuc> listKv = new List<KhuVuc>();
                    foreach (var vanPhong in NhanVienVM.VanPhongs)
                    {
                        listKv.AddRange(_unitOfWork.khuVucRepository.Find(x => x.VanPhongId == vanPhong.Id));
                    }
                    NhanVienVM.KhuVucs = listKv;
                    NhanVienVM.Roles = _unitOfWork.roleRepository.Find(x => x.Name == user.Role).ToList();
                    NhanVienVM.Roles.Add(_unitOfWork.roleRepository.Find(x => x.Name.Equals("Users")).FirstOrDefault());
                    var yearPrefix = DateTime.Now.Year.ToString().Substring(2, 2);
                    var currentPrefix = user.KhuVuc.VanPhong.MaVP + yearPrefix;

                    var nhanViens = _unitOfWork.nhanVienRepository.GetAll().OrderByDescending(x => x.MaNV);
                    var listOldNVTrung = new List<NhanVien>();
                    foreach (var nv in nhanViens)
                    {
                        var oldPrefix = nv.MaNV.Substring(0, 5);
                        if (currentPrefix == oldPrefix)
                        {
                            listOldNVTrung.Add(nv);
                        }
                    }
                    if (listOldNVTrung.Count() != 0)
                    {
                        var lastMaNV = listOldNVTrung.OrderByDescending(x => x.MaNV).FirstOrDefault();
                        NhanVienVM.NhanVien.MaNV = GetNextId.NextID(lastMaNV.MaNV.Substring(5, 4), currentPrefix);
                    }
                    else
                    {
                        NhanVienVM.NhanVien.MaNV = GetNextId.NextID("", currentPrefix);
                    }
                }
            }





            //if (nhanVien != null)
            //{
            //    var oldPrefix = nhanVien.MaNV.Substring(4);

            //    if(oldPrefix == currentPrefix)
            //    NhanVienVM.NhanVien.MaNV = GetNextId.NextID(nhanVien.MaNV, "00120");
            //}
            //else
            //{
            //    NhanVienVM.NhanVien.MaNV = GetNextId.NextID("", "00120");
            //}

            //NhanVienVM.NoiLamViecs = new List<NoiLamViecViewModel>() { new NoiLamViecViewModel() { Id = 0, Name = "--None--" } };
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
            if (!string.IsNullOrEmpty(model.NgaySinh))
            {
                model.NhanVien.NgaySinh = DateTime.Parse(model.NgaySinh);
            }

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


        public ActionResult Edit(string strUrl, string maNV, string roleName)
        {

            var user = (NhanVien)Session["UserSession"];
            if (user.Role.Equals("Users"))
            {
                return View("~/Views/Shared/AccessDeny.cshtml");
            }

            NhanVienVM.NhanVien = _unitOfWork.nhanVienRepository.GetByStringId(maNV);
            NhanVienVM.KhuVucs = _unitOfWork.khuVucRepository.Find(x => x.VanPhongId == NhanVienVM.NhanVien.KhuVuc.VanPhongId).ToList();
            NhanVienVM.Roles = _unitOfWork.roleRepository.Find(x => x.Name == NhanVienVM.NhanVien.KhuVuc.VanPhong.Role).ToList();
            NhanVienVM.Roles.Add(_unitOfWork.roleRepository.Find(x => x.Name.Equals("Users")).FirstOrDefault());
            //if (user.Role != "Admins")
            //{
            //    NhanVienVM.Roles = _unitOfWork.roleRepository.Find(x => x.Name.Equals(user.Role)).ToList();
            //    NhanVienVM.Roles.Add(_unitOfWork.roleRepository.Find(x => x.Name.Equals("Users")).FirstOrDefault());

            //    // admin khu vuc

            //    //var vanPhongs = _unitOfWork.vanPhongRepository.Find(x => x.Role == user.Role).ToList();
            //    //foreach (var item in vanPhongs)
            //    //{
            //    //    NhanVienVM.KhuVucs = new List<KhuVuc>();
            //    //    NhanVienVM.KhuVucs.AddRange(_unitOfWork.khuVucRepository.Find(x => x.VanPhongId == item.Id).ToList());
            //    //}
            //}
            //else
            //{
            //    //NhanVienVM.KhuVucs = new List<KhuVuc>();
            //}


            //else
            //{
            //    NhanVienVM.KhuVucs = _unitOfWork.khuVucRepository.GetAll().ToList();
            //}
            if (NhanVienVM.NhanVien == null)
            {
                ViewBag.ErrorMessage = "Nhân viên này không tồn tại";
                return View("~/Views/Shared/NotFound.cshtml");
            }
            NhanVienVM.NgaySinh = NhanVienVM.NhanVien.NgaySinh.ToString();
            NhanVienVM.OldPass = NhanVienVM.NhanVien.Password;
            NhanVienVM.NhanVien.Password = "";
            NhanVienVM.StrUrl = strUrl;

            if (!string.IsNullOrEmpty(roleName))
            {
                NhanVienVM.NoiLamViecs = new List<NoiLamViecViewModel>() { new NoiLamViecViewModel() { Id = 0, Name = "--None--" } };
            }

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
            try
            {
                _unitOfWork.nhanVienRepository.Delete(nhanVien);
                _unitOfWork.Complete();
            }
            catch (Exception)
            {
                SetAlert("Xóa không thành công.", "error");
                return Redirect(strUrl);
            }
            
            SetAlert("Xóa thành công.", "success");
            return Redirect(strUrl);
        }

        public JsonResult GetKVByRole(string roleName)
        {
            var user = (NhanVien)Session["UserSession"];

            if (roleName == "Users" || roleName == "Admins")
            {
                if (user.Role != "Admins")
                {
                    // admin khu vuc , chac chan khong phai Users vi Users da duoc redirect Diny View

                    var vanPhongByRoles = _unitOfWork.vanPhongRepository.Find(x => x.Role.Equals(user.Role));
                    //var khuVucByVanPhongs = new List<KhuVuc>();

                    var khuVucByVanPhongs = new List<LoaiThucDonListViewModel>();
                    foreach (var vanPhong in vanPhongByRoles)
                    {
                        var khuVucs = NhanVienVM.KhuVucs.Where(x => x.VanPhongId == vanPhong.Id).ToList();
                        if (khuVucs.Count > 0)
                        {
                            //khuVucByVanPhongs.AddRange(khuVuc);
                            foreach (var khuVuc in khuVucs)
                            {
                                khuVucByVanPhongs.Add(new LoaiThucDonListViewModel() { Id = khuVuc.Id, Name = khuVuc.Name + " - " + khuVuc.VanPhong.Name });
                            }
                        }

                    }
                    return Json(new
                    {
                        data = JsonConvert.SerializeObject(khuVucByVanPhongs)
                    }, JsonRequestBehavior.AllowGet);

                    // admin khu vuc
                    // Users khong duoc quyen tao moi NV
                }
                else
                {
                    var listKVs = NhanVienVM.KhuVucs;
                    var khuVucByVanPhongs = new List<LoaiThucDonListViewModel>();
                    if (listKVs.Count > 0)
                    {
                        //khuVucByVanPhongs.AddRange(khuVuc);
                        foreach (var khuVuc in listKVs)
                        {
                            khuVucByVanPhongs.Add(new LoaiThucDonListViewModel() { Id = khuVuc.Id, Name = khuVuc.Name + " - " + khuVuc.VanPhong.Name });
                        }
                    }
                    return Json(new
                    {
                        data = JsonConvert.SerializeObject(khuVucByVanPhongs)
                    }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                var vanPhongByRoles = _unitOfWork.vanPhongRepository.Find(x => x.Role.Equals(roleName));
                //var khuVucByVanPhongs = new List<KhuVuc>();
                var khuVucByVanPhongs = new List<LoaiThucDonListViewModel>();
                foreach (var vanPhong in vanPhongByRoles)
                {
                    var khuVucs = NhanVienVM.KhuVucs.Where(x => x.VanPhongId == vanPhong.Id).ToList();
                    if (khuVucs.Count > 0)
                    {
                        //khuVucByVanPhongs.AddRange(khuVuc);
                        foreach (var khuVuc in khuVucs)
                        {
                            khuVucByVanPhongs.Add(new LoaiThucDonListViewModel() { Id = khuVuc.Id, Name = khuVuc.Name + " - " + khuVuc.VanPhong.Name });
                        }
                    }

                }
                return Json(new
                {
                    data = JsonConvert.SerializeObject(khuVucByVanPhongs)
                }, JsonRequestBehavior.AllowGet);

            }

        }
        public JsonResult GetNextMaNV(int idKV)
        {
            var vanPhongId = _unitOfWork.khuVucRepository.GetById(idKV).VanPhongId;
            var yearPrefix = DateTime.Now.Year.ToString().Substring(2, 2);
            var currentPrefix = _unitOfWork.vanPhongRepository.GetById(vanPhongId).MaVP + yearPrefix;

            var nhanViens = _unitOfWork.nhanVienRepository.GetAll().OrderByDescending(x => x.MaNV);
            var listOldNVTrung = new List<NhanVien>();
            foreach (var nv in nhanViens)
            {
                var oldPrefix = nv.MaNV.Substring(0, 5);
                if (currentPrefix == oldPrefix)
                {
                    listOldNVTrung.Add(nv);
                }
            }
            if (listOldNVTrung.Count() != 0)
            {
                var lastMaNV = listOldNVTrung.OrderByDescending(x => x.MaNV).FirstOrDefault();
                return Json(new
                {
                    status = true,
                    data = GetNextId.NextNVID(lastMaNV.MaNV, currentPrefix)
                }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new
                {
                    status = true,
                    data = GetNextId.NextNVID("", currentPrefix)
                }, JsonRequestBehavior.AllowGet);
            }
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
        
        private List<NoiLamViecViewModel> ListPhongBan()
        {
            return new List<NoiLamViecViewModel>()
            {
                new NoiLamViecViewModel() { Id = 1, Name = "Bàn" },
                new NoiLamViecViewModel() { Id = 2, Name = "Quản Lý" }
            };
        }

        private List<NoiLamViecViewModel> ListNoiLamViec()
        {
            return new List<NoiLamViecViewModel>()
            {
                new NoiLamViecViewModel() { Id = 0, Name = "--None--" },
                new NoiLamViecViewModel() { Id = 1, Name = "Bếp" },
                new NoiLamViecViewModel() { Id = 2, Name = "Pha chế" }
            };
        }
        private List<NoiLamViecViewModel> ListChucVu()
        {
            return new List<NoiLamViecViewModel>()
            {
                new NoiLamViecViewModel() { Id = 0, Name = "Thu Ngân" },
                new NoiLamViecViewModel() { Id = 1, Name = "Tiếp Tân" },
                new NoiLamViecViewModel() { Id = 2, Name = "Quản Lý" },
                new NoiLamViecViewModel() { Id = 3, Name = "Pha Chế" },
                new NoiLamViecViewModel() { Id = 4, Name = "Tảo Hổ" }
            };
        }

    }
}