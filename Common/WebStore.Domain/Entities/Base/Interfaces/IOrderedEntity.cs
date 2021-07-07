namespace WebStore.Domain.Entities.Base.Interfaces
{
    /// <summary>Упорядоченная сущность</summary>
    public interface IOrderedEntity
    {
        /// <summary>Порядок</summary>
        int Order { get; set; }
    }
}
