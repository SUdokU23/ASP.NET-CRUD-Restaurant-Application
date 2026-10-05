using Resto.Models;

namespace Resto.Extensions;

public static class ReservationExtensions
{
    public static double GetTotalPrice(this Reservation res)
    {
        return res.Orders.Sum(item => item.Quantity * item.OrderedItem.Price);
    }
}