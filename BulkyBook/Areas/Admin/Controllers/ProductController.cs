using BulkyBook.Data.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBook.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    // GET
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHost;

    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHost)
    {
        _unitOfWork = unitOfWork;
        _webHost = webHost;
    }

    public IActionResult Index()
    {
        return View();
    }


    public IActionResult Upsert(int? id)
    {
        ProductViewModel ProductVM = new()
        {
            Product = new(),
            CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),
            CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            })
        };


        if (id == null || id == 0)
        {
            return View(ProductVM);
        }
        else
        {
            ProductVM.Product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
            return View(ProductVM);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(ProductViewModel obj, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            string wwwRootPath = _webHost.WebRootPath;
            if (file != null)
            {
                // HOW TO COPY FILE IN C###

                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"img\Products");
                var extension = Path.GetExtension(file.FileName);

                if (obj.Product.ImageUrl != null)
                {
                    var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists((oldImagePath)))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }

                obj.Product.ImageUrl = @"\img\Products\" + fileName + extension;
            }

            if (obj.Product.Id == 0)
            {
                _unitOfWork.Product.Add(obj.Product);
            }
            else
            {
                _unitOfWork.Product.Update(obj.Product);
            }

            _unitOfWork.Save();
            TempData["success"] = "Product added successfully";
            return RedirectToAction("Index");
        }

        return View(obj);
    }


    #region API CALLS

    [HttpGet]
    public IActionResult GetAll()
    {
        var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
        return Json(new { data = productList });
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int? id)
    {
        var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
        if (obj == null)
        {
            return Json(new { success = false, message = "Error while deleting" });
        }

        var oldImagePath = Path.Combine(_webHost.WebRootPath, obj.ImageUrl.TrimStart('\\'));
        if (System.IO.File.Exists((oldImagePath)))
        {
            System.IO.File.Delete(oldImagePath);
        }

        _unitOfWork.Product.Remove(obj);
        _unitOfWork.Product.Save();
        return Json(new { success = false, message = "Deleted successfully " });
    }

    #endregion
}