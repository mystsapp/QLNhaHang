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
            HoaDonVM = new HoaDonViewModel();
        }
        // GET: HoaDons
        public ActionResult Index(string maHD = null, string searchString = null, 
                                  string searchFromDate = null, string searchToDate = null, 
                                  int page = 1)
        {
            //////////////////// for delete
            if (maHD != null)
            {
                var hd = _unitOfWork.hoaDonRepository.GetByStringId(maHD);
                if (hd == null)
                {
                    var lastMaHD = _unitOfWork.hoaDonRepository
                                          .GetAllIncludeThree(x => x.NhanVien, y => y.KhachHang, z => z.Ban)
                                          .OrderByDescending(x => x.MaHD).FirstOrDefault().MaHD;
                    maHD = lastMaHD;
                }
            }

            ////////////////////
            HoaDonVM.StrUrl = Request.Url.AbsoluteUri.ToString();
            ViewBag.searchString = searchString;
            ViewBag.searchFromDate = searchFromDate;
            ViewBag.searchToDate = searchToDate;

            HoaDonVM.ChiTietHDs = _unitOfWork.chiTietHDRepository
                                             .FindIncludeTwo(x => x.HoaDon, y => y.ThucDon, z => z.MaHD.Equals(maHD));

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