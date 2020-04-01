namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TenMonnVarChar : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ThucDons", "DonViTinh", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ThucDons", "DonViTinh", c => c.String(maxLength: 20, unicode: false));
        }
    }
}
