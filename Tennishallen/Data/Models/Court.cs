using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Tennishallen.Data.Base;
using Tennishallen.Data.Models;

namespace Tennishallen.Data.Models;

public class Court : IBaseEntity<int>
{
    [Key] public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public List<Reservation> Reservations { get; set; }

    internal static void OnModelCreating(ModelBuilder model)
    {
        model.Entity<Court>().HasData(
            new Court()
            {
                Id = 1,
                Name = "Roger Federer",
                Description = "Dit tennisveld is vernoemd naar Roger Federer, een van de meest succesvolle en elegante tennisspelers aller tijden. Federer Court staat bekend om zijn perfecte onderhoud, soepele baanoppervlak en gracieuze uitstraling, passend bij de speelstijl van de legendarische speler zelf.",
                Price = 40,
            });
        model.Entity<Court>().HasData(
            new Court()
            {
                Id = 2,
                Name = "Serena Williams",
                Description = "Dit tennisveld is genoemd naar Serena Williams, een icoon van kracht, vastberadenheid en vrouwelijke dominantie op de tennisbaan. Serena Williams Arena staat bekend om zijn robuuste structuur en uitdagende speelomstandigheden, een eerbetoon aan de onverschrokkenheid van Williams tijdens haar legendarische carrière.",
                Price = 20,
            });
        model.Entity<Court>().HasData(
            new Court()
            {
                Id = 3,
                Name = "Rafael Nadal",
                Description = "Vernoemd naar Rafael Nadal, bekend om zijn ongeëvenaarde vastberadenheid, veerkracht en onverslaanbare prestaties op gravelbanen. Nadal Court biedt een uitdagende ondergrond die spelers dwingt tot uiterste inspanning en doorzettingsvermogen, net zoals Nadal dat altijd heeft laten zien.",
                Price = 1.50,
            });
        model.Entity<Court>().HasData(
            new Court()
            {
                Id = 4,
                Name = "Martina Navrátilová",
                Description =
                    "Genoemd naar Martina Navrátilová, een pionier van de vrouwentenniswereld en een symbool van technische perfectie en ongekende atletische vaardigheid. Deze tennisbaan staat bekend om zijn vlekkeloze grasoppervlak en bevordert het ontwikkelen van een verfijnde, strategische speelstijl, geïnspireerd door de legendarische speler zelf.",
                Price = 12.50,
            });
    }
}
