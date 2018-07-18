using RestaurantReviews.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReviews.Data
{
    public class Class1
    {
        static void Main(string[] args)
        {
            //using (var db = new RestaurantReviewsContext())
            //{
            //    //Restaurant restaurant = new Restaurant() { Name = "Burger King", Street = "2602 E Fletcher Ave", City = "Tampa", State = "Florida", Country = "USA", Zipcode = "33612", Phone = "(813) 977-9288", Website = "https://www.subway.com" };
            //    //db.Restaurants.Add(restaurant);
            //    //var rest = db.Restaurants.Find(1);
            //    //db.Restaurants.Remove(rest);
            //    db.SaveChanges();

            //    var query = from r in db.Restaurants
            //                orderby r.Name
            //                select r;

            //    foreach (var item in query)
            //    {
            //        Console.WriteLine(item.Id+"\n"+item.Name+"\n"+item.Address);
            //    }
            //    Console.Read();
            //}
        }
    }

    public class RestaurantReviewsContext : DbContext, IDbContext
    {
        public RestaurantReviewsContext() : base("RestaurantReviewsDb") { }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Review> Reviews { get; set; }

        IDbSet<TEntity> IDbContext.Set<TEntity>()
        {
            return base.Set<TEntity>();
        }

        public override int SaveChanges()
        {
            var addedEntities = ChangeTracker.Entries().Where(E => E.State == EntityState.Added).ToList();

            addedEntities.ForEach(E =>
            {
                E.Property("Created").CurrentValue = DateTime.Now;
                E.Property("Modified").CurrentValue = DateTime.Now;
            });

            var modifiedEntries = ChangeTracker.Entries().Where(E => E.State == EntityState.Modified).ToList();

            modifiedEntries.ForEach(E =>
            {
                E.Property("Modified").CurrentValue = DateTime.Now;
            });
            return base.SaveChanges();
        }
    }
}
