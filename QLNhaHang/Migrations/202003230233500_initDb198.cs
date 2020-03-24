namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initDb198 : DbMigration
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
                    })
                .PrimaryKey(t => t.MaBan);
            
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
                        MaKH = c.String(maxLength: 50, unicode: false),
                        MaBan = c.String(maxLength: 20, unicode: false),
                        HTThanhToan = c.String(maxLength: 50),
                        NgayTao = c.DateTime(),
                        NgayGiao = c.DateTime(),
                        GhiChu = c.String(maxLength: 300),
                        ThanhTienHD = c.Decimal(precision: 18, scale: 2),
                        PhiPhucvu = c.Decimal(precision: 18, scale: 2),
                        VAT = c.Decimal(precision: 18, scale: 2),
                        TongTien = c.Decimal(precision: 18, scale: 2),
                        NumberId = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.MaHD)
                .ForeignKey("dbo.Bans", t => t.MaBan)
                .ForeignKey("dbo.KhachHangs", t => t.MaKH)
                .ForeignKey("dbo.NhanViens", t => t.MaNV)
                .Index(t => t.MaNV)
                .Index(t => t.MaKH)
                .Index(t => t.MaBan);
            
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
                        TenDonVi = c.String(maxLength: 100),
                        MaSoThue = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.MaKH);
            
            CreateTable(
                "dbo.NhanViens",
                c => new
                    {
                        MaNV = c.String(nullable: false, maxLength: 20, unicode: false),
                        HoTen = c.String(maxLength: 50),
                        NgaySinh = c.DateTime(),
                        GioiTinh = c.String(maxLength: 5),
                        DiaChi = c.String(maxLength: 100),
                        DienThoai = c.String(maxLength: 15, unicode: false),
                        ChucVu = c.String(maxLength: 30),
                        Username = c.String(nullable: false, maxLength: 50, unicode: false),
                        Password = c.String(nullable: false, maxLength: 50, unicode: false),
                        TrangThai = c.Boolean(nullable: false),
                        NguoiTao = c.String(maxLength: 50),
                        Ngaytao = c.DateTime(),
                        NguoiCapNhat = c.String(maxLength: 50),
                        Ngaycapnhat = c.DateTime(),
                        RoleId = c.Int(nullable: false),
                        PhongBan = c.String(maxLength: 50),
                        VanPhongId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MaNV)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.VanPhongs", t => t.VanPhongId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.VanPhongId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        MieuTa = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VanPhongs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MaVP = c.String(nullable: false, maxLength: 10, unicode: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        DiaChi = c.String(maxLength: 250),
                        DienThoai = c.String(maxLength: 15, unicode: false),
                        Role = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ThucDons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenMon = c.String(maxLength: 200),
                        GiaTien = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DonViTinh = c.String(maxLength: 20, unicode: false),
                        MaLoaiId = c.Int(nullable: false),
                        GhiChu = c.String(maxLength: 200, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LoaiThucDons", t => t.MaLoaiId, cascadeDelete: true)
                .Index(t => t.MaLoaiId);
            
            CreateTable(
                "dbo.LoaiThucDons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenLoai = c.String(maxLength: 100),
                        PhuPhi = c.Decimal(precision: 18, scale: 2),
                        MoTa = c.String(maxLength: 200),
                        GhiChu = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bans", t => t.MaBan)
                .ForeignKey("dbo.ThucDons", t => t.ThucDonId, cascadeDelete: true)
                .Index(t => t.MaBan)
                .Index(t => t.ThucDonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MonDaGois", "ThucDonId", "dbo.ThucDons");
            DropForeignKey("dbo.MonDaGois", "MaBan", "dbo.Bans");
            DropForeignKey("dbo.ChiTietHDs", "MaThucDon", "dbo.ThucDons");
            DropForeignKey("dbo.ThucDons", "MaLoaiId", "dbo.LoaiThucDons");
            DropForeignKey("dbo.ChiTietHDs", "MaHD", "dbo.HoaDons");
            DropForeignKey("dbo.HoaDons", "MaNV", "dbo.NhanViens");
            DropForeignKey("dbo.NhanViens", "VanPhongId", "dbo.VanPhongs");
            DropForeignKey("dbo.NhanViens", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.HoaDons", "MaKH", "dbo.KhachHangs");
            DropForeignKey("dbo.HoaDons", "MaBan", "dbo.Bans");
            DropIndex("dbo.MonDaGois", new[] { "ThucDonId" });
            DropIndex("dbo.MonDaGois", new[] { "MaBan" });
            DropIndex("dbo.ThucDons", new[] { "MaLoaiId" });
            DropIndex("dbo.NhanViens", new[] { "VanPhongId" });
            DropIndex("dbo.NhanViens", new[] { "RoleId" });
            DropIndex("dbo.HoaDons", new[] { "MaBan" });
            DropIndex("dbo.HoaDons", new[] { "MaKH" });
            DropIndex("dbo.HoaDons", new[] { "MaNV" });
            DropIndex("dbo.ChiTietHDs", new[] { "MaThucDon" });
            DropIndex("dbo.ChiTietHDs", new[] { "MaHD" });
            DropTable("dbo.MonDaGois");
            DropTable("dbo.LoaiThucDons");
            DropTable("dbo.ThucDons");
            DropTable("dbo.VanPhongs");
            DropTable("dbo.Roles");
            DropTable("dbo.NhanViens");
            DropTable("dbo.KhachHangs");
            DropTable("dbo.HoaDons");
            DropTable("dbo.ChiTietHDs");
            DropTable("dbo.Bans");
        }
    }
}
