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

        [Display(Name = "Name of product")]
        public string ProductName { get; set; }

        [Display(Name ="Price Per Unit")]
        public double PricePerUnit { get; set; }

        [Display(Name = "Selling Price Per Unit")]
        public double PricePerUnitSelling { get; set; }

        [Display(Name ="Safety Net '.00'")]
        public double ProductSafetyNet { get; set; }
    }
}