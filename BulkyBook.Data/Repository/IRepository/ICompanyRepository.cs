using BulkyBook.Models;

namespace BulkyBook.Data.IRepository;

public interface ICompanyRepository : IRepository<Company>
{
    void Update(Company obj);
                    
}