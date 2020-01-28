using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Single_Capstone.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Inventory")]
        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }

        [Display(Name = "Name of product")]
        public string ProductName { get; set; }

        public double Quantity { get; set; }

        [Display(Name ="Purchase Price Per Unit")]
        public double PricePerUnitPurchased { get; set; }

        [Display(Name ="Selling Price Per Unit")]
        public double PricePerUnitSelling { get; set; }
    }
}