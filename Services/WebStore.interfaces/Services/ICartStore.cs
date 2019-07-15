using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Models;

namespace WebStore.interfaces.Services
{
    /// <summary> 
    /// Отвечает за хранение данных корзины.
    /// В источнике данных Cookies.
    /// </summary>
    public interface ICartStore
    {
        Cart Cart { get; set; }
    }
}
