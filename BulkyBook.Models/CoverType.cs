using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models;

public class CoverType
{
    [Key]
    public int Id { get; set; }
    [MaxLength(50)]
    [Display(Name = "Cover Type")]
    [Required]
    public string Name { get; set; }
}