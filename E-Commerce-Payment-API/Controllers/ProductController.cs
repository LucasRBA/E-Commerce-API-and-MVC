using E_Commerce_Payment_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using E_Commerce_Payment_API.Context;

namespace E_Commerce_Payment_API.Controllers
{
     public class ProductController : Controller
     {
          private readonly E_CommerceContext _context;
          public ProductController(E_CommerceContext context)
          {
               _context = context;
          }
          public IActionResult Index()
          {
               return View();
          }

          [HttpGet("/Product/{id}")]
          public IActionResult GetById(int id)
          {
               var product = _context.Products.Find(id);
               if (product == null)
               {
                    return NotFound();
               }
               return Ok(product);
          }

          [HttpGet("GetAllProducts")]
          public IActionResult GetAll()
          {
               var product = _context.Products.ToList();
               return Ok(product);
          }

          [HttpGet("GetProductsByName")]
          public IActionResult GetByName(string name)
          {
               var product = _context.Products.Where(x => x.Name.ToUpper().Contains(name.ToUpper())).ToList();
               return Ok(product);
          }

          [HttpPost("AddProduct")]
          public IActionResult Create(Product product)
          {
               if (product.Name == null || product.Name.Equals("") || product.Name.Equals(" ") || product.Name.Equals("string"))
               {
                    return BadRequest(new { Error = "Name can't be null or generic like default Swagger string value... " });
               }

               if (product.Price == null || product.Price.Equals("") || product.Price.Equals(" ") || product.Price.Equals("string") || product.Price.Equals(0) || product.Price.Equals(0.0))
               {
                    return BadRequest(new { Error = "Price can't be null or generic like default Swagger string value... " });
               }
               if (product.StockQuantity == null || product.StockQuantity.Equals("") || product.StockQuantity.Equals(" ") || product.StockQuantity.Equals("string"))
               {
                    return BadRequest(new { Error = "Stock quantity can't be null or generic like default Swagger string value... " });
               }
               if (product.Weight == null || product.Weight.Equals("") || product.Weight.Equals(" ") || product.Weight.Equals("string") || product.Weight.Equals(0) || product.Weight.Equals(0.0))
               {
                    return BadRequest(new { Error = "Weight can't be null or generic like default Swagger string value... " });
               }

               _context.Products.Add(product);
               _context.SaveChanges();

               return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
          }

          [HttpPut("/EditProductInfo/{id}")]
          public IActionResult Edit(int id, Product product)
          {
               var productInDatabase = _context.Products.Find(id);
               if (productInDatabase == null)
               {
                    return NotFound();
               }

               if (product.Name == null || product.Name.Equals("") || product.Name.Equals(" ") || product.Name.Equals("string"))
               {
                    product.Name = productInDatabase.Name;
               }
               if (product.Description == null || product.Description.Equals("") || product.Description.Equals(" ") || product.Description.Equals("string"))
               {
                    product.Description = productInDatabase.Description;
               }

               if (product.Price == null || product.Price.Equals("") || product.Price.Equals(" ") || product.Price.Equals("string") || product.Price.Equals(0) || product.Price.Equals(0.0))
               {
                    product.Price = productInDatabase.Price;
               }
               if (product.StockQuantity == null || product.StockQuantity.Equals("") || product.StockQuantity.Equals(" ") || product.StockQuantity.Equals("string"))
               {
                    product.StockQuantity = productInDatabase.StockQuantity;
               }
               if (product.Weight == null || product.Weight.Equals("") || product.Weight.Equals(" ") || product.Weight.Equals("string") || product.Weight.Equals(0) || product.Weight.Equals(0.0))
               {
                    product.Weight = productInDatabase.Weight;
               }

               productInDatabase.Name = product.Name;
               productInDatabase.Description = product.Description;
               productInDatabase.Price = product.Price;
               productInDatabase.StockQuantity = product.StockQuantity;
               productInDatabase.Weight = product.Weight;

               _context.Products.Update(productInDatabase);
               _context.SaveChanges();
               return Ok(product);
          }

          [HttpDelete("/DeleteProduct/{id}")]
          public IActionResult Delete(int id)
          {
               var productInDatabase = _context.Products.Find(id);

               if (productInDatabase == null)
               {
                    return NotFound();
               }

               _context.Products.Remove(productInDatabase);
               _context.SaveChanges();
               return NoContent();
          }

     }
}