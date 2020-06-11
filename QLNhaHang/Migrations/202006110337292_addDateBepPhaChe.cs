namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDateBepPhaChe : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Beps", "NgayTao", c => c.DateTime());
            AddColumn("dbo.PhaChes", "NgayTao", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PhaChes", "NgayTao");
            DropColumn("dbo.Beps", "NgayTao");
        }
    }
}
