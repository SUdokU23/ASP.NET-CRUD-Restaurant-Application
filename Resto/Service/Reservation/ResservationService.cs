using System.Security.Cryptography.Xml;
using Microsoft.EntityFrameworkCore;
using Resto.Data;
using Resto.Models;

namespace Resto.Service.Reservation;

public class ReservationService
{
    private readonly AppDbContext _context;

    public ReservationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Table>> GetAllTables()
    {
        return await _context.Tables.ToListAsync();
    }
    
    public List<Table> FilterByAvailability(List<Table> model)
    {
        return model.Where(t => t.Reserved == false).ToList();
    }
    
    public List<Table> FilerByNumberOfSeats(List<Table> model, int numberOfSeats)
    {
        return model.Where(t => t.Seats >= numberOfSeats).ToList();
    }

    public async Task<Table?> GetTableById(int id)
    {
        return await _context.Tables.FindAsync(id);
    }

    public async Task<List<Models.Reservation?>> GetReservationByTableId(int tableId)
    {
        return await _context.Reservations.Where(r => r.TableId == tableId).ToListAsync();
    }
    
    public async Task<Models.Reservation?> GetReservation(int id)
    {
        return await _context.Reservations
            .AsNoTracking()
            .Include(x => x.Orders)
            .ThenInclude(x => x.OrderedItem)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }
    
    public async Task<bool> CreateTable(Table model)
    {
        _context.Tables.Add(model);
        
        return await _context.SaveChangesAsync() > 0;
    }
    
    public async Task<bool> EditTable(int id, int seats, bool reserved)
    {
        var prev = await _context.Tables.FindAsync(id);

        if (prev == null) return false;
        
        prev.Seats = seats;
        prev.Reserved = reserved;

        _context.Tables.Update(prev);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteTable(int id)
    {
        var tobeDeleted = await _context.Tables.FindAsync(id);
        
        if (tobeDeleted == null)
        {
            return false;
        }
        
        _context.Tables.Remove(tobeDeleted);
        
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> CreateReservation(Models.Reservation reservation)
    {
        if(reservation == null) return false;
        
        _context.Reservations.Add(reservation);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<List<Models.Reservation>> GetAllCurrentReservations()
    {
        DateTime now = DateTime.UtcNow;
        List<Models.Reservation> allReservations = await _context.Reservations.AsNoTracking().ToListAsync();

        return allReservations.Where(x => x.ReservedFrom <= now && now <= x.ReservedTo).ToList();
    }

    public async Task OrderItemToReservation(int reservationId, int itemId, int quantity, string instructions)
    {
        Models.Reservation? res = await _context.Reservations
            .Include(x => x.Orders)
            .Where(x => x.Id == reservationId)
            .FirstOrDefaultAsync();
        if (res == null)
        {
            return;
        }
        
        res.Orders.Add(new OrderItem
        {
            OrderedItemId = itemId,
            Quantity = quantity,
            Instructions = instructions
        });

        await _context.SaveChangesAsync();
    }
}