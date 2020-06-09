using Newtonsoft.Json;
using QLNhaHang.Data.Models;
using QLNhaHang.Data.Repositories;
using QLNhaHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLNhaHang.Controllers
{
    public class PhaChesController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public PhaCheViewModel PhaCheVM { get; set; }

        public PhaChesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            PhaCheVM = new PhaCheViewModel()
            {
                MonDaGois = new List<MonDaGoi>()
            };
        }
        // GET: PhaChes
        public ActionResult Index()
        {
            var user = (NhanVien)Session["UserSession"];
            if (user.NoiLamViec == "Bếp")
            {
                return View("~/Views/Shared/AccessDeny.cshtml");
            }
            return View();
        }

        public JsonResult LoadData()
        {
            var user = (NhanVien)Session["UserSession"];
            var phaChes = _unitOfWork.monDaGoiRepository.GetAll().OrderByDescending(x => x.LanGui)
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

        public ActionResult Confirm(int idMon)
        {
            var mon = _unitOfWork.monDaGoiRepository.GetById(idMon);
            mon.DaLam = true;
            _unitOfWork.monDaGoiRepository.Update(mon);
            ///////////////////// save to phaChe tbl //////////////////
            PhaChe phaChe = new PhaChe
            {
                SoLuong = mon.SoLuong,
                ThanhTien = mon.ThanhTien,
                GiaTien = mon.GiaTien,
                PhuPhi = mon.PhuPhi,
                PhiPhucVu = mon.PhiPhucVu,
                MaBan = mon.MaBan,
                ThucDonId = mon.ThucDonId,
                LanGui = mon.LanGui,
                DaGui = mon.DaGui,
                DaLam = mon.DaLam,
                VanPhong = mon.VanPhong
            };
            _unitOfWork.phaCheRepository.Create(phaChe);
            ///////////////////// save to phaChe tbl //////////////////
            _unitOfWork.Complete();
            return View(nameof(Index));
        }

        public ActionResult PrintListMon(string listIdMon)
        {
            var listId = JsonConvert.DeserializeObject<List<PhaCheViewModel>>(listIdMon);
            
            foreach(var item in listId)
            {
                PhaCheVM.MonDaGois.Add(_unitOfWork.monDaGoiRepository.GetById(item.Id));
            }

            return View(PhaCheVM);
        }
    }
}