namespace Single_Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newnewnew : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Inventories", "COGS");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inventories", "COGS", c => c.Double(nullable: false));
        }
    }
}
