namespace MyShop.DataAcess.MySQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedOrders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        OrderId = c.String(maxLength: 128, storeType: "nvarchar"),
                        ProductId = c.String(unicode: false),
                        ProductName = c.String(unicode: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Image = c.String(unicode: false),
                        Quantity = c.Int(nullable: false),
                        CreatedAt = c.DateTime(precision: 0),
                        UpdatedAt = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        FisrtName = c.String(unicode: false),
                        LastName = c.String(unicode: false),
                        Email = c.String(unicode: false),
                        Street = c.String(unicode: false),
                        City = c.String(unicode: false),
                        State = c.String(unicode: false),
                        ZipCode = c.String(unicode: false),
                        OrderStatus = c.String(unicode: false),
                        CreatedAt = c.DateTime(precision: 0),
                        UpdatedAt = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderItems", "OrderId", "dbo.Orders");
            DropIndex("dbo.OrderItems", new[] { "OrderId" });
            DropTable("dbo.Orders");
            DropTable("dbo.OrderItems");
        }
    }
}
