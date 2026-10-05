using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resto.Models;

public sealed class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string HashPwd { get; set; } = "";
    
    [Required]
    public bool IsAdministrator { get; set; }
}