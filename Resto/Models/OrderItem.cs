using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resto.Models;

public class OrderItem
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int OrderedItemId { get; set; }

    public Item OrderedItem { get; set; } = null!;

    [Required]
    public int Quantity { get; set; }
    
    // TEXT
    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    public string Instructions { get; set; } = "";
}