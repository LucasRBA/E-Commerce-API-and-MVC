using E_Commerce_Payment_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce_Payment_API.Controllers
{
    public class ShippingController : Controller
    {

        public IActionResult Index() {
            return View();
        }
        
    }
}