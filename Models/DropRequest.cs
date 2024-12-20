﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodDonationWebApp.Models
{
    public class DropRequest
    {
        public int Id { get; set; }

        [ForeignKey("AllocatedFood")]
        public int? AllocatedFoodID { get; set; }
        public AllocatedFood? AllocatedFood { get; set; }

        [ForeignKey("Volunteer")]
        public string? VolunteerID { get; set; }
        public ApplicationUser? Volunteer { get; set; }
        public DateTime? PickupTime { get; set; }
        [MaxLength(500)]
        public string? Notes { get; set; }

        public PickupStatus PickupStatus { get; set; } = PickupStatus.Scheduled;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }

}
