using Microsoft.AspNet.Identity;
using Single_Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Single_Capstone.Controllers
{
    public class BusinessController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Business
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var business = db.Businesses.Where(b => b.ApplicationId == userId).FirstOrDefault();
            return View(business); //change this later it will be the home page
        }

        // GET: Business/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Business/Create
        public ActionResult Create()
        {
            Business business = new Business();
            return View(business); //create new business
        }

        // POST: Business/Create
        [HttpPost]
        public ActionResult Create(Business business)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                business.ApplicationId = userId;
                db.Businesses.Add(business);
                db.SaveChanges();
                return RedirectToAction("Index"); //return back to index
            }
            catch
            {
                return View();
            }
        }

        // GET: Business/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var business = db.Businesses.Find(id);
            if (business == null)
            {
                return HttpNotFound();
            }
            return View(business);
        }

        // POST: Business/Edit/5
        [HttpPost]
        public ActionResult Edit(Business business)
        {
            if (ModelState.IsValid)
            {

                var userId = User.Identity.GetUserId();
                business.ApplicationId = userId;
                db.Entry(business).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: Business/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Business/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
