using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Serialization;
using Single_Capstone.Models;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;

namespace Single_Capstone.Controllers
{
    public class ChartController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Chart
        public ActionResult Index()
        {
            var userid = User.Identity.GetUserId();
            var business = db.Businesses.Where(b => b.ApplicationId == userid).FirstOrDefault();
            var inventory = db.Inventories.Where(i => i.BusinessId == business.Id).ToList();
            List<DataPoint> dataPoints = new List<DataPoint>{ };
            int k = 0;
            //int j = 1;
            for (k = 0/*, j = 1*/; k < inventory.Count;/* && j < inventory.Count; */k++/*, j++*/)
            {
                dataPoints.Add(new DataPoint(inventory[k].TotalInventoryWorth, inventory[k].GetDate));
            }
            //new DataPoint(10, 22),
            //    new DataPoint(20, 36),
            //    new DataPoint(30, 42),
            //    new DataPoint(40, 51),
            //    new DataPoint(50, 46),
            //var today = DateTime.Today;

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View();
        }

        public ActionResult PastHistoricalDataMonth(Inventory inventory)
        {
            return View();
        }

        public ActionResult PastHistoricalDataForSpecificProduct(InventoryProducts inventoryProducts)
        {
            inventoryProducts = db.InventoryProducts.Where(ip => ip.Id == inventoryProducts.Id).FirstOrDefault();
            var inventory = db.Inventories.Where(i => i.Id == inventoryProducts.InventoryId).FirstOrDefault();
            var inventories = db.Inventories.Where(i => i.BusinessId == inventory.BusinessId).ToList();
            var invProducts = db.InventoryProducts.Where(ip => ip.ProductId == inventoryProducts.ProductId).ToList();
            List<DataPoint> dataPoints = new List<DataPoint> { };
            for (int i = 0; i < inventories.Count; i++)
            {
                if(invProducts[i].InventoryId == inventories[i].Id)
                {
                    dataPoints.Add(new DataPoint(invProducts[i].Units, inventories[i].GetDate));
                }
                //inventoryProduct += db.InventoryProducts.Where(ip => ip.InventoryId == inventories[i].Id && ip.ProductId == inventoryProducts.ProductId).ToList();

            }
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            ViewBag.ProductName = JsonConvert.SerializeObject(inventoryProducts.ProductName);
            return View();
        }

        // GET: Chart/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Chart/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Chart/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Chart/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Chart/Edit/5
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

        // GET: Chart/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Chart/Delete/5
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
