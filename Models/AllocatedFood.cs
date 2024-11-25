using System.ComponentModel.DataAnnotations;

namespace FoodDonationWebApp.Models
{
    public class AllocatedFood
    {
        [Key]
        public int Id { get; set; }
        public string RecipientId { get; set; }
        public ApplicationUser Recipient { get; set; }
        [Required]
        public FoodType RequestedFoodType { get; set; }
        [Required]
        public int Quantity { get; set; }
        public DateTime DonatedDate {  get; set; }
        public bool IsCompletedDonation { get; set; } = false;
    }
}
