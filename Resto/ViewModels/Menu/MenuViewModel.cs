using Resto.Models;

namespace Resto.ViewModels.Menu;

public class MenuViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public string Category { get; set; }
    
    public double Weight { get; set; }
    
    public string Description { get; set; }
    
    public double Price { get; set; }
    
    public string ImageUrl { get; set; }
}