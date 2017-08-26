namespace Xero.Refactor.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetNavigationProp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductOption", "CreatedBy", c => c.String(maxLength: 50));
            AddColumn("dbo.ProductOption", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.ProductOption", "ModifiedBy", c => c.String(maxLength: 50));
            AddColumn("dbo.ProductOption", "ModifiedOn", c => c.DateTime());
            AddColumn("dbo.ProductOption", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Product", "CreatedBy", c => c.String(maxLength: 50));
            AddColumn("dbo.Product", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.Product", "ModifiedBy", c => c.String(maxLength: 50));
            AddColumn("dbo.Product", "ModifiedOn", c => c.DateTime());
            AddColumn("dbo.Product", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            CreateIndex("dbo.ProductOption", "ProductId");
            AddForeignKey("dbo.ProductOption", "ProductId", "dbo.Product", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductOption", "ProductId", "dbo.Product");
            DropIndex("dbo.ProductOption", new[] { "ProductId" });
            DropColumn("dbo.Product", "RowVersion");
            DropColumn("dbo.Product", "ModifiedOn");
            DropColumn("dbo.Product", "ModifiedBy");
            DropColumn("dbo.Product", "CreatedOn");
            DropColumn("dbo.Product", "CreatedBy");
            DropColumn("dbo.ProductOption", "RowVersion");
            DropColumn("dbo.ProductOption", "ModifiedOn");
            DropColumn("dbo.ProductOption", "ModifiedBy");
            DropColumn("dbo.ProductOption", "CreatedOn");
            DropColumn("dbo.ProductOption", "CreatedBy");
        }
    }
}
