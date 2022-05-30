﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyBook.Models;

public class Product
{
    public int Id { get; set; }
    [Required]
    public string title { get; set; }
    public string Description { get; set; } 
    public string ISBN { get; set; }
    [Required]
    public string Author { get; set; }
    [Required]
    [Range(1,10000)]
    public double ListPrice { get; set; }
    [Required]
    [Range(1,10000)]
    public double Price { get; set; }
    [Required]
    [Range(1,10000)]
    public double Price50 { get; set; }
    [Required]
    [Range(1,10000)]
    public double Price100 { get; set; }
    public string ImageUrl { get; set; }
    
    [ForeignKey("CategoryId")]
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    
    public int CoverTypeId { get; set; }
    public CoverType CoverType { get; set; }
    
}