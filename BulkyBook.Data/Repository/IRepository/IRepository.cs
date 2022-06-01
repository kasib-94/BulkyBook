﻿using System.Linq.Expressions;

namespace BulkyBook.Data.IRepository;

public interface IRepository<T> where T : class
{
    //t - category
    T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
    IEnumerable<T> GetAll( string? includeProperties = null);
    void Add(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entity);

}