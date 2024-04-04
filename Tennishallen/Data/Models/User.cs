using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Tennishallen.Data.Base;
using Tennishallen.Data.Utils;

namespace Tennishallen.Data.Models;

[Index(nameof(Email), IsUnique = true)]
public class User : IBaseEntity<Guid>
{
    [Key] public Guid Id { get; set; }

    public string Email { get; set; }
    public string Phone { get; set; }

    public string Password { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    [NotMapped] public string Fullname => $"{FirstName} {LastName}".Trim(' ');

    public string Address { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public DateOnly Birthdate { get; set; }

    public bool Active { get; set; }
    public double HourlyWage { get; set; }

    public Collection<Reservation> MemberReservations { get; set; } = [];
    public Collection<Reservation> CoachReservations { get; set; } = [];
    public List<Group> Groups { get; set; }

    [NotMapped]
    public List<Reservation> Reservations
    {
        get => [.. MemberReservations ?? [], .. CoachReservations ?? []];
    }


    internal static void OnModelCreating(ModelBuilder model)
    {
        model.Entity<Group>()
            .HasOne(g => g.User)
            .WithMany(u => u.Groups)
            .HasForeignKey(g => g.UserId);


        var guid = Guid.NewGuid();
        model.Entity<User>().HasData(
            new User
            {
                Id = guid,
                FirstName = "Richard",
                LastName = "van Dongen",
                Email = "r.vandongen@tennishallenhengelo.nl",
                Password = PasswordHasher.HashPassword("admin"),
                Birthdate = DateOnly.MinValue,
                Address = "Tennisstraat 1",
                City = "Hengelo",
                PostalCode = "1234TB",
                Phone = "06 12345678",
            });
        model.Entity<Group>().HasData(
            new Group
            {
                Id = 1,
                UserId = guid,
                Name = Group.GroupName.Admin,
            });
        string[][] names =
        [
            ["Beck", "Hand"],
            ["Grant", "Slam"],
            ["Tehn", "Ishbahl"],
            ["Courtney", "Racket"],
        ];
        for (var i = 0; i < names.Length; i++)
        {
            guid = Guid.NewGuid();
            model.Entity<User>().HasData(
                new User
                {
                    Id = guid,
                    FirstName = names[i][0],
                    LastName = names[i][1],
                    Email = $"{names[i][0][0]}.{names[i][1]}@tennishallenhengelo.nl",
                    Password = PasswordHasher.HashPassword("password"),
                    Birthdate = DateOnly.MinValue,
                    Address = $"Tennisstraat {i + 2}",
                    City = "Hengelo",
                    PostalCode = "1234TB",
                    Phone = $"06 {new Random().Next(10_000_000, 99_999_999)}",
                });
            model.Entity<Group>().HasData(
                new Group
                {
                    Id = i + 2,
                    UserId = guid,
                    Name = Group.GroupName.Admin,
                });
        }
    }
}