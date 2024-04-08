using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Tennishallen.Data.Utils;

namespace Tennishallen.Models.User;

public class UserViewModel
{
    [Required(ErrorMessage = "Dit veld is verplicht")]
    [EmailAddress(ErrorMessage = "Dit is geen valide email")]
    [DisplayName("Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Dit veld is verplicht")]
    [Phone(ErrorMessage = "Dit is geen valide telefoon nummer")]
    [DisplayName("Telefoon nummer")]

    public string Phone { get; set; }


    [DataType(DataType.Password)]
    [Display(Name = "Wachtwoord")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Voornaam is verplicht")]
    [Display(Name = "Voornaam")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Achternaam is verplicht")]
    [Display(Name = "Achternaam")]
    public string LastName { get; set; }

    [Display(Name = "Adres")]
    [Required(ErrorMessage = "Adres is verplicht")]
    public string Address { get; set; }

    [Required(ErrorMessage = "Postcode is verplicht")]
    [RegularExpression(@"^\d{4}[A-Z]{2}$", ErrorMessage = "Ongeldige postcode")]
    [Display(Name = "Postcode")]
    public string PostalCode { get; set; }

    [Required(ErrorMessage = "Stad is verplicht")]
    [Display(Name = "Stad")]
    public string City { get; set; }

    [Required(ErrorMessage = "Geboortedatum is verplicht")]
    [DataType(DataType.Date)]
    [DateBeforeToday(ErrorMessage = "Geboortedatum moet voor vandaag zijn")]
    [Display(Name = "Geboortedatum")]
    public DateTime Birthdate { get; set; }

    [Display(Name = "Admin")] public bool IsAdmin { get; set; }
    [Display(Name = "Coach")] public bool IsCoach { get; set; }
    [Display(Name = "Lid")] public bool IsMember { get; set; }
    [Display(Name = "Actief")] public bool IsActive { get; set; }

    [Required(ErrorMessage = "Uurloon moet gezet zijn")]
    [Range(0, double.MaxValue, ErrorMessage = "Uurloon moet boven 0 liggen")]
    [Display(Name = "Loon per uur in euro")]
    public double HourlyWage { get; set; }
}