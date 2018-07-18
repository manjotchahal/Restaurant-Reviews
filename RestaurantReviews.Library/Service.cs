using RestaurantReviews.Data;
using RestaurantReviews.Library.Models;
using RestaurantReviews.Library.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReviews.Library
{
    public class Service
    {
        private RestaurantReviewsContext db;
        private RestaurantRepository restrepo;
        private ReviewRepository revrepo;

        public Service()
        {
            db = new RestaurantReviewsContext();
            restrepo = new RestaurantRepository(db);
            revrepo = new ReviewRepository(db);
        }

        public Restaurant GetRestaurantById(object id)
        {
            return restrepo.GetById(id);
        }

        public void InsertRestaurant(Restaurant entity)
        {
            restrepo.Insert(entity);
        }

        public void UpdateRestaurant(Restaurant entity)
        {
            restrepo.Update(entity);
        }

        public void DeleteRestaurant(int id)
        {
            restrepo.Delete(id);
        }

        public virtual IEnumerable<Restaurant> GetAllRestaurants()
        {
            return restrepo.GetAll;
        }

        public virtual IEnumerable<Restaurant> SortByNameAscending(string q = null)
        {
            return restrepo.SortByNameAscending(q);
        }

        public virtual IEnumerable<Restaurant> SortByNameDescending(string q = null)
        {
            return restrepo.SortByNameDescending(q);
        }

        public virtual IEnumerable<Restaurant> SortByRating(string q = null)
        {
            return restrepo.SortByRating(q);
        }

        public virtual IEnumerable<Restaurant> Top3()
        {
            return restrepo.Top3();
        }

        public virtual IEnumerable<Restaurant> SortByNumberOfReviews(string q = null)
        {
            return restrepo.SortByNumberOfReviews(q);
        }

        public virtual IEnumerable<Restaurant> SearchRestaurants(string q)
        {
            return restrepo.SearchRestaurants(q);
        }

        public Review GetReviewById(object id)
        {
            return revrepo.GetById(id);
        }

        public void InsertReview(Review entity)
        {
            revrepo.Insert(entity);
        }

        public void UpdateReview(Review entity)
        {
            revrepo.Update(entity);
        }

        public void DeleteReview(int id)
        {
            revrepo.Delete(id);
        }

        public virtual IEnumerable<Review> GetAllReviews()
        {
            return revrepo.GetAll;
        }

        public virtual IEnumerable<Review> GetAllReviewsByRestaurant(int id)
        {
            return revrepo.GetAllByRestaurant(id);
        }

        public virtual IEnumerable<Review> SortByRatingAscending(int? id, string q = null)
        {
            return revrepo.SortByRatingAscending(id, q);
        }

        public virtual IEnumerable<Review> SortByRatingDescending(int? id, string q = null)
        {
            return revrepo.SortByRatingDescending(id, q);
        }

        public virtual IEnumerable<Review> SortByNewest(int? id, string q = null)
        {
            return revrepo.SortByNewest(id, q);
        }

        public virtual IEnumerable<Review> SortByOldest(int? id, string q = null)
        {
            return revrepo.SortByOldest(id, q);
        }

        public virtual IEnumerable<Review> SearchReviews(int id, string q = null)
        {
            return revrepo.SearchReviews(id, q);
        }
    }
}
