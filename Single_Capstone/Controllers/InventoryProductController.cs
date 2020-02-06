using Microsoft.AspNet.Identity;
using Single_Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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
        public ActionResult Index()//Not sure what im doing here yet--------------------
        {
            var userId = User.Identity.GetUserId();
            var business = db.Businesses.Where(b => b.ApplicationId == userId).FirstOrDefault();
            var inventory = db.Inventories.Where(i => i.BusinessId == business.Id).FirstOrDefault();
            var products = db.InventoryProducts.Where(p => p.InventoryId == inventory.Id).ToList();
            return View(products);
        }

        // GET: InventoryProduct/Details/5
        public ActionResult Details()
        {//This allows the user to come and view all of the products and units tied to there inventories
            var userId = User.Identity.GetUserId();
            var business = db.Businesses.Where(b => b.ApplicationId == userId).FirstOrDefault();
            var findMax = db.Inventories.Where(f => f.BusinessId == business.Id).ToList();
            int max = findMax.Max(m => m.Id);//finds the newest inventory done to add the invoice to
            var products = db.InventoryProducts.Where(p => p.InventoryId == max).ToList();
            return View(products);
        }


        public void LoopToAssignValuesToProductInventory(InventoryProducts inventoryProducts, Inventory inventory)//after create sent here to a ssign values
        {//This assigns values to all of the product when the new inventoryProducts is instantiated
            inventoryProducts.InventoryId = inventory.Id;
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
                inventoryProducts.TimesOrdered = 0;
                inventoryProducts.AmountOrdered = 0;//Add GMROI to 0
                db.InventoryProducts.Add(inventoryProducts);
                db.SaveChanges();
            }
            //return RedirectToAction("Index", "Inventory");
        }

        // GET: InventoryProduct/Create
        public ActionResult Create(ProductViewModel productView, Inventory inventory)//comes here to create a new inventory
        {
            InventoryProducts inventoryProducts = new InventoryProducts();
            if(productView.ProductName == null)
            {
                 LoopToAssignValuesToProductInventory(inventoryProducts, inventory);
                 //return RedirectToAction("Index", "Inventory");
                return RedirectToAction("SelectedInventoryDetails", "Inventory", inventory);
            }
            return View(inventoryProducts);//For Initial Creation Only
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
                inventoryProducts = FindGMROI(inventoryProducts);
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

        public InventoryProducts FindGMROI(InventoryProducts inventoryProducts)
        {
            var product = db.Products.Where(p => p.Id == inventoryProducts.ProductId).FirstOrDefault();            
           double totalProfit = (product.PricePerUnitSelling * inventoryProducts.Units);
            inventoryProducts.GMROI = Math.Round(totalProfit / inventoryProducts.TotalValueOfProducts, 2);
            return inventoryProducts;
        }

        public InventoryProducts FindParLevel(InventoryProducts inventoryProducts)
        {
            var inventory = db.Inventories.Find(inventoryProducts.InventoryId);
            var oldInventory = db.InventoryProducts.Where(ip => ip.InventoryId == inventory.LastInventoryId).ToList();
            var lastInventoryProduct = oldInventory.Where(r => r.ProductId == inventoryProducts.ProductId).First();
            var usage = (lastInventoryProduct.Units - inventoryProducts.Units);
            var safetyNet = db.Products.Where(s => s.Id == lastInventoryProduct.ProductId).FirstOrDefault();
            var result = (usage * safetyNet.ProductSafetyNet);
            inventoryProducts.ParLevel = (usage + result)
;            return inventoryProducts;
        }

        public InventoryProducts FindHowMuchSold(InventoryProducts inventoryProducts)
        {
            var inventory = db.Inventories.Where(i => i.Id == inventoryProducts.InventoryId).FirstOrDefault();
            var lastInventoryProduct = db.InventoryProducts.Where(l => l.InventoryId == inventory.LastInventoryId && l.ProductId == inventoryProducts.ProductId).FirstOrDefault();
            inventoryProducts.AmountSold = (lastInventoryProduct.Units - inventoryProducts.Units);
            return inventoryProducts;
        }

        // GET: InventoryProduct/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.InventoryProducts.Where(p => p.Id == id).FirstOrDefault();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: InventoryProduct/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, InventoryProducts inventoryProducts)
        {
            if (ModelState.IsValid)
            {
                var iP = db.InventoryProducts.Where(p => p.Id == inventoryProducts.Id).FirstOrDefault();
                iP.Units = inventoryProducts.Units;
                iP = FindTotalValueOfProducts(iP);
                iP = FindProfitPerUnit(iP);
                iP = FindGMROI(iP);
                iP = FindParLevel(iP);
                iP = FindHowMuchSold(iP);
                iP.GetDate = DateTime.Now.ToShortDateString();
                db.Entry(iP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CalcInventoryValue", "Inventory", iP);
            }
            return RedirectToAction("Index", "Inventory");
        }

        public ActionResult AddInventory(int id)//Comes here to add unit to a product
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.InventoryProducts.Where(p => p.Id == id).FirstOrDefault();
            product.Units = 0;
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        /// /////////////////////////////////////////////////////////////////////////////

        [HttpPost]
        public ActionResult AddInventory(int id, InventoryProducts inventoryProducts)
        {
            if (ModelState.IsValid)
            {
                var ip = db.InventoryProducts.Where(p => p.Id == id).FirstOrDefault();
                ip.TimesOrdered++;
                ip.AmountOrdered += inventoryProducts.Units;
                ip.Units = (ip.Units + inventoryProducts.Units);
                ip = FindTotalValueOfProducts(ip);
                ip = FindProfitPerUnit(ip);
                ip = FindGMROI(ip);
                //ip = FindParLevel(ip);
                ip.GetDate = DateTime.Now.ToShortDateString();
                db.Entry(ip).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "Inventory");
            }
            return RedirectToAction("Index", "Inventory");
        }
        /// /////////////////////////////////////////////////////////////////////////////
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
