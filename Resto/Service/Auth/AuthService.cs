using Microsoft.EntityFrameworkCore;
using Resto.Data;
using Resto.Models;

namespace Resto.Service;

public class AuthService
{
    private readonly AppDbContext _context;

    public AuthService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddUser(User user)
    {
        await _context.Users.AddAsync(user);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> IsUserValid(int id)
    {
        if (await _context.Users.FindAsync(id) != null)
        {
            return true;
        }
        return false;
    }

    public async Task<bool> IsPasswordCorrect(int id, string password)
    {
        var user = await _context.Users.FindAsync(id);
        return BCrypt.Net.BCrypt.Verify(password, user!.HashPwd);
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _context.Users.AsNoTracking().ToListAsync();
    }

    public async Task<string> GetRole(int Id)
    {
        var user = await _context.Users.FindAsync(Id);
        if (user!.IsAdministrator)
        {
            return "Administrator";
        }
        return "User";
    }
}