﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDonationWebApp.Models
{
    public class Donation
    {
        public int Id { get; set; }
        [ForeignKey("Doner")]
        public string DonorID { get; set; }
        public ApplicationUser? Donor {  get; set; }
        public FoodType FoodType { get; set; }
        public int Quantity { get; set; }
        public DateTime? ExpiraryDate { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public DonationStatus Status { get; set; } = DonationStatus.Available;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt {  get; set; } = DateTime.Now;
    }
    public enum DonationStatus
    {
        Available,
        Pending,
        PickedUp,
        Completed
    }

    public enum FoodType
    {
        Grains = 1,
        Vegetables = 2,
        Fruits = 3,
        Dairy = 4,
        Meat = 5,
        CannedGoods = 6,
        Beverages = 7,
        Snacks = 8,
        Other = 9
    }
}
