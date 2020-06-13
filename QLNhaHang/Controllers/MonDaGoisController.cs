using CrystalDecisions.CrystalReports.Engine;
using QLNhaHang.Data.Models;
using QLNhaHang.Data.Repositories;
using QLNhaHang.Models;
using QLNhaHang.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNhaHang.Controllers
{
    public class MonDaGoisController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public MonDaGoiViewModel MonDaGoiVM { get; set; }
        public MonDaGoisController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            MonDaGoiVM = new MonDaGoiViewModel()
            {
                Ban = new Data.Models.Ban(),
                MonDaGoi = new Data.Models.MonDaGoi(),
                LoaiThucDons = _unitOfWork.loaiThucDonRepository.GetAll().OrderBy(x => x.Id),
                VanPhongs = _unitOfWork.vanPhongRepository.GetAll().ToList(),
                MonDaGois = new List<MonDaGoi>(),
                HoaDon = new HoaDon(),
                ChiTietHD = new ChiTietHD(),
                VanPhong = new VanPhong(),
                NhanVien = new NhanVien()
            };
        }
        // GET: MonDaGois
        public ActionResult GoiMon(string maBan = null, string strUrl = null, int maTD = 0, int soLuong = 0, int ddlLoai = 0)
        {
            var user = (NhanVien)Session["UserSession"];
            MonDaGoiVM.ThucDons = _unitOfWork.thucDonRepository.GetAll().OrderBy(x => x.Id).ToList();
            
            ////////////// theo Role ////////////////
            if (user.Role != "Admins")
            {
                if (user.Role == "Users")
                {
                    MonDaGoiVM.ThucDons = MonDaGoiVM.ThucDons.Where(x => x.VanPhong == user.KhuVuc.VanPhong.Name && 
                                                                    !x.LoaiThucDon.TenLoai.ToLower().Contains("buffet") && 
                                                                    x.MaLoaiId == MonDaGoiVM.LoaiThucDons.FirstOrDefault().Id).ToList();
                    if (MonDaGoiVM.ThucDons.Count == 0)
                    {
                        MonDaGoiVM.ThucDons.Add(new ThucDon { Id = 0, TenMon = "Chưa có món nào." });
                    }
                }
                else
                {
                    var vanPhongs = _unitOfWork.vanPhongRepository.Find(x => x.Role == user.Role).ToList();
                    List<ThucDon> thucDons = new List<ThucDon>();
                    foreach (var item in vanPhongs)
                    {
                        thucDons.AddRange(_unitOfWork.thucDonRepository.Find(x => x.VanPhong == item.Name));
                    }
                    MonDaGoiVM.ThucDons = thucDons;
                    if (MonDaGoiVM.ThucDons.Count == 0)
                    {
                        MonDaGoiVM.ThucDons = new List<ThucDon>();
                    }
                }
            }
            ////////////// theo Role ////////////////
            ///////////////// theo Loai ////////////////
            if (ddlLoai != 0)
            {
                ViewBag.idLoai = ddlLoai;
                var listTD = _unitOfWork.thucDonRepository.Find(x => x.MaLoaiId == ddlLoai && x.VanPhong == user.KhuVuc.VanPhong.Name).ToList();
                if (listTD.Count != 0)
                {
                    MonDaGoiVM.ThucDons = listTD;
                }
                else
                {
                    MonDaGoiVM.thucDonListIsNull = "Loại này chưa có thực đơn nào";
                    MonDaGoiVM.ThucDons = new List<ThucDon>() { new ThucDon { Id = 0, TenMon = "Chưa có món nào." } };
                }
            }
            ///////////////// theo Loai ////////////////
            TempData["thucDon"] = MonDaGoiVM.ThucDons;
            MonDaGoiVM.StrUrl = strUrl;
            MonDaGoiVM.MonDaGois = _unitOfWork.monDaGoiRepository
                                              .GetAllInclude(x => x.Ban, y => y.ThucDon)
                                              .Where(x => x.MaBan == maBan)
                                              .OrderBy(x => x.LanGui)
                                              .ToList();

            if (!string.IsNullOrEmpty(maBan))
            {
                MonDaGoiVM.MonDaGoi.MaBan = maBan;
                MonDaGoiVM.Ban = _unitOfWork.banRepository.GetByStringId(maBan);
            }

            if (maTD != 0)
            {
                MonDaGoiVM.MonDaGoi.ThucDonId = maTD;
                MonDaGoiVM.MonDaGoi.GiaTien = _unitOfWork.thucDonRepository.GetById(maTD).GiaTien;
            }
            else
            {
                var tD = MonDaGoiVM.ThucDons.FirstOrDefault();
                MonDaGoiVM.MonDaGoi.GiaTien = tD.GiaTien;
                maTD = tD.Id;
            }
            MonDaGoiVM.ThucDon = _unitOfWork.thucDonRepository.GetById(maTD);
            if (soLuong == 0)
            {
                MonDaGoiVM.MonDaGoi.SoLuong = soLuong = 1;
            }
            if (soLuong != 0)
            {
                if (maTD != 0)
                {
                    var loaiTD = _unitOfWork.thucDonRepository.GetById(maTD).LoaiThucDon;
                    if (loaiTD.PhuPhi != null)
                    {
                        MonDaGoiVM.MonDaGoi.PhuPhi = loaiTD.PhuPhi * soLuong;
                    }
                    else
                    {
                        MonDaGoiVM.MonDaGoi.PhuPhi = 0;
                    }
                }

                MonDaGoiVM.MonDaGoi.ThanhTien = MonDaGoiVM.MonDaGoi.GiaTien * soLuong;

                MonDaGoiVM.MonDaGoi.SoLuong = soLuong;

            }
            return View(MonDaGoiVM);
        }

        [HttpPost]
        public ActionResult GoiMon(MonDaGoiViewModel model, string maBan = null, string strUrl = null)
        {
            var monDaGoi = _unitOfWork.monDaGoiRepository.Find(x => x.MaBan.Equals(maBan))
                                                         .Where(x => x.ThucDonId.Equals(model.MonDaGoi.ThucDonId) && !x.DaGui)
                                                         .FirstOrDefault();

            /////////////// update mon if exist ///////////
            if (monDaGoi != null)
            {
                model.MonDaGoi.SoLuong += monDaGoi.SoLuong;
                model.MonDaGoi.PhuPhi += monDaGoi.PhuPhi;
                model.MonDaGoi.ThanhTien += monDaGoi.ThanhTien;
            }
            /////////////// update mon if exist ///////////
            _unitOfWork.monDaGoiRepository.Create(model.MonDaGoi);

            if (monDaGoi != null)
            {
                _unitOfWork.monDaGoiRepository.Delete(monDaGoi);

            }
            //////////// update flag and total money ///////
            var banFromDb = _unitOfWork.banRepository.GetByStringId(maBan);
            banFromDb.Flag = true;

            _unitOfWork.banRepository.Update(banFromDb);
            //////////// update flag and total money ///////
            _unitOfWork.Complete();
            SetAlert("Thêm mới thành công!", "success");
            return RedirectToAction(nameof(GoiMon), new { maBan = maBan, strUrl = strUrl });
        }

        [HttpPost]
        public ActionResult Delete(int id, string strUrl, string maBan)
        {
            var monDaGoi = _unitOfWork.monDaGoiRepository.GetById(id);
            _unitOfWork.monDaGoiRepository.Delete(monDaGoi);
            /////////////////// off flag ban if null mon'/////////////
            var monInBan = _unitOfWork.monDaGoiRepository.Find(x => x.MaBan == maBan);
            if (monInBan.Count() == 0)
            {
                var banById = _unitOfWork.banRepository.GetByStringId(maBan);
                banById.Flag = false;
                _unitOfWork.banRepository.Update(banById);

            }
            /////////////////// off flag ban if null mon'/////////////
            _unitOfWork.Complete();
            SetAlert("Xóa thành công!", "success");
            return RedirectToAction(nameof(GoiMon), new { maBan = maBan, strUrl = strUrl });
        }

        /// tinh tien
        public ActionResult TinhTien(string maBan = null, string strUrl = null)
        {
            var user = (NhanVien)Session["UserSession"];
            MonDaGoiVM.StrUrl = strUrl;
            MonDaGoiVM.MonDaGois = _unitOfWork.monDaGoiRepository
                                              .FindIncludeTwo(x => x.Ban, y => y.ThucDon, z => z.MaBan == maBan)
                                              .OrderBy(x => x.ThucDon.TenMon)
                                              .ToList();

            /////// cong don /////////////
            var arrayList = MonDaGoiVM.MonDaGois.ToArray();
        quaylai:
            for (int i = 0; i < arrayList.Length; i++)
            {
                if (i < arrayList.Length - 1)
                {
                    if (arrayList[i].ThucDonId == arrayList[i + 1].ThucDonId)
                    {
                        arrayList[i].SoLuong = arrayList[i].SoLuong + arrayList[i + 1].SoLuong;
                        arrayList[i].PhuPhi = arrayList[i].PhuPhi + arrayList[i + 1].PhuPhi;
                        arrayList[i].ThanhTien = arrayList[i].ThanhTien + arrayList[i + 1].ThanhTien;
                        arrayList = arrayList.Where(val => val != arrayList[i + 1]).ToArray();
                        goto quaylai;

                    }

                }
            }
            MonDaGoiVM.MonDaGois = arrayList.ToList();
            /////// cong don /////////////

            MonDaGoiVM.Ban = _unitOfWork.banRepository.GetByStringId(maBan);
            var vanPhongId = MonDaGoiVM.Ban.KhuVuc.VanPhongId;
            ///// get VP by Ban
            MonDaGoiVM.VanPhong = _unitOfWork.vanPhongRepository.Find(x => x.Id == vanPhongId).FirstOrDefault();
            MonDaGoiVM.NhanVien = _unitOfWork.nhanVienRepository.GetByStringId(user.MaNV);
            MonDaGoiVM.TongTien = MonDaGoiVM.MonDaGois.Select(x => x.ThanhTien).Sum();

            // get last numberId in HD table find next  here
            var yearPrefix = DateTime.Now.Year.ToString().Substring(2, 2);
            // MaVP by ban
            //var currentPrefix = user.KhuVuc.VanPhong.MaVP + yearPrefix + MonDaGoiVM.Ban.MaSo;
            var currentPrefix = MonDaGoiVM.Ban.KhuVuc.VanPhong.MaVP + yearPrefix + MonDaGoiVM.Ban.MaSo;

            var hoaDon = _unitOfWork.hoaDonRepository.GetAll().OrderByDescending(x => x.NumberId);
            var listOldHDTrung = new List<HoaDon>();
            foreach (var hd in hoaDon)
            {
                var oldPrefix = hd.NumberId.Substring(0, 9);
                if (currentPrefix == oldPrefix)
                {
                    listOldHDTrung.Add(hd);
                }
            }
            if (listOldHDTrung.Count() != 0)
            {
                var lastNumId = listOldHDTrung.OrderByDescending(x => x.NumberId).FirstOrDefault();
                var lastId = lastNumId.NumberId.Substring(9, 7);
                MonDaGoiVM.NumberId = GetNextId.NextNumID(lastId, currentPrefix);
            }
            else
            {
                MonDaGoiVM.NumberId = GetNextId.NextNumID("", currentPrefix);
            }
            // prefix in VPandYear
            //var hoaDon = _unitOfWork.hoaDonRepository.GetAll()
            //                                                .OrderByDescending(x => x.NumberId)
            //                                                .FirstOrDefault();
            //if (hoaDon != null)
            //{
            //    MonDaGoiVM.NumberId = GetNextId.NextID(hoaDon.NumberId, "00120");
            //}
            //else
            //{
            //    MonDaGoiVM.NumberId = GetNextId.NextID("", "00120");
            //}

            return View(MonDaGoiVM);
        }

        /// tinh tien post => add HD, CTHD, remove Ban flag, MonDaGoi clear
        public ActionResult TinhTienPost(string strUrl, string maBan, string numberId)
        {
            //var user = Session[]
            var user = (NhanVien)Session["UserSession"];

            MonDaGoiVM.MonDaGois = _unitOfWork.monDaGoiRepository
                                              .FindIncludeTwo(x => x.Ban, y => y.ThucDon, z => z.MaBan == maBan)
                                              .ToList();
            // đã có người tính trước đó rồi
            if (MonDaGoiVM.MonDaGois == null)
            {
                return Redirect(strUrl);
            }
            /////// cong don /////////////
            var arrayList = MonDaGoiVM.MonDaGois.ToArray();
        quaylai:
            for (int i = 0; i < arrayList.Length; i++)
            {
                if (i < arrayList.Length - 1)
                {
                    if (arrayList[i].ThucDonId == arrayList[i + 1].ThucDonId)
                    {
                        arrayList[i].SoLuong = arrayList[i].SoLuong + arrayList[i + 1].SoLuong;
                        arrayList[i].PhuPhi = arrayList[i].PhuPhi + arrayList[i + 1].PhuPhi;
                        arrayList[i].ThanhTien = arrayList[i].ThanhTien + arrayList[i + 1].ThanhTien;
                        arrayList = arrayList.Where(val => val != arrayList[i + 1]).ToArray();
                        goto quaylai;

                    }

                }
            }
            MonDaGoiVM.MonDaGois = arrayList.ToList();
            /////// cong don /////////////

            MonDaGoiVM.Ban = _unitOfWork.banRepository.GetByStringId(maBan);
            //////////// add to HD, CTHD ///////////////
            /////maHD
            var yearPrefix = DateTime.Now.Year.ToString().Substring(2, 2);

            // MaVP by ban
            //var currentPrefix = user.KhuVuc.VanPhong.MaVP + yearPrefix;
            var currentPrefix = MonDaGoiVM.Ban.KhuVuc.VanPhong.MaVP + yearPrefix;

            var hoaDon = _unitOfWork.hoaDonRepository.GetAll().OrderByDescending(x => x.MaHD);
            var listOldHDTrung = new List<HoaDon>();
            foreach (var hd in hoaDon)
            {
                var oldPrefix = hd.MaHD.Substring(0, 5);
                if (currentPrefix == oldPrefix)
                {
                    listOldHDTrung.Add(hd);
                }
            }
            if (listOldHDTrung.Count() != 0)
            {
                var lastMaHD = listOldHDTrung.OrderByDescending(x => x.MaHD).FirstOrDefault();
                MonDaGoiVM.HoaDon.MaHD = GetNextId.NextNumID(lastMaHD.MaHD.Substring(5, 7), currentPrefix);
            }
            else
            {
                MonDaGoiVM.HoaDon.MaHD = GetNextId.NextNumID("", currentPrefix);
            }

            //var hoaDon = _unitOfWork.hoaDonRepository.GetAll()
            //                                                .OrderByDescending(x => x.MaHD)
            //                                                .FirstOrDefault();
            //if (hoaDon != null)
            //{
            //    MonDaGoiVM.HoaDon.MaHD = GetNextId.NextID(hoaDon.MaHD, "00120");
            //}
            //else
            //{
            //    MonDaGoiVM.HoaDon.MaHD = GetNextId.NextID("", "00120");
            //}
            MonDaGoiVM.HoaDon.NumberId = numberId;
            MonDaGoiVM.HoaDon.MaNV = user.MaNV;
            //MonDaGoiVM.HoaDon.MaKH = "001200001";
            MonDaGoiVM.HoaDon.TenKH = "Khách Lẽ";
            MonDaGoiVM.HoaDon.MaBan = maBan;
            MonDaGoiVM.HoaDon.NgayTao = DateTime.Now;
            MonDaGoiVM.HoaDon.HTThanhToan = "TM/CK";
            // VP by ban
            //MonDaGoiVM.HoaDon.VanPhongId = user.KhuVuc.VanPhongId;
            MonDaGoiVM.HoaDon.VanPhongId = MonDaGoiVM.Ban.KhuVuc.VanPhongId;
            MonDaGoiVM.HoaDon.ThanhTienHD = MonDaGoiVM.MonDaGois.Select(x => x.ThanhTien).Sum();

            // ghi log
            MonDaGoiVM.HoaDon.LogFile = MonDaGoiVM.HoaDon.LogFile + "-User tạo: " + user.Username + " vào lúc: " + System.DateTime.Now.ToString();

            _unitOfWork.hoaDonRepository.Create(MonDaGoiVM.HoaDon);
            _unitOfWork.Complete();
            /////maHD
            /// CTHD
            foreach (var monDaGoi in MonDaGoiVM.MonDaGois)
            {
                MonDaGoiVM.ChiTietHD.MaHD = MonDaGoiVM.HoaDon.MaHD;
                MonDaGoiVM.ChiTietHD.MaThucDon = monDaGoi.ThucDonId;
                MonDaGoiVM.ChiTietHD.DonGia = monDaGoi.GiaTien;
                MonDaGoiVM.ChiTietHD.SoLuong = monDaGoi.SoLuong;

                _unitOfWork.chiTietHDRepository.Create(MonDaGoiVM.ChiTietHD);
                _unitOfWork.Complete();
            }
            /// CTHD
            /////////////// add to HD, CTHD ///////////////
            /// clear MonDaGois <-> Ban Flag
            _unitOfWork.monDaGoiRepository.DeleteRange(MonDaGoiVM.MonDaGois);
            MonDaGoiVM.Ban.Flag = false;
            _unitOfWork.banRepository.Update(MonDaGoiVM.Ban);
            _unitOfWork.Complete();
            /// /// clear MonDaChons <-> Ban Flag
            return Redirect(strUrl);
        }


        public ActionResult GuiYeuCau(string strUrl, string maBan)
        {
            var monChuaGois = _unitOfWork.monDaGoiRepository.Find(x => x.MaBan.Equals(maBan) && !x.DaGui).ToList();

            ///// tim kiem xem truoc do' da gui lan nao chua
            var monDaGuis = _unitOfWork.monDaGoiRepository.Find(x => x.MaBan.Equals(maBan) && x.DaGui);
            int lanGuiSau = 0;
            if (monDaGuis.Count() != 0)
            {
                lanGuiSau = monDaGuis.OrderByDescending(x => x.LanGui).FirstOrDefault().LanGui;
            }

            foreach (var mon in monChuaGois)
            {
                mon.DaGui = true;

                // chua gui
                if (lanGuiSau == 0)
                {
                    mon.LanGui = 1;
                }
                // gui roi
                else
                {
                    mon.LanGui = lanGuiSau + 1;
                }

                _unitOfWork.monDaGoiRepository.Update(mon);
                _unitOfWork.Complete();
            }

            return Redirect(strUrl);
        }

        ///////// Tiec Buffet //////////////////////////////////////////////////////////////////////////////////////////
        public ActionResult TiecBuffet(int maTD = 0, int soLuong = 0, string vanPhongName = null)
        {
            MonDaGoiVM.StrUrl = Request.Url.AbsoluteUri.ToString();
            var user = (NhanVien)Session["UserSession"];
            MonDaGoiVM.ThucDons = _unitOfWork.thucDonRepository.GetAll().ToList().Where(x => x.LoaiThucDon.TenLoai.ToLower().Contains("buffet")).OrderBy(x => x.Id).ToList();
            
            // for title
            ViewBag.vanPhongName = MonDaGoiVM.VPName = user.KhuVuc.VanPhong.Name;
            //// mons 
            var monDaGois = _unitOfWork.monDaGoiRepository
                                             .GetAllInclude(x => x.Ban, y => y.ThucDon)
                                             .Where(x => x.VanPhong == user.KhuVuc.VanPhong.Name
                                             && x.ThucDon.LoaiThucDon.TenLoai.ToLower().Contains("buffet"))
                                             .OrderBy(x => x.LanGui)
                                             .ToList();

            if (monDaGois.Count != 0)
            {
                MonDaGoiVM.MonDaGois.AddRange(monDaGois);
            }

            MonDaGoiVM.ThucDons = MonDaGoiVM.ThucDons.Where(x => x.VanPhong == user.KhuVuc.VanPhong.Name &&
                                                            x.LoaiThucDon.TenLoai.ToLower().Contains("buffet")).ToList();
            if (MonDaGoiVM.ThucDons.Count == 0)
            {
                MonDaGoiVM.ThucDons.Add(new ThucDon { Id = 0, TenMon = "Chưa có món nào." });
            }
            ////////////// vp by Role ////////////////
            if(user.Role != "Admins")
            {
                if(user.Role == "Users")
                {
                    MonDaGoiVM.VanPhongs = MonDaGoiVM.VanPhongs.Where(x => x.Id == user.KhuVuc.VanPhongId).ToList();
                }
                else
                {

                    MonDaGoiVM.VanPhongs = MonDaGoiVM.VanPhongs.Where(x => x.Role == user.Role).ToList();
                }
            }
            ////////////// vp by Role ////////////////
            ///////////// chon VP --> Admin /////////////////
            if (!string.IsNullOrEmpty(vanPhongName))
            {
                ViewBag.vanPhongName = MonDaGoiVM.VPName = vanPhongName;
                // for Tbl
                MonDaGoiVM.MonDaGois = _unitOfWork.monDaGoiRepository
                                         .GetAllInclude(x => x.Ban, y => y.ThucDon)
                                         .Where(x => x.VanPhong == vanPhongName && x.ThucDon.LoaiThucDon.TenLoai.ToLower().Contains("buffet"))
                                         .OrderBy(x => x.LanGui)
                                         .ToList();
                // dropdown thucdon - goi lai getall()
                MonDaGoiVM.ThucDons = _unitOfWork.thucDonRepository.Find(x => x.LoaiThucDon.TenLoai.ToLower().Contains("buffet")).OrderBy(x => x.Id).ToList();
                MonDaGoiVM.ThucDons = MonDaGoiVM.ThucDons.Where(x => x.VanPhong == vanPhongName).ToList();
                // for title
                MonDaGoiVM.VPName = vanPhongName;
            }
            ///////////// chon VP --> Admin /////////////////
            TempData["thucDon"] = MonDaGoiVM.ThucDons;



            if (maTD != 0)
            {
                MonDaGoiVM.MonDaGoi.ThucDonId = maTD;
                MonDaGoiVM.MonDaGoi.GiaTien = _unitOfWork.thucDonRepository.GetById(maTD).GiaTien;
            }
            else
            {
                // VP phai co tiec buffet
                if (MonDaGoiVM.ThucDons.Count() != 0)
                {
                    var tD = MonDaGoiVM.ThucDons.FirstOrDefault();
                    MonDaGoiVM.MonDaGoi.GiaTien = tD.GiaTien;
                    maTD = tD.Id;
                }

            }
            //MonDaGoiVM.ThucDon = _unitOfWork.thucDonRepository.GetById(maTD);
            // moi load vao
            if (soLuong == 0)
            {
                MonDaGoiVM.MonDaGoi.SoLuong = soLuong = 1;
            }
            // soluong thay doi
            if (soLuong != 0)
            {
                //if (maTD != 0)
                //{
                //    var loaiTD = _unitOfWork.thucDonRepository.GetById(maTD).LoaiThucDon;
                //    if (loaiTD.PhuPhi != null)
                //    {
                //        MonDaGoiVM.MonDaGoi.PhuPhi = loaiTD.PhuPhi * soLuong;
                //    }
                //    else
                //    {
                //        MonDaGoiVM.MonDaGoi.PhuPhi = 0;
                //    }
                //}

                ViewBag.vanPhongName = vanPhongName;
                MonDaGoiVM.MonDaGoi.ThanhTien = MonDaGoiVM.MonDaGoi.GiaTien * soLuong;

                MonDaGoiVM.MonDaGoi.SoLuong = soLuong;

            }
            return View(MonDaGoiVM);
        }

        [HttpPost]
        public ActionResult TiecBuffet(MonDaGoiViewModel model)
        {
            model.MonDaGoi.VanPhong = model.VPName;
            /////// lay het tat cac mon buffet theo VP truoc --> so sanh ///////
            var monDaGoi = _unitOfWork.monDaGoiRepository.Find(x => x.VanPhong == model.VPName)
                                                      .Where(x => x.ThucDonId.Equals(model.MonDaGoi.ThucDonId) && !x.DaGui)
                                                      .FirstOrDefault();

            /////////////// update mon if exist ///////////
            if (monDaGoi != null)
            {
                model.MonDaGoi.SoLuong += monDaGoi.SoLuong;
                model.MonDaGoi.PhuPhi += monDaGoi.PhuPhi;
                model.MonDaGoi.ThanhTien += monDaGoi.ThanhTien;
            }
            /////////////// update mon if exist ///////////
            _unitOfWork.monDaGoiRepository.Create(model.MonDaGoi);

            if (monDaGoi != null)
            {
                _unitOfWork.monDaGoiRepository.Delete(monDaGoi);

            }

            _unitOfWork.Complete();
            SetAlert("Thêm mới thành công!", "success");
            return RedirectToAction(nameof(TiecBuffet), new { /*maBan = model.ThucDon.Id,*/ vanPhongName = model.VPName });
        }


        [HttpPost]
        public ActionResult DeleteBuffet(int id, string vanPhongName)
        {
            var monDaGoi = _unitOfWork.monDaGoiRepository.GetById(id);
            _unitOfWork.monDaGoiRepository.Delete(monDaGoi);

            _unitOfWork.Complete();
            SetAlert("Xóa thành công!", "success");
            return RedirectToAction(nameof(TiecBuffet), new { vanPhongName = vanPhongName });
        }


        /// tinh tien buffet
        public ActionResult TinhTienBuffet(string vanPhongName = null, string strUrl = null)
        {
            MonDaGoiVM.StrUrl = strUrl;
            var user = (NhanVien)Session["UserSession"];

            MonDaGoiVM.MonDaGois = _unitOfWork.monDaGoiRepository
                                              .FindIncludeTwo(x => x.Ban, y => y.ThucDon, z => z.VanPhong == vanPhongName)
                                              .ToList()
                                              .Where(x => x.ThucDon.LoaiThucDon.TenLoai.ToLower().Contains("buffet"))
                                              .OrderBy(x => x.ThucDon.TenMon)
                                              .ToList();

            /////// cong don /////////////
            var arrayList = MonDaGoiVM.MonDaGois.ToArray();
        quaylai:
            for (int i = 0; i < arrayList.Length; i++)
            {
                if (i < arrayList.Length - 1)
                {
                    if (arrayList[i].ThucDonId == arrayList[i + 1].ThucDonId)
                    {
                        arrayList[i].SoLuong = arrayList[i].SoLuong + arrayList[i + 1].SoLuong;
                        arrayList[i].PhuPhi = arrayList[i].PhuPhi + arrayList[i + 1].PhuPhi;
                        arrayList[i].ThanhTien = arrayList[i].ThanhTien + arrayList[i + 1].ThanhTien;
                        arrayList = arrayList.Where(val => val != arrayList[i + 1]).ToArray();
                        goto quaylai;

                    }

                }
            }
            MonDaGoiVM.MonDaGois = arrayList.ToList();
            /////// cong don /////////////
            // VP luc chon mon buffet
            MonDaGoiVM.VanPhong = _unitOfWork.vanPhongRepository.Find(x => x.Name == vanPhongName).FirstOrDefault();
            MonDaGoiVM.NhanVien = _unitOfWork.nhanVienRepository.GetByStringId(user.MaNV);
            MonDaGoiVM.TongTien = MonDaGoiVM.MonDaGois.Select(x => x.ThanhTien).Sum();

            // get last numberId in HD table find next  here
            var yearPrefix = DateTime.Now.Year.ToString().Substring(2, 2);

            // MaVP by vanPhongName
            //var currentPrefix = user.KhuVuc.VanPhong.MaVP + yearPrefix + MonDaGoiVM.Ban.MaSo;
            var currentPrefix = MonDaGoiVM.VanPhong.MaVP + yearPrefix;

            var hoaDon = _unitOfWork.hoaDonRepository.Find(x => x.NumberId.Length == 12).OrderByDescending(x => x.NumberId);
            var listOldHDTrung = new List<HoaDon>();
            foreach (var hd in hoaDon)
            {
                var oldPrefix = hd.NumberId.Substring(0, 5); // var currentPrefix = MonDaGoiVM.VanPhong.MaVP + yearPrefix;
                if (currentPrefix == oldPrefix)
                {
                    listOldHDTrung.Add(hd);
                }
            }
            if (listOldHDTrung.Count() != 0)
            {
                var lastNumId = listOldHDTrung.OrderByDescending(x => x.NumberId).FirstOrDefault();
                var lastId = lastNumId.NumberId.Substring(5, 7);
                MonDaGoiVM.NumberId = GetNextId.NextBuffetNumID(lastId, currentPrefix);
            }
            else
            {
                MonDaGoiVM.NumberId = GetNextId.NextBuffetNumID("", currentPrefix);
            }
            // prefix in VPandYear
            //var hoaDon = _unitOfWork.hoaDonRepository.GetAll()
            //                                                .OrderByDescending(x => x.NumberId)
            //                                                .FirstOrDefault();
            //if (hoaDon != null)
            //{
            //    MonDaGoiVM.NumberId = GetNextId.NextID(hoaDon.NumberId, "00120");
            //}
            //else
            //{
            //    MonDaGoiVM.NumberId = GetNextId.NextID("", "00120");
            //}

            return View(MonDaGoiVM);
        }

        /// tinh tien post => add HD, CTHD, remove Ban flag, MonDaGoi clear
        public ActionResult TinhTienBuffetPost(string vanPhongName, string numberId)
        {
            //var user = Session[]
            var user = (NhanVien)Session["UserSession"];

            MonDaGoiVM.MonDaGois = _unitOfWork.monDaGoiRepository
                                              .FindIncludeTwo(x => x.Ban, y => y.ThucDon, z => z.VanPhong == vanPhongName)
                                              .ToList()
                                              .Where(x => x.ThucDon.LoaiThucDon.TenLoai.ToLower().Contains("buffet"))
                                              .OrderBy(x => x.ThucDon.TenMon)
                                              .ToList();

            // đã có người tính trước đó rồi
            if (MonDaGoiVM.MonDaGois == null)
            {
                return RedirectToAction(nameof(TinhTienBuffet), new { vanPhongName = vanPhongName });
            }
            /////// cong don /////////////
            var arrayList = MonDaGoiVM.MonDaGois.ToArray();
        quaylai:
            for (int i = 0; i < arrayList.Length; i++)
            {
                if (i < arrayList.Length - 1)
                {
                    if (arrayList[i].ThucDonId == arrayList[i + 1].ThucDonId)
                    {
                        arrayList[i].SoLuong = arrayList[i].SoLuong + arrayList[i + 1].SoLuong;
                        arrayList[i].PhuPhi = arrayList[i].PhuPhi + arrayList[i + 1].PhuPhi;
                        arrayList[i].ThanhTien = arrayList[i].ThanhTien + arrayList[i + 1].ThanhTien;
                        arrayList = arrayList.Where(val => val != arrayList[i + 1]).ToArray();
                        goto quaylai;

                    }

                }
            }
            MonDaGoiVM.MonDaGois = arrayList.ToList();
            /////// cong don /////////////

            //MonDaGoiVM.Ban = _unitOfWork.banRepository.GetByStringId(maBan);
            //////////// add to HD, CTHD ///////////////
            /////maHD
            var yearPrefix = DateTime.Now.Year.ToString().Substring(2, 2);
            // mavp by vanPhongName
            //var currentPrefix = user.KhuVuc.VanPhong.MaVP + yearPrefix;
            var currentPrefix = "b" + _unitOfWork.vanPhongRepository.Find(x => x.Name == vanPhongName).FirstOrDefault().MaVP + yearPrefix;

            var hoaDon = _unitOfWork.hoaDonRepository.Find(x => x.MaHD.Length == 13).OrderByDescending(x => x.MaHD);
            var listOldHDTrung = new List<HoaDon>();
            foreach (var hd in hoaDon)
            {
                var oldPrefix = hd.MaHD.Substring(0, 6);
                if (currentPrefix == oldPrefix)
                {
                    listOldHDTrung.Add(hd);
                }
            }
            if (listOldHDTrung.Count() != 0)
            {
                var lastMaHD = listOldHDTrung.OrderByDescending(x => x.MaHD).FirstOrDefault();
                MonDaGoiVM.HoaDon.MaHD = GetNextId.NextNumID(lastMaHD.MaHD.Substring(6, 7), currentPrefix);
            }
            else
            {
                MonDaGoiVM.HoaDon.MaHD = GetNextId.NextNumID("", currentPrefix);
            }

            //var hoaDon = _unitOfWork.hoaDonRepository.GetAll()
            //                                                .OrderByDescending(x => x.MaHD)
            //                                                .FirstOrDefault();
            //if (hoaDon != null)
            //{
            //    MonDaGoiVM.HoaDon.MaHD = GetNextId.NextID(hoaDon.MaHD, "00120");
            //}
            //else
            //{
            //    MonDaGoiVM.HoaDon.MaHD = GetNextId.NextID("", "00120");
            //}
            MonDaGoiVM.HoaDon.NumberId = numberId;
            MonDaGoiVM.HoaDon.MaNV = user.MaNV;
            //MonDaGoiVM.HoaDon.MaKH = "001200001";
            MonDaGoiVM.HoaDon.TenKH = "Khách Lẽ";
            //MonDaGoiVM.HoaDon.MaBan = "Buffet " + vanPhongName;
            MonDaGoiVM.HoaDon.MaBan = _unitOfWork.banRepository.Find(x => x.KhuVucId == user.KhuVucId).FirstOrDefault().MaBan;
            MonDaGoiVM.HoaDon.NgayTao = DateTime.Now;
            MonDaGoiVM.HoaDon.HTThanhToan = "TM/CK";
            // VP by vanPhongName
            //MonDaGoiVM.HoaDon.VanPhongId = user.KhuVuc.VanPhongId;
            MonDaGoiVM.HoaDon.VanPhongId = _unitOfWork.vanPhongRepository.Find(x => x.Name == vanPhongName).FirstOrDefault().Id;
            MonDaGoiVM.HoaDon.ThanhTienHD = MonDaGoiVM.MonDaGois.Select(x => x.ThanhTien).Sum();

            _unitOfWork.hoaDonRepository.Create(MonDaGoiVM.HoaDon);
            _unitOfWork.Complete();
            /////maHD
            /// CTHD
            foreach (var monDaGoi in MonDaGoiVM.MonDaGois)
            {
                MonDaGoiVM.ChiTietHD.MaHD = MonDaGoiVM.HoaDon.MaHD;
                MonDaGoiVM.ChiTietHD.MaThucDon = monDaGoi.ThucDonId;
                MonDaGoiVM.ChiTietHD.DonGia = monDaGoi.GiaTien;
                MonDaGoiVM.ChiTietHD.SoLuong = monDaGoi.SoLuong;

                _unitOfWork.chiTietHDRepository.Create(MonDaGoiVM.ChiTietHD);
                _unitOfWork.Complete();
            }
            /// CTHD
            /////////////// add to HD, CTHD ///////////////
            /// clear MonDaGois
            _unitOfWork.monDaGoiRepository.DeleteRange(MonDaGoiVM.MonDaGois);
            _unitOfWork.Complete();
            /// /// clear MonDaChons
            return RedirectToAction(nameof(TiecBuffet), new { vanPhongName = vanPhongName });
        }

        ///////// Tiec Buffet
    }
}