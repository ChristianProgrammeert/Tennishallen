using Microsoft.EntityFrameworkCore;
using Tennishallen.Data.Models;

namespace Tennishallen.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Court> Courts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        Reservation.OnModelCreating(modelBuilder);
    }
}