using BulkyBook.Data.Data;
using BulkyBook.Data.Repository;
using BulkyBook.Models;

namespace BulkyBook.Data.IRepository;

public class CoverTypeRepository : Repository<CoverType> ,ICoverTypeRepository
{
    private ApplicationDbContext _db;
    public CoverTypeRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(CoverType obj)
    {
        _db.CoverTypes.Update(obj);
    }

    public void Save()
    {
        _db.SaveChanges();
    }
}