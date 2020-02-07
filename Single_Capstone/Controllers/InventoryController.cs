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
    public class InventoryController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Inventory
        public ActionResult Index()//Shows all inventorys in order from earliest to newest of that business owner
        {
            var userId = User.Identity.GetUserId();
            var business = db.Businesses.Where(b => b.ApplicationId == userId).FirstOrDefault();
            var inventory = db.Inventories.Where(i => i.BusinessId == business.Id).ToList();
            return View(inventory);
        }

        public ActionResult CalcInventoryValue(InventoryProducts inventoryProducts)//Running total calculation
        {
            var inventory = db.Inventories.Where(i => i.Id == inventoryProducts.InventoryId).FirstOrDefault();
            inventory.TotalInventoryWorth += inventoryProducts.TotalValueOfProducts;
            inventory.ProfitMargin += (inventoryProducts.ProfitToBeMadePerUnit * inventoryProducts.Units);
            if(inventory.ProfitMargin == 0 && inventory.TotalInventoryWorth == 0)
            {
                inventory.GMROI = 0;
            }
            else if(inventory.ProfitMargin != 0 || inventory.TotalInventoryWorth != 0)
            {
                inventory.GMROI = Math.Round(inventory.ProfitMargin / inventory.TotalInventoryWorth, 2);
            }
            db.Entry(inventory).State = EntityState.Modified;
            db.SaveChanges();
            if (inventoryProducts.Units < inventoryProducts.ParLevel)
            {
                return RedirectToAction("SendSms", "SMS", inventoryProducts);
            }
            return RedirectToAction("SelectedInventoryDetails", inventory);
        }

        public ActionResult SelectedInventoryDetails(int id, Inventory inventory)//This allows user to look deeper into previous inventory when they hit details of an inventory from the index
        {
            if(inventory.Id != null)
            {
                var iP = db.InventoryProducts.Where(ip => ip.InventoryId == inventory.Id).ToList();
                return View(iP);
            }
            var inventoryProducts = db.InventoryProducts.Where(ip => ip.InventoryId == id).ToList();
            return View(inventoryProducts);
        }

        public ActionResult FindLastYearsInventory(int id) //come heres for recommendations, it finds the prior years future month
        {
            var inventory = db.Inventories.Find(id);
            var myDate = DateTime.Parse(inventory.GetDate);
            var myMonth = myDate.AddMonths(+1);
            var myYear = myDate.AddYears(-1);
            var allInventories = db.Inventories.Where(i => i.BusinessId == inventory.BusinessId).ToList();
            //int invTocheck;
            for (int i = 0; i< allInventories.Count; i++)
            {                
                var oldDate = DateTime.Parse(allInventories[i].GetDate);
                if (oldDate.Month == myMonth.Month && oldDate.Year == myYear.Year)
                {
                    RecommendViewModel.InventoryId = allInventories[i].Id;
                }
            }
            var inv = db.Inventories.Where(j => j.Id == RecommendViewModel.InventoryId).FirstOrDefault();
            return RedirectToAction("GetProductFromLastYear", inv);
        }

        public ActionResult GetProductFromLastYear(Inventory inventory)//pulls all the product from found inventory and compares and composes message to send if there is one
        {
            var userId = User.Identity.GetUserId();
            var business = db.Businesses.Where(b => b.ApplicationId == userId).FirstOrDefault();
            var inventories = db.Inventories.Where(i => i.BusinessId == business.Id).ToList();
            var thisInventoryId = inventories.Max(i => i.Id);
            var thisInventory = db.InventoryProducts.Where(i => i.InventoryId == thisInventoryId).ToList();
            var inventoryProducts = db.InventoryProducts.Where(ip => ip.InventoryId == inventory.Id).ToList();
            for (int i = 0; i < inventoryProducts.Count; i++)
            {
                if (thisInventory[i].Units < inventoryProducts[i].AmountSold)
                {
                    var amountToOrder = (inventoryProducts[i].AmountSold - thisInventory[i].Units);
                     RecommendViewModel.MessageToSend += "You sold " + inventoryProducts[i].AmountSold + " " + inventoryProducts[i].ProductName + " next month, last year. Currently you have " +
                        thisInventory[i].Units + " " + inventoryProducts[i].ProductName + ". We recommend ordering " + amountToOrder + " " + inventoryProducts[i].ProductName + ". ";
                }
            }
            if(RecommendViewModel.MessageToSend == null)
            {
                return RedirectToAction("NoRecommendationToSend", "SMS");
            }
            return RedirectToAction("SendRecommendations", "SMS");
        }

        public int FindLastYearThisMonth(Inventory inventory)//ProfitMissedOutOn calls to find the inventory id of last year this months inventory and returns it
        {
            var myDate = DateTime.Parse(inventory.GetDate);
            var myYear = myDate.AddYears(-1);
            var allInventories = db.Inventories.Where(a => a.BusinessId == inventory.BusinessId).ToList();
            for (int i = 0; i < allInventories.Count; i++)
            {
                var oldDate = DateTime.Parse(allInventories[i].GetDate);
                if (oldDate.Month == myDate.Month && oldDate.Year == myYear.Year)
                {
                    return allInventories[i].Id;
                }
            }
            return 0;
        }

        public ActionResult ProfitMissedOutOn(int id)//Assigns the list of inventory products to the profitmissedoutonviewmodel and then redirects to charts controller
        {
            ClearProfitMissedOutOnModel();
            var inventory = db.Inventories.Find(id);
            var oldInvId = FindLastYearThisMonth(inventory);
            ProfitMissedOutOnViewModel.ThisInventoryProducts = db.InventoryProducts.Where(i => i.InventoryId == inventory.Id).ToList();
            ProfitMissedOutOnViewModel.OldInventoryProducts = db.InventoryProducts.Where(o => o.InventoryId == oldInvId).ToList();
            return RedirectToAction("DisplayIfProfitWasLostOnProduct", "Chart");
        }
        public void ClearProfitMissedOutOnModel()//Clears the model to be used again
        {
            ProfitMissedOutOnViewModel.OldInventoryProducts = null;
            ProfitMissedOutOnViewModel.ThisInventoryProducts = null;
        }


        // GET: Inventory/Details/5
        public ActionResult Details()
        {
            var userId = User.Identity.GetUserId();
            var business = db.Businesses.Where(b => b.ApplicationId == userId).FirstOrDefault();
            var inventory = db.Inventories.Where(i => i.BusinessId == business.Id).FirstOrDefault();
            return View();
        }

        // GET: Inventory/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            var business = db.Businesses.Where(b => b.ApplicationId == userId).FirstOrDefault();
            var key = db.Inventories.Where(k => k.BusinessId == business.Id).FirstOrDefault();
            if(key == null)
            {
                Inventory Inventory = new Inventory();
                Inventory.BusinessId = business.Id;
                Inventory.GetDate = DateTime.Now.ToShortDateString();
                db.Inventories.Add(Inventory);
                db.SaveChanges();
                return RedirectToAction("Index");//if this is first inventory taken it goes here
            }
            var findMax = db.Inventories.Where(f => f.BusinessId == business.Id).ToList();
            int max = findMax.Max(m => m.Id);
            Inventory inventory = new Inventory();
            inventory.BusinessId = business.Id;
            inventory.GetDate = DateTime.Now.ToShortDateString();
            inventory.LastInventoryId = max;
            db.Inventories.Add(inventory);
            db.SaveChanges();
            return RedirectToAction("Create", "InventoryProduct", inventory);
            
        }

       [HttpPost]
        public ActionResult Create(Inventory inventory)
        {
            try
            {

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Inventory/Edit/5
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

        // GET: Inventory/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Inventory/Delete/5
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
