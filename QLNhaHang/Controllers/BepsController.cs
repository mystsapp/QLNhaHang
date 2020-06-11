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
    public class BepsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public BepViewModel BepVM { get; set; }
        public BepsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            BepVM = new BepViewModel()
            {
                MonDaGois = new List<MonDaGoi>()
            };
        }
        // GET: Beps
        public ActionResult Index()
        {
            var user = (NhanVien)Session["UserSession"];
            if (user.NoiLamViec == "Pha chế")
            {
                return View("~/Views/Shared/AccessDeny.cshtml");
            }
            return View();
        }


        public JsonResult LoadData()
        {
            var user = (NhanVien)Session["UserSession"];
            var beps = _unitOfWork.monDaGoiRepository.GetAll().OrderByDescending(x => x.LanGui)
                                        .Where(x => x.Ban.KhuVucId == user.KhuVucId && x.DaGui && x.ThucDon.LoaiThucDon.NoiLamViec.Equals("Bếp"));
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
            var count = beps.ToList().Count;
            return Json(new
            {
                status = true,
                data = beps
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Confirm(int idMon)
        {
            var user = (NhanVien)Session["UserSession"];
            var mon = _unitOfWork.monDaGoiRepository.GetById(idMon);
            var thucdon = _unitOfWork.thucDonRepository.Find(x => x.Id == mon.ThucDonId).FirstOrDefault().TenMon;
            mon.DaLam = true;
            _unitOfWork.monDaGoiRepository.Update(mon);
            ///////////////////// save to bep tbl //////////////////
            Bep bep = new Bep
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
                VanPhong = mon.VanPhong,
                Username = user.Username,
                NgayTao = DateTime.Now
            };
            _unitOfWork.bepRepository.Create(bep);
            ///////////////////// save to bep tbl //////////////////
            _unitOfWork.Complete();
            return View(nameof(Index));
        }

        public ActionResult PrintListMon(string listIdMon)
        {
            var listId = JsonConvert.DeserializeObject<List<BepViewModel>>(listIdMon);

            foreach (var item in listId)
            {
                BepVM.MonDaGois.Add(_unitOfWork.monDaGoiRepository.GetById(item.Id));
            }

            return View(BepVM);
        }
    }
}