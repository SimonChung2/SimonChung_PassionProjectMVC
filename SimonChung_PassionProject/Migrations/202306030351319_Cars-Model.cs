namespace SimonChung_PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CarsModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CarModels",
                c => new
                    {
                        ModelID = c.Int(nullable: false, identity: true),
                        ModelName = c.String(),
                        Make = c.String(),
                    })
                .PrimaryKey(t => t.ModelID);
            
            AddColumn("dbo.Cars", "ModelID", c => c.Int(nullable: false));
            CreateIndex("dbo.Cars", "ModelID");
            AddForeignKey("dbo.Cars", "ModelID", "dbo.CarModels", "ModelID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cars", "ModelID", "dbo.CarModels");
            DropIndex("dbo.Cars", new[] { "ModelID" });
            DropColumn("dbo.Cars", "ModelID");
            DropTable("dbo.CarModels");
        }
    }
}
