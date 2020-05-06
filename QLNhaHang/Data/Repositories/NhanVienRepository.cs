using Data.Interfaces;
using PagedList;
using QLNhaHang.Data.Models;
using QLNhaHang.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNhaHang.Data.Repositories
{
    public interface INhanVienRepository : IRepository<NhanVien>
    {
        IPagedList<NhanVien> ListNhanVien(string role, string roleVP, string gioiTinh, string searchString, int? page, int idVP);
        int Login(string username, string password);
        int Changepass(string username, string newpass);
    }
    public class NhanVienRepository : Repository<NhanVien>, INhanVienRepository
    {
        public NhanVienRepository(QLNhaHangDbContext context) : base(context)
        {
        }

        public IPagedList<NhanVien> ListNhanVien(string role, string roleVP, string gioiTinh, string searchString, int? page, int idVP)
        {
            
            // return a 404 if user browses to before the first page
            if (page != 0 && page < 1)
                return null;

            // retrieve list from database/whereverand
            var list = GetAllInclude(x => x.Role, y => y.VanPhong).AsQueryable();
            if (role != "Admins")
            {
                list = list.Where(x => x.VanPhong.Role.Equals(role) || x.VanPhong.Name.Equals(roleVP));
            }
            
            //list = list.Where(x => x.NguoiCap == hoTen);
            //if (!string.IsNullOrEmpty(gioiTinh))
            //{
            //    if(gioiTinh != "None")
            //    {
            //        list = list.Where(x => x.GioiTinh.Equals(gioiTinh));
            //    }
                
            //}

            if(idVP != 0)
            {
                list = list.Where(x => x.VanPhongId == idVP);
            }
            
            if (!string.IsNullOrEmpty(searchString))
            {
                list = list.Where(x => x.MaNV.ToLower().Contains(searchString.ToLower()) ||
                                       x.Username.ToLower().Contains(searchString.ToLower()) ||
                                       x.HoTen.ToLower().Contains(searchString.ToLower()));
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
            var listPaged = list.OrderBy(x => x.NgayTao).ToPagedList(page ?? 1, pageSize);
            //if (page > listPaged.PageCount)
            //    page--;
            // return a 404 if user browses to pages beyond last page. special case first page if no items exist
            if (listPaged.PageNumber != 1 && page != 0 && page > listPaged.PageCount)
                return null;

            return listPaged;
        }

        public int Login(string username, string password)
        {
            var result = Find(x => x.Username.Trim().ToLower() == username.Trim().ToLower()).FirstOrDefault();
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.TrangThai == false)
                {
                    return -1;
                }
                else
                {
                    if (result.Password == MaHoaSHA1.EncodeSHA1(password))
                    {
                        return 1;
                    }
                    else
                    {
                        return -2;
                    }
                }
            }
        }

        public int Changepass(string username, string newpass)
        {
            try
            {
                var result = Find(x => x.Username.Trim().ToLower() == username.Trim().ToLower()).FirstOrDefault();

                result.Password = newpass;
                _context.SaveChanges();
                return 1;
            }
            catch { throw; }
        }
    }
}