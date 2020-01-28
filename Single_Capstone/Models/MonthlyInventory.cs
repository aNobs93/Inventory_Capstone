using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Single_Capstone.Models
{
    public class MonthlyInventory
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey()]

        public string Month { get; set; }

        public string Year { get; set; }

        public List<Product> Products { get; set; }

    }
}