using BulkyBook.Models;

namespace BulkyBook.Data.IRepository;

public interface IOrderDetailRepository : IRepository<OrderDetail>
{
    void Update(OrderDetail obj);
    void Save();
}