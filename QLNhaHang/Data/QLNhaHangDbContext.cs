using QLNhaHang.Data.Models;
using System.Data.Entity;

namespace QLNhaHang.Data
{
    public class QLNhaHangDbContext : DbContext
    {
        public QLNhaHangDbContext()
            : base("name=QLNhaHangDbContext")
        {
        }

        public DbSet<Ban> Bans { get; set; }
        public DbSet<ChiTietHD> ChiTietHDs { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<LoaiThucDon> LoaiThucDons { get; set; }
        public DbSet<MonDaGoi> MonDaGois { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<ThucDon> ThucDons { get; set; }
        public DbSet<VanPhong> VanPhongs { get; set; }
        public DbSet<ThongTinHD> ThongTinHDs { get; set; }
        public DbSet<KhuVuc> KhuVucs { get; set; }

    }
}