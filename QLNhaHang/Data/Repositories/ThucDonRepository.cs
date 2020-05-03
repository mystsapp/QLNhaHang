using Data.Interfaces;
using PagedList;
using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNhaHang.Data.Repositories
{
    public interface IThucDonRepository : IRepository<ThucDon>
    {
        IPagedList<ThucDon> ListThucDon(string searchString, int? page, int ddlLoai);
    }
    public class ThucDonRepository : Repository<ThucDon>, IThucDonRepository
    {
        public ThucDonRepository(QLNhaHangDbContext context) : base(context)
        {
        }

        public IPagedList<ThucDon> ListThucDon(string searchString, int? page, int ddlLoai)
        {
            // return a 404 if user browses to before the first page
            if (page != 0 && page < 1)
                return null;

            // retrieve list from database/whereverand

            var list = GetAllIncludeOne(x => x.LoaiThucDon).AsQueryable();
            //list = list.Where(x => x.NguoiCap == hoTen);
            if(ddlLoai != 0)
            {
                list = list.Where(x => x.MaLoaiId == ddlLoai);
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                list = list.Where(x => x.TenMon.ToLower().Contains(searchString.ToLower()) ||
                                       x.LoaiThucDon.TenLoai.ToLower().Contains(searchString.ToLower()) || 
                                       x.DonViTinh.ToLower().Contains(searchString.ToLower()));
            }
            decimal donGia;
            if(decimal.TryParse(searchString, out donGia))
            {
                list = list.Where(x => x.GiaTien == donGia);
            }

            var count = list.Count();

            // page the list
            const int pageSize = 10;
            decimal aa = (decimal)list.Count() / (decimal)pageSize;
            var bb = Math.Ceiling(aa);
            if (page > bb)
            {
                page--;
            }
            page = (page == 0) ? 1 : page;
            var listPaged = list.OrderBy(x => x.Id).ToPagedList(page ?? 1, pageSize);
            //if (page > listPaged.PageCount)
            //    page--;
            // return a 404 if user browses to pages beyond last page. special case first page if no items exist
            if (listPaged.PageNumber != 1 && page != 0 && page > listPaged.PageCount)
                return null;

            return listPaged;
        }
    }
}