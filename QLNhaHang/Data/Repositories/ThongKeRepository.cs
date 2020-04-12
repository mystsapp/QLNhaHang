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
            return list;
        }

        public IEnumerable<HoaDon> ListSevenDay()
        {
            return GetAll().OrderByDescending(x => x.NgayTao);
        }
    }
}