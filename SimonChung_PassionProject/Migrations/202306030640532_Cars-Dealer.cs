namespace SimonChung_PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CarsDealer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Dealers",
                c => new
                    {
                        DealerID = c.Int(nullable: false, identity: true),
                        DealerName = c.String(),
                    })
                .PrimaryKey(t => t.DealerID);
            
            AddColumn("dbo.Cars", "DealerID", c => c.Int(nullable: false));
            CreateIndex("dbo.Cars", "DealerID");
            AddForeignKey("dbo.Cars", "DealerID", "dbo.Dealers", "DealerID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cars", "DealerID", "dbo.Dealers");
            DropIndex("dbo.Cars", new[] { "DealerID" });
            DropColumn("dbo.Cars", "DealerID");
            DropTable("dbo.Dealers");
        }
    }
}
