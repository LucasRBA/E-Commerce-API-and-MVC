using E_Commerce_Payment_API.Models;
using E_Commerce_Payment_API.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce_Payment_API.Controllers
{
    public class ProductController : Controller
    {
        private readonly E_CommerceContext _context;

        public ProductController(E_CommerceContext context) {
        _context = context;
      }


        public IActionResult Index() {
            var product = _context.Products.ToList();
            return View(product);
        }

        public IActionResult ProductsList() {
          var product = _context.Products.ToList();
          return View(product);
        }
        

        public IActionResult SellerIndex() {
            var product = _context.Products.ToList();
            return View(product);
        }

        [HttpGet]
        public IActionResult RegisterProduct() {
        return View(nameof(RegisterProduct));
      }

      [HttpPost]
      public IActionResult RegisterProduct(Product product) {
        if(product.Name == null || product.Name.Equals("") || product.Name.Equals(" ") || product.Name.Equals("string")) {
          return BadRequest(new {Error = "Name can't be null or generic like default Swagger string value... "});
        }
        if(product.Price == null || product.Price.Equals("") || product.Price.Equals(" ") || product.Price.Equals("string")) {
          return BadRequest(new {Error = "Price can't be null or generic like default Swagger string value... "});
        }
        if(product.StockQuantity == null || product.StockQuantity.Equals("") || product.StockQuantity.Equals(" ") || product.StockQuantity.Equals("string")) {
          return BadRequest(new {Error = "Quantity name can't be null or generic like default Swagger string value... "});
        }
        if(product.Weight == null || product.Weight.Equals("") || product.Weight.Equals(" ") || product.Weight.Equals("string")) {
          return BadRequest(new {Error = "weight can't be null or generic like default Swagger string value... "});
        }

        if(ModelState.IsValid) {
          _context.Products.Add(product);
          _context.SaveChanges();
          return RedirectToAction(nameof(SellerIndex));
        }

        return View(product);
      }

      [HttpGet]
      public IActionResult Index(string searchName) {

        ViewData["SearchProducts"] = searchName;

        if(!string.IsNullOrEmpty(searchName) ) {
        return View( _context.Products.Where(x=>x.Name.ToUpper().Contains(searchName.ToUpper()) || searchName == null).ToList());
        } 
        return View();
      }

      public IActionResult Edit(int id) {
         var product = _context.Products.Find(id);

        if(product == null) {
          return RedirectToAction(nameof(SellerIndex));
        }
        return View(product);
      }

       [HttpPost]
      public IActionResult Edit(Product product) {

      var productInDatabase = _context.Products.Find(product.Id); 

      if(productInDatabase == null) {
        return NotFound();
      }
      if(product.Name == null || product.Name.Equals("") || product.Name.Equals(" ") || product.Name.Equals("string")) {
        product.Name = productInDatabase.Name;
      }
      if(product.Price == null || product.Price.Equals("") || product.Price.Equals(" ") || product.Price.Equals("string")) {
        product.Name = productInDatabase.Name;
      }
      if(product.StockQuantity == null || product.StockQuantity.Equals("") || product.StockQuantity.Equals(" ") || product.StockQuantity.Equals("string")) {
        product.StockQuantity = productInDatabase.StockQuantity;
      }
      if(product.Weight == null || product.Weight.Equals("") || product.Weight.Equals(" ") || product.Weight.Equals("string")) {
        product.Weight = productInDatabase.Weight;
      }



      productInDatabase.Name = product.Name;
      productInDatabase.Price = product.Price;
      productInDatabase.StockQuantity = product.StockQuantity;
      productInDatabase.Weight = product.Weight;


      _context.Products.Update(productInDatabase);
      _context.SaveChanges();

      return RedirectToAction(nameof(SellerIndex));

    }

       public IActionResult DeleteProduct(int id) { 
        var product = _context.Products.Find(id);

        if(product == null) {
          return RedirectToAction(nameof(SellerIndex));
        }

        return View(product);
      }

      [HttpPost]
      public IActionResult DeleteProduct(Product product) {

        var productInDatabase = _context.Products.Find(product.Id);

        _context.Products.Remove(productInDatabase);
        _context.SaveChanges();
       
        return RedirectToAction(nameof(SellerIndex));        
      }
        
    }
}