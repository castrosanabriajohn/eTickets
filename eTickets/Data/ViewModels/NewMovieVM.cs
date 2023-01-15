using eTickets.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eTickets.Data.ViewModels
{
    public class NewMovieVM
    {
        [Display(Name = "Movie name"), Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Display(Name = "Description"), Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        [Display(Name = "Movie price"), Required(ErrorMessage = "Movie price is required")]
        public double Price { get; set; }
        [Display(Name = "Image URL"), Required(ErrorMessage = "Movie poster URL is required")]
        public string ImageURL { get; set; }
        [Display(Name = "Start date"), Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End date"), Required(ErrorMessage = "End date poster URL is required")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Movie Category"), Required(ErrorMessage = "Movie category is required")]
        public MovieCategory MovieCategory { get; set; }
        // Relationships
        [Display(Name = "Select actor(s)"), Required(ErrorMessage = "Actor(s) is required")]
        // Actor Ids
        public List<int> ActorIds { get; set; }
        [Display(Name = "Select cinema(s)"), Required(ErrorMessage = "Cinema(s) is required")]
        // Cinema
        public int CinemaId { get; set; }
        [Display(Name = "Select producer(s)"), Required(ErrorMessage = "Producer(s) is required")]
        // Producer
        public int ProducerId { get; set; }
    }
}
