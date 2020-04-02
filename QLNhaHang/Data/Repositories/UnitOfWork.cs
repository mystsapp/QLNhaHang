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
        IHoaDonRepository hoaDonRepository { get; }
        IChiTietHDRepository chiTietHDRepository { get; }
        IKhachHangRepository khachHangRepository { get; }
        IThongTinHDRepository thongTinHDRepository { get; }
        ILoaiThucDonRepository loaiThucDonRepository { get; }
        IRoleRepository roleRepository { get; }
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
            hoaDonRepository = new HoaDonRepository(_context);
            chiTietHDRepository = new ChiTietHDRepository(_context);
            khachHangRepository = new KhachHangRepository(_context);
            thongTinHDRepository = new ThongTinHDRepository(_context);
            loaiThucDonRepository = new LoaiThucDonRepository(_context);
            roleRepository = new RoleRepository(_context);
        }

        public IBanRepository banRepository { get; }

        public IVanPhongRepository vanPhongRepository { get; }

        public IMonDaGoiRepository monDaGoiRepository { get; }

        public IThucDonRepository thucDonRepository { get; }

        public IHoaDonRepository hoaDonRepository { get; }

        public IChiTietHDRepository chiTietHDRepository { get; }

        public IKhachHangRepository khachHangRepository { get; }

        public IThongTinHDRepository thongTinHDRepository { get; }

        public ILoaiThucDonRepository loaiThucDonRepository { get; }

        public IRoleRepository roleRepository { get; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.Collect();
        }
    }
}
