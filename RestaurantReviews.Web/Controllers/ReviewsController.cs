using RestaurantReviews.Library;
using RestaurantReviews.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace RestaurantReviews.Web.Controllers
{
    public class ReviewsController : Controller
    {
        private Service service;

        public ReviewsController()
        {
            service = new Service();
        }

        // GET: Reviews
        //add sorting
        public ActionResult Index(int? id, string sort, string q)
        {
            //var revs = service.GetAllReviewsByRestaurant(id).OrderByDescending(x => x.Modified).ToList();

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Restaurant rest = service.GetRestaurantById(id);

            if (rest == null)
                return HttpNotFound();

            IEnumerable<Review> revs;

            switch (sort)
            {
                case "date_asc":
                    revs = service.SortByOldest(id, q);
                    break;
                case "rating_desc":
                    revs = service.SortByRatingDescending(id, q);
                    break;
                case "rating_asc":
                    revs = service.SortByRatingAscending(id, q);
                    break;
                default:
                    sort = "date_desc";
                    revs = service.SortByNewest(id, q);
                    break;
            }

            ViewBag.Query = q;
            ViewBag.Sort = sort;
            ViewBag.RestaurantName = rest.Name;
            TempData["RestaurantId"] = id;
            TempData.Keep("RestaurantId");
            ViewBag.RestaurantId = TempData.Peek("RestaurantId");
            TempData.Keep("RestaurantId");
            return View(revs);
        }

        // GET: Reviews/Create
        public ActionResult Create()
        {
            TempData.Keep("RestaurantId");
            ViewBag.RestaurantId = TempData.Peek("RestaurantId");
            TempData.Keep("RestaurantId");
            return View();
        }

        // POST: Reviews/Create
        [HttpPost]
        public ActionResult Create(Review review)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int id = Convert.ToInt32(TempData.Peek("RestaurantId"));
                    review.RestaurantId = id;
                    //TempData.Keep("RestaurantId");
                    service.InsertReview(review);
                    return RedirectToAction("Index", new { id = id });
                }
                else
                    return View(review);
            }
            catch
            {
                return View();
            }
        }

        // GET: Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Review rev = service.GetReviewById(id);

            if (rev == null)
                return HttpNotFound();

            TempData.Keep("RestaurantId");
            ViewBag.RestaurantId = TempData.Peek("RestaurantId");
            TempData.Keep("RestaurantId");
            return View(rev);
        }

        // POST: Reviews/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Review review)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    service.UpdateReview(review);
                    int restid = Convert.ToInt32(TempData.Peek("RestaurantId"));
                    return RedirectToAction("Index", new { id = restid });
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Review rev = service.GetReviewById(id);

            if (rev == null)
                return HttpNotFound();

            TempData.Keep("RestaurantId");
            ViewBag.RestaurantId = TempData.Peek("RestaurantId");
            TempData.Keep("RestaurantId");
            return View(rev);
        }

        // POST: Reviews/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var rev = service.GetReviewById(id);
            string name = rev.User + " " + rev.Rating + " " + rev.Comment;
            try
            {
                service.DeleteReview(id);
                int restid = Convert.ToInt32(TempData.Peek("RestaurantId"));
                return RedirectToAction("Index", new { id = restid });
            }
            catch
            {
                return View();
            }
        }
    }
}