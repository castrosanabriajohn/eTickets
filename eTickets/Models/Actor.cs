using eTickets.Data.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eTickets.Models
{
    public class Actor : IEntityBase
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Profile Picture"), Required(ErrorMessage = "Profile picture is required")]
        public string ProfilePictureURL { get; set; }
        [Display(Name = "Full Name"), Required(ErrorMessage = "Full name is required"), StringLength(50, MinimumLength = 3, ErrorMessage = "Lenght must be between 3 and 50 characters")]
        public string FullName { get; set; }
        [Display(Name = "Biography"), Required(ErrorMessage = "Bio is required"), StringLength(50, MinimumLength = 3, ErrorMessage = "Lenght must be between 3 and 50 characters")]
        public string Bio { get; set; }
        // Relationships
        public List<Actor_Movie> Actors_Movies { get; set; }
    }
}
