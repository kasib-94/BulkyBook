using BulkyBook.Models;
using BulkyBook.Models.ViewModels;

namespace BulkyBook.Data.IRepository;

public interface IShoppingCartRepository : IRepository<ShoppingCart>
{
   int IncrementCount(ShoppingCart shoppingCart, int count);
   int DecrementCount(ShoppingCart shoppingCart, int count);
}