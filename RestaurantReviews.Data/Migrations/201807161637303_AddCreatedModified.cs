namespace RestaurantReviews.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreatedModified : DbMigration
    {
        public override void Up()
        {
            AddColumn("Restaurant.Restaurant", "Created", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("Restaurant.Restaurant", "Modified", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AddColumn("Restaurant.Review", "Created", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("Restaurant.Review", "Modified", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            DropColumn("Restaurant.Review", "Modified");
            DropColumn("Restaurant.Review", "Created");
            DropColumn("Restaurant.Restaurant", "Modified");
            DropColumn("Restaurant.Restaurant", "Created");
        }
    }
}
