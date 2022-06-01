using BulkyBook.Models;

namespace BulkyBook.Data.IRepository;

public interface IProductRepository:IRepository<Product>
{
    void Update(Product obj);
    void Save();
}