using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    /// <summary>Бренд</summary>
    public class Brand : NamedEntity, IOrderedEntity
    {
        /// <summary>Порядок</summary>
        public int Order { get; set; }
    }
}
