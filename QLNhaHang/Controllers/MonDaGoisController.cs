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
                LoaiThucDons = _unitOfWork.loaiThucDonRepository.GetAll(),
                HoaDon = new HoaDon(),
                ChiTietHD = new ChiTietHD(),
                VanPhong = new VanPhong(),
                NhanVien = new NhanVien()
            };
        }
        // GET: MonDaGois
        public ActionResult GoiMon(string maBan = null, string strUrl = null, int maTD = 0, int soLuong = 0, int ddlLoai = 0)
        {

            MonDaGoiVM.ThucDons = _unitOfWork.thucDonRepository.GetAll().OrderBy(x => x.Id).ToList();

            if (ddlLoai != 0)
            {
                ViewBag.idLoai = ddlLoai;
                var listTD = MonDaGoiVM.ThucDons.Where(x => x.MaLoaiId == ddlLoai).ToList();
                if (listTD.Count != 0)
                {
                    MonDaGoiVM.ThucDons = listTD;
                }
                else
                {
                    MonDaGoiVM.thucDonListIsNull = "Loại này chưa có thực đơn nào";
                }
            }

            TempData["thucDon"] = MonDaGoiVM.ThucDons;
            MonDaGoiVM.StrUrl = strUrl;
            MonDaGoiVM.MonDaGois = _unitOfWork.monDaGoiRepository
                                              .GetAllInclude(x => x.Ban, y => y.ThucDon)
                                              .Where(x => x.MaBan.Equals(maBan))
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
                var loaiTD = _unitOfWork.thucDonRepository.GetById(maTD).LoaiThucDon;
                MonDaGoiVM.MonDaGoi.ThanhTien = MonDaGoiVM.MonDaGoi.GiaTien * soLuong;
                if (loaiTD.PhuPhi != null)
                {
                    MonDaGoiVM.MonDaGoi.PhuPhi = loaiTD.PhuPhi * soLuong;
                }
                else
                {
                    MonDaGoiVM.MonDaGoi.PhuPhi = 0;
                }
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
                                              .FindIncludeTwo(x => x.Ban, y => y.ThucDon, z => z.MaBan.Equals(maBan))
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
            MonDaGoiVM.VanPhong = _unitOfWork.vanPhongRepository.GetById(user.KhuVuc.VanPhongId);
            MonDaGoiVM.NhanVien = _unitOfWork.nhanVienRepository.GetByStringId(user.MaNV);
            MonDaGoiVM.TongTien = MonDaGoiVM.MonDaGois.Select(x => x.ThanhTien).Sum();

            // get last numberId in HD table find next  here
            var yearPrefix = DateTime.Now.Year.ToString().Substring(2, 2);
            var currentPrefix = user.KhuVuc.VanPhong.MaVP + yearPrefix + MonDaGoiVM.Ban.MaSo;

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
                                              .FindIncludeTwo(x => x.Ban, y => y.ThucDon, z => z.MaBan.Equals(maBan))
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
            var currentPrefix = user.KhuVuc.VanPhong.MaVP + yearPrefix;

            var hoaDon = _unitOfWork.hoaDonRepository.GetAll().OrderByDescending(x => x.MaHD);
            var listOldHDTrung = new List<HoaDon>();
            foreach (var hd in hoaDon)
            {
                var oldPrefix = hd.NumberId.Substring(0, 5);
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
            MonDaGoiVM.HoaDon.VanPhongId = user.KhuVuc.VanPhongId;
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


    }
}