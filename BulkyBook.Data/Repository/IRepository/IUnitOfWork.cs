namespace BulkyBook.Data.IRepository;

public interface IUnitOfWork
{
    ICategoryRepository Category{ get; }
    void Save();
}