using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Single_Capstone.Models
{
    public class Inventory
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Business")]
        public int BusinessId { get; set; }
        public Business Business { get; set; }

        [Display(Name ="Total Value")]
        public double TotalInventoryWorth { get; set; }

        [Display(Name ="Profit Margin")]
        public double ProfitMargin { get; set; }

        //public List<MonthlyInventory> MonthlyInventories { get; set; }
        public List<Product> Products { get; set; }
    }
}