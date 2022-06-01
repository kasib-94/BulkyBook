using Microsoft.EntityFrameworkCore;
using BulkyBook.Models;

namespace BulkyBook.Data.Data;

public class ApplicationDbContext : DbContext
{

    public DbSet<Category>? Categories { get; set; }  
    public DbSet<CoverType>? CoverTypes { get; set; }
    public DbSet<Product> Products { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
            
    }

    
}