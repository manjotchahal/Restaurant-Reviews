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
    public class RestaurantsController : Controller
    {
        private Service service;

        public RestaurantsController()
        {
            service = new Service();
        }

        public ActionResult Index(string sort, string q)
        {
            IEnumerable<Restaurant> rests;

            switch (sort)
            {
                case "name_desc":
                    rests = service.SortByNameDescending(q);
                    break;
                case "rating_desc":
                    rests = service.SortByRating(q);
                    break;
                case "review_count":
                    rests = service.SortByNumberOfReviews(q);
                    break;
                default:
                    sort = "name_asc";
                    rests = service.SortByNameAscending(q);
                    break;
            }

            ViewBag.Query = q;
            ViewBag.Sort = sort;
            return View(rests);
        }

        // GET: Restaurants/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Restaurant rest = service.GetRestaurantById(id);

            if (rest == null)
                return HttpNotFound();

            return View(rest);
        }

        // GET: Restaurants/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Restaurants/Create
        [HttpPost]
        public ActionResult Create(Restaurant restaurant)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    service.InsertRestaurant(restaurant);
                    return RedirectToAction("Index");
                }
                else
                    return View(restaurant);
            }
            //log success 
            catch
            {
                //log problem
                return View();
            }
        }

        // GET: Restaurants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Restaurant rest = service.GetRestaurantById(id);

            if (rest == null)
                return HttpNotFound();

            return View(rest);
        }

        // POST: Restaurants/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Restaurant restaurant)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    service.UpdateRestaurant(restaurant);
                    return RedirectToAction("Index");
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

        // GET: Restaurants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Restaurant rest = service.GetRestaurantById(id);

            if (rest == null)
                return HttpNotFound();

            return View(rest);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            string name = service.GetRestaurantById(id).Name;
            try
            {
                service.DeleteRestaurant(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}