using OfficeOpenXml;
using OfficeOpenXml.Style;
using QLNhaHang.Data.Models;
using QLNhaHang.Data.Repositories;
using QLNhaHang.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace QLNhaHang.Controllers
{
    public class ThongKesController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public ThongKeViewModel ThongKeVM { get; set; }
        public ThongKesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            ThongKeVM = new ThongKeViewModel()
            {
                DaInModels = new List<DaInModel>()
                {
                    new DaInModel() { Id = "", Name = "None" },
                    new DaInModel() { Id = "True", Name = "Đã in" },
                    new DaInModel() { Id = "Null", Name = "Chưa in" }
                },
                VanPhongs = _unitOfWork.vanPhongRepository.GetAll().ToList(),
                KhuVucs = _unitOfWork.khuVucRepository.GetAll().ToList(),
                ThucDons = new List<Data.Models.ThucDon>(),
                NhanViens = _unitOfWork.nhanVienRepository.GetAll().ToList()
            };
        }
        // GET: ThongKes
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TheoNgay()
        {
            return View(ThongKeVM);
        }

        [HttpPost]
        public ActionResult TheoNgay(ThongKeViewModel model)
        {
            ViewBag.searchFromDate = model.TuNgay;
            ViewBag.searchToDate = model.DenNgay;
            ViewBag.daIn = model.DaIn;
            ViewBag.vanPhong = model.VanPhongId;

            var vanPhong = _unitOfWork.vanPhongRepository.GetById(model.VanPhongId);
            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");
            // Định dạng chiều dài cho cột
            xlSheet.Column(1).Width = 10;//stt
            xlSheet.Column(2).Width = 30;// sales
            xlSheet.Column(3).Width = 30;//stt
            xlSheet.Column(4).Width = 20;// doanh so
            xlSheet.Column(5).Width = 20;// doanh thu sale

            xlSheet.Cells[2, 1].Value = "BÁO CÁO DOANH THU THEO NGÀY BÁN VP " + vanPhong.Name.ToUpper();
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            xlSheet.Cells[2, 1, 2, 5].Merge = true;
            setCenterAligment(2, 1, 2, 5, xlSheet);
            // dinh dang tu ngay den ngay
            if (model.TuNgay == model.DenNgay)
            {
                fromTo = "Ngày: " + model.TuNgay;
            }
            else
            {
                fromTo = "Từ ngày: " + model.TuNgay + " đến ngày: " + model.DenNgay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1, 3, 5].Merge = true;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 14, FontStyle.Bold));
            setCenterAligment(3, 1, 3, 5, xlSheet);

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Nhân viên ";
            xlSheet.Cells[5, 3].Value = "Cơ sở ";
            xlSheet.Cells[5, 4].Value = "Ngày tạo";
            xlSheet.Cells[5, 5].Value = "Doanh số";

            xlSheet.Cells[5, 1, 5, 5].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));

            // do du lieu tu table
            int dong = 5;

            var d = _unitOfWork.thongKeRepository.ListHoaDonTheoNgayTao(model.VanPhongId, model.DaIn, model.TuNgay, model.DenNgay);// Session["daily"].ToString(), Session["khoi"].ToString());
            d = d.OrderBy(x => x.NhanVien.NoiLamViec);
            //du lieu
            int iRowIndex = 6;
            int idem = 1;

            if (d != null)
            {
                foreach (var vm in d)
                {
                    xlSheet.Cells[iRowIndex, 1].Value = vm.NhanVien.NoiLamViec;
                    TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 1].Style.Font.Italic = true;

                    xlSheet.Cells[iRowIndex, 2].Value = idem;
                    TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[iRowIndex, 3].Value = vm.NhanVien.HoTen;
                    TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[iRowIndex, 4].Value = vm.VanPhong.Name;
                    TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[iRowIndex, 5].Value = vm.NgayTao.Value.ToShortDateString();
                    TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[iRowIndex, 6].Value = vm.ThanhTienHD;
                    TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    iRowIndex += 1;
                    idem += 1;
                    dong++;
                }
            }
            else
            {
                SetAlert("No sale.", "warning");
                return RedirectToAction(nameof(TheoNgay));
            }

            dong++;
            //// Merger cot 4,5 ghi tổng tiền
            //setRightAligment(dong, 3, dong, 3, xlSheet);
            //xlSheet.Cells[dong, 1, dong, 2].Merge = true;
            //xlSheet.Cells[dong, 1].Value = "Tổng tiền: ";

            // Sum tổng tiền
            xlSheet.Cells[dong, 4].Value = "TC:";
            //DateTimeFormat(6, 4, 6 + d.Count(), 4, xlSheet);
            DateTimeFormat(6, 4, 9, 4, xlSheet);
            setCenterAligment(6, 4, 9, 4, xlSheet);
            xlSheet.Cells[dong, 5].Formula = "SUM(E6:E" + (6 + d.Count() - 1) + ")";

            setBorder(5, 1, 5 + d.Count(), 5, xlSheet);
            setFontBold(5, 1, 5, 5, 11, xlSheet);
            setFontSize(6, 1, 6 + d.Count(), 5, 11, xlSheet);
            // canh giua cot stt
            setCenterAligment(6, 1, 6 + d.Count(), 1, xlSheet);
            // canh giua code chinhanh
            setCenterAligment(6, 3, 6 + d.Count(), 3, xlSheet);
            NumberFormat(6, 4, 6 + d.Count(), 5, xlSheet);
            // định dạng số cot tong cong
            NumberFormat(dong, 4, dong, 5, xlSheet);
            setBorder(dong, 4, dong, 5, xlSheet);
            setFontBold(dong, 4, dong, 5, 12, xlSheet);

            //xlSheet.View.FreezePanes(6, 20);

            //end du lieu

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }
            string sFilename = "DoanhThuSale_" + vanPhong.Name + "_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
            );

        }

        public ActionResult TheoNgayIn()
        {
            return View(ThongKeVM);
        }

        [HttpPost]
        public ActionResult TheoNgayIn(ThongKeViewModel model)
        {
            ViewBag.searchFromDate = model.TuNgay;
            ViewBag.searchToDate = model.DenNgay;
            ViewBag.daIn = model.DaIn;
            ViewBag.vanPhong = model.VanPhongId;

            var vanPhong = _unitOfWork.vanPhongRepository.GetById(model.VanPhongId);
            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");
            // Định dạng chiều dài cho cột
            xlSheet.Column(1).Width = 10;//stt
            xlSheet.Column(2).Width = 30;// sales
            xlSheet.Column(3).Width = 30;//stt
            xlSheet.Column(4).Width = 20;// doanh so
            xlSheet.Column(5).Width = 20;// doanh thu sale

            xlSheet.Cells[2, 1].Value = "BÁO CÁO DOANH THU THEO NGÀY XUẤT HD VP " + vanPhong.Name.ToUpper();
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            xlSheet.Cells[2, 1, 2, 5].Merge = true;
            setCenterAligment(2, 1, 2, 5, xlSheet);
            // dinh dang tu ngay den ngay
            if (model.TuNgay == model.DenNgay)
            {
                fromTo = "Ngày: " + model.TuNgay;
            }
            else
            {
                fromTo = "Từ ngày: " + model.TuNgay + " đến ngày: " + model.DenNgay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1, 3, 5].Merge = true;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 14, FontStyle.Bold));
            setCenterAligment(3, 1, 3, 5, xlSheet);

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Nhân viên ";
            xlSheet.Cells[5, 3].Value = "Cơ sở ";
            xlSheet.Cells[5, 4].Value = "Ngày xuất";
            xlSheet.Cells[5, 5].Value = "Doanh số";

            xlSheet.Cells[5, 1, 5, 5].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));

            // do du lieu tu table
            int dong = 5;

            var d = _unitOfWork.thongKeRepository.ListHoaDonTheoNgayIn(model.VanPhongId, model.TuNgay, model.DenNgay);// Session["daily"].ToString(), Session["khoi"].ToString());

            //du lieu
            int iRowIndex = 6;
            int idem = 1;

            if (d != null)
            {
                foreach (var vm in d)
                {
                    xlSheet.Cells[iRowIndex, 1].Value = idem;
                    TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[iRowIndex, 2].Value = vm.NhanVien.HoTen;
                    TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[iRowIndex, 3].Value = vm.VanPhong.Name;
                    TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[iRowIndex, 4].Value = vm.NgayIn.Value.ToShortDateString();
                    TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[iRowIndex, 5].Value = vm.ThanhTienHD;
                    TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    iRowIndex += 1;
                    idem += 1;
                    dong++;
                }
            }
            else
            {
                SetAlert("No sale.", "warning");
                return RedirectToAction(nameof(TheoNgayIn));
            }

            dong++;
            //// Merger cot 4,5 ghi tổng tiền
            //setRightAligment(dong, 3, dong, 3, xlSheet);
            //xlSheet.Cells[dong, 1, dong, 2].Merge = true;
            //xlSheet.Cells[dong, 1].Value = "Tổng tiền: ";

            // Sum tổng tiền
            xlSheet.Cells[dong, 4].Value = "TC:";
            //DateTimeFormat(6, 4, 6 + d.Count(), 4, xlSheet);
            DateTimeFormat(6, 4, 9, 4, xlSheet);
            setCenterAligment(6, 4, 9, 4, xlSheet);
            xlSheet.Cells[dong, 5].Formula = "SUM(E6:E" + (6 + d.Count() - 1) + ")";

            setBorder(5, 1, 5 + d.Count(), 5, xlSheet);
            setFontBold(5, 1, 5, 5, 11, xlSheet);
            setFontSize(6, 1, 6 + d.Count(), 5, 11, xlSheet);
            // canh giua cot stt
            setCenterAligment(6, 1, 6 + d.Count(), 1, xlSheet);
            // canh giua code chinhanh
            setCenterAligment(6, 3, 6 + d.Count(), 3, xlSheet);
            NumberFormat(6, 4, 6 + d.Count(), 5, xlSheet);
            // định dạng số cot tong cong
            NumberFormat(dong, 4, dong, 5, xlSheet);
            setBorder(dong, 4, dong, 5, xlSheet);
            setFontBold(dong, 4, dong, 5, 12, xlSheet);

            //xlSheet.View.FreezePanes(6, 20);

            //end du lieu

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }
            string sFilename = "DoanhThuSale_" + vanPhong.Name + "_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
            );

        }

        public ActionResult TheoMon(int vanPhongId = 0, int khuVucId = 0, string tuNgay = null, string denNgay = null)
        {
            ThongKeVM.VanPhongs.Add(new Data.Models.VanPhong() { Id = 0, Name = "-- select --" });
            ThongKeVM.KhuVucs.Add(new Data.Models.KhuVuc() { Id = 0, Name = "-- select --" });
            ThongKeVM.ThucDons = _unitOfWork.thucDonRepository.Find(x => x.TenMon.ToLower().Contains("buffet")).ToList();
            ThongKeVM.ThucDons.Add(new Data.Models.ThucDon() { Id = 0, TenMon = "-- select --" });

            if (!string.IsNullOrEmpty(tuNgay))
            {
                ViewBag.searchFromDate = tuNgay;
            }
            if (!string.IsNullOrEmpty(denNgay))
            {
                ViewBag.searchToDate = denNgay;
            }

            ViewBag.khuVuc = khuVucId;
            if (vanPhongId != 0)
            {
                ViewBag.vanPhong = vanPhongId;
                ThongKeVM.KhuVucs.Add(new Data.Models.KhuVuc() { Id = 0, Name = "-- select --" });
                ThongKeVM.KhuVucs = ThongKeVM.KhuVucs.Where(x => x.VanPhongId == vanPhongId).ToList();
            }

            return View(ThongKeVM);
        }

        [HttpPost]
        public ActionResult TheoMon(ThongKeViewModel model)
        {
            ViewBag.searchFromDate = model.TuNgay;
            ViewBag.searchToDate = model.DenNgay;
            ViewBag.daIn = model.DaIn;
            ViewBag.vanPhong = model.VanPhongId;
            ViewBag.khuVuc = model.KhuVucId;
            ViewBag.thucDon = model.ThucDonId;

            var vanPhong = _unitOfWork.vanPhongRepository.GetById(model.VanPhongId);
            var khuVuc = _unitOfWork.khuVucRepository.GetById(model.KhuVucId);

            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");
            // Định dạng chiều dài cho cột
            xlSheet.Column(1).Width = 10;//stt
            xlSheet.Column(2).Width = 30;// sales
            xlSheet.Column(3).Width = 30;//stt
            xlSheet.Column(4).Width = 20;// doanh so
            xlSheet.Column(5).Width = 20;// doanh thu sale
            xlSheet.Column(6).Width = 30;// doanh thu sale

            xlSheet.Cells[2, 1].Value = "BÁO CÁO DOANH THU THEO TIỆC BUFFET CƠ SỞ " + vanPhong.Name.ToUpper() + " KHU VỰC " + khuVuc.Name.ToUpper();
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            xlSheet.Cells[2, 1, 2, 6].Merge = true;
            setCenterAligment(2, 1, 2, 6, xlSheet);
            // dinh dang tu ngay den ngay
            if (model.TuNgay == model.DenNgay)
            {
                fromTo = "Ngày: " + model.TuNgay;
            }
            else
            {
                fromTo = "Từ ngày: " + model.TuNgay + " đến ngày: " + model.DenNgay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1, 3, 5].Merge = true;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 14, FontStyle.Bold));
            setCenterAligment(3, 1, 3, 5, xlSheet);

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Nhân viên ";
            xlSheet.Cells[5, 3].Value = "Cơ sở ";
            xlSheet.Cells[5, 4].Value = "Khu vực ";
            xlSheet.Cells[5, 5].Value = "Ngày xuất";
            xlSheet.Cells[5, 6].Value = "Doanh số";

            xlSheet.Cells[5, 1, 5, 5].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));

            // do du lieu tu table
            int dong = 5;

            var d = _unitOfWork.thongKeRepository.ListHoaDonTheoTiecBuffet(model.VanPhongId, model.KhuVucId, model.ThucDonId, model.TuNgay, model.DenNgay);// Session["daily"].ToString(), Session["khoi"].ToString());

            //du lieu
            int iRowIndex = 6;
            int idem = 1;

            if (d != null)
            {
                foreach (var vm in d)
                {
                    xlSheet.Cells[iRowIndex, 1].Value = idem;
                    TrSetCellBorder(xlSheet, iRowIndex, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[iRowIndex, 2].Value = vm.NhanVien.HoTen;
                    TrSetCellBorder(xlSheet, iRowIndex, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Left, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[iRowIndex, 3].Value = vm.VanPhong.Name;
                    TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[iRowIndex, 4].Value = khuVuc.Name;
                    TrSetCellBorder(xlSheet, iRowIndex, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[iRowIndex, 5].Value = vm.NgayTao.Value.ToShortDateString();
                    TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    xlSheet.Cells[iRowIndex, 6].Value = vm.ThanhTienHD;
                    TrSetCellBorder(xlSheet, iRowIndex, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                    xlSheet.Cells[iRowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    iRowIndex += 1;
                    idem += 1;
                    dong++;
                }
            }
            else
            {
                SetAlert("No sale.", "warning");
                return RedirectToAction(nameof(TheoMon));
            }

            dong++;
            //// Merger cot 4,5 ghi tổng tiền
            //setRightAligment(dong, 3, dong, 3, xlSheet);
            //xlSheet.Cells[dong, 1, dong, 2].Merge = true;
            //xlSheet.Cells[dong, 1].Value = "Tổng tiền: ";

            // Sum tổng tiền
            xlSheet.Cells[dong, 5].Value = "TC:";
            //DateTimeFormat(6, 4, 6 + d.Count(), 4, xlSheet);
            // DateTimeFormat(6, 4, 9, 4, xlSheet);
            setCenterAligment(6, 4, 9, 4, xlSheet);
            xlSheet.Cells[dong, 6].Formula = "SUM(F6:F" + (6 + d.Count() - 1) + ")";

            setBorder(5, 1, 5 + d.Count(), 6, xlSheet);
            setFontBold(5, 1, 5, 6, 11, xlSheet);
            setFontSize(6, 1, 6 + d.Count(), 6, 11, xlSheet);
            // canh giua cot stt
            setCenterAligment(6, 1, 6 + d.Count(), 1, xlSheet);
            // canh giua code chinhanh
            setCenterAligment(6, 3, 6 + d.Count(), 3, xlSheet);
            NumberFormat(6, 6, 6 + d.Count(), 6, xlSheet);
            // định dạng số cot tong cong
            //  NumberFormat(dong, 5, dong, 6, xlSheet);
            setBorder(dong, 5, dong, 6, xlSheet);
            setFontBold(dong, 5, dong, 6, 12, xlSheet);

            //xlSheet.View.FreezePanes(6, 20);

            //end du lieu

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }
            string sFilename = "DoanhThuBuffet_" + khuVuc.Name + "_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
            );

        }

        public ActionResult BaoCaoTheoNhanVien(string tuNgay = null, string denNgay = null, int khuVucId = 0)
        {
            var user = (NhanVien)Session["UserSession"];

            if (!string.IsNullOrEmpty(tuNgay))
            {
                ViewBag.searchFromDate = tuNgay;
            }
            if (!string.IsNullOrEmpty(denNgay))
            {
                ViewBag.searchToDate = denNgay;
            }

            if (khuVucId != 0)
            {
                ViewBag.khuVuc = khuVucId;
                ThongKeVM.NhanViens = ThongKeVM.NhanViens.Where(x => x.KhuVucId == khuVucId).ToList();
                if (ThongKeVM.NhanViens.Count == 0)
                {
                    ThongKeVM.NhanViens = new List<NhanVien>() { new NhanVien { MaNV = "0", Username = "Chưa có nhân viên nào" } };
                }
            }

            //// moi load vao
            if (user.Role != "Admins")
            {
                if (user.Role == "Users")
                {
                    ThongKeVM.VanPhongs = new List<VanPhong>() { new Data.Models.VanPhong() { Id = user.KhuVuc.VanPhongId, Name = user.KhuVuc.VanPhong.Name } };
                    ThongKeVM.KhuVucs = new List<KhuVuc>() { new Data.Models.KhuVuc() { Id = user.KhuVucId, Name = user.KhuVuc.Name } };
                    ThongKeVM.NhanViens = new List<NhanVien>() { new NhanVien() { MaNV = user.MaNV, HoTen = user.HoTen } };
                }
                else
                {
                    ThongKeVM.VanPhongs = ThongKeVM.VanPhongs.Where(x => x.Role == user.Role).ToList();
                    ThongKeVM.KhuVucs = new List<KhuVuc>();
                    foreach (var item in ThongKeVM.VanPhongs)
                    {
                        ThongKeVM.KhuVucs.AddRange(_unitOfWork.khuVucRepository.Find(x => x.VanPhongId == item.Id).ToList());
                    }

                    ThongKeVM.NhanViens = new List<NhanVien>();
                    foreach (var item in ThongKeVM.KhuVucs)
                    {
                        ThongKeVM.NhanViens.AddRange(_unitOfWork.nhanVienRepository.Find(x => x.KhuVucId == item.Id));
                    }
                }
            }

            return View(ThongKeVM);
        }

        [HttpPost]
        public ActionResult BaoCaoTheoNhanVien(ThongKeViewModel model)
        {

            ViewBag.searchFromDate = model.TuNgay;
            ViewBag.searchToDate = model.DenNgay;
            ViewBag.vanPhong = model.VanPhongId;
            ViewBag.khuVuc = model.KhuVucId;
            ViewBag.nhanVien = model.NhanVienId;

            var nhanvien = _unitOfWork.nhanVienRepository.GetByStringId(model.NhanVienId);
            if (nhanvien == null)
            {
                ViewBag.searchFromDate = model.TuNgay;
                ViewBag.searchToDate = model.DenNgay;
                SetAlert("Chưa chọn nhân viên.", "warning");
                return RedirectToAction(nameof(BaoCaoTheoNhanVien));
            }
            var khuVuc = _unitOfWork.khuVucRepository.GetById(model.KhuVucId);

            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");
            // Định dạng chiều dài cho cột
            xlSheet.Column(1).Width = 10;// STT
            xlSheet.Column(2).Width = 30;// Nhân viên
            xlSheet.Column(3).Width = 30;// Cơ sở
            xlSheet.Column(4).Width = 30;// Khu vực
            xlSheet.Column(5).Width = 20;// Ngày bán
            xlSheet.Column(6).Width = 30;// Tên món
            xlSheet.Column(7).Width = 10;// Số lượng
            xlSheet.Column(8).Width = 20;// Ban

            xlSheet.Cells[2, 1].Value = "BÁO CÁO THEO NHÂN VIÊN - " + nhanvien.HoTen.ToUpper();
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            xlSheet.Cells[2, 1, 2, 8].Merge = true;
            setCenterAligment(2, 1, 2, 8, xlSheet);
            // dinh dang tu ngay den ngay
            if (string.IsNullOrEmpty(model.TuNgay) && string.IsNullOrEmpty(model.DenNgay))
            {
                ViewBag.searchFromDate = model.TuNgay;
                ViewBag.searchToDate = model.DenNgay;
                SetAlert("Từ ngày đến ngày không được để trống.", "warning");
                return RedirectToAction(nameof(BaoCaoTheoNhanVien));
            }
            if (model.TuNgay == model.DenNgay)
            {
                fromTo = "Ngày: " + model.TuNgay;
            }
            else
            {
                fromTo = "Từ ngày: " + model.TuNgay + " đến ngày: " + model.DenNgay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1, 3, 8].Merge = true;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 14, FontStyle.Bold));
            setCenterAligment(3, 1, 3, 8, xlSheet);

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Nhân viên ";
            xlSheet.Cells[5, 3].Value = "Cơ sở ";
            xlSheet.Cells[5, 4].Value = "Khu vực ";
            xlSheet.Cells[5, 5].Value = "Ngày bán";
            xlSheet.Cells[5, 6].Value = "Tên món";
            xlSheet.Cells[5, 7].Value = "Số lượng";
            xlSheet.Cells[5, 8].Value = "Bàn";

            xlSheet.Cells[5, 1, 5, 8].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));

            // do du lieu tu table
            int dong = 6;

            var hoaDons = _unitOfWork.thongKeRepository.ListHoaDonTheoNhanVien(model.NhanVienId, model.TuNgay, model.DenNgay);// Session["daily"].ToString(), Session["khoi"].ToString());
            var d = new List<ChiTietHD>();
            if (hoaDons != null)
            {
                foreach (var item in hoaDons)
                {
                    d.AddRange(_unitOfWork.chiTietHDRepository.Find(x => x.MaHD == item.MaHD));
                }
            }

            ///////////////////////////////// group by ////////////////////////////////////////////
            List<ChiTietHdViewModel> chiTietHdViewModels = new List<ChiTietHdViewModel>();
            foreach (var item in d)
            {
                chiTietHdViewModels.Add(new ChiTietHdViewModel
                {
                    HoTen = item.HoaDon.NhanVien.HoTen,
                    VPName = item.HoaDon.VanPhong.Name,
                    KVName = item.HoaDon.NhanVien.KhuVuc.Name,
                    NgayTao = item.HoaDon.NgayTao,
                    TenMon = item.ThucDon.TenMon,
                    SoLuong = item.SoLuong,
                    TenBan = item.HoaDon.Ban.TenBan,
                    NoiLamViec = item.ThucDon.LoaiThucDon.NoiLamViec
                });
            }
            //With Query Syntax

            List<ChiTietHDGroupByResultViewModel> results1 = (
                from p in chiTietHdViewModels
                group p by p.NoiLamViec into g
                select new ChiTietHDGroupByResultViewModel()
                {
                    NoiLamViec = g.Key,
                    ChiTietHdViewModels = g.ToList()
                }
                ).ToList();


            foreach (var item in results1)
            {
                System.Diagnostics.Debug.WriteLine(item.NoiLamViec);
                foreach (var car in item.ChiTietHdViewModels)
                {
                    System.Diagnostics.Debug.WriteLine(car.TenMon);
                }
            }

            System.Diagnostics.Debug.WriteLine("-----------");

            //////////////////////////// group by/////////////////////////////////////////////////

            //du lieu
            //int iRowIndex = 6;
            int idem = 1;

            if (hoaDons != null)
            {
                foreach (var vm in results1)
                {
                    xlSheet.Cells[dong, 1, dong, 8].Merge = true;
                    xlSheet.Cells[dong, 1].Value = vm.NoiLamViec;
                    TrSetCellBorder(xlSheet, dong, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Center, Color.Silver, "Times New Roman", 12, FontStyle.Regular);
                    xlSheet.Cells[dong, 1].Style.Font.Bold = true;

                    dong++;

                    foreach (var item in vm.ChiTietHdViewModels)
                    {

                        xlSheet.Cells[dong, 1].Value = idem;
                        TrSetCellBorder(xlSheet, dong, 1, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[dong, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        
                        xlSheet.Cells[dong, 2].Value = item.HoTen;
                        TrSetCellBorder(xlSheet, dong, 2, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[dong, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        xlSheet.Cells[dong, 3].Value = item.VPName;
                        TrSetCellBorder(xlSheet, dong, 3, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[dong, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        xlSheet.Cells[dong, 4].Value = item.KVName;
                        TrSetCellBorder(xlSheet, dong, 4, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[dong, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        xlSheet.Cells[dong, 5].Value = item.NgayTao.Value.ToShortDateString();
                        TrSetCellBorder(xlSheet, dong, 5, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[dong, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        xlSheet.Cells[dong, 6].Value = item.TenMon;
                        TrSetCellBorder(xlSheet, dong, 6, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[dong, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        xlSheet.Cells[dong, 7].Value = item.SoLuong;
                        TrSetCellBorder(xlSheet, dong, 7, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[dong, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        xlSheet.Cells[dong, 8].Value = item.TenBan;
                        TrSetCellBorder(xlSheet, dong, 8, ExcelBorderStyle.Dotted, ExcelHorizontalAlignment.Right, Color.Silver, "Times New Roman", 10, FontStyle.Regular);
                        xlSheet.Cells[dong, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        
                        dong++;
                        idem += 1;
                    }
                    idem = 1;


                }
            }
            else
            {
                SetAlert("No sale.", "warning");
                return RedirectToAction(nameof(BaoCaoTheoNhanVien));
            }

            dong++;
            //// Merger cot 4,5 ghi tổng tiền
            //setRightAligment(dong, 3, dong, 3, xlSheet);
            //xlSheet.Cells[dong, 1, dong, 2].Merge = true;
            //xlSheet.Cells[dong, 1].Value = "Tổng tiền: ";

            // Sum tổng tiền
            // xlSheet.Cells[dong, 5].Value = "TC:";
            //DateTimeFormat(6, 4, 6 + d.Count(), 4, xlSheet);
            // DateTimeFormat(6, 4, 9, 4, xlSheet);
            // setCenterAligment(6, 4, 9, 4, xlSheet);
            // xlSheet.Cells[dong, 6].Formula = "SUM(F6:F" + (6 + d.Count() - 1) + ")";

            setBorder(5, 1, 5 + d.Count() + 2, 8, xlSheet);
            //setFontBold(5, 1, 5, 8, 11, xlSheet);
            //setFontSize(6, 1, 6 + d.Count() + 2, 8, 11, xlSheet);
            // canh giua cot stt
            setCenterAligment(6, 1, 6 + d.Count() + 2, 1, xlSheet);
            // canh giua code chinhanh
            setCenterAligment(6, 3, 6 + d.Count() + 2, 3, xlSheet);
            // NumberFormat(6, 6, 6 + d.Count(), 6, xlSheet);
            // định dạng số cot tong cong
            //  NumberFormat(dong, 5, dong, 6, xlSheet);

            // setBorder(dong, 5, dong, 6, xlSheet);
            // setFontBold(dong, 5, dong, 6, 12, xlSheet);

            //xlSheet.View.FreezePanes(6, 20);

            //end du lieu

            byte[] fileContents;
            fileContents = ExcelApp.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return View("~/Views/Shared/NotFound.cshtml");
            }
            string sFilename = "BaoCaoTheoNhanVien_" + nhanvien.HoTen + "_" + System.DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: sFilename
            );

        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private static void NumberFormat(int fromRow, int fromColumn, int toRow, int toColumn, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                range.Style.Numberformat.Format = "#,#0";
            }
        }

        private static void DateFormat(int fromRow, int fromColumn, int toRow, int toColumn, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.Numberformat.Format = "dd/MM/yyyy";
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
        }

        private static void DateTimeFormat(int fromRow, int fromColumn, int toRow, int toColumn, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.Numberformat.Format = "dd/MM/yyyy HH:mm";
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
        }

        private static void setRightAligment(int fromRow, int fromColumn, int toRow, int toColumn, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }
        }

        private static void setCenterAligment(int fromRow, int fromColumn, int toRow, int toColumn, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
        }

        private static void setFontSize(int fromRow, int fromColumn, int toRow, int toColumn, int fontSize, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.Font.SetFromFont(new Font("Times New Roman", fontSize, FontStyle.Regular));
            }
        }

        private static void setFontBold(int fromRow, int fromColumn, int toRow, int toColumn, int fontSize, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.Font.SetFromFont(new Font("Times New Roman", fontSize, FontStyle.Bold));
            }
        }

        private static void setBorder(int fromRow, int fromColumn, int toRow, int toColumn, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            }
        }

        private static void setBorderAround(int fromRow, int fromColumn, int toRow, int toColumn, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
        }

        private static void PhantramFormat(int fromRow, int fromColumn, int toRow, int toColumn, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.Numberformat.Format = "0 %";
            }
        }

        private static void mergercell(int fromRow, int fromColumn, int toRow, int toColumn, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Merge = true;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                range.Style.WrapText = true;
            }
        }

        private static void numberMergercell(int fromRow, int fromColumn, int toRow, int toColumn, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                var a = sheet.Cells[fromRow, fromColumn].Value;
                range.Merge = true;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                range.Clear();
                sheet.Cells[fromRow, fromColumn].Value = a;
            }
        }

        private static void wrapText(int fromRow, int fromColumn, int toRow, int toColumn, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.WrapText = true;
            }
        }

        public static string convertToUnSign3(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        ///////////////// new ///////////////////
        ///
        public void TrSetCellBorder(ExcelWorksheet xlSheet, int iRowIndex, int colIndex, ExcelBorderStyle excelBorderStyle, ExcelHorizontalAlignment excelHorizontalAlignment, Color borderColor, string fontName, int fontSize, FontStyle fontStyle)
        {
            xlSheet.Cells[iRowIndex, colIndex].Style.HorizontalAlignment = excelHorizontalAlignment;
            // Set Border
            xlSheet.Cells[iRowIndex, colIndex].Style.Border.Left.Style = excelBorderStyle;
            xlSheet.Cells[iRowIndex, colIndex].Style.Border.Top.Style = excelBorderStyle;
            xlSheet.Cells[iRowIndex, colIndex].Style.Border.Right.Style = excelBorderStyle;
            xlSheet.Cells[iRowIndex, colIndex].Style.Border.Bottom.Style = excelBorderStyle;
            // Set màu ch Border
            //xlSheet.Cells[iRowIndex, colIndex].Style.Border.Left.Color.SetColor(borderColor);
            //xlSheet.Cells[iRowIndex, colIndex].Style.Border.Top.Color.SetColor(borderColor);
            //xlSheet.Cells[iRowIndex, colIndex].Style.Border.Right.Color.SetColor(borderColor);
            //xlSheet.Cells[iRowIndex, colIndex].Style.Border.Bottom.Color.SetColor(borderColor);

            // Set Font cho text  trong Range hiện tại
            xlSheet.Cells[iRowIndex, colIndex].Style.Font.SetFromFont(new Font(fontName, fontSize, fontStyle));
        }
    }
}