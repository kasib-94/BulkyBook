using BulkyBook.Data.Data;
using BulkyBook.Data.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;

namespace BulkyBook.Data.Repository;

public class ShoppingCartRepository :Repository<ShoppingCart>, IShoppingCartRepository
{
    private ApplicationDbContext _db;
    public ShoppingCartRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }


    public int IncrementCount(ShoppingCart shoppingCart, int count)
    {
        shoppingCart.Count += count;
        return shoppingCart.Count;
    }

    public int DecrementCount(ShoppingCart shoppingCart, int count)
    {
        shoppingCart.Count -= count;
        return shoppingCart.Count;
    }
}