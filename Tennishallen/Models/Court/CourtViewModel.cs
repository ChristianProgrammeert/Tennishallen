using System.ComponentModel.DataAnnotations;

namespace Tennishallen.Models.Court;

public class CourtViewModel
{
    public int Id { get; set; }


    [Display(Name = "Naam")]
    [Required(ErrorMessage = "Naam is een verplicht veld")]
    public string Name { get; set; }

    [Display(Name = "Beschrijving")]
    [Required(ErrorMessage = "Beschrijving is een verplicht veld")]
    public string Description { get; set; }

    [Display(Name = "Prijs")]
    [Required(ErrorMessage = "Prijs is een verplicht veld")]
    [Range(0, double.MaxValue, ErrorMessage = "Prijs moet boven 0 liggen")]
    public double Price { get; set; }
}