using Data.Interfaces;
using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PagedList;

namespace QLNhaHang.Data.Repositories
{
    public interface IHoaDonRepository : IRepository<HoaDon>
    {
        IPagedList<HoaDon> ListHoaDon(string searchString, string searchFromDate, string searchToDate, int page);
    }
    public class HoaDonRepository : Repository<HoaDon>, IHoaDonRepository
    {
        public HoaDonRepository(QLNhaHangDbContext context) : base(context)
        {
        }

        public IPagedList<HoaDon> ListHoaDon(string searchString, string searchFromDate, string searchToDate, int page)
        {

            // return a 404 if user browses to before the first page
            if (page != 0 && page < 1)
                return null;

            // retrieve list from database/whereverand

            var list = GetAllIncludeThree(x => x.Ban, y => y.NhanVien, z => z.KhachHang).AsQueryable();
            //list = list.Where(x => x.NguoiCap == hoTen);
            if (!string.IsNullOrEmpty(searchString))
            {
                list = list.Where(x => x.MaHD.ToLower().Contains(searchString.ToLower()) ||
                                       x.NhanVien.HoTen.ToLower().Contains(searchString.ToLower()) ||
                                       x.KhachHang.TenKH.ToLower().Contains(searchString.ToLower()));
            }

            var count = list.Count();

            if (!string.IsNullOrEmpty(searchFromDate) && !string.IsNullOrEmpty(searchToDate))
            {
                //var f = DateTime.ParseExact(searchFromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //var t = DateTime.ParseExact(searchToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //list.Where(x => x.NgayTao >= f && x.NgayTao < DbFunctions.AddDays(t, 1))/*.ToPagedList(page, pageSize)*/;
                DateTime fromDate, toDate;
                if (DateTime.TryParse(searchFromDate, out fromDate) &&
                   DateTime.TryParse(searchToDate, out toDate))
                {
                    list.Where(x => x.NgayTao >= fromDate && x.NgayTao < (toDate.AddDays(1))/*.ToPagedList(page, pageSize)*/;
                }
            }

            // page the list
            const int pageSize = 2;
            decimal aa = (decimal)list.Count() / (decimal)pageSize;
            var bb = Math.Ceiling(aa);
            if (page > bb)
            {
                page--;
            }
            var listPaged = list.OrderBy(x => x.NgayTao).ToPagedList(page, pageSize);
            //if (page > listPaged.PageCount)
            //    page--;
            // return a 404 if user browses to pages beyond last page. special case first page if no items exist
            if (listPaged.PageNumber != 1 && page != 0 && page > listPaged.PageCount)
                return null;


            return listPaged;
        }
    }
}