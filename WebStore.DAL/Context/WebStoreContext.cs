using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entities;

namespace WebStore.DAL.Context
{
    public class WebStoreContext: DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Section> Sections { get; set; }

        WebStoreContext(DbContextOptions<WebStoreContext> options)
            :base(options)
        {

        }
    }
}
