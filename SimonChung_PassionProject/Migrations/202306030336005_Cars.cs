namespace SimonChung_PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cars : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        CarID = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        Mileage = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CarID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Cars");
        }
    }
}
