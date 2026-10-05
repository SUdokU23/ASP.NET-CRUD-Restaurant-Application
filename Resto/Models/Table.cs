using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Resto.Models;

public class Table
{
    [Key]
    public int Id { get; set; }
    
    public bool Reserved { get; set; } = false;
    
    [Range(1,6)]
    public int Seats { get; set; }

    [DefaultValue(0)]
    [Range(0, 15)]
    public int DisplayPosX { get; set; }
    
    [DefaultValue(0)]
    [Range(0, 12)]
    public int DisplayPosY { get; set; }
}