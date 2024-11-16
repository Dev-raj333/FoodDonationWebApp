using FoodDonationWebApp.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace FoodDonationWebApp.ViewModels
{
    public class PickupRequestVM
    {
        public int Id { get; set; }

        [Required]
        public int DonationID { get; set; }

        [Required]
        public string VolunteerID { get; set; }

        [Required(ErrorMessage = "Please specify a pickup time.")]
        [Display(Name = "Pickup Time")]
        public DateTime PickupTime { get; set; }

        [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string Notes { get; set; }

        [Required]
        [Display(Name = "Pickup Status")]
        public PickupStatus PickupStatus { get; set; } = PickupStatus.Scheduled;
    }
}
