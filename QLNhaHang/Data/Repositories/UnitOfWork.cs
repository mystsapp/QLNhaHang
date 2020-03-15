using QLNhaHang.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNhaHang.Data.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IBanRepository banRepository { get; }
        IVanPhongRepository vanPhongRepository { get; }
        Task<int> Complete();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly QLNhaHangDbContext _context;

        public UnitOfWork(QLNhaHangDbContext context)
        {
            _context = context;
            banRepository = new BanRepository(_context);
            vanPhongRepository = new VanPhongRepository(_context);
        }

        public IBanRepository banRepository { get; }

        public IVanPhongRepository vanPhongRepository { get; }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
