using Newtonsoft.Json;
using QLNhaHang.Data.Models;
using QLNhaHang.Data.Repositories;
using QLNhaHang.Models;
using System.Collections;
using System.Linq;
using System.Web.Mvc;

namespace QLNhaHang.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeViewModel HomeVM { get; set; }

        public HomeController(IUnitOfWork unitOfWork, IVanPhongRepository vanPhongRepository)
        {
            _unitOfWork = unitOfWork;
            HomeVM = new HomeViewModel()
            {
                ChartPieMs = new System.Collections.Generic.List<ChartPieModel>()
            };
        }

        public ActionResult Index()
        {
            ListSevenDays();
            return View(HomeVM);
        }
        public JsonResult ListSevenDay()
        {
            ListSevenDays();
            //var list7 = _unitOfWork.thongKeRepository.ListSevenDay().Select(x => new
            //{
            //    NgayBan = x.NgayTao.Value.ToShortDateString(),
            //    TongTien = x.ThanhTienHD
            //});
            //foreach (var item in list7)
            //{
            //    HomeVM.ChartPieMs.Add(new ChartPieModel()
            //    {
            //        NgayBan = item.NgayTao.Value.ToShortDateString(),
            //        TongTien = item.ThanhTienHD.Value.ToString("N0")
            //    });
            //}
            return Json(new { data = HomeVM.ChartPieMs }, JsonRequestBehavior.AllowGet);
        }

        public void ListSevenDays()
        {
            var listHD = _unitOfWork.thongKeRepository.ListSevenDay().ToList();
            var arrayListHD = listHD.ToArray();

        quaylai:
            for (int i = 0; i < arrayListHD.Length; i++)
            {
                if (i < arrayListHD.Length - 1)
                {
                    if (arrayListHD[i].NgayTao.Value.ToShortDateString() == arrayListHD[i + 1].NgayTao.Value.ToShortDateString())
                    {
                        arrayListHD[i].ThanhTienHD = arrayListHD[i].ThanhTienHD + arrayListHD[i + 1].ThanhTienHD;
                        arrayListHD = arrayListHD.Where(val => val != arrayListHD[i + 1]).ToArray();
                        goto quaylai;

                    }

                }
            }
            listHD = arrayListHD.ToList();
            var IEnum = listHD.AsEnumerable().Take(7);
            foreach (var item in IEnum)
            {

                HomeVM.ChartPieMs.Add(new ChartPieModel()
                {
                    NgayBan = item.NgayTao.Value.ToShortDateString(),
                    TongTien = item.ThanhTienHD
                });
            }
        }

        public ActionResult Aboutt()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contactt()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}