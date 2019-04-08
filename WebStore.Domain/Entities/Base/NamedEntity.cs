namespace WebStore.Domain.Entities.Base
{
    /// <summary>Именованая сущность</summary>
    public abstract class NamedEntity: BaseEntity, Interfaces.INamedEntity
    {
        public string Name { get; set; }
    }
}
