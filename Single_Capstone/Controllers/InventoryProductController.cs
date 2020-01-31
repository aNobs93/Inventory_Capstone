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


        public void LoopToAssignValuesToProductInventory(InventoryProducts inventoryProducts, Inventory inventory)//after create sent here to a ssign values
        {
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
            var iP = db.InventoryProducts.Where(p => p.Id == inventoryProducts.Id).FirstOrDefault();
            var product = db.Products.Where(p => p.Id == iP.ProductId).FirstOrDefault();
            inventoryProducts.TotalValueOfProducts = (inventoryProducts.Units * product.PricePerUnit);
            return inventoryProducts;
        }

        public InventoryProducts FindProfitPerUnit(InventoryProducts inventoryProducts)//this works for new product thats added
        {
            var iP = db.InventoryProducts.Where(p => p.Id == inventoryProducts.Id).FirstOrDefault();
            var product = db.Products.Where(p => p.Id == iP.ProductId).FirstOrDefault();
            inventoryProducts.ProfitToBeMadePerUnit = (product.PricePerUnitSelling - product.PricePerUnit);
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
                //inventoryProducts.ProductId = product.ProductId;
                //inventoryProducts.ProductName = product.ProductName;
                //inventoryProducts.InventoryId = product.InventoryId;
                iP = FindTotalValueOfProducts(iP);
                iP = FindProfitPerUnit(iP);
                iP.GetDate = DateTime.Now.ToShortDateString();
                //db.Set<InventoryProducts>().AddOrUpdate(inventoryProducts);

/*                db.Set<InventoryProducts>().Attach(inventoryProducts); *///attach

                db.Entry(iP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CalcInventoryValue", "Inventory", iP);
            }
            return RedirectToAction("Index", "Inventory");
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
