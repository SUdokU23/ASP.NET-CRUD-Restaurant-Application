using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resto.Models;

public class Reservation
{
    [Key]
    public int Id { get; set; }

    [Required] 
    [StringLength(40)]
    public string Booker { get; set; } = "Undefined";
    
    [Required]
    public DateTime ReservedFrom { get; set; } = DateTime.Now;
    
    [Required]
    public DateTime ReservedTo { get; set; } = DateTime.Now + TimeSpan.FromHours(2);
    
    
    public int TableId { get; set; }
    
    [ForeignKey("TableId")]
    public virtual Table Table { get; set; }
    public ICollection<OrderItem> Orders { get; set; }
}