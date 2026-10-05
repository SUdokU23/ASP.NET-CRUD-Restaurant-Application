using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Resto.Models;

namespace Resto.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext()
    {
        
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>()
            .HasMany<OrderItem>()
            .WithOne(x => x.OrderedItem);
        
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string dbConnStr = Env.GetString("SQL_CONN");
        optionsBuilder.UseNpgsql(dbConnStr);
        base.OnConfiguring(optionsBuilder);
    }
}