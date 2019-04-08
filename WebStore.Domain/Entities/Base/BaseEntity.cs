using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.Domain.Entities.Base
{
    /// <summary>Сущность</summary>
    public abstract class BaseEntity : Interfaces.IBaseEntity
    {
        public int Id { get; set; }
    }
}
