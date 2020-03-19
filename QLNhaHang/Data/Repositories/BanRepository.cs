using Data.Interfaces;
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

    }
    public class BanRepository : Repository<Ban>, IBanRepository
    {
        public BanRepository(QLNhaHangDbContext context) : base(context)
        {
        }
    }
}
