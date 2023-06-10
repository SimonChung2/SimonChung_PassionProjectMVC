namespace SimonChung_PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DealersModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DealerCarModels",
                c => new
                    {
                        Dealer_DealerID = c.Int(nullable: false),
                        CarModel_ModelID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Dealer_DealerID, t.CarModel_ModelID })
                .ForeignKey("dbo.Dealers", t => t.Dealer_DealerID, cascadeDelete: true)
                .ForeignKey("dbo.CarModels", t => t.CarModel_ModelID, cascadeDelete: true)
                .Index(t => t.Dealer_DealerID)
                .Index(t => t.CarModel_ModelID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DealerCarModels", "CarModel_ModelID", "dbo.CarModels");
            DropForeignKey("dbo.DealerCarModels", "Dealer_DealerID", "dbo.Dealers");
            DropIndex("dbo.DealerCarModels", new[] { "CarModel_ModelID" });
            DropIndex("dbo.DealerCarModels", new[] { "Dealer_DealerID" });
            DropTable("dbo.DealerCarModels");
        }
    }
}
