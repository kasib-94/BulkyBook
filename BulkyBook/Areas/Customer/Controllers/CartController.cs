using System.Security.Claims;
using BulkyBook.Data.IRepository;
using BulkyBook.Data.Repository;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Controllers;
[Area("Customer")]
[Authorize]
public class CartController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public  ShoppingCartVM ShoppingCartVm;
    public int OrderTotal;

    public CartController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public IActionResult Index()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        ShoppingCartVm = new ShoppingCartVM()
        {
            ListCart = _unitOfWork.ShoppingCart.GetAll(u=> u.ApplicationUserId == claim.Value,
              includeProperties:"Product"  )
        };
        foreach (var cart  in ShoppingCartVm.ListCart)
        {
            cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50,
                cart.Product.Price100);
        }
        return View(ShoppingCartVm);
    }

    private double GetPriceBasedOnQuantity(double quantity, double price, double price50, double price100)
    {
        if (quantity<=50)
        {
            return price;
        }
        else
        {
            if (quantity<=100)
            {
                return price50;
            }

            return price100;
        }
       
        
    }
}