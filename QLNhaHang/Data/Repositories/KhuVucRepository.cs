using Data.Interfaces;
using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNhaHang.Data.Repositories
{
    public interface IKhuVucRepository : IRepository<KhuVuc>
    {

    }
    public class KhuVucRepository : Repository<KhuVuc>, IKhuVucRepository
    {
        public KhuVucRepository(QLNhaHangDbContext context) : base(context)
        {
        }
    }
}