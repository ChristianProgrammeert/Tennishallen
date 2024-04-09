using System.ComponentModel.DataAnnotations;

namespace Tennishallen.Models.Auth
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Oud wachtwoord mag niet leeg zijn")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Nieuw wachtwoord mag niet leeg zijn")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Wachtwoord en wachtwoord confirmatie zijn niet identiek")]
        public string ConfirmPassword { get; set; }
    }
}