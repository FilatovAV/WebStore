using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Infrastructure.Interfaces;
using WebStore.Infrastructure.Map;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _productData;

        public CatalogController(IProductData productData)
        {
            this._productData = productData;
        }
        public IActionResult Shop(int? SectionId, int? BrandId)
        {
            var products = _productData.GetProducts(new Domain.Entities.ProductFilter {
                SectionId = SectionId
                , BrandId = BrandId
            });

            var catalog_model = new CatalogViewModel
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Products = products.Select(ProductViewModelMapper.CreateViewModel)
                //Products = products.Select(p => new ProductViewModel {
                //    Id = p.Id,
                //    Name = p.Name,
                //    ImageUrl = p.ImageUrl,
                //    Order = p.Order,
                //    Price = p.Price }
                // )
            };

            return View(catalog_model);
        }
    }
}