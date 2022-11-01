using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce_Payment_API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        [DisplayName("Stock Quantity")]
        public int StockQuantity { get; set; }
        public decimal Weight { get; set; }
    }

}
