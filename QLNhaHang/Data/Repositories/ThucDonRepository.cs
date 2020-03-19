using Data.Interfaces;
using QLNhaHang.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLNhaHang.Data.Repositories
{
    public interface IThucDonRepository : IRepository<ThucDon>
    {

    }
    public class ThucDonRepository : Repository<ThucDon>, IThucDonRepository
    {
        public ThucDonRepository(QLNhaHangDbContext context) : base(context)
        {
        }
    }
}