using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReviews.Library.Repositories
{
    public interface IRestaurantReviewsRepository<T>
    {
        T GetById(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(object id);
        IEnumerable<T> GetAll { get; }
    }
}
