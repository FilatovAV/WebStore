using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    /// <summary>Бренд</summary>
    [Table("Brands")]
    public class Brand : NamedEntity, IOrderedEntity
    {
        /// <summary>Порядок</summary>
        public int Order { get; set; }

        /// <summary>virtual - позволяет entity framework сделать данное свойство навигационным/// </summary>
        public virtual ICollection<Product> Product { get; set; } = new List<Product>();
    }
}
