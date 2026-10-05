using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resto.Models;

public class Item
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(40)]
    public string Name { get; set; } = "Undefined";
    
    [StringLength(150)]
    public string Description { get; set; } = "Write item description here!";
    
    [Required]
    [Range(1,2000)]
    public double Weight { get; set; } = 1.0;
    
    public string ImageUrl { get; set; } = "Undefined";
    
    [Required]
    public int CategoryId { get; set; }
    
    [Required]
    public double Price { get; set; }
    
    [Required]
    public bool IsAvailable { get; set; }
    
    public virtual OrderItem? OrderDetail { get; set; }
    
    public virtual Category? Category { get; set; }
    
}