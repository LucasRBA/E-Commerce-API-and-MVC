using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using E_Commerce_Payment_API.Models;

namespace E_Commerce_Payment_API.Context
{
    public class E_CommerceContext : DbContext
    {
        public E_CommerceContext(DbContextOptions<E_CommerceContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Shipping> Shippings { get; set; }

    }
}
