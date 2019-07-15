using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.DTO.Product;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.Infrastructure.Implementations
{
    public class SqlProductData : IProductData
    {
        private readonly WebStoreContext _Db;

        public SqlProductData(WebStoreContext _db)
        {
            _Db = _db;
        }
        public IEnumerable<Brand> GetBrands()
        {
            return _Db.Brands/*.Include(b => b.Product)*/.AsEnumerable();
        }

        public ProductDTO GetProductById(int id)
        {
            var p = _Db.Products
                .Include(i => i.Brand)
                .Include(i => i.Section)
                .FirstOrDefault(f => f.Id == id);
            return p is null ? null : new ProductDTO
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
            };
        }

        public IEnumerable<ProductDTO> GetProducts(ProductFilter filter)
        {
            IQueryable<Product> products = _Db.Products;
            if (filter is null)
            {
                return products.
                    AsEnumerable().
                    Select(p => new ProductDTO
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

            if (filter.SectionId != null)
            {
                products = products.Where(p => p.SectionId == filter.SectionId);
            }

            if (filter.BrandId != null)
            {
                products = products.Where(p => p.BrandId == filter.BrandId);
            }

            return products.AsEnumerable().Select(p => new ProductDTO
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

        public IEnumerable<Section> GetSections()
        {
            //Указываем что при загрузке данных из таблицы Sections необходимо также загрузить связанные данные
            //из таблицы Products и представить это все как Enumerable
            return _Db.Sections/*.Include(s => s.Products)*/.AsEnumerable();
        }
    }
}
