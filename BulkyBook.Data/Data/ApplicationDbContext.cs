using Microsoft.EntityFrameworkCore;
using BulkyBook.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BulkyBook.Data.Data;

public class ApplicationDbContext : IdentityDbContext
{

    public DbSet<Category>? Categories { get; set; }  
    public DbSet<CoverType>? CoverTypes { get; set; }
    public DbSet<Product> Products { get; set; }
    
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Company> Companies { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
            
    }

    
}