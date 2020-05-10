using Data.Interfaces;
using Newtonsoft.Json;
using PagedList;
using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNhaHang.Data.Repositories
{
    public interface IKhuVucRepository : IRepository<KhuVuc>
    {
        IPagedList<KhuVuc> ListKhuVuc(string role, int vpId, string searchString, string vanPhongByRoles, int ddlVP, int? page);
    }
    public class KhuVucRepository : Repository<KhuVuc>, IKhuVucRepository
    {
        public KhuVucRepository(QLNhaHangDbContext context) : base(context)
        {
        }

        public IPagedList<KhuVuc> ListKhuVuc(string role, int vpId, string searchString, string vanPhongByRoles, int ddlVP, int? page)
        {
            // return a 404 if user browses to before the first page
            if (page != 0 && page < 1)
                return null;

            // retrieve list from database/whereverand

            var list = GetAll().AsQueryable();
            if (ddlVP != 0)
            {
                list = list.Where(x => x.VanPhongId == ddlVP);
            }
            if (role != "Admins")
            {
                if (role == "Users")
                {
                    list = list.Where(x => x.VanPhongId.Equals(vpId));
                }
                else
                {
                    //// get all ban in VP by role
                    //var vanPhongs = _context.VanPhongs.Where(x => x.Role.Equals(role));
                    var vanPhongs = JsonConvert.DeserializeObject<List<VanPhong>>(vanPhongByRoles);
                    var kvList = new List<KhuVuc>();
                    foreach (var vanPhong in vanPhongs)
                    {
                        var khuVucs = Find(x => x.VanPhongId == vanPhong.Id);

                        if (khuVucs != null)
                        {
                            kvList.AddRange(khuVucs);
                        }

                    }
                    list = kvList.AsQueryable();
                    //list = list.Where(x => x.VanPhong.Role.Equals(role) || x.VanPhong.Name.Equals(roleVP));
                }

            }
            //list = list.Where(x => x.NguoiCap == hoTen);
            if (!string.IsNullOrEmpty(searchString))
            {
                list = list.Where(x => x.Name.ToLower().Contains(searchString.ToLower()) ||
                                       x.VanPhong.Name.ToLower().Contains(searchString.ToLower()));

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