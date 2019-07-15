using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.Domain.ViewModels.Product
{
    public class BrandViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        /// <summary> Кол-во товаров по заданному бренду </summary>
        public int ProductsCount { get; set; }
    }
}
