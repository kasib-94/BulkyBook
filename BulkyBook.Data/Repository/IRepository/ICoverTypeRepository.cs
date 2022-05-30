using BulkyBook.Models;

namespace BulkyBook.Data.IRepository;

public interface ICoverTypeRepository : IRepository<CoverType>
{
    void Update(CoverType obj);
    void Save();
}