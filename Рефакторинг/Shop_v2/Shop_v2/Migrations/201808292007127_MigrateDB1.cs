namespace Shop_v2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDB1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Flat", c => c.String());
            DropColumn("dbo.Users", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Users", "Flat");
        }
    }
}
