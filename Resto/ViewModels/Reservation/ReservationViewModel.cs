namespace Resto.ViewModels.Reservation;

public class ReservationViewModel
{ 
    public DateTime LastReservationDate { get; set; }
    
    public int TableId { get; set; }
    
    public Models.Reservation Reservation { get; set; } = new Models.Reservation();

    public IEnumerable<Models.Reservation> FutureReservations { get; set; } = [];
}