using System.ComponentModel.DataAnnotations;

namespace Tennishallen.Models.Auth;

public class LoginViewModel
{
    [Display(Name = "Email Adres")]
    [Required(ErrorMessage = "Email mag niet leeg zijn.")]
    [MaxLength(255, ErrorMessage = "Email is maximaal 255 karakters lang")]
    [EmailAddress(ErrorMessage = "Dit is geen geldig email address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Wachtwoord mag niet leeg zijn.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}