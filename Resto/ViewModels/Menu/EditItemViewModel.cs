using System.ComponentModel.DataAnnotations;
using Resto.Models;

namespace Resto.ViewModels.Menu;

public class EditItemViewModel
{
    public Item Item { get; set; }
    
    [Required]
    public string CategoryName { get; set; }

    public IFormFile? image { get; set; } = null;
}