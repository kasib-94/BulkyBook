using System.Linq.Expressions;
using BulkyBook.Data.Data;
using BulkyBook.Data.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.Data.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _db;
    internal DbSet<T> dbSet;

    public Repository(ApplicationDbContext db)
    {
        _db = db;
     //  _db.Products.Include(u => u.Category).Include(c=>c.CoverType );
        this.dbSet = _db.Set<T>();
    }

    public void Add(T entity)
    {
        dbSet.Add(entity);
    }

    public IEnumerable<T> GetAll( string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;
        if (includeProperties != null)
        {
            foreach (var includeProp in includeProperties.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries))
            {
               query = query.Include(includeProp);
            }
        }
        return query.ToList();
    }

    public T GetFirstOrDefault(Expression<Func<T, bool>> filter,  string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;
        if (includeProperties != null)
        {
            foreach (var includeProp in includeProperties.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }
        query = query.Where(filter);
        return query.FirstOrDefault();
    }

    public void Remove(T entity)
    {
        dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entity)
    {
        dbSet.RemoveRange(entity);
    }
}