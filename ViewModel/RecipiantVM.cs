using FoodDonationWebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace FoodDonationWebApp.ViewModel
{
    public class RecipiantVM
    {
        public string RecipientId { get; set; }
        [Required]
        [Display(Name = "RequestedFoodType")]
        public FoodType RequestedFoodType { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public RequestStatus RequestStatus { get; set; } = RequestStatus.Pending;
        public bool IsCancelled { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
    
}
