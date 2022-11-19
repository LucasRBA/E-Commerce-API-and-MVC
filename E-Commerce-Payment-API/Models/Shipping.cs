using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace E_Commerce_Payment_API.Models
{
    public class Shipping
    {
        [Key()]
        public int Id { get; set; }
        [ForeignKey("SaleId")]
        public int SaleId { get; set; }
        internal virtual Sale Sale { get; set; }
        public decimal ShippingCost { get; set; } //TODO Calculate Shipping cost based on shipping weight
        public decimal ShippingWeight { get; set; } // TODO Calculate Shipping weight based on products, quantity, + 5%(box and bubble plastic)
        public DateTime EstimatedDeliveryDate { get; set; }
        public SaleStatusEnum DeliveryStatus { get; set; }

        public string SaleTrackingNumber { get; set; }
    }
}
