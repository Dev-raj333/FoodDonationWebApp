using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FoodDonationWebApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; } 
        public string? LastName { get; set; }
        public string? PhoenNumber { get; set; }
        public string? Address { get; set; }
        public string? Profile { get; set; }
        public string? StreetName { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public ICollection<RecipientRequest> RecipientRequests { get; set; } = new List<RecipientRequest>();
    }
}
