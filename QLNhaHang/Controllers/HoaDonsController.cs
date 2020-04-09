using CrystalDecisions.CrystalReports.Engine;
using Newtonsoft.Json;
using NumToWords;
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
    public class HoaDonsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HoaDonViewModel HoaDonVM { get; set; }

        public HoaDonsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            HoaDonVM = new HoaDonViewModel()
            {
                HoaDon = new Data.Models.HoaDon(),
                ThongTinHD = new Data.Models.ThongTinHD(),
                KhachHang = new Data.Models.KhachHang(),
                VAT = 10
            };
        }
        // GET: HoaDons
        public ActionResult Index(string maHD = null, string searchString = null,
                                  string searchFromDate = null, string searchToDate = null,
                                  int page = 1)
        {
            HoaDonVM.StrUrl = Request.Url.AbsoluteUri.ToString();
            //////////////////// for delete
            if (!string.IsNullOrEmpty(maHD))
            {
                var hd = _unitOfWork.hoaDonRepository.GetByStringId(maHD);
                if (hd == null)
                {
                    var lastMaHD = _unitOfWork.hoaDonRepository
                                          .GetAllInclude(x => x.NhanVien, y => y.Ban)
                                          .OrderByDescending(x => x.MaHD).FirstOrDefault().MaHD;
                    maHD = lastMaHD;
                }

            }

            ////////////////////

            ViewBag.searchString = searchString;
            ViewBag.searchFromDate = searchFromDate;
            ViewBag.searchToDate = searchToDate;

            HoaDonVM.ChiTietHDs = _unitOfWork.chiTietHDRepository
                                             .FindIncludeTwo(x => x.HoaDon, y => y.ThucDon, z => z.MaHD.Equals(maHD));
            HoaDonVM.TongTien = HoaDonVM.ChiTietHDs.Sum(x => (x.DonGia * x.SoLuong));

            HoaDonVM.HoaDons = _unitOfWork.hoaDonRepository.ListHoaDon(searchString, searchFromDate, searchToDate, page);
            if (HoaDonVM.HoaDons == null)
            {
                HoaDonVM.HoaDons = _unitOfWork.hoaDonRepository.ListHoaDon("", "", "", 1);
                SetAlert("Lỗi định dạng ngày tháng.", "error");
            }
            HoaDonVM.HoaDon = _unitOfWork.hoaDonRepository.GetByStringId(maHD);
            return View(HoaDonVM);
        }

        [HttpPost]
        public ActionResult Delete(string id, string strUrl)
        {
            var hoaDon = _unitOfWork.hoaDonRepository.GetByStringId(id);
            _unitOfWork.hoaDonRepository.Delete(hoaDon);

            _unitOfWork.Complete();
            SetAlert("Xóa thành công!", "success");
            return Redirect(strUrl);
        }

        public ActionResult HoaDonTuDong(decimal ppv = 0, decimal vat = 0, string maHD = null, string strUrl = null, int maThongTinHDId = 0, string maKH = null)
        {

            HoaDonVM.StrUrl = strUrl;
            HoaDonVM.HoaDon = _unitOfWork.hoaDonRepository.GetByStringId(maHD);
            if(ppv != 0)
            {
                HoaDonVM.TyLePPV = ppv;
                HoaDonVM.TienPPV = (ppv / 100 * HoaDonVM.HoaDon.ThanhTienHD).ToString().Split('.')[0];
                HoaDonVM.ThanhTienSauPPV = ( decimal.Parse(HoaDonVM.TienPPV) + HoaDonVM.HoaDon.ThanhTienHD).ToString().Split('.')[0];
                
                if (vat == 0)
                {
                    HoaDonVM.TienThueVAT = (HoaDonVM.VAT / 100 * decimal.Parse(HoaDonVM.ThanhTienSauPPV)).ToString().Split('.')[0];
                    HoaDonVM.ThanhTienVAT = (decimal.Parse(HoaDonVM.TienThueVAT) + HoaDonVM.ThanhTienSauPPV).ToString().Split('.')[0];
                    //HoaDonVM.VAT = HoaDonVM.VAT;

                }
                else
                {
                    HoaDonVM.VAT = vat;
                    HoaDonVM.TienThueVAT = (vat / 100 * decimal.Parse(HoaDonVM.ThanhTienSauPPV)).ToString().Split('.')[0];
                    HoaDonVM.ThanhTienVAT = (decimal.Parse(HoaDonVM.TienThueVAT) + decimal.Parse(HoaDonVM.ThanhTienSauPPV)).ToString().Split('.')[0];
                    
                }
            }
            else
            {
                if (vat == 0)
                {
                    HoaDonVM.TienThueVAT = (HoaDonVM.VAT / 100 * HoaDonVM.HoaDon.ThanhTienHD).ToString().Split('.')[0];
                    HoaDonVM.ThanhTienVAT = (decimal.Parse(HoaDonVM.TienThueVAT) + HoaDonVM.HoaDon.ThanhTienHD).ToString().Split('.')[0];
                    //HoaDonVM.VAT = HoaDonVM.VAT;

                }
                else
                {
                    HoaDonVM.VAT = vat;
                    HoaDonVM.TienThueVAT = (vat / 100 * HoaDonVM.HoaDon.ThanhTienHD).ToString().Split('.')[0];
                    HoaDonVM.ThanhTienVAT = (decimal.Parse(HoaDonVM.TienThueVAT) + HoaDonVM.HoaDon.ThanhTienHD).ToString().Split('.')[0];
                }
            }
            
            
            HoaDonVM.ThongTinHDs = _unitOfWork.thongTinHDRepository.GetAll();
            HoaDonVM.KhachHangs = _unitOfWork.khachHangRepository.GetAll();
            if (maThongTinHDId != 0)
            {
                HoaDonVM.ThongTinHD = _unitOfWork.thongTinHDRepository.GetById(maThongTinHDId);
            }
            if (!string.IsNullOrEmpty(maKH))
            {
                HoaDonVM.KhachHang = _unitOfWork.khachHangRepository.GetByStringId(maKH);
            }
            return View(HoaDonVM);
        }

        [HttpPost, ActionName("HoaDonTuDong")]
        public ActionResult HoaDonTuDongPost(HoaDonViewModel model, string maHD)
        {
            var hoaDon = _unitOfWork.hoaDonRepository
                                    .FindIncludeTwo(x => x.Ban, y => y.NhanVien, z => z.MaHD.Equals(maHD))
                                    .FirstOrDefault();
            //// Khach hang
            hoaDon.TenKH = model.KhachHang.TenKH;
            hoaDon.Phone = model.KhachHang.Phone;
            hoaDon.DiaChi = model.KhachHang.DiaChi;
            hoaDon.TenDonVi = model.KhachHang.TenDonVi;
            hoaDon.MaSoThue = model.KhachHang.MaSoThue;
            //// thong tin HD
            hoaDon.MauSo = model.ThongTinHD.MauSo;
            hoaDon.KyHieu = model.ThongTinHD.KyHieu;
            hoaDon.QuyenSo = model.ThongTinHD.QuyenSo;
            hoaDon.So = model.ThongTinHD.So;
            hoaDon.SoThuTu = model.ThongTinHD.SoThuTu;

            hoaDon.HTThanhToan = model.HoaDon.HTThanhToan;

            hoaDon.TyLePPV = model.TyLePPV;
            hoaDon.PhiPhucvu = model.TyLePPV / 100 * hoaDon.ThanhTienHD;
            hoaDon.TongTienSauPPV = decimal.Parse(model.ThanhTienSauPPV);

            hoaDon.VAT = model.VAT;
            hoaDon.TienThueVAT = model.VAT / 100 * hoaDon.TongTienSauPPV;
            hoaDon.ThanhTienVAT = hoaDon.TienThueVAT + hoaDon.TongTienSauPPV; // Or == model.ThanhTienVAT
            ///// Currency to money
            string s = SoSangChu.DoiSoSangChu(model.ThanhTienVAT);
            string c = AmountToWords.changeCurrencyToWords(hoaDon.ThanhTienVAT.ToString().ToLower());
            //string t = String.IsNullOrEmpty(loaitien) ? "" : " Exchange rate USD/VND";
            hoaDon.SoTienBangChu =  char.ToUpper(s[0]) + s.Substring(1) + " đồng " + " / " + char.ToUpper(c[0]) + c.Substring(1).ToLower() + " Exchange rate USD/VND";

            hoaDon.NgayIn = DateTime.Now;
            hoaDon.DaIn = true;
            ////// update hoa don
            _unitOfWork.hoaDonRepository.Update(hoaDon);
            _unitOfWork.Complete();
            return RedirectToAction(nameof(Export), new { id = hoaDon.MaHD, strUrl = model.StrUrl });
        }

        public ActionResult ExportList(string stringId, string strUrl)
        {

            var idList = JsonConvert.DeserializeObject<List<HoaDonViewModel>>(stringId);
            if (idList.Count == 1)
            {
                return RedirectToAction(nameof(Export), new { id = idList.FirstOrDefault().Id, strUrl = strUrl });
            }
            return View();
        }

        public ActionResult Export(string id, string strUrl)
        {
            var hoaDon = _unitOfWork.hoaDonRepository.FindIncludeThree(id);
            var ctHoaDons = _unitOfWork.chiTietHDRepository
                                       .FindIncludeTwo(x => x.HoaDon, y => y.ThucDon, z => z.MaHD.Equals(id))
                                       .ToList();
            var list = ctHoaDons.Where(item1 => hoaDon.Any(item2 => item1.MaHD == item2.MaHD));
            var items = list.Select(x => new
            {
                x.MaHD,
                x.HoaDon.NumberId,
                x.HoaDon.MauSo,
                x.HoaDon.KyHieu,
                x.HoaDon.SoThuTu,
                x.HoaDon.QuyenSo,
                x.HoaDon.So,
                TenVP = x.HoaDon.VanPhong.Name,
                DiaChiVP = x.HoaDon.VanPhong.DiaChi,
                DienThoaiVP = x.HoaDon.VanPhong.DienThoai,
                MstVP = x.HoaDon.VanPhong.MaSoThue,
                HoTenNV = x.HoaDon.NhanVien.HoTen,
                x.HoaDon.Ban.TenBan,
                x.HoaDon.HTThanhToan,
                x.HoaDon.NgayTao,
                x.HoaDon.GhiChu,
                x.HoaDon.ThanhTienHD,
                x.HoaDon.TyLePPV,
                x.HoaDon.PhiPhucvu,
                x.HoaDon.TongTienSauPPV,
                x.HoaDon.VAT,                
                x.HoaDon.TienThueVAT,
                x.HoaDon.ThanhTienVAT,
                x.HoaDon.SoTienBangChu,
                TenKH = x.HoaDon.TenKH,
                DienThoaiKH = x.HoaDon.Phone,
                DiaChiKH = x.HoaDon.DiaChi,
                TenDonViKH = x.HoaDon.TenDonVi,
                MstKH = x.HoaDon.MaSoThue,
                x.HoaDon.SoTien,
                x.ThucDon.TenMon,
                x.DonGia,
                x.ThucDon.DonViTinh,
                x.SoLuong,
                ThanhTienHH = x.DonGia * x.SoLuong

            });
            var dt = EntityToTable.ToDataTable(items);
            ReportDocument rd = new ReportDocument();
            string reportPath = Path.Combine(Server.MapPath("~/Report"), "DS_HoaDon_Report.rpt");
            return new CrystalReportPdfResult(reportPath, dt);
        }

        public ActionResult HoaDonTay(string soTien, decimal ppv = 0, decimal vat = 0, string maHD = null, string strUrl = null, int maThongTinHDId = 0, string maKH = null)
        {

            //HoaDonVM.VAT = 0;
            HoaDonVM.StrUrl = strUrl;
            HoaDonVM.HoaDon = _unitOfWork.hoaDonRepository.GetByStringId(maHD);
            if (ppv != 0)
            {
                HoaDonVM.TyLePPV = ppv;
                HoaDonVM.TienPPV = (ppv / 100 * decimal.Parse(soTien)).ToString().Split('.')[0];
                HoaDonVM.ThanhTienSauPPV = (decimal.Parse(HoaDonVM.TienPPV) + decimal.Parse(soTien)).ToString().Split('.')[0];
                if (vat == 0)
                {
                    HoaDonVM.TienThueVAT = (HoaDonVM.VAT / 100 * decimal.Parse(HoaDonVM.ThanhTienSauPPV)).ToString().Split('.')[0];
                    HoaDonVM.ThanhTienVAT = (decimal.Parse(HoaDonVM.TienThueVAT) + decimal.Parse(HoaDonVM.ThanhTienSauPPV)).ToString().Split('.')[0];
                    //HoaDonVM.VAT = HoaDonVM.VAT;
                    HoaDonVM.SoTien = soTien;
                }
                else
                {
                    HoaDonVM.VAT = vat;
                    HoaDonVM.TienThueVAT = (vat / 100 * decimal.Parse(HoaDonVM.ThanhTienSauPPV)).ToString().Split('.')[0];
                    HoaDonVM.ThanhTienVAT = (decimal.Parse(HoaDonVM.TienThueVAT) + decimal.Parse(HoaDonVM.ThanhTienSauPPV)).ToString().Split('.')[0];
                    HoaDonVM.SoTien = soTien;
                }
            }
            else
            {
                if (vat == 0)
                {
                    HoaDonVM.TienThueVAT = (HoaDonVM.VAT / 100 * HoaDonVM.HoaDon.ThanhTienHD).ToString().Split('.')[0];
                    HoaDonVM.ThanhTienVAT = (decimal.Parse(HoaDonVM.TienThueVAT) + HoaDonVM.HoaDon.ThanhTienHD).ToString().Split('.')[0];
                    //HoaDonVM.VAT = HoaDonVM.VAT;
                    HoaDonVM.SoTien = soTien;
                }
                else
                {
                    HoaDonVM.VAT = vat;
                    HoaDonVM.TienThueVAT = (vat / 100 * HoaDonVM.HoaDon.ThanhTienHD).ToString().Split('.')[0];
                    HoaDonVM.ThanhTienVAT = (decimal.Parse(HoaDonVM.TienThueVAT) + HoaDonVM.HoaDon.ThanhTienHD).ToString().Split('.')[0];
                    HoaDonVM.SoTien = soTien;

                }
            }
            //if (vat != 0 && !string.IsNullOrEmpty(sotien))
            //{
            //    HoaDonVM.ThanhTienVAT = (vat / 100 * decimal.Parse(sotien) + decimal.Parse(sotien)).ToString().Split('.')[0];
            //    HoaDonVM.VAT = vat;
            //}
            HoaDonVM.ThongTinHDs = _unitOfWork.thongTinHDRepository.GetAll();
            HoaDonVM.KhachHangs = _unitOfWork.khachHangRepository.GetAll();
            if (maThongTinHDId != 0)
            {
                HoaDonVM.ThongTinHD = _unitOfWork.thongTinHDRepository.GetById(maThongTinHDId);
            }
            if (!string.IsNullOrEmpty(maKH))
            {
                HoaDonVM.KhachHang = _unitOfWork.khachHangRepository.GetByStringId(maKH);
            }
            return View(HoaDonVM);
        }

        [HttpPost, ActionName("HoaDonTay")]
        public ActionResult HoaDonTayPost(HoaDonViewModel model, string maHD)
        {
            var hoaDon = _unitOfWork.hoaDonRepository
                                    .FindIncludeTwo(x => x.Ban, y => y.NhanVien, z => z.MaHD.Equals(maHD))
                                    .FirstOrDefault();
            //// Khach hang
            hoaDon.TenKH = model.KhachHang.TenKH;
            hoaDon.Phone = model.KhachHang.Phone;
            hoaDon.DiaChi = model.KhachHang.DiaChi;
            hoaDon.TenDonVi = model.KhachHang.TenDonVi;
            hoaDon.MaSoThue = model.KhachHang.MaSoThue;
            //// thong tin HD
            hoaDon.MauSo = model.ThongTinHD.MauSo;
            hoaDon.KyHieu = model.ThongTinHD.KyHieu;
            hoaDon.QuyenSo = model.ThongTinHD.QuyenSo;
            hoaDon.So = model.ThongTinHD.So;
            hoaDon.SoThuTu = model.ThongTinHD.SoThuTu;

            hoaDon.HTThanhToan = model.HoaDon.HTThanhToan;

            hoaDon.TyLePPV = model.TyLePPV;
            hoaDon.PhiPhucvu = model.TyLePPV / 100 * decimal.Parse(model.SoTien);
            hoaDon.TongTienSauPPV = decimal.Parse(model.ThanhTienSauPPV);

            hoaDon.VAT = model.VAT;
            hoaDon.TienThueVAT = model.VAT / 100 * hoaDon.TongTienSauPPV;
            hoaDon.ThanhTienVAT = hoaDon.TienThueVAT + hoaDon.TongTienSauPPV; // Or == model.ThanhTienVAT

            hoaDon.NgayIn = DateTime.Now;
            hoaDon.DaIn = true;
            hoaDon.SoTien = decimal.Parse(model.SoTien);
            hoaDon.NoiDung = model.NoiDung;

            hoaDon.VAT = model.VAT;
            hoaDon.ThanhTienVAT = decimal.Parse(model.ThanhTienVAT);

            ///// Currency to money
            string s = SoSangChu.DoiSoSangChu(model.ThanhTienVAT);
            string c = AmountToWords.changeCurrencyToWords(hoaDon.ThanhTienVAT.ToString().ToLower());
            //string t = String.IsNullOrEmpty(loaitien) ? "" : " Exchange rate USD/VND";
            hoaDon.SoTienBangChu = char.ToUpper(s[0]) + s.Substring(1) + " đồng " + " / " + char.ToUpper(c[0]) + c.Substring(1).ToLower() + " Exchange rate USD/VND";

            ////// update hoa don
            _unitOfWork.hoaDonRepository.Update(hoaDon);
            _unitOfWork.Complete();
            return RedirectToAction(nameof(ExportHDTay), new { id = hoaDon.MaHD, strUrl = model.StrUrl });
        }

        public ActionResult ExportHDTay(string id, string strUrl)
        {
            var list = _unitOfWork.hoaDonRepository
                                    .GetAllIncludeThree(x => x.Ban, y => y.NhanVien, v => v.VanPhong)
                                    .Where(x => x.MaHD.Equals(id))
                                    .ToList();

            var items = list.Select(x => new
            {
                x.MaHD,
                x.NumberId,
                x.MauSo,
                x.KyHieu,
                x.SoThuTu,
                x.QuyenSo,
                x.So,
                TenVP = x.VanPhong.Name,
                DiaChiVP = x.VanPhong.DiaChi,
                DienThoaiVP = x.VanPhong.DienThoai,
                MstVP = x.VanPhong.MaSoThue,
                HoTenNV = x.NhanVien.HoTen,
                x.Ban.TenBan,
                x.HTThanhToan,
                x.NgayTao,
                x.GhiChu,
                x.ThanhTienHD,
                x.TyLePPV,
                x.PhiPhucvu,
                x.TongTienSauPPV,
                x.VAT,
                x.TienThueVAT,
                x.ThanhTienVAT,
                x.SoTienBangChu,
                TenKH = x.TenKH,
                DienThoaiKH = x.Phone,
                DiaChiKH = x.DiaChi,
                TenDonViKH = x.TenDonVi,
                MstKH = x.MaSoThue,
                x.SoTien,
                x.NoiDung
            });

            var dt = EntityToTable.ToDataTable(items);
            ReportDocument rd = new ReportDocument();
            string reportPath = Path.Combine(Server.MapPath("~/Report"), "DS_HoaDonTay_Report.rpt");
            return new CrystalReportPdfResult(reportPath, dt);
        }

        public JsonResult GetByTTHDId(int id)
        {
            return Json(JsonConvert.SerializeObject(_unitOfWork.thongTinHDRepository.GetById(id)), JsonRequestBehavior.AllowGet);
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