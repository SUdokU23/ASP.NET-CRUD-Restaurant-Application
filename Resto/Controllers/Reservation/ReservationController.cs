using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resto.Models;
using Resto.Service.Reservation;
using Resto.ViewModels.Reservation;


namespace Resto.Controllers.Reservation;

public class ReservationController(ReservationService _reservationService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = await _reservationService.GetAllTables();
        
        
        ViewBag.NumberOfSeats = 0;
        ViewBag.isAvailable = false;
        
        return View(model);
    }   
    
    [HttpPost]
    public async Task<IActionResult> Index(bool isAvailable, int numberOfSeats)
    {
        var model = await _reservationService.GetAllTables();

        if (numberOfSeats != 0)
        {
            model = _reservationService.FilerByNumberOfSeats(model, numberOfSeats);
        }
        
        if (isAvailable)
        {
            model = _reservationService.FilterByAvailability(model);
        }
        
        ViewBag.NumberOfSeats = numberOfSeats;
        ViewBag.isAvailable = isAvailable;
        
        return View(model);
    }
    
    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult AddTable()
    {
        var model = new Table();
        return View(model);
    }
    
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> AddTable(Table model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Incorrect data";
            
            return View(model);
        }

        if (await _reservationService.CreateTable(model))
        {
            return RedirectToAction("Index");
        }
        
        TempData["Error"] = "Something went wrong";
        
        return View(model);
    }
    
    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> EditTable(int id)
    {
        var model = await  _reservationService.GetTableById(id);
        return View(model);
    }
    
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> EditTable(int id, int seats, bool reserved)
    {

        if (await _reservationService.EditTable(id, seats, reserved))
        {
            return RedirectToAction("Index");
        }
        
        TempData["Error"] = "Something went wrong";
        
        return RedirectToAction("Index");
    }
    
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteTable(int tableId)
    {
        if (tableId <= 0)
        {
            TempData["Error"] = "Could not find table";
            return RedirectToAction("Index");
        }

        if (await _reservationService.DeleteTable(tableId))
        {
            return RedirectToAction("Index");
        }
        
        TempData["Error"] = "Something went wrong";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public async Task<IActionResult> ReserveTable(int id)
    {
        DateTime now = DateTime.UtcNow;
        IEnumerable<Models.Reservation> futureReservations = (await _reservationService.GetReservationByTableId(id))
            .Where(x => x != null)
            .Select(x => x!)
            .Where(x => x.ReservedFrom >= now || x.ReservedTo >= now)
            ;
        
        var model = new ReservationViewModel
        {
            TableId = id,
            FutureReservations = futureReservations
        };
        
        if (model == null)
        {
            TempData["Error"] = "Could not find table";
            return RedirectToAction("Index");
        }
        
        var table = await _reservationService.GetTableById(id);
        
        ViewBag.IsReserved = table.Reserved;
        
        return View(model);
    }
    
    
    [HttpPost]
    public async Task<IActionResult> ReserveTable(ReservationViewModel model)
    {
        if (model == null)
        {
            TempData["Error"] = "Could not find table";
            
            return View(model);
        }

        model.Reservation.ReservedFrom = model.Reservation.ReservedFrom.ToUniversalTime();
        model.Reservation.ReservedTo = model.Reservation.ReservedTo.ToUniversalTime();

        if (model.Reservation.ReservedFrom > model.Reservation.ReservedTo)
        {
            TempData["Error"] = "Reservation must start before it ends";
            
            return View(model);
        }
        
        model.Reservation.TableId = model.TableId;

        if (await _reservationService.CreateReservation(model.Reservation))
        {
            return RedirectToAction("Index");
        }
        
        TempData["Error"] = "Something went wrong";
        return View(model);
    }
    
    
    [Authorize]
    public async Task<IActionResult> ManageReservation(int id)
    {
        Models.Reservation? reservation = await _reservationService.GetReservation(id);
        if (reservation == null)
        {
            // FUCK YOU!!!!!!!!!!!
            return BadRequest();
        }

        return View(reservation);
    }
    
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> AddOrder(int reservationId)
    {
        return View(new AddOrderViewModel { ReservationId = reservationId });
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddOrder(AddOrderViewModel model)
    {
        await _reservationService.OrderItemToReservation(model.ReservationId, model.OrderedItem, model.Quantity, model.Instructions);
        return RedirectToAction("ManageReservation", new { id = model.ReservationId });
    }

}