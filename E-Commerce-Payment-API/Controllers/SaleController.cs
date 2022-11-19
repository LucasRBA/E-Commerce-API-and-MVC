using E_Commerce_Payment_API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_Commerce_Payment_API.Context;

namespace E_Commerce_Payment_API.Controllers
{
     public class SaleController : Controller
     {
          private readonly E_CommerceContext _context;

          public SaleController(E_CommerceContext context)
          {
               _context = context;
          }

          [HttpGet("/Order/{id}")]
          public IActionResult GetById(int id)
          {
               var sale = _context.Sales.Find(id);
               if (sale == null)
               {
                    return NotFound();
               }
               return Ok(sale);
          }

          [HttpGet("GetAllOrders")]
          public IActionResult GetAll()
          {
               var sale = _context.Sales.ToList();
               return Ok(sale);
          }

          [HttpGet("GetOrdersByDate")]
          public IActionResult GetByDate(string date)
          {
               var searchDate = date;
               try
               {
                    searchDate = Convert.ToDateTime(date).ToString("dd/MM/yyyy");
                    var sale = _context.Sales.ToList().Where(x => x.SaleDate.ToString("dd/MM/yyyy").Contains(searchDate)).ToArray();
                    return Ok(sale);
               }
               catch
               {
                    return BadRequest($"There was no date that matches the give input: {searchDate}. \n Try to enter the following pattern next time: day/month/fullyear or dd/MM/yyyy ... ");
               }

          }

          [HttpGet("GetOrdersByStatus")]
          public IActionResult GetByStatus(SaleStatusEnum status)
          {
               var sale = _context.Sales.ToList().Where(x => x.Status == status);
               return Ok(sale);
          }


          public Double CalculateShippingCost(int min, int max)
          {
               var rand = new Random();
               var intpart = rand.Next(min, max);
               var decimalpart = Math.Round(rand.NextDouble(), 2);
               double ShippingCost = intpart + decimalpart;
               return ShippingCost;
          }

          [HttpPost("PlaceAnOrder")]
          public IActionResult PlaceOrder(Sale sale, int ProductId)
          {
               var product = _context.Products.Find(ProductId);

               if (sale.Id != null)
               {
                    sale.Id = Convert.ToInt32("0");
               }
               if (sale.SaleDate != null || sale.SaleDate.Equals("") || sale.SaleDate.Equals(" ") || sale.SaleDate.Equals("string") || sale.SaleDate.Equals("{01/01/0001 00:00:00}"))
               {
                    sale.SaleDate = DateTime.Now;
               }
               if (sale.ProductId == null || sale.ProductId.Equals("") || sale.ProductId.Equals(" ") || sale.ProductId.Equals("string") || sale.ProductId <= 0)
               {
                    return BadRequest(new { Error = "Product Id can't be null or generic like default Swagger string value... " });
               }
               if (sale.ProductQuantity == null || sale.ProductQuantity.Equals("") || sale.ProductQuantity.Equals(" ") || sale.ProductQuantity.Equals("string") ||
               sale.ProductQuantity <= 0 || sale.ProductQuantity > product.StockQuantity)
               {
                    return BadRequest(new
                    {
                         Error = "Product Quantity can't be null or generic like default Swagger string value, zero or" +
                    "greather than the current stock qauntity of this product... "
                    });
               }
               var partialValue = sale.TotalValue = (sale.ProductQuantity * product.Price);
               var totalWeight = product.Weight * sale.ProductQuantity;
               if (totalWeight <= 15)
               {
                    var shippingCost = CalculateShippingCost(5, 50);
                    sale.TotalValue = partialValue + Convert.ToDecimal(shippingCost);
               }
               else
               {
                    var shippingCost = CalculateShippingCost(50, 100);
                    sale.TotalValue = partialValue + Convert.ToDecimal(shippingCost);
               }
               if (sale.Status != SaleStatusEnum.AwaitingPayment)
               {
                    sale.Status = SaleStatusEnum.AwaitingPayment;
               }
               if (ModelState.IsValid)
               {
                    _context.Sales.Add(sale);
                    _context.SaveChanges();
                    return CreatedAtAction(nameof(GetById), new { id = sale.Id }, sale);
               }

               return NotFound();

          }

          [HttpPut("UpdateOrderStatus")]
          public IActionResult UpdateStatus(Sale sale)
          {
               var saleInDatabase = _context.Sales.Find(sale.Id);

               if (saleInDatabase == null)
               {
                    return NotFound();
               }
               if (saleInDatabase.Status == SaleStatusEnum.AwaitingPayment)
               {
                    Enum[] newStatus = { SaleStatusEnum.Paid, SaleStatusEnum.Cancelled };
                    if (newStatus.Contains(sale.Status))
                    {
                         saleInDatabase.Status = sale.Status;
                    }
               }
               if (saleInDatabase.Status == SaleStatusEnum.Paid)
               {
                    Enum[] newStatus = { SaleStatusEnum.Cancelled, SaleStatusEnum.HandedOverToCarrier };
                    if (newStatus.Contains(sale.Status))
                    {
                         saleInDatabase.Status = sale.Status;
                    }
               }
               if (saleInDatabase.Status == SaleStatusEnum.HandedOverToCarrier)
               {
                    Enum[] newStatus = { SaleStatusEnum.OnRoute };
                    if (newStatus.Contains(sale.Status))
                    {
                         saleInDatabase.Status = sale.Status;
                    }
               }
               if (saleInDatabase.Status == SaleStatusEnum.OnRoute)
               {
                    Enum[] newStatus = { SaleStatusEnum.Delivered };
                    if (newStatus.Contains(sale.Status))
                    {
                         saleInDatabase.Status = sale.Status;
                    }
               }


               sale.Id = saleInDatabase.Id;
               sale.SaleDate = saleInDatabase.SaleDate;
               sale.ProductId = saleInDatabase.ProductId;
               sale.ProductQuantity = saleInDatabase.ProductQuantity;
               sale.Status = saleInDatabase.Status;
               sale.TotalValue = saleInDatabase.TotalValue;


               _context.Sales.Update(saleInDatabase);
               _context.SaveChanges();
               return Ok(sale);

          }

          [HttpDelete("DeleteCancelledOrders")]
          public IActionResult DeleteOrders(Sale sale)
          {
               var saleInDatabase = _context.Sales.Find(sale.Id);

               if (saleInDatabase == null)
               {
                    return NotFound();
               }

               if (saleInDatabase.Status == SaleStatusEnum.Cancelled)
               {
                    _context.Sales.Remove(saleInDatabase);
                    _context.SaveChanges();
                    return NoContent();
               }
               return BadRequest($"Only orders with Status of Cancelled can be deleted...");

          }

     }
}