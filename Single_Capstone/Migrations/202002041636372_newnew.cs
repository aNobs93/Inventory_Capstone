namespace Single_Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newnew : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Businesses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationId = c.String(maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        BusinessName = c.String(),
                        StreetAddress = c.String(),
                        Suite_APT = c.String(),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.Int(nullable: false),
                        Country = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationId)
                .Index(t => t.ApplicationId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Inventories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BusinessId = c.Int(nullable: false),
                        TotalInventoryWorth = c.Double(nullable: false),
                        ProfitMargin = c.Double(nullable: false),
                        GetDate = c.String(),
                        LastInventoryId = c.Int(nullable: false),
                        AverageInventory = c.Double(nullable: false),
                        GMROI = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InventoryProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InventoryId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        ProductName = c.String(),
                        Units = c.Double(nullable: false),
                        GetDate = c.String(),
                        TotalValueOfProducts = c.Double(nullable: false),
                        ProfitToBeMadePerUnit = c.Double(nullable: false),
                        TimesOrdered = c.Int(nullable: false),
                        AmountOrdered = c.Double(nullable: false),
                        AmountSold = c.Double(nullable: false),
                        GMROI = c.Double(nullable: false),
                        ParLevel = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inventories", t => t.InventoryId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.InventoryId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductName = c.String(),
                        PricePerUnit = c.Double(nullable: false),
                        PricePerUnitSelling = c.Double(nullable: false),
                        ProductSafetyNet = c.Double(nullable: false),
                        InventoryProducts_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InventoryProducts", t => t.InventoryProducts_Id)
                .Index(t => t.InventoryProducts_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Products", "InventoryProducts_Id", "dbo.InventoryProducts");
            DropForeignKey("dbo.InventoryProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.InventoryProducts", "InventoryId", "dbo.Inventories");
            DropForeignKey("dbo.Businesses", "ApplicationId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Products", new[] { "InventoryProducts_Id" });
            DropIndex("dbo.InventoryProducts", new[] { "ProductId" });
            DropIndex("dbo.InventoryProducts", new[] { "InventoryId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Businesses", new[] { "ApplicationId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Products");
            DropTable("dbo.InventoryProducts");
            DropTable("dbo.Inventories");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Businesses");
        }
    }
}
