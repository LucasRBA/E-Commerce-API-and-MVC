using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Payment_API.Models
{
    public enum SaleStatusEnum
    {
        [Display(Name="Awaiting Payment")]
        AwaitingPayment,
        Paid,
        Cancelled,
        [Display(Name ="Handed over to the carrier")]
        HandedOverToCarrier,
        [Display(Name ="On route")]
        OnRoute,
        Delivered
    }
}
