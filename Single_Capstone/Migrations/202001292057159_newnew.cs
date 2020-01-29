namespace Single_Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newnew : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "InventoryProducts_Id", c => c.Int());
            CreateIndex("dbo.Products", "InventoryProducts_Id");
            AddForeignKey("dbo.Products", "InventoryProducts_Id", "dbo.InventoryProducts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "InventoryProducts_Id", "dbo.InventoryProducts");
            DropIndex("dbo.Products", new[] { "InventoryProducts_Id" });
            DropColumn("dbo.Products", "InventoryProducts_Id");
        }
    }
}
