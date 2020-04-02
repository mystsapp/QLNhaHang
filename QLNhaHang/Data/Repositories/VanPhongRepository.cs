using Data.Interfaces;
using PagedList;
using QLNhaHang.Data.Models;

namespace QLNhaHang.Data.Repositories
{
    public interface IVanPhongRepository : IRepository<VanPhong>
    {
        IPagedList<VanPhong> ListVanPhong(string searchString, int page);
    }

    public class VanPhongRepository : Repository<VanPhong>, IVanPhongRepository
    {
        public VanPhongRepository(QLNhaHangDbContext context) : base(context)
        {
        }

        public IPagedList<VanPhong> ListVanPhong(string searchString, int page)
        {
            // return a 404 if user browses to before the first page
            if (page != 0 && page < 1)
                return null;

            // retrieve list from database/whereverand

            var list = GetAllIncludeOne(x => x.);
            //list = list.Where(x => x.NguoiCap == hoTen);
            if (!string.IsNullOrEmpty(searchString))
            {
                list = list.Where(x => x.TenMon.ToLower().Contains(searchString.ToLower()) ||
                                       x.LoaiThucDon.TenLoai.ToLower().Contains(searchString.ToLower()) ||
                                       x.DonViTinh.ToLower().Contains(searchString.ToLower()));
            }
            decimal donGia;
            if (decimal.TryParse(searchString, out donGia))
            {
                list = list.Where(x => x.GiaTien == donGia);
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