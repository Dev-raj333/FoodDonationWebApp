using FoodDonationWebApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodDonationWebApp.ViewModel
{
    public class DropRequestVM
    {
        public int Id { get; set; }

        public int? AllocatedFoodID { get; set; }
        public AllocatedFood? AllocatedFood { get; set; }

        public string? VolunteerID { get; set; }
        public ApplicationUser? Volunteer { get; set; }
        public DateTime? PickupTime { get; set; }
        [MaxLength(500)]
        public string? Notes { get; set; }
      
        public PickupStatus PickupStatus { get; set; } = PickupStatus.Scheduled;
       
    }
}
