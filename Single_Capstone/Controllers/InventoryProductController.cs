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
            var userId = User.Identity.GetUserId();
            var business = db.Businesses.Where(b => b.ApplicationId == userId).FirstOrDefault();
            var inventory = db.Inventories.Where(i => i.BusinessId == business.Id).FirstOrDefault();
            var products = db.InventoryProducts.Where(p => p.InventoryId == inventory.Id).ToList();
            return View(products);
        }

        // GET: InventoryProduct/Details/5
        public ActionResult Details(int id)
        {
            var products = db.InventoryProducts.Where(p => p.InventoryId == id).ToList();
            return View(products);
        }

        //public ActionResult InitalInventoryCreation()
        //{
        //    InventoryProducts inventoryProducts = new InventoryProducts();
        //    inventoryProducts.InventoryId = inventory.Id;
        //    db.InventoryProducts.Add(inventoryProducts);
        //    db.SaveChanges();
        //    return RedirectToAction("Create", "Product", inventoryProducts);
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

        public void LoopToAssignValuesToProductInventory(InventoryProducts inventoryProducts, Inventory inventory)//after create sent here to a ssign values
        {
            inventoryProducts.InventoryId = inventory.Id;
            //var business = db.Businesses.Where(b => b.Id == inventory.BusinessId).FirstOrDefault();
            //var findMax = db.Inventories.Where(f => f.BusinessId == business.Id).ToList();           
            //int max = findMax.Max(m => m.Id);
            var products = db.InventoryProducts.Where(ip => ip.InventoryId == inventory.LastInventoryId).ToList();
            for (int i = 0; i < products.Count; i++)
            {
                inventoryProducts.InventoryId = inventory.Id;
                inventoryProducts.ProductId = products[i].ProductId;
                inventoryProducts.GetDate = DateTime.Now.ToShortDateString();
                inventoryProducts.ProductName = products[i].ProductName;
                inventoryProducts.ProfitToBeMadePerUnit = 0;
                inventoryProducts.Units = 0;
                inventoryProducts.TotalValueOfProducts = 0;
                db.InventoryProducts.Add(inventoryProducts);
                db.SaveChanges();
            }
            //return RedirectToAction("Index", "Inventory");
        }

        // GET: InventoryProduct/Create
        public ActionResult Create(ProductViewModel productView, Inventory inventory)//For Initial Creation Only
        {
            InventoryProducts inventoryProducts = new InventoryProducts();
            if(productView.ProductName == null)
            {
                 LoopToAssignValuesToProductInventory(inventoryProducts, inventory);
                 return RedirectToAction("Index", "Inventory");
            }
            return View(inventoryProducts);
        }

        // POST: InventoryProduct/Create
        [HttpPost]
        public ActionResult Create(InventoryProducts inventoryProducts, ProductViewModel productView)//For Initial Creation Only
        {
            try
            {
                inventoryProducts.InventoryId = productView.InventoryId;
                inventoryProducts.ProductId = productView.ProductId;
                inventoryProducts.GetDate = DateTime.Now.ToShortDateString();
                inventoryProducts = FindTotalValueOfProducts(inventoryProducts);
                inventoryProducts = FindProfitPerUnit(inventoryProducts);
                db.InventoryProducts.Add(inventoryProducts);
                db.SaveChanges();
                var inventory = db.Inventories.Where(i => i.Id == productView.InventoryId).FirstOrDefault();
                inventory.TotalInventoryWorth += inventoryProducts.TotalValueOfProducts;
                inventory.ProfitMargin += (inventoryProducts.ProfitToBeMadePerUnit * inventoryProducts.Units);
                db.Entry(inventory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Inventory");
            }
            catch
            {
                return View();
            }
        }

        public InventoryProducts FindTotalValueOfProducts(InventoryProducts inventoryProducts)//this works for new product thats added
        {
            var product = db.Products.Where(p => p.Id == inventoryProducts.ProductId).FirstOrDefault();
            inventoryProducts.TotalValueOfProducts = (inventoryProducts.Units * product.PricePerUnit);
            return inventoryProducts;
        }

        public InventoryProducts FindProfitPerUnit(InventoryProducts inventoryProducts)//this works for new product thats added
        {
            var product = db.Products.Where(p => p.Id == inventoryProducts.ProductId).FirstOrDefault();
            inventoryProducts.ProfitToBeMadePerUnit = (product.PricePerUnitSelling - product.PricePerUnit);
            return inventoryProducts;
        }

        //public ActionResult TakingInventoryEdit(int id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var inventoryProduct = db.InventoryProducts.Find(id);
        //    if (inventoryProduct == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(inventoryProduct);
        //}

        //[HttpPost]
        //public ActionResult TakingInventoryEdit(int id, InventoryProducts inventoryProducts)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //var userId = User.Identity.GetUserId();
        //        //var business = db.Businesses.Where(b => b.ApplicationId == userId).FirstOrDefault();
        //        var product = db.InventoryProducts.Where(i => i.Id == id).FirstOrDefault();
        //        var findProduct = db.Products.Where(p => p.Id == product.ProductId).FirstOrDefault();
        //        //findProduct.Units = (findProduct.Units - inventoryProducts.Units);
        //        //findProduct.TotalProductValue  return RedirectToAction("FindProductInventoryTotal", "Product", findProduct);


        //        db.Entry(inventoryProducts).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return RedirectToAction("Index");
        //}

        // GET: InventoryProduct/Edit/5
        public ActionResult Edit(int id)
        {
            var product = db.InventoryProducts.Where(p => p.Id == id).FirstOrDefault();
            return View(product);
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
