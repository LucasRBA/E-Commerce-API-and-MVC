using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace E_Commerce_Payment_API.Models
{
    public class Sale
    {
        [Key()]
        public int Id { get; set; }
        public DateTime SaleDate { get; set; }
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        internal virtual Product Product { get; set; }
        public int ProductQuantity { get; set; }
        public decimal TotalValue { get; set; } // TODO Calculate total value salevalue + shipping cost and display 
        public SaleStatusEnum Status { get; set; }
    }
}

