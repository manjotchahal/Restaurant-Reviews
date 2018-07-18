using RestaurantReviews.Data;
using RestaurantReviews.Library.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReviews.Library.Repositories
{
    public class RestaurantRepository : IRestaurantReviewsRepository<Restaurant>
    {
        private readonly RestaurantReviewsContext _context;

        public RestaurantRepository(RestaurantReviewsContext context)
        {
            _context = context;
        }

        public Restaurant GetById(object id)
        {
            var rest = _context.Restaurants.Find(id);
            if (rest != null)
                return DataToLibrary(rest);
            return null;
        }

        public void Insert(Restaurant entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                this._context.Restaurants.Add(LibraryToData(entity));
                this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                    }
                }
                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public void Update(Restaurant entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }

                var oldRest = _context.Restaurants.Find(entity.Id);

                if (oldRest == null)
                {
                    throw new ArgumentNullException("entity");
                }
                //_context.Entry(entity).State = EntityState.Modified;
                //_context.Entry(oldRest).CurrentValues.SetValues(entity);
                oldRest.Name = entity.Name;
                oldRest.Street = entity.Street;
                oldRest.City = entity.City;
                oldRest.State = entity.State;
                oldRest.Country = entity.Country;
                oldRest.Zipcode = entity.Zipcode;
                oldRest.Phone = entity.Phone;
                oldRest.Website = entity.Website;

                this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public void Delete(object id)
        {
            try
            {
                var rest = this._context.Restaurants.Find(id);
                if (rest == null)
                {
                    throw new ArgumentNullException("entity");
                }
                this._context.Restaurants.Remove(rest);
                this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public virtual IEnumerable<Restaurant> GetAll
        {
            get
            {
                var list = this._context.Restaurants.ToList();
                return list.Select(x => DataToLibrary(x)).ToList();
            }
        }

        public virtual IEnumerable<Restaurant> SortByNameAscending(string q = null)
        {
            if (q != null)
                return SearchRestaurants(q).OrderBy(x => x.Name);
            return GetAll.OrderBy(x => x.Name);
        }

        public virtual IEnumerable<Restaurant> SortByNameDescending(string q = null)
        {
            if (q != null)
                return SearchRestaurants(q).OrderByDescending(x => x.Name);
            return GetAll.OrderByDescending(x => x.Name);
        }

        public virtual IEnumerable<Restaurant> SortByRating(string q = null)
        {
            if (q != null)
                return SearchRestaurants(q).OrderByDescending(x => x.AverageRating)
                .ThenByDescending(x => x.ReviewCount);
            return GetAll.OrderByDescending(x => x.AverageRating)
                .ThenByDescending(x => x.ReviewCount);
        }

        public virtual IEnumerable<Restaurant> Top3()
        {
            return GetAll.OrderByDescending(x => x.AverageRating)
                .ThenByDescending(x => x.ReviewCount).Take(3);
        }

        public virtual IEnumerable<Restaurant> SortByNumberOfReviews(string q = null)
        {
            if (q != null)
                return SearchRestaurants(q).OrderByDescending(x => x.ReviewCount)
                .ThenByDescending(x => x.AverageRating);
            return GetAll.OrderByDescending(x => x.ReviewCount)
                .ThenByDescending(x => x.AverageRating);
        }

        public virtual IEnumerable<Restaurant> SearchRestaurants(string q)
        {
            var list = _context.Restaurants.Where(s => s.Name.Contains(q) || s.Street.Contains(q) || s.City.Contains(q) || s.State.Contains(q) || s.Zipcode.Contains(q) || s.Phone.Contains(q)).ToList();
            return list.Select(x => DataToLibrary(x)).ToList();
        }

        public Restaurant DataToLibrary(Data.Models.Restaurant dataModel)
        {
            double rating = 0;
            var reviews = _context.Reviews.Where(r => r.RestaurantId == dataModel.Id);
            if (reviews.Count() != 0)
                rating = reviews.Average(r => r.Rating);
            //var list = dataModel.Reviews.ToList();

            var libModel = new Restaurant()
            {
                Id = dataModel.Id,
                Name = dataModel.Name,
                Street = dataModel.Street,
                City = dataModel.City,
                State = dataModel.State,
                Country = dataModel.Country,
                Zipcode = dataModel.Zipcode,
                Phone = dataModel.Phone,
                Website = dataModel.Website,
                Created = dataModel.Created,
                Modified = dataModel.Modified,
                AverageRating = Math.Truncate(rating * 100) / 100,
                ReviewCount = reviews.Count(),
                //Reviews = list.Select(x => ReviewRepository.DataToLibrary(x)).ToList()
            };
            return libModel;
        }

        public Data.Models.Restaurant LibraryToData(Restaurant libModel)
        {
            var dataModel = new Data.Models.Restaurant()
            {
                Name = libModel.Name,
                Street = libModel.Street,
                City = libModel.City,
                State = libModel.State,
                Country = libModel.Country,
                Zipcode = libModel.Zipcode,
                Phone = libModel.Phone,
                Website = libModel.Website
            };
            return dataModel;
        }
    }
}
