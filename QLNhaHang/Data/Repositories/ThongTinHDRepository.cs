using Data.Interfaces;
using PagedList;
using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNhaHang.Data.Repositories
{
    public interface IThongTinHDRepository : IRepository<ThongTinHD>
    {
        IPagedList<ThongTinHD> ListThongTinHD(string searchString, int? page);
    }
    public class ThongTinHDRepository : Repository<ThongTinHD>, IThongTinHDRepository
    {
        public ThongTinHDRepository(QLNhaHangDbContext context) : base(context)
        {
        }

        public IPagedList<ThongTinHD> ListThongTinHD(string searchString, int? page)
        {
            // return a 404 if user browses to before the first page
            if (page != 0 && page < 1)
                return null;

            // retrieve list from database/whereverand

            var list = GetAll().AsQueryable();
            //list = list.Where(x => x.NguoiCap == hoTen);
            if (!string.IsNullOrEmpty(searchString))
            {
                list = list.Where(x => x.MauSo.ToLower().Contains(searchString.ToLower()) ||
                                       x.KyHieu.ToLower().Contains(searchString.ToLower()) ||
                                       x.SoThuTu.ToLower().Contains(searchString.ToLower()));
                int intNum;
                if(Int32.TryParse(searchString, out intNum))
                {
                    list = list.Where(x => x.QuyenSo == intNum ||
                                           x.So == intNum);
                }
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