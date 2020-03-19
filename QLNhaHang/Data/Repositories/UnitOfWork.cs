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
        IMonDaGoiRepository monDaGoiRepository { get; }
        IThucDonRepository thucDonRepository { get; }
        int Complete();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly QLNhaHangDbContext _context;

        public UnitOfWork(QLNhaHangDbContext context)
        {
            _context = context;
            banRepository = new BanRepository(_context);
            vanPhongRepository = new VanPhongRepository(_context);
            monDaGoiRepository = new MonDaGoiRepository(_context);
            thucDonRepository = new ThucDonRepository(_context);
        }

        public IBanRepository banRepository { get; }

        public IVanPhongRepository vanPhongRepository { get; }

        public IMonDaGoiRepository monDaGoiRepository { get; }

        public IThucDonRepository thucDonRepository { get; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
