namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixPhuPhi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LoaiThucDons", "PhuPhi", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.ThucDons", "PhuPhi");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ThucDons", "PhuPhi", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.LoaiThucDons", "PhuPhi");
        }
    }
}
