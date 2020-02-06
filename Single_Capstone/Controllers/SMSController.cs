using Single_Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Twilio.AspNet.Mvc;
using Twilio.Rest.Api.V2010.Account;

namespace Single_Capstone.Controllers
{
    public class SMSController : TwilioController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: SMS
        public ActionResult SendSms(InventoryProducts inventoryProducts)
        {
            TwilioClient.Init(PrivateKeys.AccountSID, PrivateKeys.AuthToken);

            var message = MessageResource.Create(
                body: inventoryProducts.ProductName + ", is under Par level of " + inventoryProducts.ParLevel + ". Please make sure to add it to your order.",
                from: new Twilio.Types.PhoneNumber(PrivateKeys.TwilioPhoneNumber),
                to: new Twilio.Types.PhoneNumber(PrivateKeys.MyPhoneNumber)
                );
            var inventory = db.Inventories.Where(i => i.Id == inventoryProducts.InventoryId).FirstOrDefault();
            return RedirectToAction("SelectedInventoryDetails", "Inventory", inventory);
        }

        public ActionResult SendRecommendations()
        {
            TwilioClient.Init(PrivateKeys.AccountSID, PrivateKeys.AuthToken);
                        var message = MessageResource.Create(
                body: RecommendViewModel.MessageToSend,
                from: new Twilio.Types.PhoneNumber(PrivateKeys.TwilioPhoneNumber),
                to: new Twilio.Types.PhoneNumber(PrivateKeys.MyPhoneNumber)
                );
            return RedirectToAction("ClearMessage");
        }

        public ActionResult NoRecommendationToSend()
        {
            TwilioClient.Init(PrivateKeys.AccountSID, PrivateKeys.AuthToken);
                    var message = MessageResource.Create(
                body: "No recommendations at this time.",
                from: new Twilio.Types.PhoneNumber(PrivateKeys.TwilioPhoneNumber),
                to: new Twilio.Types.PhoneNumber(PrivateKeys.MyPhoneNumber)
                );
            return RedirectToAction("Index", "Inventory");
        }

        public ActionResult ClearMessage()
        {
            RecommendViewModel.InventoryId = 0;
            RecommendViewModel.MessageToSend = null;
            return RedirectToAction("Index", "Inventory");
        }
    }
}