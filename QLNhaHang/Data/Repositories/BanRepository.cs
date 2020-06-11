using Data.Interfaces;
using Newtonsoft.Json;
using PagedList;
using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaHang.Data.Repositories
{
    public interface IBanRepository : IRepository<Ban>
    {
        IPagedList<Ban> ListBan(string role, int khuVucId, string vanPhongByRoles, int ddlKV, string searchString, int? page);
    }
    public class BanRepository : Repository<Ban>, IBanRepository
    {
        public BanRepository(QLNhaHangDbContext context) : base(context)
        {
        }

        public IPagedList<Ban> ListBan(string role, int khuVucId, string vanPhongByRoles, int ddlKV, string searchString, int? page)
        {
            // return a 404 if user browses to before the first page
            if (page != 0 && page < 1)
                return null;

            // retrieve list from database/whereverand

            var list = GetAll().Where(x => x.Xoa != true).AsQueryable();
            
            if (role != "Admins")
            {
                if (role == "Users")
                {
                    list = list.Where(x => x.KhuVucId == khuVucId);
                }
                else
                {
                    //// get all ban in VP by role
                    //var vanPhongs = _context.VanPhongs.Where(x => x.Role.Equals(role));
                    var vanPhongs = JsonConvert.DeserializeObject<List<VanPhong>>(vanPhongByRoles);
                    var banList = new List<Ban>();
                    foreach(var vanPhong in vanPhongs)
                    {
                        var bans = Find(x => x.TenVP.Equals(vanPhong.Name));
                        
                        if(bans != null)
                        {
                            banList.AddRange(bans);
                        }
                        
                    }
                    list = banList.AsQueryable();
                    //list = list.Where(x => x.VanPhong.Role.Equals(role) || x.VanPhong.Name.Equals(roleVP));
                }
                
            }

            if (ddlKV != 0)
            {
                list = list.Where(x => x.KhuVucId == ddlKV);
            }
            //list = list.Where(x => x.NguoiCap == hoTen);
            if (!string.IsNullOrEmpty(searchString))
            {
                list = list.Where(x => x.MaBan.ToLower().Contains(searchString.ToLower()) ||
                                       x.TenBan.ToLower().Contains(searchString.ToLower()) ||
                                       x.TenVP.ToLower().Contains(searchString.ToLower()) ||
                                       x.SoLuongKhach.ToString().ToLower().Contains(searchString.ToLower()));

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
            var listPaged = list.OrderBy(x => x.MaBan).ToPagedList(page ?? 1, pageSize);
            //if (page > listPaged.PageCount)
            //    page--;
            // return a 404 if user browses to pages beyond last page. special case first page if no items exist
            if (listPaged.PageNumber != 1 && page != 0 && page > listPaged.PageCount)
                return null;

            return listPaged;
        }
    }
}
