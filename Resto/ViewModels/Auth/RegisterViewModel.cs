namespace Resto.ViewModels.Auth;

public class RegisterViewModel
{
    public string Password { get; set; }
    
    public string ConfPass { get; set; }
    
    public bool IsAdmin { get; set; } = false;
}