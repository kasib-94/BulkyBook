using BulkyBook.Models;

namespace BulkyBook.Data.IRepository;

public interface ICategoryRepository : IRepository<Category>
{
    void Update(Category obj);
    void Save();
}