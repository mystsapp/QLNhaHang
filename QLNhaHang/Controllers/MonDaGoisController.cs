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
    public class MonDaGoisController : Controller
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
                HoaDon = new HoaDon(),
                ChiTietHD = new ChiTietHD()
            };
        }
        // GET: MonDaGois
        public ActionResult GoiMon(string maBan = null, string strUrl = null, int maTD = 0, int soLuong = 0)
        {
            MonDaGoiVM.ThucDons = _unitOfWork.thucDonRepository.GetAll().OrderBy(x => x.Id).ToList();
            TempData["thucDon"] = MonDaGoiVM.ThucDons;
            MonDaGoiVM.StrUrl = strUrl;
            MonDaGoiVM.MonDaGois = _unitOfWork.monDaGoiRepository
                                              .GetAllInclude(x => x.Ban, y => y.ThucDon)
                                              .Where(x => x.MaBan.Equals(maBan))
                                              .OrderBy(x => x.ThucDon.TenMon)
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
                if(loaiTD.PhuPhi != null)
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
                                                         .Where(x => x.ThucDonId.Equals(model.MonDaGoi.ThucDonId))
                                                         .FirstOrDefault();
            /////////////// update mon if exist ///////////
            if(monDaGoi != null)
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
            return RedirectToAction(nameof(GoiMon), new { maBan = maBan, strUrl = strUrl});
        }

        [HttpPost]
        public ActionResult Delete(int id, string strUrl, string maBan)
        {
            var monDaGoi = _unitOfWork.monDaGoiRepository.GetById(id);
            _unitOfWork.monDaGoiRepository.Delete(monDaGoi);
            /////////////////// off flag ban if null mon'/////////////
            var monInBan = _unitOfWork.monDaGoiRepository.Find(x => x.MaBan == maBan);
            if(monInBan.Count() == 0)
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
            MonDaGoiVM.StrUrl = strUrl;
            MonDaGoiVM.MonDaGois = _unitOfWork.monDaGoiRepository
                                              .FindIncludeTwo(x => x.Ban, y => y.ThucDon, z => z.MaBan.Equals(maBan))
                                              .ToList();
            MonDaGoiVM.Ban = _unitOfWork.banRepository.GetByStringId(maBan);
            MonDaGoiVM.TongTien = MonDaGoiVM.MonDaGois.Select(x => x.ThanhTien).Sum();

            // get last numberId in HD table find next  here
            // prefix in VPandYear
            var hoaDon =_unitOfWork.hoaDonRepository.GetAll()
                                                            .OrderByDescending(x => x.NumberId)
                                                            .FirstOrDefault();
            if(hoaDon != null)
            {
                MonDaGoiVM.NumberId = GetNextId.NextID(hoaDon.NumberId, "00120");
            }
            else
            {
                MonDaGoiVM.NumberId = GetNextId.NextID("", "00120");
            }

            return View(MonDaGoiVM);
        }

        /// tinh tien post => add HD, CTHD, remove Ban flag, MonDaGoi clear
        public ActionResult TinhTienPost(string strUrl, string maBan, string numberId)
        {
            //var user = Session[]
            MonDaGoiVM.MonDaGois = _unitOfWork.monDaGoiRepository
                                              .FindIncludeTwo(x => x.Ban, y => y.ThucDon, z => z.MaBan.Equals(maBan))
                                              .ToList();
            MonDaGoiVM.Ban = _unitOfWork.banRepository.GetByStringId(maBan);
            //////////// add to HD, CTHD ///////////////
            /////maHD

            //var lastMaHD = _unitOfWork.hoaDonRepository
            //                          .Find(x => x.MaNV.Equals(user.HoTen))
            //                          .OrderByDescending(x => x.MaHD).FirstOrDefault().MaHD;
            //var lastMaCTPrefix = lastMaHD.Split('/')[0];

            //var maVP = _unitOfWork.vanPhongRepository.Find(x => x.Name == user.VanPhong).SingleOrDefault().MaVP;
            //var namTao = DateTime.Now.Year.ToString().Substring(2);
            //var nowPrefixMaCT = maVP + namTao;

            //if (lastMaCTPrefix == nowPrefixMaCT)
            //{
            //    var prefixMaCT = lastMaCTPrefix + "/";
            //    MonDaGoiVM.HoaDon.MaHD = GetNextId.NextID(lastMaHD, prefixMaCT);
            //}
            //else
            //{
            //    var prefixMaCT = nowPrefixMaCT + "/";
            //    MonDaGoiVM.HoaDon.MaHD = GetNextId.NextID("", prefixMaCT);
            //}

            var hoaDon = _unitOfWork.hoaDonRepository.GetAll()
                                                            .OrderByDescending(x => x.NumberId)
                                                            .FirstOrDefault();
            if (hoaDon != null)
            {
                MonDaGoiVM.HoaDon.MaHD = GetNextId.NextID(hoaDon.MaHD, "00120");
            }
            else
            {
                MonDaGoiVM.HoaDon.MaHD = GetNextId.NextID("", "00120");
            }
            MonDaGoiVM.HoaDon.NumberId = numberId;
            MonDaGoiVM.HoaDon.MaNV = "001200001";
            MonDaGoiVM.HoaDon.MaKH = "001200001";
            MonDaGoiVM.HoaDon.MaBan = maBan;
            MonDaGoiVM.HoaDon.NgayTao = DateTime.Now;
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