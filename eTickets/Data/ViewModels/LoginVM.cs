using System.ComponentModel.DataAnnotations;

namespace eTickets.Data.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Email address is required"), Display(Name = "Email address")]
        public string EmailAddress { get; set; } /**/
        [Required(ErrorMessage = "Password required"), DataType(DataType.Password), Display(Name = "Password"), RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,15}$", ErrorMessage = "Must be a strong password")]
        public string Password { get; set; }
    }
}
