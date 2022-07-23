using System.Security.Claims;
using BulkyBook.Data.IRepository;
using BulkyBook.Data.Repository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace BulkyBook.Controllers;

[Area("Customer")]
[Authorize]
public class CartController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    [BindProperty] public ShoppingCartVM ShoppingCartVm { get; set; }


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
            ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,
                includeProperties: "Product"),
            OrderHeader = new OrderHeader(),
        };
        foreach (var cart in ShoppingCartVm.ListCart)
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
            ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,
                includeProperties: "Product"),
            OrderHeader = new OrderHeader(),
        };
        ShoppingCartVm.OrderHeader.ApplicationUser =
            _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);
        ShoppingCartVm.OrderHeader.PhoneNumber = ShoppingCartVm.OrderHeader.ApplicationUser.PhoneNumber;
        ShoppingCartVm.OrderHeader.Name = ShoppingCartVm.OrderHeader.ApplicationUser.Name;
        ShoppingCartVm.OrderHeader.StreetAdress = ShoppingCartVm.OrderHeader.ApplicationUser.StreetAdress;
        ShoppingCartVm.OrderHeader.City = ShoppingCartVm.OrderHeader.ApplicationUser.City;
        ShoppingCartVm.OrderHeader.State = ShoppingCartVm.OrderHeader.ApplicationUser.State;
        ShoppingCartVm.OrderHeader.PostalCode = ShoppingCartVm.OrderHeader.ApplicationUser.PostalCode;

        foreach (var cart in ShoppingCartVm.ListCart)
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

        
        ShoppingCartVm.OrderHeader.OrderDate = System.DateTime.Now;
        ShoppingCartVm.OrderHeader.ApplicationUserId = claim.Value;


        foreach (var cart in ShoppingCartVm.ListCart)
        {
            cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50,
                cart.Product.Price100);
            ShoppingCartVm.OrderHeader.OrderTotal += cart.Price * cart.Count;
        } 
        
        ApplicationUser applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);
        if (applicationUser.CompanyId.GetValueOrDefault()==0)
        {
            ShoppingCartVm.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVm.OrderHeader.OrderStatus = SD.StatusPending;
        }
        else
        {
            ShoppingCartVm.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
            ShoppingCartVm.OrderHeader.OrderStatus = SD.StatusApproved;
        }

        _unitOfWork.OrderHeader.Add(ShoppingCartVm.OrderHeader);
        _unitOfWork.Save();

        foreach (var cart in ShoppingCartVm.ListCart)
        {
            OrderDetail orderDetail = new()
            { 
                ProductId = cart.ProductId,
                OrderId = ShoppingCartVm.OrderHeader.Id,
                Price = cart.Price,
                Count = cart.Count,
            };
            _unitOfWork.OrderDetail.Add(orderDetail);
            _unitOfWork.Save();
        }

        

//stripe settings
        if (applicationUser.CompanyId.GetValueOrDefault() == 0)
        {
            var domain = "https://localhost:7010/";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"customer/cart/OderConfirmation?id={ShoppingCartVm.OrderHeader.Id}",
                CancelUrl = domain + $"customer/cart/index",
            };

            foreach (var item in ShoppingCartVm.ListCart)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.title,
                            Description = item.Product.Description
                        }
                    },
                    Quantity = item.Count
                };
                options.LineItems.Add(sessionLineItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);
            _unitOfWork.OrderHeader.UpdateStripePaymentId(ShoppingCartVm.OrderHeader.Id, session.Id,
                session.PaymentIntentId);
            _unitOfWork.Save();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
        else
        {
            return RedirectToAction("OderConfirmation", "Cart" ,new {id=ShoppingCartVm.OrderHeader.Id});
        }
    }

    private double GetPriceBasedOnQuantity(double quantity, double price, double price50, double price100)
    {
        if (quantity <= 50)
        {
            return price;
        }
        else
        {
            if (quantity <= 100)
            {
                return price50;
            }

            return price100;
        }
    }

    public IActionResult OderConfirmation(int id)
    {
        OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);
        if (orderHeader.PaymentStatus!=SD.PaymentStatusDelayedPayment)
        {
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);
            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.OrderHeader.UpdateStatus(id,SD.StatusApproved,SD.PaymentStatusApproved);
                _unitOfWork.Save();
            }
        }
       

        List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
        _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
        _unitOfWork.Save();

        return View(id);
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
        if (cart.Count <= 1)
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