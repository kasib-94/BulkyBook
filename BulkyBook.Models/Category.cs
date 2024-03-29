﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Range(1,100,ErrorMessage = "Display Order must be between 1 and 100")]
    [DisplayName("Display Order")]
    public int DisplayOrder { get; set; }
    public DateTime CreatedDateTime { get; set; } = DateTime.Now;
}