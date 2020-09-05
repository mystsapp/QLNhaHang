namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bans",
                c => new
                    {
                        MaBan = c.String(nullable: false, maxLength: 20, unicode: false),
                        TenBan = c.String(nullable: false, maxLength: 50),
                        SoLuongKhach = c.Int(nullable: false),
                        CheckBan = c.Int(nullable: false),
                        GhiChu = c.String(maxLength: 250),
                        Flag = c.Boolean(nullable: false),
                        TenVP = c.String(nullable: false, maxLength: 100),
                        NgayTao = c.DateTime(nullable: false),
                        NguoiTao = c.String(maxLength: 50),
                        MaSo = c.String(nullable: false, maxLength: 10, unicode: false),
                        KhuVucId = c.Int(nullable: false),
                        Xoa = c.Boolean(),
                        LogFile = c.String(),
                    })
                .PrimaryKey(t => t.MaBan)
                .ForeignKey("dbo.KhuVucs", t => t.KhuVucId, cascadeDelete: true)
                .Index(t => t.KhuVucId);
            
            CreateTable(
                "dbo.KhuVucs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        GhiChu = c.String(maxLength: 250),
                        VanPhongId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VanPhongs", t => t.VanPhongId, cascadeDelete: true)
                .Index(t => t.VanPhongId);
            
            CreateTable(
                "dbo.VanPhongs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MaVP = c.String(nullable: false, maxLength: 10, unicode: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        DiaChi = c.String(maxLength: 250),
                        DienThoai = c.String(maxLength: 15, unicode: false),
                        Role = c.String(maxLength: 15),
                        NgayTao = c.DateTime(nullable: false),
                        NguoiTao = c.String(maxLength: 50),
                        MaSoThue = c.String(maxLength: 20, unicode: false),
                        MauSo = c.String(maxLength: 20, unicode: false),
                        KyHieu = c.String(maxLength: 20, unicode: false),
                        QuyenSo = c.String(maxLength: 20, unicode: false),
                        So = c.String(maxLength: 20, unicode: false),
                        SoThuTu = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Beps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SoLuong = c.Int(nullable: false),
                        ThanhTien = c.Decimal(precision: 18, scale: 2),
                        GiaTien = c.Decimal(precision: 18, scale: 2),
                        PhuPhi = c.Decimal(precision: 18, scale: 2),
                        PhiPhucVu = c.Boolean(nullable: false),
                        MaBan = c.String(maxLength: 20, unicode: false),
                        ThucDonId = c.Int(nullable: false),
                        LanGui = c.Int(nullable: false),
                        DaGui = c.Boolean(nullable: false),
                        DaLam = c.Boolean(nullable: false),
                        VanPhong = c.String(maxLength: 100),
                        Username = c.String(maxLength: 50, unicode: false),
                        NgayTao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bans", t => t.MaBan)
                .ForeignKey("dbo.ThucDons", t => t.ThucDonId, cascadeDelete: true)
                .Index(t => t.MaBan)
                .Index(t => t.ThucDonId);
            
            CreateTable(
                "dbo.ThucDons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenMon = c.String(maxLength: 200),
                        GiaTien = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DonViTinh = c.String(maxLength: 20),
                        MaLoaiId = c.Int(nullable: false),
                        GhiChu = c.String(maxLength: 200),
                        NgayTao = c.DateTime(nullable: false),
                        NguoiTao = c.String(maxLength: 50),
                        VanPhong = c.String(nullable: false, maxLength: 100),
                        Xoa = c.Boolean(),
                        LogFile = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LoaiThucDons", t => t.MaLoaiId, cascadeDelete: true)
                .Index(t => t.MaLoaiId);
            
            CreateTable(
                "dbo.LoaiThucDons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenLoai = c.String(nullable: false, maxLength: 100),
                        PhuPhi = c.Decimal(precision: 18, scale: 2),
                        MoTa = c.String(maxLength: 200),
                        GhiChu = c.String(maxLength: 200),
                        NgayTao = c.DateTime(nullable: false),
                        NguoiTao = c.String(maxLength: 50),
                        NoiLamViec = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ChiTietHDs",
                c => new
                    {
                        MaCTHD = c.Int(nullable: false, identity: true),
                        MaHD = c.String(maxLength: 20, unicode: false),
                        MaThucDon = c.Int(nullable: false),
                        DonGia = c.Decimal(precision: 18, scale: 2),
                        SoLuong = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MaCTHD)
                .ForeignKey("dbo.HoaDons", t => t.MaHD)
                .ForeignKey("dbo.ThucDons", t => t.MaThucDon, cascadeDelete: true)
                .Index(t => t.MaHD)
                .Index(t => t.MaThucDon);
            
            CreateTable(
                "dbo.HoaDons",
                c => new
                    {
                        MaHD = c.String(nullable: false, maxLength: 20, unicode: false),
                        MaNV = c.String(maxLength: 20, unicode: false),
                        VanPhongId = c.Int(nullable: false),
                        MaBan = c.String(maxLength: 20, unicode: false),
                        HTThanhToan = c.String(maxLength: 50),
                        NgayTao = c.DateTime(),
                        NgayGiao = c.DateTime(),
                        GhiChu = c.String(maxLength: 300),
                        ThanhTienHD = c.Decimal(precision: 18, scale: 2),
                        TyLePPV = c.Decimal(precision: 18, scale: 2),
                        PhiPhucvu = c.Decimal(precision: 18, scale: 2),
                        TongTienSauPPV = c.Decimal(precision: 18, scale: 2),
                        VAT = c.Decimal(precision: 18, scale: 2),
                        TienThueVAT = c.Decimal(precision: 18, scale: 2),
                        ThanhTienVAT = c.Decimal(precision: 18, scale: 2),
                        SoTienBangChu = c.String(maxLength: 200),
                        NumberId = c.String(maxLength: 20, unicode: false),
                        MauSo = c.String(maxLength: 20, unicode: false),
                        KyHieu = c.String(maxLength: 20, unicode: false),
                        QuyenSo = c.String(maxLength: 20, unicode: false),
                        So = c.String(maxLength: 20, unicode: false),
                        SoThuTu = c.String(maxLength: 20, unicode: false),
                        TenKH = c.String(maxLength: 100),
                        Phone = c.String(maxLength: 15, unicode: false),
                        DiaChi = c.String(maxLength: 250),
                        TenDonVi = c.String(maxLength: 100),
                        MaSoThue = c.String(maxLength: 20, unicode: false),
                        DaIn = c.Boolean(),
                        NgayIn = c.DateTime(),
                        NoiDung = c.String(maxLength: 300),
                        SoTien = c.Decimal(precision: 18, scale: 2),
                        Xoa = c.Boolean(),
                        LogFile = c.String(),
                    })
                .PrimaryKey(t => t.MaHD)
                .ForeignKey("dbo.Bans", t => t.MaBan)
                .ForeignKey("dbo.NhanViens", t => t.MaNV)
                .ForeignKey("dbo.VanPhongs", t => t.VanPhongId, cascadeDelete: true)
                .Index(t => t.MaNV)
                .Index(t => t.VanPhongId)
                .Index(t => t.MaBan);
            
            CreateTable(
                "dbo.NhanViens",
                c => new
                    {
                        MaNV = c.String(nullable: false, maxLength: 20, unicode: false),
                        HoTen = c.String(nullable: false, maxLength: 50),
                        NgaySinh = c.DateTime(),
                        GioiTinh = c.String(maxLength: 5),
                        DiaChi = c.String(maxLength: 100),
                        DienThoai = c.String(maxLength: 15, unicode: false),
                        ChucVu = c.String(maxLength: 30),
                        Username = c.String(nullable: false, maxLength: 50, unicode: false),
                        Password = c.String(nullable: false, maxLength: 50, unicode: false),
                        TrangThai = c.Boolean(nullable: false),
                        NguoiTao = c.String(maxLength: 50),
                        NgayTao = c.DateTime(),
                        NguoiCapNhat = c.String(maxLength: 50),
                        NgayCapNhat = c.DateTime(),
                        Role = c.String(maxLength: 15),
                        PhongBan = c.String(maxLength: 50),
                        KhuVucId = c.Int(nullable: false),
                        NoiLamViec = c.String(maxLength: 50),
                        Xoa = c.Boolean(),
                        LogFile = c.String(),
                    })
                .PrimaryKey(t => t.MaNV)
                .ForeignKey("dbo.KhuVucs", t => t.KhuVucId, cascadeDelete: true)
                .Index(t => t.KhuVucId);
            
            CreateTable(
                "dbo.KhachHangs",
                c => new
                    {
                        MaKH = c.String(nullable: false, maxLength: 50, unicode: false),
                        TenKH = c.String(maxLength: 100),
                        GioiTinh = c.String(maxLength: 10),
                        NgayTao = c.DateTime(nullable: false),
                        NguoiTao = c.String(maxLength: 50),
                        Email = c.String(maxLength: 50, unicode: false),
                        Phone = c.String(maxLength: 15, unicode: false),
                        DiaChi = c.String(maxLength: 250),
                        TenDonVi = c.String(maxLength: 100),
                        MaSoThue = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.MaKH);
            
            CreateTable(
                "dbo.MonDaGois",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SoLuong = c.Int(nullable: false),
                        ThanhTien = c.Decimal(precision: 18, scale: 2),
                        GiaTien = c.Decimal(precision: 18, scale: 2),
                        PhuPhi = c.Decimal(precision: 18, scale: 2),
                        PhiPhucVu = c.Boolean(nullable: false),
                        MaBan = c.String(maxLength: 20, unicode: false),
                        ThucDonId = c.Int(nullable: false),
                        LanGui = c.Int(nullable: false),
                        DaGui = c.Boolean(nullable: false),
                        DaLam = c.Boolean(nullable: false),
                        VanPhong = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bans", t => t.MaBan)
                .ForeignKey("dbo.ThucDons", t => t.ThucDonId, cascadeDelete: true)
                .Index(t => t.MaBan)
                .Index(t => t.ThucDonId);
            
            CreateTable(
                "dbo.PhaChes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SoLuong = c.Int(nullable: false),
                        ThanhTien = c.Decimal(precision: 18, scale: 2),
                        GiaTien = c.Decimal(precision: 18, scale: 2),
                        PhuPhi = c.Decimal(precision: 18, scale: 2),
                        PhiPhucVu = c.Boolean(nullable: false),
                        MaBan = c.String(maxLength: 20, unicode: false),
                        ThucDonId = c.Int(nullable: false),
                        LanGui = c.Int(nullable: false),
                        DaGui = c.Boolean(nullable: false),
                        DaLam = c.Boolean(nullable: false),
                        VanPhong = c.String(maxLength: 100),
                        Username = c.String(maxLength: 50, unicode: false),
                        NgayTao = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bans", t => t.MaBan)
                .ForeignKey("dbo.ThucDons", t => t.ThucDonId, cascadeDelete: true)
                .Index(t => t.MaBan)
                .Index(t => t.ThucDonId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        MieuTa = c.String(maxLength: 150),
                        NgayTao = c.DateTime(nullable: false),
                        NguoiTao = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ThongTinHDs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MauSo = c.String(maxLength: 20, unicode: false),
                        KyHieu = c.String(maxLength: 20, unicode: false),
                        QuyenSo = c.Int(nullable: false),
                        So = c.Long(nullable: false),
                        SoThuTu = c.String(nullable: false, maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PhaChes", "ThucDonId", "dbo.ThucDons");
            DropForeignKey("dbo.PhaChes", "MaBan", "dbo.Bans");
            DropForeignKey("dbo.MonDaGois", "ThucDonId", "dbo.ThucDons");
            DropForeignKey("dbo.MonDaGois", "MaBan", "dbo.Bans");
            DropForeignKey("dbo.ChiTietHDs", "MaThucDon", "dbo.ThucDons");
            DropForeignKey("dbo.ChiTietHDs", "MaHD", "dbo.HoaDons");
            DropForeignKey("dbo.HoaDons", "VanPhongId", "dbo.VanPhongs");
            DropForeignKey("dbo.HoaDons", "MaNV", "dbo.NhanViens");
            DropForeignKey("dbo.NhanViens", "KhuVucId", "dbo.KhuVucs");
            DropForeignKey("dbo.HoaDons", "MaBan", "dbo.Bans");
            DropForeignKey("dbo.Beps", "ThucDonId", "dbo.ThucDons");
            DropForeignKey("dbo.ThucDons", "MaLoaiId", "dbo.LoaiThucDons");
            DropForeignKey("dbo.Beps", "MaBan", "dbo.Bans");
            DropForeignKey("dbo.Bans", "KhuVucId", "dbo.KhuVucs");
            DropForeignKey("dbo.KhuVucs", "VanPhongId", "dbo.VanPhongs");
            DropIndex("dbo.PhaChes", new[] { "ThucDonId" });
            DropIndex("dbo.PhaChes", new[] { "MaBan" });
            DropIndex("dbo.MonDaGois", new[] { "ThucDonId" });
            DropIndex("dbo.MonDaGois", new[] { "MaBan" });
            DropIndex("dbo.NhanViens", new[] { "KhuVucId" });
            DropIndex("dbo.HoaDons", new[] { "MaBan" });
            DropIndex("dbo.HoaDons", new[] { "VanPhongId" });
            DropIndex("dbo.HoaDons", new[] { "MaNV" });
            DropIndex("dbo.ChiTietHDs", new[] { "MaThucDon" });
            DropIndex("dbo.ChiTietHDs", new[] { "MaHD" });
            DropIndex("dbo.ThucDons", new[] { "MaLoaiId" });
            DropIndex("dbo.Beps", new[] { "ThucDonId" });
            DropIndex("dbo.Beps", new[] { "MaBan" });
            DropIndex("dbo.KhuVucs", new[] { "VanPhongId" });
            DropIndex("dbo.Bans", new[] { "KhuVucId" });
            DropTable("dbo.ThongTinHDs");
            DropTable("dbo.Roles");
            DropTable("dbo.PhaChes");
            DropTable("dbo.MonDaGois");
            DropTable("dbo.KhachHangs");
            DropTable("dbo.NhanViens");
            DropTable("dbo.HoaDons");
            DropTable("dbo.ChiTietHDs");
            DropTable("dbo.LoaiThucDons");
            DropTable("dbo.ThucDons");
            DropTable("dbo.Beps");
            DropTable("dbo.VanPhongs");
            DropTable("dbo.KhuVucs");
            DropTable("dbo.Bans");
        }
    }
}
