using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Single_Capstone.Models
{
    public class InventoryProducts
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Inventory")]
        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public double Units { get; set; }

        public string GetDate { get; set; }

        [Display(Name ="Product")]
        public string ProductName { get; set; }

        [Display(Name ="Total Value of Products")]
        public double TotalValueOfProducts { get; set; }

        [Display(Name = "Purchase Price Per Unit")]
        public double PricePerUnitPurchased { get; set; }

        [Display(Name = "Selling Price Per Unit")]
        public double PricePerUnitSelling { get; set; }

        [Display(Name = "Profit Per Unit")]
        public double ProfitToBeMadePerUnit { get; set; }

        public List<Product> Products { get; set; }
    }
}