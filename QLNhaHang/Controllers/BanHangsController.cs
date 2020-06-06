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
    public class BanHangsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public BanHangViewModel BanHangVM { get; set; }
        public BanHangsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            BanHangVM = new BanHangViewModel()
            {
                Ban = new Data.Models.Ban(),
                MonDaGoi = new Data.Models.MonDaGoi(),
                KhuVucs = new List<KhuVuc>(),
                LoaiViewModels = new List<LoaiThucDonListViewModel>() { new LoaiThucDonListViewModel() { Id = 0, Name = "-- select --" } }
            };
        }
        // GET: BanHangs
        public ActionResult Index(string maBan = null, string maBanFrom = null, string maBanTo = null, int idKhuVuc = 0)
        {
            ////// moi load vao
            //if (idKhuVuc == 0)
            //{
            //    ViewBag.idKhuVuc = 0;
            //}
            //else
            //{
            ViewBag.idKhuVuc = idKhuVuc;
            //}

            var user = (NhanVien)Session["UserSession"];
            if (user.NoiLamViec == "Bếp" || user.NoiLamViec == "Pha chế")
            {
                return View("~/Views/Shared/AccessDeny.cshtml");
            }
            ///////////////////// load Ban by flag /////////////////////
            //var banByFlags = _unitOfWork.banRepository.Find(x => x.Flag);
            ///////////////////// load Ban by flag /////////////////////
            //BanHangVM.StrUrl = UriHelper.GetDisplayUrl(Request);
            ViewBag.strUrl = Request.Url.AbsoluteUri.ToString();

            BanHangVM.Bans = _unitOfWork.banRepository.GetAll().ToList();
            ///// lay KhuVuc va ban
            if (user.Role != "Admins")
            {
                List<Ban> bans = new List<Ban>();
                if (user.Role != "Users")
                {
                    var vanPhongs = _unitOfWork.vanPhongRepository.Find(x => x.Role == user.Role).ToList();
                    foreach (var vanPhong in vanPhongs)
                    {
                        var khuVucs = _unitOfWork.khuVucRepository.Find(x => x.VanPhongId == vanPhong.Id).ToList();
                        if (khuVucs.Any())
                        {
                            BanHangVM.KhuVucs.AddRange(khuVucs);
                        }
                    }

                    foreach (var khuVuc in BanHangVM.KhuVucs)
                    {
                        BanHangVM.LoaiViewModels.Add(new LoaiThucDonListViewModel() { Id = khuVuc.Id, Name = khuVuc.Name + " - " + khuVuc.VanPhong.Name });
                    }
                    BanHangVM.Bans = BanHangVM.Bans.Where(x => x.TenVP.Equals(user.KhuVuc.VanPhong.Name)).ToList();
                }
                else
                {
                    var kv = _unitOfWork.khuVucRepository.GetById(user.KhuVucId);
                    BanHangVM.LoaiViewModels.Add(new LoaiThucDonListViewModel() { Id = kv.Id, Name = kv.Name });

                    BanHangVM.Bans = _unitOfWork.banRepository.Find(x => x.KhuVucId == user.KhuVucId).ToList();


                }

            }
            else
            {
                foreach (var khuVuc in _unitOfWork.khuVucRepository.GetAll().ToList())
                {
                    BanHangVM.LoaiViewModels.Add(new LoaiThucDonListViewModel() { Id = khuVuc.Id, Name = khuVuc.Name + " - " + khuVuc.VanPhong.Name });
                }
            }
            // sau khi chon khu vuc
            if (idKhuVuc != 0)
            {
                BanHangVM.Bans = BanHangVM.Bans.Where(x => x.KhuVucId == idKhuVuc).ToList();
            }
            // sau khi chon khu vuc
            if (!string.IsNullOrEmpty(maBan))
            {
                BanHangVM.Ban = _unitOfWork.banRepository.GetByStringId(maBan);
                BanHangVM.MonDaGois = _unitOfWork.monDaGoiRepository
                                                  .FindIncludeTwo(x => x.Ban, y => y.ThucDon, z => z.MaBan == maBan)
                                                  .OrderBy(x => x.LanGui)
                                                  .ToList();

                var listDecimal = BanHangVM.MonDaGois.Select(x => x.ThanhTien).ToList();
                BanHangVM.TongTien = listDecimal.Sum();
            }

            if (!string.IsNullOrEmpty(maBanFrom) && !string.IsNullOrEmpty(maBanTo))
            {
                var monInBanFrom = _unitOfWork.monDaGoiRepository
                                                  .FindIncludeTwo(x => x.Ban, y => y.ThucDon, z => z.MaBan.Equals(maBanFrom))
                                                  .OrderBy(x => x.ThucDon.TenMon)
                                                  .ToList();

                var monInBanTo = _unitOfWork.monDaGoiRepository
                                                  .FindIncludeTwo(x => x.Ban, y => y.ThucDon, z => z.MaBan.Equals(maBanTo))
                                                  .OrderBy(x => x.ThucDon.TenMon)
                                                  .ToList();
                if (monInBanFrom.Any(x => !x.DaLam) || monInBanTo.Any(x => !x.DaLam))
                {
                    SetAlert("Còn món nào đó chưa làm", "error");
                    return RedirectToAction(nameof(Index)/*, new { idKhuVuc = 0}*/);
                }
                if (monInBanTo.Count == 0)
                {
                    var banTo = _unitOfWork.banRepository.GetByStringId(maBanTo);
                    banTo.Flag = true;
                    var banFrom = _unitOfWork.banRepository.GetByStringId(maBanFrom);
                    banFrom.Flag = false;
                    _unitOfWork.banRepository.Update(banTo);
                    _unitOfWork.banRepository.Update(banFrom);

                    foreach (var item in monInBanFrom)
                    {
                        item.MaBan = maBanTo;
                    }
                    _unitOfWork.monDaGoiRepository.CreateRangeAsync(monInBanFrom);
                    var monInBanFrom1 = _unitOfWork.monDaGoiRepository
                                                  .FindIncludeTwo(x => x.Ban, y => y.ThucDon, z => z.MaBan.Equals(maBanFrom))
                                                  .OrderBy(x => x.ThucDon.TenMon)
                                                  .ToList();
                    _unitOfWork.monDaGoiRepository.DeleteRange(monInBanFrom1);

                    _unitOfWork.Complete();
                    SetAlert("Đã chuyển thành công.", "success");
                    //// sau khi da chuyen man moi --> change ma ban, va mondagoi qua ban moi
                    BanHangVM.Ban = _unitOfWork.banRepository.GetByStringId(maBanTo);
                    BanHangVM.MonDaGois = _unitOfWork.monDaGoiRepository
                                                  .FindIncludeTwo(x => x.Ban, y => y.ThucDon, z => z.MaBan.Equals(maBanTo))
                                                  .OrderBy(x => x.LanGui)
                                                  .ToList();
                    var listDecimal = BanHangVM.MonDaGois.Select(x => x.ThanhTien).ToList();
                    BanHangVM.TongTien = listDecimal.Sum();
                }
                else
                {
                    List<MonDaGoi> listMonDaGoiFrom = monInBanFrom;
                    _unitOfWork.monDaGoiRepository.DeleteRange(monInBanFrom);
                    _unitOfWork.Complete();
                    // cong don monInBanFrom to monInBanTo
                    foreach (var itemFrom in listMonDaGoiFrom)
                    {

                        itemFrom.MaBan = monInBanTo.FirstOrDefault().MaBan;
                        _unitOfWork.monDaGoiRepository.Create(itemFrom);
                        _unitOfWork.Complete();
                    }

                    //foreach (var itemFrom in monInBanFrom)
                    //{
                    //    foreach (var itemTo in monInBanTo)
                    //    {
                    //        if (itemFrom.ThucDonId == itemTo.ThucDonId)
                    //        {
                    //            itemTo.SoLuong += itemFrom.SoLuong;
                    //            itemTo.PhuPhi += itemFrom.PhuPhi;
                    //            itemTo.ThanhTien += itemFrom.ThanhTien;
                    //            _unitOfWork.monDaGoiRepository.Update(itemTo);
                    //        }
                    //        else
                    //        {
                    //            var mon = new MonDaGoi()
                    //            {
                    //                SoLuong = itemFrom.SoLuong,
                    //                ThanhTien = itemFrom.ThanhTien,
                    //                GiaTien = itemFrom.GiaTien,
                    //                PhuPhi = itemFrom.PhuPhi,
                    //                PhiPhucVu = itemFrom.PhiPhucVu,
                    //                MaBan = maBanTo,
                    //                ThucDonId = itemFrom.ThucDonId
                    //            };
                    //            // kt tenmon xem co trong ds mon cua banTo ko
                    //            if (!_unitOfWork.monDaGoiRepository.Find(x => x.MaBan.Equals(maBanTo)).Any(x => x.ThucDonId == mon.ThucDonId))
                    //            {
                    //                _unitOfWork.monDaGoiRepository.Create(mon);
                    //                _unitOfWork.Complete();
                    //            }

                    //        }
                    //    }

                    //}


                    //////////// update flag ///////
                    var banFrom = _unitOfWork.banRepository.GetByStringId(maBanFrom);
                    banFrom.Flag = false;

                    _unitOfWork.banRepository.Update(banFrom);
                    //////////// update flag and ///////
                    _unitOfWork.Complete();
                    SetAlert("Đã chuyển thành công.", "success");

                    //// sau khi da chuyen man moi --> change ma ban, va mondagoi qua ban moi
                    BanHangVM.Ban = _unitOfWork.banRepository.GetByStringId(maBanTo);
                    BanHangVM.MonDaGois = _unitOfWork.monDaGoiRepository
                                                  .FindIncludeTwo(x => x.Ban, y => y.ThucDon, z => z.MaBan.Equals(maBanTo))
                                                  .OrderBy(x => x.LanGui)
                                                  .ToList();
                    var listDecimal = BanHangVM.MonDaGois.Select(x => x.ThanhTien).ToList();
                    BanHangVM.TongTien = listDecimal.Sum();
                }
            }

            return View(BanHangVM);
        }

        public JsonResult MonInBan(string maBan)
        {
            var mons = _unitOfWork.monDaGoiRepository.Find(x => x.MaBan == maBan).ToList();

            if (mons.Count() > 0)
            {
                return Json(new
                {
                    status = true,
                    data = JsonConvert.SerializeObject(mons)
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    status = false
                }, JsonRequestBehavior.AllowGet);
            }
        }

        
        public ActionResult DetailsRedirect(string strUrl)
        {
            return Redirect(strUrl);
        }
    }
}