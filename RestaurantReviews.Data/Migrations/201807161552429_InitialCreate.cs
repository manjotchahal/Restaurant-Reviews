namespace RestaurantReviews.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Restaurant.Restaurant",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Street = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Country = c.String(),
                        Zipcode = c.String(nullable: false),
                        Phone = c.String(),
                        Website = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Restaurant.Review",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rating = c.Int(nullable: false),
                        User = c.String(),
                        Comment = c.String(maxLength: 500),
                        RestaurantId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Restaurant.Restaurant", t => t.RestaurantId, cascadeDelete: true)
                .Index(t => t.RestaurantId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Restaurant.Review", "RestaurantId", "Restaurant.Restaurant");
            DropIndex("Restaurant.Review", new[] { "RestaurantId" });
            DropTable("Restaurant.Review");
            DropTable("Restaurant.Restaurant");
        }
    }
}
