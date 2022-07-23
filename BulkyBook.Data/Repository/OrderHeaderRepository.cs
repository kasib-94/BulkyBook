using BulkyBook.Data.Data;
using BulkyBook.Data.IRepository;
using BulkyBook.Models;

namespace BulkyBook.Data.Repository;

public class OrderHeaderRepository :Repository<OrderHeader>, IOrderHeaderRepository
{
    private ApplicationDbContext _db;
    public OrderHeaderRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(OrderHeader obj)
    {
        _db.OrderHeaders.Update(obj);
    }

    public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
    {
        var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
        if (orderFromDb != null)
        {
            orderFromDb.OrderStatus = orderStatus;
            if (paymentStatus!=null)
            {
                orderFromDb.PaymentStatus = paymentStatus;
            }
        }
    }

    
    public void UpdateStripePaymentId(int id, string sessionId, string? PaymentIntentId )
    {
        var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
        orderFromDb.PaymentDate=DateTime.Now;
        orderFromDb.SessionId = sessionId;
        orderFromDb.PaymentIntentId = PaymentIntentId;
    }
    
    public void Save()
    {
        _db.SaveChanges();
    }
}