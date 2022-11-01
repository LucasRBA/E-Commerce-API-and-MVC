using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_Payment_API.Models
{
    public class Sale
    {
        [Key()]
        public int Id { get; set; }
        [DisplayName("Sale date")]
        public DateTime SaleDate { get; set; }
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        [DisplayName("Product quantity")]
        public int ProductQuantity { get; set; }
        [DisplayName("Total value")]
        public decimal TotalValue { get; set; } // TODO Calculate total value salevalue + shipping cost and display 
        public SaleStatusEnum Status { get; set; }
    }
}

