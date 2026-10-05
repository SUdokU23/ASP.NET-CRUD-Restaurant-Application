using System.ComponentModel.DataAnnotations;
using Resto.Models;

namespace Resto.ViewModels.Menu;

public class AddItemViewModel
{
    public Item Item { get; set; } = new Item();
    
    [Required]
    public string CategoryName { get; set; }
    
    public IFormFile image { get; set; }
}