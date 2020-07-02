using Newtonsoft.Json;
using QLNhaHang.Data.Models;
using QLNhaHang.Data.Repositories;
using QLNhaHang.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace QLNhaHang.Controllers
{
    // serialization and deserialization
    public class XmlUtil
    {
        #region deserialization
        // deserialization
        public static object Deserialize(Type type, string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    return xmldes.Deserialize(sr);
                }
            }
            catch (Exception e)
            {

                return null;
            }
        }
        // deserialization
        public static object Deserialize(Type type, Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(type);
            return xmldes.Deserialize(stream);
        }
        #endregion

        #region serialization
        // serialization
        public static string Serializer(Type type, object obj)
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);
            try
            {
                //Serialized object
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }
        #endregion
    }
    public class Student
    {
        public string Name { set; get; }
        public int Age { set; get; }
    }
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
            

            var user = (NhanVien)Session["UserSession"];
            if (user.NoiLamViec == "Bếp" || user.NoiLamViec == "Pha chế")
            {
                //return View("~/Views/Shared/AccessDeny.cshtml");
                if (user.NoiLamViec == "Pha chế")
                {
                    return RedirectToAction("Index", "PhaChes");
                }
                if (user.NoiLamViec == "Bếp")
                {
                    return RedirectToAction("Index", "Beps");
                }
            }
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
            var listHD = _unitOfWork.thongKeRepository.ListSevenDay().ToList().Take(7);
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