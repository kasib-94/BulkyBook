﻿using BulkyBook.Data.Data;
using BulkyBook.Data.IRepository;
using BulkyBook.Models;

namespace BulkyBook.Data.Repository;

public class CategoryRepository :Repository<Category>, ICategoryRepository
{
    private ApplicationDbContext _db;
    public CategoryRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(Category obj)
    {
        _db.Categories.Update(obj);
    }

    public void Save()
    {
        _db.SaveChanges();
    }
}