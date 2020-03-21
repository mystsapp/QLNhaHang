using Data.Interfaces;
using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNhaHang.Data.Repositories
{
    public interface IChiTietHDRepository : IRepository<ChiTietHD>
    {

    }
    public class ChiTietHDRepository : Repository<ChiTietHD>, IChiTietHDRepository
    {
        public ChiTietHDRepository(QLNhaHangDbContext context) : base(context)
        {
        }
    }
}