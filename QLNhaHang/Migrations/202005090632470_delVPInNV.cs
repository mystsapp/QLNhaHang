namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class delVPInNV : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.NhanViens", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.NhanViens", "VanPhongId", "dbo.VanPhongs");
            DropIndex("dbo.NhanViens", new[] { "RoleId" });
            DropIndex("dbo.NhanViens", new[] { "VanPhongId" });
            AddColumn("dbo.NhanViens", "Role", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.NhanViens", "KhuVucId", c => c.Int(nullable: false));
            CreateIndex("dbo.NhanViens", "KhuVucId");
            AddForeignKey("dbo.NhanViens", "KhuVucId", "dbo.KhuVucs", "Id", cascadeDelete: true);
            DropColumn("dbo.NhanViens", "RoleId");
            DropColumn("dbo.NhanViens", "VanPhongId");
            DropTable("dbo.Roles");
        }
        
        public override void Down()
        {
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
            
            AddColumn("dbo.NhanViens", "VanPhongId", c => c.Int(nullable: false));
            AddColumn("dbo.NhanViens", "RoleId", c => c.Int(nullable: false));
            DropForeignKey("dbo.NhanViens", "KhuVucId", "dbo.KhuVucs");
            DropIndex("dbo.NhanViens", new[] { "KhuVucId" });
            DropColumn("dbo.NhanViens", "KhuVucId");
            DropColumn("dbo.NhanViens", "Role");
            CreateIndex("dbo.NhanViens", "VanPhongId");
            CreateIndex("dbo.NhanViens", "RoleId");
            AddForeignKey("dbo.NhanViens", "VanPhongId", "dbo.VanPhongs", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NhanViens", "RoleId", "dbo.Roles", "Id", cascadeDelete: true);
        }
    }
}
