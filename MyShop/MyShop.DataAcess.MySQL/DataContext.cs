﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.Models;


namespace MyShop.DataAcess.MySQL
{
    public class DataContext : DbContext
    {
        public DataContext() : base("Connection_db_mysql")
        {
            

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; } 
    }
}
