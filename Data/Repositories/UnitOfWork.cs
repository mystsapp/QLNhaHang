using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IBanRepository banRepository { get; }
        
        Task<int> Complete();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly QLNhaHangDbContext _context;

        public UnitOfWork(QLNhaHangDbContext context)
        {
            _context = context;
            banRepository = new BanRepository(_context);            
        }

        public IBanRepository banRepository { get; }


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
