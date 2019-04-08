using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Components
{
    public class SectionsViewComponent: ViewComponent
    {
        private readonly IProductData _productData;

        public SectionsViewComponent(IProductData productData)
        {
            this._productData = productData;
        }
        public IViewComponentResult Invoke()
        {
            var section = GetSections();
            return View(section);
        }

        //Асинхронная версия
        //public async Task<IViewComponentResult> InvokeAsync()
        //{ }


        private IEnumerable<SectionViewModel> GetSections()
        {
            var sections = _productData.GetSections();

            var parent_sections = sections
                .Where(s => s.ParentId == null)
                .Select(s => new SectionViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Order = s.Order
                }).ToList();

            foreach (var item in parent_sections)
            {
                var child_sections = sections
                    .Where(s => s.ParentId == item.Id)
                    .Select(s => new SectionViewModel {
                        Id = s.Id,
                        Name = s.Name,
                        Order = s.Order
                    });

                item.ChildSections.AddRange(child_sections);
                item.ChildSections.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
            }
            parent_sections.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
            return parent_sections;
        }
    }
}
