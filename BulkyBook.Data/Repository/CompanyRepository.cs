using BulkyBook.Data.Data;
using BulkyBook.Data.Repository;
using BulkyBook.Models;

namespace BulkyBook.Data.IRepository;

public class CompanyRepository : Repository<Company> ,ICompanyRepository
{
    private ApplicationDbContext _db;
    public CompanyRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(Company obj)
    {
        _db.Companies.Update(obj);
    }

    public void Save()
    {
        _db.SaveChanges();
    }
}