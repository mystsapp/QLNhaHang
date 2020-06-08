namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPCAndBTbl : DbMigration
    {
        public override void Up()
        {
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bans", t => t.MaBan)
                .ForeignKey("dbo.ThucDons", t => t.ThucDonId, cascadeDelete: true)
                .Index(t => t.MaBan)
                .Index(t => t.ThucDonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PhaChes", "ThucDonId", "dbo.ThucDons");
            DropForeignKey("dbo.PhaChes", "MaBan", "dbo.Bans");
            DropForeignKey("dbo.Beps", "ThucDonId", "dbo.ThucDons");
            DropForeignKey("dbo.Beps", "MaBan", "dbo.Bans");
            DropIndex("dbo.PhaChes", new[] { "ThucDonId" });
            DropIndex("dbo.PhaChes", new[] { "MaBan" });
            DropIndex("dbo.Beps", new[] { "ThucDonId" });
            DropIndex("dbo.Beps", new[] { "MaBan" });
            DropTable("dbo.PhaChes");
            DropTable("dbo.Beps");
        }
    }
}
