using System.ComponentModel.DataAnnotations;

namespace Resto.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } = "Undefined";
    
    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}