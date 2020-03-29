using Newtonsoft.Json;
using QLNhaHang.Data.Repositories;
using QLNhaHang.Models;
using System;
using System.Collections.Generic;
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
                KhachHang = new Data.Models.KhachHang()
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
            HoaDonVM.TongTien = HoaDonVM.ChiTietHDs.Sum(x => x.DonGia);

            HoaDonVM.HoaDons = _unitOfWork.hoaDonRepository.ListHoaDon(searchString, searchFromDate, searchToDate, page);
            if(HoaDonVM.HoaDons == null)
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

        public ActionResult HoaDonTuDong(string maHD, string strUrl, int maThongTinHDId = 0, string maKH = null)
        {
            HoaDonVM.StrUrl = strUrl;
            HoaDonVM.HoaDon = _unitOfWork.hoaDonRepository.GetByStringId(maHD);
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

        public ActionResult ExportList(string stringId, string strUrl)
        {

            var idList = JsonConvert.DeserializeObject<List<HoaDonViewModel>>(stringId);
            if (idList.Count == 1)
            {
                return RedirectToAction(nameof(Export), new { id = idList.FirstOrDefault().Id, strUrl = strUrl });
            }
            return View();
        }

        public ActionResult Export(int id, string strUrl)
        {
            return View();
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