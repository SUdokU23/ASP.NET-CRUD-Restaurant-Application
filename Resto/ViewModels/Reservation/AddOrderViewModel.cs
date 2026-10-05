using Resto.Models;

namespace Resto.ViewModels.Reservation;

public sealed class AddOrderViewModel
{
    public int ReservationId { get; set; }
    public int OrderedItem { get; set; }
    public int Quantity { get; set; }

    public string Instructions { get; set; } = "";
}