using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resto.Models;
using Resto.Service.Reservation;
using Resto.ViewModels.Reservation;

namespace Resto.Controllers.Waiters;

public sealed class WaiterController : Controller
{
    private ReservationService _reservationService;

    public WaiterController(ReservationService reservationService)
    {
        _reservationService = reservationService;
    }
    
    
    [Authorize]
    public async Task<IActionResult> Index()
    {
        List<Table> tables = await _reservationService.GetAllTables();
        List<Models.Reservation> currentReservations = await _reservationService.GetAllCurrentReservations();

        IEnumerable<WaiterTableViewModel> data = tables.Select(tbl => new WaiterTableViewModel
        {
            Id = tbl.Id,
            ReservationId = currentReservations.FirstOrDefault(x => x.TableId == tbl.Id)?.Id ?? -1,
            X = tbl.DisplayPosX,
            Y = tbl.DisplayPosY
        });
        
        return View(data);
    }
}