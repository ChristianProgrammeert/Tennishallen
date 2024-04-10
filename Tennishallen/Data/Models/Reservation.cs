using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Tennishallen.Data.Base;
using Tennishallen.Data.Models;

namespace Tennishallen.Data.Models;

public class Reservation : IBaseEntity<int>
{
    [Key] public int Id { get; set; }
    public Guid MemberId { get; set; }
    public User Member{ get; set; }
    public Guid? CoachId { get; set; }
    public User? Coach { get; set; }
    public int CourtId { get; set; }
    public Court Court { get; set; }
    public double Price { get; set; }
    public DateOnly Date { get; set; }
    public int Hour { get; set; }

    
    /// <summary>
    /// Set the relations of Resrvation
    /// </summary>
    /// <param name="model">the model to create the raltions on</param>
    internal static void OnModelCreating(ModelBuilder model)
    {
        model.Entity<Reservation>()
            .HasOne(r => r.Member)
            .WithMany(u => u.MemberReservations)
            .HasForeignKey(a => a.MemberId)
            .OnDelete(DeleteBehavior.NoAction);

        model.Entity<Reservation>()
            .HasOne(r => r.Coach)
            .WithMany(u => u.CoachReservations)
            .HasForeignKey(a => a.CoachId)
            .OnDelete(DeleteBehavior.NoAction);
        
        model.Entity<Reservation>()
            .HasOne(r => r.Court)
            .WithMany(u => u.Reservations)
            .HasForeignKey(a => a.CourtId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
