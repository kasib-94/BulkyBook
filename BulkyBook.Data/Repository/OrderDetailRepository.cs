using BulkyBook.Data.Data;
using BulkyBook.Data.IRepository;
using BulkyBook.Models;

namespace BulkyBook.Data.Repository;

public class OrderDetailRepository :Repository<OrderDetail>, IOrderDetailRepository
{
    private ApplicationDbContext _db;
    public OrderDetailRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(OrderDetail obj)
    {
        _db.OrderDetail.Update(obj);
    }

    public void Save()
    {
        _db.SaveChanges();
    }
}