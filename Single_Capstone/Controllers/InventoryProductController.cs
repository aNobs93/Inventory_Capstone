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
    public class InventoryProductController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: InventoryProduct
        public ActionResult Index()
        {
            return View();
        }

        // GET: InventoryProduct/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //public ActionResult InitalInventoryCreation(Inventory inventory)//When you create all your product and start an inventory it comes here to do you first ever inventory
        //{
        //    InventoryProducts inventoryProducts = new InventoryProducts();
        //    var userId = User.Identity.GetUserId();
        //    var business = db.Businesses.Where(b => b.ApplicationId == userId).FirstOrDefault();
        //    inventory.Products = db.Products.Where(p => p.BusinessId == business.Id).ToList();
        //    for (int i = 0; i < inventory.Products.Count; i++)
        //    {
        //        inventoryProducts.InventoryId = inventory.Id;
        //        inventoryProducts.ProductId = inventory.Products[i].Id;
        //        inventoryProducts.Units = inventory.Products[i].Units;
        //        inventoryProducts.GetDate = DateTime.Now.ToShortDateString();
        //        inventoryProducts.ProductName = inventory.Products[i].ProductName;
        //        inventoryProducts.TotalValueOfProducts = inventory.Products[i].TotalProductValue;
        //        inventoryProducts.ProfitToBeMadePerUnit = inventory.Products[i].ProfitToBeMadePerUnit;
        //        inventoryProducts.PricePerUnitPurchased = inventory.Products[i].PricePerUnitPurchased;
        //        inventoryProducts.PricePerUnitSelling = inventory.Products[i].PricePerUnitSelling;
        //        db.InventoryProducts.Add(inventoryProducts);
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("CalcInventoryValue", "Inventory", inventory);//Redirect to calc inventory value n profit estimate
        //}

        //public InventoryProducts LoopToAssignValuesToProductInventory(InventoryProducts inventoryProducts, Inventory inventory)//after create sent here to a ssign values
        //{
        //    inventoryProducts.InventoryId = inventory.Id;
        //    var business = db.Businesses.Where(b => b.Id == inventory.BusinessId).FirstOrDefault();
        //    inventory.Products = db.Products.Where(ip => ip.BusinessId == business.Id).ToList();
        //    for (int i = 0; i < inventory.Products.Count; i++)
        //    {
        //        inventoryProducts.InventoryId = inventory.Id;
        //        inventoryProducts.ProductId = inventory.Products[i].Id;
        //        inventoryProducts.Units = inventory.Products[i].Units;
        //        inventoryProducts.GetDate = DateTime.Now.ToShortDateString();
        //        inventoryProducts.ProductName = inventory.Products[i].ProductName;
        //        inventoryProducts.TotalValueOfProducts = inventory.Products[i].TotalProductValue;
        //        inventoryProducts.ProfitToBeMadePerUnit = inventory.Products[i].ProfitToBeMadePerUnit;
        //        inventoryProducts.PricePerUnitPurchased = inventory.Products[i].PricePerUnitPurchased;
        //        inventoryProducts.PricePerUnitSelling = inventory.Products[i].PricePerUnitSelling;
        //        db.InventoryProducts.Add(inventoryProducts);
        //        db.SaveChanges();
        //    }
        //    return inventoryProducts;//returns back to create to go to the view
        //}

        // GET: InventoryProduct/Create
        public ActionResult Create(Inventory inventory)//after an inventory is created it is sent here to do inventory on products
        {
            var inventoryId = inventory.Id;
            if (inventory.ProfitMargin == 0)
            {
                InventoryProducts inventoryProducts = new InventoryProducts();
                //LoopToAssignValuesToProductInventory(inventoryProducts, inventory);
            }//goes into the loop to assign values
            //iP.Products = db.InventoryProducts.Where(i => i.InventoryId == inventoryId).ToList();
            var products = db.InventoryProducts.Where(p => p.InventoryId == inventoryId).ToList();
            ViewBag.Inventory = inventory;
            return View(products);
        }

        // POST: InventoryProduct/Create
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

        public ActionResult TakingInventoryEdit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var inventoryProduct = db.InventoryProducts.Find(id);
            if (inventoryProduct == null)
            {
                return HttpNotFound();
            }
            return View(inventoryProduct);
        }

        [HttpPost]
        public ActionResult TakingInventoryEdit(int id, InventoryProducts inventoryProducts)
        {
            if (ModelState.IsValid)
            {
                //var userId = User.Identity.GetUserId();
                //var business = db.Businesses.Where(b => b.ApplicationId == userId).FirstOrDefault();
                var product = db.InventoryProducts.Where(i => i.Id == id).FirstOrDefault();
                var findProduct = db.Products.Where(p => p.Id == product.ProductId).FirstOrDefault();
                //findProduct.Units = (findProduct.Units - inventoryProducts.Units);
                //findProduct.TotalProductValue  return RedirectToAction("FindProductInventoryTotal", "Product", findProduct);


                db.Entry(inventoryProducts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: InventoryProduct/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: InventoryProduct/Edit/5
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

        // GET: InventoryProduct/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InventoryProduct/Delete/5
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
