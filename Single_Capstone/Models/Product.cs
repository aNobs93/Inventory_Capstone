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

        public int BusinessId { get; set; }    

        [Display(Name = "Name of product")]
        public string ProductName { get; set; }

        public double Units { get; set; }

        [Display(Name ="Purchase Price Per Unit")]
        public double PricePerUnitPurchased { get; set; }

        [Display(Name = "Selling Price Per Unit")]
        public double PricePerUnitSelling { get; set; }

        [Display(Name ="Profit Per Unit")]
        public double ProfitToBeMadePerUnit { get; set; }

        [Display(Name ="Total Value of Product")]
        public double TotalProductValue { get; set; }

        [Display(Name ="Updated On")]
        public string GetDate { get; set; }
    }
}