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
        IPagedList<HoaDon> ListHoaDon(string role, string roleVP, string searchString, string searchFromDate, string searchToDate, int? page);
        List<HoaDon> FindIncludeThree(string name);
    }
    public class HoaDonRepository : Repository<HoaDon>, IHoaDonRepository
    {
        public HoaDonRepository(QLNhaHangDbContext context) : base(context)
        {
        }

        public IPagedList<HoaDon> ListHoaDon(string role, string roleVP, string searchString, string searchFromDate, string searchToDate, int? page)
        {

            // return a 404 if user browses to before the first page
            if (page != 0 && page < 1)
                return null;

            // retrieve list from database/whereverand

            var list = GetAllInclude(x => x.Ban, y => y.NhanVien).AsQueryable();
            if (role != "Admins")
            {
                list = list.Where(x => x.VanPhong.Role.Equals(role) || x.VanPhong.Name.Equals(roleVP));
            }
            //list = list.Where(x => x.NguoiCap == hoTen);
            if (!string.IsNullOrEmpty(searchString))
            {
                list = list.Where(x => x.MaHD.ToLower().Contains(searchString.ToLower()) ||
                                       x.NhanVien.HoTen.ToLower().Contains(searchString.ToLower()) ||
                                       x.TenKH.ToLower().Contains(searchString.ToLower()));
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


                //list.Where(x => x.NgayTao >= fromDate && x.NgayTao < (toDate.AddDays(1))/*.ToPagedList(page, pageSize)*/;



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
            // page the list
            const int pageSize = 10;
            decimal aa = (decimal)list.Count() / (decimal)pageSize;
            var bb = Math.Ceiling(aa);
            if (page > bb)
            {
                page--;
            }
            page = (page == 0) ? 1 : page;
            var listPaged = list.OrderByDescending(x => x.NgayTao).ToPagedList(page ?? 1, pageSize);
            //if (page > listPaged.PageCount)
            //    page--;
            // return a 404 if user browses to pages beyond last page. special case first page if no items exist
            if (listPaged.PageNumber != 1 && page != 0 && page > listPaged.PageCount)
                return null;


            return listPaged;
        }

        public List<HoaDon> FindIncludeThree(string name)
        {
            return _context.HoaDons.Include(x => x.Ban).Include(y => y.NhanVien).Include(z => z.VanPhong).Where(a => a.MaHD.Equals(name)).ToList();
        }

    }
}