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
    public class SellerController : Controller
    {
      private readonly E_CommerceContext _context;

      public SellerController(E_CommerceContext context) {
        _context = context;
      }

      public IActionResult Index() {
        return View();
      }
        
      public IActionResult SellerActionsPage() {
        var seller = _context.Sellers.ToList();
        return View(seller);
      }

      public IActionResult Edit() {
        return View();
      }

      public IActionResult Delete() {
        return View();
      }

      public IActionResult RegisterProduct() {
        return View();
      }
      public IActionResult DeleteProduct(){
       return View();
      }

      public IActionResult Register() {
        return View();
      }

      [HttpGet("/Seller/{id}")]
      public IActionResult GetById(int id) {
        var seller = _context.Sellers.Find(id);
        if(seller == null) {
          return NotFound();
        }
        return Ok(seller);
      }

      [HttpGet("GetAllsellers")]
      public IActionResult GetAll() {
        var seller = _context.Sellers.ToList();
        return Ok(seller);
      }
      
      [HttpGet("GetBySellerByName")]
      public IActionResult GetByName(string name) {
        var seller = _context.Sellers.Where(x=> x.Name.ToUpper().Contains(name.ToUpper())).ToList();
        return Ok(seller);
      }

      [HttpGet("GetByDocument")]
      public IActionResult GetByDocument(string document) {
        var seller = _context.Sellers.Where(x=> x.Document.ToUpper().Contains(document.ToUpper())).ToList();
        return Ok(seller);
      } //TODO Apply Document Formatter to Frontend

      [HttpPost("Create")]
      public IActionResult Create(Seller seller) {
        if(seller.Document == null || seller.Document.Equals("") || seller.Document.Equals(" ") || seller.Document.Equals("string")) {
          return BadRequest(new {Error = "Document can't be null or generic like default Swagger string value... "});
        }
        if(seller.Name == null || seller.Name.Equals("") || seller.Name.Equals(" ") || seller.Name.Equals("string")) {
          return BadRequest(new {Error = "Name can't be null or generic like default Swagger string value... "});
        }
        if(seller.LastName == null || seller.LastName.Equals("") || seller.LastName.Equals(" ") || seller.LastName.Equals("string")) {
          return BadRequest(new {Error = "Last name can't be null or generic like default Swagger string value... "});
        }
        if(seller.Email == null || seller.Email.Equals("") || seller.Email.Equals(" ") || seller.Email.Equals("string")) {
          return BadRequest(new {Error = "Email can't be null or generic like default Swagger string value... "});
        }

        _context.Sellers.Add(seller);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetById), new {id = seller.Id}, seller);
      }

      [HttpPut("/Edit/{id}")]
      public IActionResult Edit(int id, Seller seller) {
        var sellerInDatabase = _context.Sellers.Find(id); 
        if(sellerInDatabase == null) {
          return NotFound();
        }
        if(seller.Document == null || seller.Document.Equals("") || seller.Document.Equals(" ") || seller.Document.Equals("string")) {
          seller.Document = sellerInDatabase.Document;
        }
        if(seller.Name == null || seller.Name.Equals("") || seller.Name.Equals(" ") || seller.Name.Equals("string")) {
          seller.Name = sellerInDatabase.Name;
        }
        if(seller.LastName == null || seller.LastName.Equals("") || seller.LastName.Equals(" ") || seller.LastName.Equals("string")) {
          seller.LastName = sellerInDatabase.LastName;
        }
        if(seller.Email == null || seller.Email.Equals("") || seller.Email.Equals(" ") || seller.Email.Equals("string")) {
          seller.Email = sellerInDatabase.Email;
        }

        if(seller.PhoneNumber == null || seller.PhoneNumber.Equals("") || seller.PhoneNumber.Equals(" ") || seller.PhoneNumber.Equals("string")) {
          seller.PhoneNumber = sellerInDatabase.PhoneNumber;
        }

        sellerInDatabase.Document = seller.Document;
        sellerInDatabase.Name = seller.Name;
        sellerInDatabase.LastName = seller.LastName;
        sellerInDatabase.Email = seller.Email;
        sellerInDatabase.PhoneNumber = seller.PhoneNumber;

        _context.Sellers.Update(sellerInDatabase);
        _context.SaveChanges();
        return Ok(seller);
      }

      [HttpDelete("/Delete/{id}")]
      public IActionResult Delete(int id) {
        var sellerInDatabase = _context.Sellers.Find(id);

        if(sellerInDatabase == null) {
          return NotFound();
        }

        _context.Sellers.Remove(sellerInDatabase);
        _context.SaveChanges();
        return NoContent();
      }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
    }
}