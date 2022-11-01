using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
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
        public virtual Sale Sale { get; set; }
        [DisplayName("Shipping cost")]
        public decimal ShippingCost { get; set; } //TODO Calculate Shipping cost based on shipping weight
        [DisplayName("Total weight")]
        public decimal ShippingWeight { get; set; } // TODO Calculate Shipping weight based on products, quantity, + 5%(box and bubble plastic)
        [DisplayName("Estimated delivery")]
        public DateTime EstimatedDeliveryDate { get; set; }
        [DisplayName("Delivery status")]
        public SaleStatusEnum DeliveryStatus {get; set; }
    }
}
