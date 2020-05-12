using QLNhaHang.Data.Models;
using QLNhaHang.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNhaHang.Controllers
{
    public class PhaChesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PhaChesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET: PhaChes
        public ActionResult Index()
        {

            return View();
        }

        public JsonResult LoadData()
        {
            var user = (NhanVien)Session["UserSession"];
            var phaChes = _unitOfWork.monDaGoiRepository.GetAllInclude(x => x.Ban, y => y.ThucDon).ToList()
                                        .Where(x => x.Ban.KhuVucId == user.KhuVucId && x.DaGui && x.ThucDon.LoaiThucDon.NoiLamViec.Equals("Pha chế"));
            //var listMon = new List<MonDaGoi>();
            //foreach (var mon in phaChes)
            //{
            //    var noiLam = mon.ThucDon.LoaiThucDon.NoiLamViec;
            //    if (!string.IsNullOrEmpty(noiLam))
            //    {
            //        if (noiLam == "Pha chế")
            //            listMon.Add(mon);
            //    }
            //}
            var count = phaChes.ToList().Count;
            return Json(new
            {
                status = true,
                data = phaChes
            }, JsonRequestBehavior.AllowGet);
        }
    }
}