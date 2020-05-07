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
                KhuVucs = _unitOfWork.khuVucRepository.GetAll()
            };
        }
        // GET: BanHangs
        public ActionResult Index(string maBan = null, string maBanFrom = null, string maBanTo = null, string idKhuVuc = null)
        {
            var user = (NhanVien)Session["UserSession"];

            ///////////////////// load Ban by flag /////////////////////
            //var banByFlags = _unitOfWork.banRepository.Find(x => x.Flag);
            ///////////////////// load Ban by flag /////////////////////
            //BanHangVM.StrUrl = UriHelper.GetDisplayUrl(Request);
            ViewBag.strUrl = Request.Url.AbsoluteUri.ToString();

            BanHangVM.Bans = _unitOfWork.banRepository.GetAll().ToList();

            if (user.Role.Name != "Admins")
            {
                if (user.Role.Name != "Users")
                {
                    BanHangVM.KhuVucs = BanHangVM.KhuVucs.Where(x => x.VanPhongId == (int)Session["VPId"]);
                }
                else
                {
                    BanHangVM.KhuVucs = JsonConvert.DeserializeObject<List<KhuVuc>>(Session["listKV"].ToString());
                }
                    BanHangVM.Bans = BanHangVM.Bans.Where(x => x.TenVP.Equals(user.VanPhong.Name)).ToList();
            }

            if (!string.IsNullOrEmpty(maBan))
            {
                BanHangVM.Ban = _unitOfWork.banRepository.GetByStringId(maBan);
                BanHangVM.MonDaGois = _unitOfWork.monDaGoiRepository
                                                  .FindIncludeTwo(x => x.Ban, y => y.ThucDon, z => z.MaBan.Equals(maBan))
                                                  .OrderBy(x => x.ThucDon.TenMon)
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
                if(monInBanTo.Count == 0)
                {
                    var banTo = _unitOfWork.banRepository.GetByStringId(maBanTo);
                    banTo.Flag = true;
                    var banFrom = _unitOfWork.banRepository.GetByStringId(maBanFrom);
                    banFrom.Flag = false;
                    _unitOfWork.banRepository.Update(banTo);
                    _unitOfWork.banRepository.Update(banFrom);
                    
                    foreach(var item in monInBanFrom)
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
                                                  .OrderBy(x => x.ThucDon.TenMon)
                                                  .ToList();
                    var listDecimal = BanHangVM.MonDaGois.Select(x => x.ThanhTien).ToList();
                    BanHangVM.TongTien = listDecimal.Sum();
                }
                else
                {
                    
                    foreach(var itemFrom in monInBanFrom)
                    {
                        foreach (var itemTo in monInBanTo)
                        {
                            if(itemFrom.ThucDonId == itemTo.ThucDonId)
                            {
                                itemTo.SoLuong += itemFrom.SoLuong;
                                itemTo.PhuPhi += itemFrom.PhuPhi;
                                itemTo.ThanhTien += itemFrom.ThanhTien;
                                _unitOfWork.monDaGoiRepository.Update(itemTo);
                            }
                            else
                            {
                                var mon = new MonDaGoi()
                                {
                                    SoLuong = itemFrom.SoLuong,
                                    ThanhTien = itemFrom.ThanhTien,
                                    GiaTien = itemFrom.GiaTien,
                                    PhuPhi = itemFrom.PhuPhi,
                                    PhiPhucVu = itemFrom.PhiPhucVu,
                                    MaBan = maBanTo,
                                    ThucDonId = itemFrom.ThucDonId
                                };
                                // kt tenmon xem co trong ds mon cua banTo ko
                                if(!_unitOfWork.monDaGoiRepository.Find(x => x.MaBan.Equals(maBanTo)).Any(x => x.ThucDonId == mon.ThucDonId))
                                {
                                    _unitOfWork.monDaGoiRepository.Create(mon);
                                    _unitOfWork.Complete();
                                }
                                
                            }
                        }
                       
                    }
                    _unitOfWork.monDaGoiRepository.DeleteRange(monInBanFrom);
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
                                                  .OrderBy(x => x.ThucDon.TenMon)
                                                  .ToList();
                    var listDecimal = BanHangVM.MonDaGois.Select(x => x.ThanhTien).ToList();
                    BanHangVM.TongTien = listDecimal.Sum();
                }
            }

            return View(BanHangVM);
        }

        public JsonResult MonInBan(string maBan)
        {
            var mons = _unitOfWork.monDaGoiRepository.Find(x => x.MaBan.Equals(maBan));
            if (mons.Count() > 0)
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }

        public ActionResult creat()
        {
            return View();
        }

        public ActionResult edit()
        {
            return View();
        }

        public ActionResult DetailsRedirect(string strUrl)
        {
            return Redirect(strUrl);
        }
    }
}