using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Domain.DTO.Product;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.Infrastructure.Implementations
{
    public class InMemoryProductsData : IProductData
    {
        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public IEnumerable<Section> GetSections() => TestData.Sections;

        public IEnumerable<ProductDTO> GetProducts(ProductFilter filter)
        {
            var products = TestData.Products;
            if (filter is null)
            { return products.Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    Order = p.Order,
                    Brand = p.Brand is null
                        ? null
                        : new BrandDTO()
                        {
                            Id = p.Brand.Id,
                            Name = p.Brand.Name
                        }
                });
            }

            if (filter.BrandId != null)
            {
                products = products.Where(f => f.BrandId == filter.BrandId);
            }
            if (filter.SectionId != null)
            {
                products = products.Where(f => f.SectionId == filter.SectionId);
            }
            return products.Select(p => new ProductDTO {
                Id = p.Id,
                Name = p.Name,
                ImageUrl = p.ImageUrl,
                Price = p.Price,
                Order = p.Order,
                Brand = p.Brand is null 
                    ? null 
                    : new BrandDTO()
                    {
                        Id = p.Brand.Id,
                        Name = p.Brand.Name
                    }
            });
        }

        public ProductDTO GetProductById(int id)
        {
            var p = TestData.Products.FirstOrDefault(f => f.Id == id);

            return p is null 
                ? null 
                : new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Brand = p.Brand is null ? null : new BrandDTO()
                    {
                        Id = p.Brand.Id,
                        Name = p.Brand.Name
                    },
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    Order = p.Order
                };
        }
    }
}
