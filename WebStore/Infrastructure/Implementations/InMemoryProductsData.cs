using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.Infrastructure.Implementations
{
    public class InMemoryProductsData : IProductData
    {
        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public IEnumerable<Section> GetSections() => TestData.Sections;

        public IEnumerable<Product> GetProducts(ProductFilter filter)
        {
            var products = TestData.Products;
            if (filter is null)
            { return products; }

            if (filter.BrandId != null)
            {
                products = products.Where(f => f.BrandId == filter.BrandId);
            }
            if (filter.SectionId != null)
            {
                products = products.Where(f => f.SectionId == filter.SectionId);
            }
            return products;
        }
        
    }
}
