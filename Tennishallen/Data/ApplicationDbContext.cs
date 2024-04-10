using Microsoft.EntityFrameworkCore;
using Tennishallen.Data.Models;

namespace Tennishallen.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Court> Courts { get; set; }

    
    /// <summary>
    /// create the model for each model
    /// </summary>
    /// <param name="modelBuilder">the model to create</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        User.OnModelCreating(modelBuilder);
        Reservation.OnModelCreating(modelBuilder);  
        Court.OnModelCreating(modelBuilder);
    }
}