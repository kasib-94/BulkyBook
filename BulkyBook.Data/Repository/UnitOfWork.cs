using BulkyBook.Data.Data;
using BulkyBook.Data.IRepository;

namespace BulkyBook.Data.Repository;

public class UnitOfWork: IUnitOfWork
{
    private readonly ApplicationDbContext _db;
    
    public UnitOfWork(ApplicationDbContext db) 
    {
        _db = db;
        Category = new CategoryRepository(_db);
    }
    public ICategoryRepository Category { get; private set; }

    public void Save()
    {
        throw new NotImplementedException();
    }
}