using Data.Interfaces;
using QLNhaHang.Data.Models;

namespace QLNhaHang.Data.Repositories
{
    public interface IVanPhongRepository : IRepository<VanPhong>
    {
    }

    public class VanPhongRepository : Repository<VanPhong>, IVanPhongRepository
    {
        public VanPhongRepository(QLNhaHangDbContext context) : base(context)
        {
        }
    }
}