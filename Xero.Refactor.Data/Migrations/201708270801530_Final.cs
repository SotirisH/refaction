namespace Xero.Refactor.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Final : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.ProductOption", "ProductId", "dbo.Product");
            //DropPrimaryKey("dbo.ProductOption");
            //DropPrimaryKey("dbo.Product");
            AlterColumn("dbo.ProductOption", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Product", "Id", c => c.Guid(nullable: false));
            //AddPrimaryKey("dbo.ProductOption", "Id");
            //AddPrimaryKey("dbo.Product", "Id");
            //AddForeignKey("dbo.ProductOption", "ProductId", "dbo.Product", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductOption", "ProductId", "dbo.Product");
            DropPrimaryKey("dbo.Product");
            DropPrimaryKey("dbo.ProductOption");
            AlterColumn("dbo.Product", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.ProductOption", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.Product", "Id");
            AddPrimaryKey("dbo.ProductOption", "Id");
            AddForeignKey("dbo.ProductOption", "ProductId", "dbo.Product", "Id", cascadeDelete: true);
        }
    }
}
