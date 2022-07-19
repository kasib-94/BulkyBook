using System.Security.Claims;
using BulkyBook.Data.IRepository;
using BulkyBook.Data.Repository;
using BulkyBook.Models;
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
              includeProperties:"Product"  ),
            OrderHeader = new OrderHeader(),
        };
        foreach (var cart  in ShoppingCartVm.ListCart)
        {
            cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50,
                cart.Product.Price100);
            ShoppingCartVm.OrderHeader.OrderTotal += cart.Price * cart.Count;
        }
        return View(ShoppingCartVm);
    }
    
    public IActionResult Summary()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        ShoppingCartVm = new ShoppingCartVM()
        {
            ListCart = _unitOfWork.ShoppingCart.GetAll(u=> u.ApplicationUserId == claim.Value,
                includeProperties:"Product"  ),
            OrderHeader = new OrderHeader(),
        };
       ShoppingCartVm.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);
       ShoppingCartVm.OrderHeader.PhoneNumber = ShoppingCartVm.OrderHeader.ApplicationUser.PhoneNumber;
       ShoppingCartVm.OrderHeader.Name = ShoppingCartVm.OrderHeader.ApplicationUser.Name;
       ShoppingCartVm.OrderHeader.StreetAdress = ShoppingCartVm.OrderHeader.ApplicationUser.StreetAdress;
       ShoppingCartVm.OrderHeader.City = ShoppingCartVm.OrderHeader.ApplicationUser.City;
       ShoppingCartVm.OrderHeader.State = ShoppingCartVm.OrderHeader.ApplicationUser.State;
       ShoppingCartVm.OrderHeader.PostalCode = ShoppingCartVm.OrderHeader.ApplicationUser.PostalCode;
        
        foreach (var cart  in ShoppingCartVm.ListCart)
        {
            cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50,
                cart.Product.Price100);
            ShoppingCartVm.OrderHeader.OrderTotal += cart.Price * cart.Count;
        }
        return View(ShoppingCartVm);


       
    }
    [HttpPost]
    [ActionName("Summary")]
    [ValidateAntiForgeryToken]
    public IActionResult SummaryPOST()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        ShoppingCartVm.ListCart =
            _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Product");
       
       
        
        foreach (var cart  in ShoppingCartVm.ListCart)
        {
            cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50,
                cart.Product.Price100);
            ShoppingCartVm.OrderHeader.OrderTotal += cart.Price * cart.Count;
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

    public IActionResult plus(int cartId)
    {
        var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
        _unitOfWork.ShoppingCart.IncrementCount(cart, 1);
        _unitOfWork.Save();
        
        return RedirectToAction(nameof(Index));

    }
    public IActionResult minus(int cartId)
    {
        
        var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
        if (cart.Count<=1)
        {
            _unitOfWork.ShoppingCart.Remove(cart);
        }
        else
        {
            _unitOfWork.ShoppingCart.DecrementCount(cart, 1);
        }
        
        _unitOfWork.Save();
        
        return RedirectToAction(nameof(Index));

    }
    public IActionResult remove(int cartId)
    {
        var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
        _unitOfWork.ShoppingCart.Remove(cart);
        _unitOfWork.Save();
        
        return RedirectToAction(nameof(Index));

    }
}