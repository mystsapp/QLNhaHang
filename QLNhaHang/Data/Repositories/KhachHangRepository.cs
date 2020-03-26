using Data.Interfaces;
using PagedList;
using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNhaHang.Data.Repositories
{
    public interface IKhachHangRepository : IRepository<KhachHang>
    {
        IPagedList<KhachHang> ListKhachHang(string searchString, int? page);
    }
    public class KhachHangRepository : Repository<KhachHang>, IKhachHangRepository
    {
        public KhachHangRepository(QLNhaHangDbContext context) : base(context)
        {
        }

        public IPagedList<KhachHang> ListKhachHang(string searchString, int? page)
        {

            // return a 404 if user browses to before the first page
            if (page != 0 && page < 1)
                return null;

            // retrieve list from database/whereverand

            var list = GetAll().AsQueryable();
            //list = list.Where(x => x.NguoiCap == hoTen);
            if (!string.IsNullOrEmpty(searchString))
            {
                list = list.Where(x => x.MaKH.ToLower().Contains(searchString.ToLower()) ||
                                       x.TenKH.ToLower().Contains(searchString.ToLower()) ||
                                       x.MaSoThue.ToLower().Contains(searchString.ToLower()) ||
                                       x.TenDonVi.ToLower().Contains(searchString.ToLower()));
            }

            var count = list.Count();
            
            // page the list
            const int pageSize = 2;
            decimal aa = (decimal)list.Count() / (decimal)pageSize;
            var bb = Math.Ceiling(aa);
            if (page > bb)
            {
                page--;
            }
            var listPaged = list.OrderBy(x => x.NgayTao).ToPagedList(page ?? 1, pageSize);
            //if (page > listPaged.PageCount)
            //    page--;
            // return a 404 if user browses to pages beyond last page. special case first page if no items exist
            if (listPaged.PageNumber != 1 && page != 0 && page > listPaged.PageCount)
                return null;


            return listPaged;
        }
    }
}