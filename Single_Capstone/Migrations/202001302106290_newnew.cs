namespace Single_Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newnew : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InventoryProducts", "ProductName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InventoryProducts", "ProductName");
        }
    }
}
