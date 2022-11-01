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
    public class SaleController : Controller
    {
        private readonly E_CommerceContext _context;

        public SaleController(E_CommerceContext context) {
            _context = context;
        }

        [HttpGet]
        public IActionResult Cart(int id) {
          var product = _context.Products.Find(id);
          var productsInCart = new List<Product>() {};
          productsInCart.Add(product);
          return View(productsInCart);
        }

    }
}