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
    public class ReviewRepository : IRestaurantReviewsRepository<Review>
    {
        private readonly RestaurantReviewsContext _context;

        public ReviewRepository(RestaurantReviewsContext context)
        {
            this._context = context;
        }

        public Review GetById(object id)
        {
            var rev = _context.Reviews.Find(id);
            if (rev != null)
                return DataToLibrary(rev);
            return null;
        }

        public void Insert(Review entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                this._context.Reviews.Add(LibraryToData(entity));
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
                //Call Nlog Code...
                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public void Update(Review entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }

                var oldRev = _context.Reviews.Find(entity.Id);

                if (oldRev == null)
                {
                    throw new ArgumentNullException("entity");
                }

                oldRev.Rating = entity.Rating;
                oldRev.User = entity.User;
                oldRev.Comment = entity.Comment;

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
                var rev = this._context.Reviews.Find(id);
                if (rev == null)
                {
                    throw new ArgumentNullException("entity");
                }
                this._context.Reviews.Remove(rev);
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

        public virtual IEnumerable<Review> GetAll
        {
            get
            {
                var list = this._context.Reviews.ToList();
                return list.Select(x => DataToLibrary(x)).ToList();
            }
        }

        public virtual IEnumerable<Review> GetAllByRestaurant(int? id)
        {
            var list = this._context.Reviews.Where(x => x.Restaurant.Id == id).ToList();
            return list.Select(x => DataToLibrary(x)).ToList();
        }

        public virtual IEnumerable<Review> SortByRatingAscending(int? id, string q = null)
        {
            if (q != null)
                return SearchReviews(id, q).OrderBy(x => x.Rating)
                    .ThenByDescending(x => x.Modified);
            return GetAllByRestaurant(id).OrderBy(x => x.Rating)
                .ThenByDescending(x => x.Modified);
        }

        public virtual IEnumerable<Review> SortByRatingDescending(int? id, string q = null)
        {
            if (q != null)
                return SearchReviews(id, q).OrderByDescending(x => x.Rating)
                    .ThenByDescending(x => x.Modified);
            return GetAllByRestaurant(id).OrderByDescending(x => x.Rating)
                .ThenByDescending(x => x.Modified);
        }

        public virtual IEnumerable<Review> SortByNewest(int? id, string q = null)
        {
            if (q != null)
                return SearchReviews(id, q).OrderByDescending(x => x.Modified);
            return GetAllByRestaurant(id).OrderByDescending(x => x.Modified);
        }

        public virtual IEnumerable<Review> SortByOldest(int? id, string q = null)
        {
            if (q != null)
                return SearchReviews(id, q).OrderBy(x => x.Modified);
            return GetAllByRestaurant(id).OrderBy(x => x.Modified);
        }

        public virtual IEnumerable<Review> SearchReviews(int? id, string q)
        {
            var list = _context.Reviews.Where(x => x.Restaurant.Id == id && (x.User.Contains(q) || x.Comment.Contains(q) || x.Modified.ToString().Contains(q) || x.Rating.ToString().Contains(q))).ToList();
            return list.Select(x => DataToLibrary(x)).ToList();
        }

        public static Review DataToLibrary(Data.Models.Review dataModel)
        {
            var libModel = new Review()
            {
                Id = dataModel.Id,
                Rating = dataModel.Rating,
                Comment = dataModel.Comment,
                User = dataModel.User,
                Created = dataModel.Created,
                Modified = dataModel.Modified,
                RestaurantId = dataModel.RestaurantId
            };
            return libModel;
        }

        public static Data.Models.Review LibraryToData(Review libModel)
        {
            var dataModel = new Data.Models.Review()
            {
                Rating = libModel.Rating,
                Comment = libModel.Comment,
                User = libModel.User,
                RestaurantId = libModel.RestaurantId
            };
            return dataModel;
        }
    }
}
