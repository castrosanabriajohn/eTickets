using System.ComponentModel.DataAnnotations;

namespace eTickets.Data.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Full name is required."), Display(Name = "Full name")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Email address is required."), Display(Name = "Email address"), RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "Must be a valid email.")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Password is required."), DataType(DataType.Password), Display(Name = "Password"), RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,15}$", ErrorMessage = "Pasword must contain at least one number, one uppercase letter, one lowercase letter, and one special character.")]
        public string Password { get; set; }
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match."), Required(ErrorMessage = "Confirm is password required"), DataType(DataType.Password), Display(Name = "Confirm Password"), RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,15}$", ErrorMessage = "Pasword must contain at least one number, one uppercase letter, one lowercase letter, and one special character.")]
        public string ConfirmPassword { get; set; }
    }
}
