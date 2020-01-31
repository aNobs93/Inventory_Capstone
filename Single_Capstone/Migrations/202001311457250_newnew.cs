namespace Single_Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newnew : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inventories", "LastInventoryId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inventories", "LastInventoryId");
        }
    }
}
