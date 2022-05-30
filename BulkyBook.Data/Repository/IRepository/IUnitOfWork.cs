namespace BulkyBook.Data.IRepository;

public interface IUnitOfWork
{
    ICategoryRepository Category{ get; }
    ICoverTypeRepository CoverType { get; }
    void Save();
}