using E_Commerce_Payment_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using E_Commerce_Payment_API.Context;
using Nager.Country;


namespace E_Commerce_Payment_API.Controllers
{
     public class ShippingController : Controller
     {
          private readonly E_CommerceContext _context;

          public ShippingController(E_CommerceContext context)
          {
               _context = context;
          }

          public IActionResult Index()
          {
               return View();
          }

          [HttpGet("/Shippings/{id}")]
          public IActionResult GetShippingById(int id)
          {
               var shipping = _context.Shippings.Find(id);
               if (shipping == null)
               {
                    return NotFound();
               }
               return Ok(shipping);
          }

          [HttpGet("GetAllShippedOrders")]
          public IActionResult GetAllShippings()
          {
               var shipping = _context.Shippings.ToList();
               return Ok(shipping);
          }

          [HttpGet("GetShippingOrdersByStatus")]
          public IActionResult GetByStatus(SaleStatusEnum status)
          {
               var shipping = _context.Shippings.ToList().Where(x => x.DeliveryStatus == status);
               return Ok(shipping);
          }

          public Double CalculateDeliveryDate(int min, int max)
          {
               var rand = new Random();
               var days = rand.Next(min, max);
               return days;

          }

          [HttpGet("FindByTrackingNumber")]
          public IActionResult GetByTrackingNumber(string trackingNumber)
          {
               var shipping = _context.Shippings.ToList().Where(x => x.SaleTrackingNumber == trackingNumber);
               return Ok(shipping);
          }

          public string GenerateTrackingNumber()
          {
               var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
               var number1 = CalculateDeliveryDate(0, 9);
               var number2 = CalculateDeliveryDate(0, 9);
               var number3 = CalculateDeliveryDate(000, 999);
               var sequence1 = new char[3];
               var sequence2 = new char[3];
               var sequence3 = new char[3]; // nine letters 
               var random = new Random();
               ICountryProvider countryProvider = new CountryProvider();
               var country = countryProvider.GetCountries();
               int k = Convert.ToInt32(CalculateDeliveryDate(0, 249));
               var selectedCountry = country.ToList()[k].Alpha2Code;

               for (int i = 0; i < sequence1.Length; i++)
               {
                    sequence1[i] = chars[random.Next(chars.Length)];
                    sequence2[i] = chars[random.Next(chars.Length)];
                    sequence3[i] = chars[random.Next(chars.Length)];

               }

               var string1 = new String(sequence1);
               var string2 = new String(sequence2);
               var string3 = new String(sequence3);

               return $"{string1}{number1}-{string2}{number2}-{string3}-{number3}-{selectedCountry}"; //11


          }

          [HttpPost("CreateTrackingInfo")]
          public IActionResult CreateShipping(Shipping shipping, int SaleId)
          {

               if (SaleId == null)
               {
                    return NotFound();
               }

               var sale = _context.Sales.Find(SaleId);
               if (sale == null)
               {
                    return BadRequest("There's no order corresponding with the given input");
               }
               var product = _context.Products.Find(sale.ProductId);

               if (shipping.Id != null)
               {
                    shipping.Id = Convert.ToInt32("0");
               }


               if (shipping.ShippingCost != null || shipping.ShippingCost.Equals("") || shipping.ShippingCost.Equals(" ") ||
                    shipping.ShippingCost.Equals("string") || shipping.ShippingCost <= 0)
               {
                    shipping.ShippingCost = sale.TotalValue - (sale.ProductQuantity * product.Price);
               }

               if (shipping.ShippingWeight != null || shipping.ShippingWeight.Equals("") || shipping.ShippingWeight.Equals(" ") ||
                    shipping.ShippingWeight.Equals("string") || shipping.ShippingWeight <= (product.Weight * sale.ProductQuantity))
               {
                    shipping.ShippingWeight = 1.05m * ((product.Weight) * (sale.ProductQuantity));
               }

               if (shipping.EstimatedDeliveryDate != null || shipping.EstimatedDeliveryDate.Equals("") ||
                shipping.EstimatedDeliveryDate.Equals(" ") || shipping.EstimatedDeliveryDate.Equals("string") ||
                shipping.EstimatedDeliveryDate <= sale.SaleDate)
               {
                    var daysUntillDelivery = CalculateDeliveryDate(2, 15);
                    shipping.EstimatedDeliveryDate = sale.SaleDate.AddDays(daysUntillDelivery);
               }

               if (shipping.DeliveryStatus != null || shipping.DeliveryStatus.Equals("") || shipping.DeliveryStatus.Equals(" ") ||
                    shipping.DeliveryStatus.Equals("string") || shipping.DeliveryStatus == SaleStatusEnum.AwaitingPayment)
               {
                    shipping.DeliveryStatus = sale.Status;
               }

               shipping.SaleTrackingNumber = GenerateTrackingNumber();


               if (ModelState.IsValid)
               {
                    _context.Shippings.Add(shipping);
                    _context.SaveChanges();
                    return CreatedAtAction(nameof(GetShippingById), new { id = shipping.Id }, shipping);
               }

               return NotFound();
          }

          [HttpPut("EditStatus&DeliveryDate")]
          public IActionResult EditInfo(Shipping shipping)
          {

               var shippingInDatabase = _context.Shippings.Find(shipping.Id);

               if (shippingInDatabase == null)
               {
                    return NotFound();
               }

               shipping.SaleId = shippingInDatabase.SaleId;

               shipping.ShippingCost = shippingInDatabase.ShippingCost;

               shipping.ShippingWeight = shippingInDatabase.ShippingWeight;

               if (shipping.EstimatedDeliveryDate == null || shipping.EstimatedDeliveryDate.Equals("") ||
                shipping.EstimatedDeliveryDate.Equals(" ") || shipping.EstimatedDeliveryDate.Equals("string") ||
                shipping.EstimatedDeliveryDate < shippingInDatabase.EstimatedDeliveryDate)
               {
                    shipping.EstimatedDeliveryDate = shippingInDatabase.EstimatedDeliveryDate;
               }

               if (shipping.DeliveryStatus == null || shipping.DeliveryStatus.Equals("") || shipping.DeliveryStatus.Equals(" ") ||
                    shipping.DeliveryStatus.Equals("string") || shipping.DeliveryStatus == SaleStatusEnum.AwaitingPayment)
               {
                    shipping.DeliveryStatus = shippingInDatabase.DeliveryStatus;
               }

               if (shippingInDatabase.DeliveryStatus == SaleStatusEnum.AwaitingPayment)
               {
                    Enum[] newStatus = { SaleStatusEnum.Paid, SaleStatusEnum.Cancelled };
                    if (newStatus.Contains(shipping.DeliveryStatus))
                    {
                         shippingInDatabase.DeliveryStatus = shipping.DeliveryStatus;
                    }
               }
               if (shippingInDatabase.DeliveryStatus == SaleStatusEnum.Paid)
               {
                    Enum[] newStatus = { SaleStatusEnum.Cancelled, SaleStatusEnum.HandedOverToCarrier };
                    if (newStatus.Contains(shipping.DeliveryStatus))
                    {
                         shippingInDatabase.DeliveryStatus = shipping.DeliveryStatus;
                    }
               }
               if (shippingInDatabase.DeliveryStatus == SaleStatusEnum.HandedOverToCarrier)
               {
                    Enum[] newStatus = { SaleStatusEnum.OnRoute };
                    if (newStatus.Contains(shipping.DeliveryStatus))
                    {
                         shippingInDatabase.DeliveryStatus = shipping.DeliveryStatus;
                    }
               }
               if (shippingInDatabase.DeliveryStatus == SaleStatusEnum.OnRoute)
               {
                    Enum[] newStatus = { SaleStatusEnum.Delivered };
                    if (newStatus.Contains(shipping.DeliveryStatus))
                    {
                         shippingInDatabase.DeliveryStatus = shipping.DeliveryStatus;
                    }
               }

               shipping.SaleTrackingNumber = shippingInDatabase.SaleTrackingNumber;
               shipping.DeliveryStatus = shippingInDatabase.DeliveryStatus;


               _context.Shippings.Update(shippingInDatabase);
               _context.SaveChanges();
               return Ok(shipping);

          }

          [HttpDelete("RemoveShippingData")]
          public IActionResult DeleteShippingData(Shipping shipping)
          {

               var shippingInDatabase = _context.Shippings.Find(shipping.Id);

               var sale = _context.Sales.Find(shippingInDatabase.SaleId);
               
               if (shippingInDatabase == null)
               {
                    return NotFound();
               }

               Enum[] newStatus = { SaleStatusEnum.Delivered, SaleStatusEnum.Cancelled };

               if (newStatus.Contains(shipping.DeliveryStatus))
               {

                var today = DateTime.Now;
                    if(shipping.EstimatedDeliveryDate.AddDays(30) <= today) {
                        _context.Shippings.Remove(shippingInDatabase);
                        _context.SaveChanges();
                        return NoContent();
                    }         
               }

               return BadRequest($"Only orders with Status of Cancelled or Delivered with more than 30 days of completion can be deleted..." +
               $"\nThis order is still {shipping.DeliveryStatus} and was made was made in {sale.SaleDate.ToString("dd/MM/yyyy")}.");
                    

          }
     }
}