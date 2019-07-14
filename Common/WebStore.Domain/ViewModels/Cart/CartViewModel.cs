using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.ViewModels.Product;
using System.Linq;

namespace WebStore.Domain.ViewModels.Cart
{
    public class CartViewModel
    {
        public Dictionary<ProductViewModel, int> Items { get; set; } = new Dictionary<ProductViewModel, int>();
        public int ItemsCount => Items?.Sum(items => items.Value) ?? 0;
    }
}
