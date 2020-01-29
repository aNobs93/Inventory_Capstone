using Microsoft.AspNet.Identity;
using Single_Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Single_Capstone.Controllers
{
    public class ProductController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Product
        public ActionResult Index()
        {
            var userID = User.Identity.GetUserId();
            var business = db.Businesses.Where(b => b.ApplicationId == userID).FirstOrDefault();
            var products = db.Products.Where(p => p.BusinessId == business.Id).ToList();
            return View(products);//Come here to see a list of the products you have/update product/or add new.
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            Product product = new Product();
            return View(product);
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(Product product)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var business = db.Businesses.Where(b => b.ApplicationId == userId).FirstOrDefault();
                product.BusinessId = business.Id;
                product.GetDate = DateTime.Now.ToShortDateString();//shows what day you got the product
                var p = FindProductProfit(product);
                var pd = FindProductInventoryTotal(p);
                db.Products.Add(pd);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public Product FindProductProfit(Product product)
        {
            product.ProfitToBeMadePerUnit = (product.PricePerUnitSelling - product.PricePerUnitPurchased);
            return product;//maybe look back here not sure what i was doing
        }

        public Product FindProductInventoryTotal(Product product)//and here
        {
            product.TotalProductValue = (product.Units * product.PricePerUnitPurchased);
            return product;
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Product/Delete/5
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
