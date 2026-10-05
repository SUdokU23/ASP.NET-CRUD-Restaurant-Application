namespace Resto.ViewModels.Reservation;

public sealed class WaiterTableViewModel
{
    public const string StatusFree = "free";
    public const string StatusReserved = "reserved";
    
    public int Id { get; set; }
    
    public int ReservationId { get; set; }

    public string Status => ReservationId > 0 ? StatusReserved : StatusFree;

    public int X { get; set; }

    public int Y { get; set; }
}