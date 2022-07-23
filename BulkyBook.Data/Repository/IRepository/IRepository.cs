﻿using System.Linq.Expressions;

namespace BulkyBook.Data.IRepository;

public interface IRepository<T> where T : class
{
    //t - category
    T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true );
    IEnumerable<T> GetAll( Expression<Func<T, bool>>? filter=null ,string? includeProperties = null);
    
    void Add(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entity);

}