﻿using BulkyBook.Data.Data;
using BulkyBook.Data.IRepository;
using BulkyBook.Models;

namespace BulkyBook.Data.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        Category = new CategoryRepository(_db);
        CoverType = new CoverTypeRepository(_db);
        Product = new ProductRepository(_db);
        Company = new CompanyRepository(_db);
        ApplicationUser = new ApplicationUserRepository(_db);
        ShoppingCart = new ShoppingCartRepository(_db);
        OrderHeader = new OrderHeaderRepository(_db);
        OrderDetail = new OrderDetailRepository(_db);
    }

    public ICategoryRepository Category { get; private set; }
    public ICoverTypeRepository CoverType { get; private set; }

    public IProductRepository Product { get; private set; }
    public ICompanyRepository Company { get; }
    public IShoppingCartRepository ShoppingCart { get; }
    public IApplicationUserRepository ApplicationUser { get; }

    public IOrderHeaderRepository OrderHeader { get; }
    
    public IOrderDetailRepository OrderDetail { get; }


    public void Save()
    {
        _db.SaveChanges();
    }
}