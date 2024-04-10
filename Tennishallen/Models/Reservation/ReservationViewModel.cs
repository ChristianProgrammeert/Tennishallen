using System.ComponentModel.DataAnnotations;

namespace Tennishallen.Models.Reservation;

public class ReservationViewModel
{
    
    public int Id { get; set; }

    
    [Display(Name = "Tijd")]
    [Required(ErrorMessage = "Tijd is een verplicht veld")]
    public string Hour { get; set; }
    
    [Display(Name = "Datum")]
    [Required(ErrorMessage = "Datum is een verplicht veld")]
    public string Date { get; set; }
    
    [Display(Name = "Baan")]
    [Required(ErrorMessage = "Baan is een verplicht veld")]
    public double Court { get; set; }

}