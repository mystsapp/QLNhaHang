using Data.Interfaces;
using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages;

namespace QLNhaHang.Data.Repositories
{
    public interface IThongKeRepository : IRepository<HoaDon>
    {
        IEnumerable<HoaDon> ListHoaDonTheoNgayIn(int vanPhongId, string searchFromDate, string searchToDate);
        IEnumerable<HoaDon> ListHoaDonTheoNgayTao(int vanPhongId, string tinhTrang, string searchFromDate, string searchToDate);
        IEnumerable<HoaDon> ListSevenDay();
        IEnumerable<HoaDon> ListHoaDonTheoTiecBuffet(int vanPhongId, int khuVucId, int thucDonId, string tuNgay, string denNgay);
        IEnumerable<HoaDon> ListHoaDonTheoNhanVien(string nhanVienId, string tuNgay, string denNgay);
    }
    public class ThongKeRepository : Repository<HoaDon>, IThongKeRepository
    {
        public ThongKeRepository(QLNhaHangDbContext context) : base(context)
        {
        }

        public IEnumerable<HoaDon> ListHoaDonTheoNgayTao(int vanPhongId, string tinhTrang, string searchFromDate, string searchToDate)
        {

            var list = GetAllIncludeThree(x => x.Ban, y => y.NhanVien, z => z.VanPhong);

            // search VP
            if (vanPhongId != 0)
            {
                list = list.Where(x => x.VanPhongId == vanPhongId);
            }

            if (!tinhTrang.IsEmpty())
            {
                list = list.Where(x => x.DaIn == bool.Parse(tinhTrang));
            }

            var count = list.Count();
            DateTime fromDate, toDate;
            if (!string.IsNullOrEmpty(searchFromDate) && !string.IsNullOrEmpty(searchToDate))
            {

                try
                {
                    fromDate = DateTime.Parse(searchFromDate);
                    toDate = DateTime.Parse(searchToDate);

                    if (fromDate > toDate)
                    {
                        return null;
                    }
                    list = list.Where(x => x.NgayTao >= fromDate &&
                                       x.NgayTao < toDate.AddDays(1));
                }
                catch (Exception)
                {

                    return null;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(searchFromDate))
                {
                    try
                    {
                        fromDate = DateTime.Parse(searchFromDate);
                        list = list.Where(x => x.NgayTao >= fromDate);
                    }
                    catch (Exception)
                    {
                        return null;
                    }

                }
                if (!string.IsNullOrEmpty(searchToDate))
                {
                    try
                    {
                        toDate = DateTime.Parse(searchToDate);
                        list = list.Where(x => x.NgayTao < toDate.AddDays(1));

                    }
                    catch (Exception)
                    {
                        return null;
                    }

                }
            }

            count = list.Count();
            if (count == 0)
            {
                return null;
            }
            return list;
        }
        
        public IEnumerable<HoaDon> ListHoaDonTheoNgayIn(int vanPhongId, string searchFromDate, string searchToDate)
        {

            var list = GetAllIncludeThree(x => x.Ban, y => y.NhanVien, z => z.VanPhong);
            // search VP
            if (vanPhongId != 0)
            {
                list = list.Where(x => x.VanPhongId == vanPhongId);
            }

            var count = list.Count();
            DateTime fromDate, toDate;
            if (!string.IsNullOrEmpty(searchFromDate) && !string.IsNullOrEmpty(searchToDate))
            {

                try
                {
                    fromDate = DateTime.Parse(searchFromDate);
                    toDate = DateTime.Parse(searchToDate);

                    if (fromDate > toDate)
                    {
                        return null;
                    }
                    list = list.Where(x => x.NgayIn >= fromDate &&
                                       x.NgayIn < toDate.AddDays(1));
                }
                catch (Exception)
                {

                    return null;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(searchFromDate))
                {
                    try
                    {
                        fromDate = DateTime.Parse(searchFromDate);
                        list = list.Where(x => x.NgayIn >= fromDate);
                    }
                    catch (Exception)
                    {
                        return null;
                    }

                }
                if (!string.IsNullOrEmpty(searchToDate))
                {
                    try
                    {
                        toDate = DateTime.Parse(searchToDate);
                        list = list.Where(x => x.NgayIn < toDate.AddDays(1));

                    }
                    catch (Exception)
                    {
                        return null;
                    }

                }
            }

            count = list.Count();
            if (count == 0)
            {
                return null;
            }
            return list;
        }

        public IEnumerable<HoaDon> ListSevenDay()
        {
            return GetAll().OrderByDescending(x => x.NgayTao);
        }

        public IEnumerable<HoaDon> ListHoaDonTheoTiecBuffet(int vanPhongId, int khuVucId, int thucDonId, string searchFromDate, string searchToDate)
        {
            var list = GetAllIncludeThree(x => x.Ban, y => y.NhanVien, z => z.VanPhong).ToList();
            // search VP
            if (vanPhongId != 0)
            {
                list = list.Where(x => x.VanPhongId == vanPhongId).ToList();
            }

            var count = list.Count();
            DateTime fromDate, toDate;
            if (!string.IsNullOrEmpty(searchFromDate) && !string.IsNullOrEmpty(searchToDate))
            {

                try
                {
                    fromDate = DateTime.Parse(searchFromDate);
                    toDate = DateTime.Parse(searchToDate);

                    if (fromDate > toDate)
                    {
                        return null;
                    }
                    if (fromDate == toDate)
                    {
                        list = list.Where(x => x.NgayTao.Value.ToShortDateString() == fromDate.ToShortDateString()).ToList();
                    }
                    else
                    {
                        list = list.Where(x => x.NgayTao >= fromDate &&
                                       x.NgayTao < toDate.AddDays(1)).ToList();
                    }
                    
                }
                catch (Exception)
                {

                    return null;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(searchFromDate))
                {
                    try
                    {
                        fromDate = DateTime.Parse(searchFromDate);
                        list = list.Where(x => x.NgayTao >= fromDate).ToList();
                    }
                    catch (Exception)
                    {
                        return null;
                    }

                }
                if (!string.IsNullOrEmpty(searchToDate))
                {
                    try
                    {
                        toDate = DateTime.Parse(searchToDate);
                        list = list.Where(x => x.NgayTao < toDate.AddDays(1)).ToList();

                    }
                    catch (Exception)
                    {
                        return null;
                    }

                }
            }
            /////////// thuc don ////////////
            if(khuVucId != 0 && thucDonId != 0)
            {
                List<HoaDon> hoaDons = new List<HoaDon>();
                foreach (var hd in list)
                {
                    List<ChiTietHD> chiTietHDs = _context.ChiTietHDs.Where(x => x.MaHD == hd.MaHD).ToList();
                    foreach(var chiTiet in chiTietHDs)
                    {
                        if(chiTiet.MaThucDon == thucDonId)
                        {
                            hoaDons.Add(hd);
                        }
                    }
                }
                list = hoaDons;
            }

            count = list.Count();
            if(count == 0)
            {
                return null;
            }
            return list;
        }

        public IEnumerable<HoaDon> ListHoaDonTheoNhanVien(string nhanVienId, string searchFromDate, string searchToDate)
        {
            var list = GetAllIncludeThree(x => x.Ban, y => y.NhanVien, z => z.VanPhong).ToList();
            // search VP
            if (!string.IsNullOrEmpty(nhanVienId))
            {
                list = list.Where(x => x.MaNV == nhanVienId).ToList();
            }

            var count = list.Count();

            // search Date
            DateTime fromDate, toDate;
            if (!string.IsNullOrEmpty(searchFromDate) && !string.IsNullOrEmpty(searchToDate))
            {

                try
                {
                    fromDate = DateTime.Parse(searchFromDate);
                    toDate = DateTime.Parse(searchToDate);

                    if (fromDate > toDate)
                    {
                        return null;
                    }
                    if (fromDate == toDate)
                    {
                        list = list.Where(x => x.NgayTao.Value.ToShortDateString() == fromDate.ToShortDateString()).ToList();
                    }
                    else
                    {
                        list = list.Where(x => x.NgayTao >= fromDate &&
                                       x.NgayTao < toDate.AddDays(1)).ToList();
                    }

                }
                catch (Exception)
                {

                    return null;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(searchFromDate))
                {
                    try
                    {
                        fromDate = DateTime.Parse(searchFromDate);
                        list = list.Where(x => x.NgayTao >= fromDate).ToList();
                    }
                    catch (Exception)
                    {
                        return null;
                    }

                }
                if (!string.IsNullOrEmpty(searchToDate))
                {
                    try
                    {
                        toDate = DateTime.Parse(searchToDate);
                        list = list.Where(x => x.NgayTao < toDate.AddDays(1)).ToList();

                    }
                    catch (Exception)
                    {
                        return null;
                    }

                }
            }
            
            count = list.Count();
            if (count == 0)
            {
                return null;
            }
            return list;
        }
    }
}