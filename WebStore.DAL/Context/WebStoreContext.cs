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

        public WebStoreContext(DbContextOptions<WebStoreContext> options)
            :base(options)
        {

        }


        //FluentAPI
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    //Отношение один ко многим с указанием внешнего ключа
        //    modelBuilder.Entity<Section>()
        //        .HasMany(section => section.Products)
        //        .WithOne(product => product.Section)
        //        .HasForeignKey(product => product.SectionId);
        //}
    }
}
