using FoodDonationWebApp.Data;
using FoodDonationWebApp.Models;
using FoodDonationWebApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Transactions;
using X.PagedList;
using X.PagedList.Extensions;

namespace FoodDonationWebApp.Services.Implementation
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly FoodDbContext _foodDbContext;
        public InventoryRepository(FoodDbContext foodDbContext) {
            _foodDbContext = foodDbContext;
        }

        public async Task<List<Inventory>> AllocateFoodAsync(FoodType foodType, int requestedQuantity)
        {
            var availableItems = await GetAvailableItemsAsync(foodType);
            if(availableItems.Count == 0)
            {
                return null;
            }

            int remainingQuantity = requestedQuantity;
            var allocatedItems = new List<Inventory>();

            foreach (var item in availableItems)
            {
                if (remainingQuantity <= 0) break;

                if(item.Quantity <= remainingQuantity)
                {
                    allocatedItems.Add(item);
                    remainingQuantity = item.Quantity;
                    item.FoodStatus = true;
                }
                else
                {
                    allocatedItems.Add(new Inventory
                    {
                        FoodType = item.FoodType,
                        Quantity = item.Quantity -  remainingQuantity,
                        Description = item.Description,
                        ExpiryDate = item.ExpiryDate,
                        Location = item.Location,
                        DonationDate = item.DonationDate,
                        CreatedAt = item.CreatedAt,
                        UpdatedAt = item.UpdatedAt,
                        FoodStatus = true
                    });
                }
            }
            return allocatedItems;
        }

        public async Task<List<Inventory>> GetAvailableItemsAsync(FoodType foodType)
        {
            return await _foodDbContext.inventories
                .Where(i => i.FoodType == foodType && !i.FoodStatus && (i.ExpiryDate == null || i.ExpiryDate > DateTime.UtcNow))
                .OrderBy(i => i.ExpiryDate.HasValue ? i.ExpiryDate.Value : i.DonationDate) // FIFO sorting
                .ToListAsync();
        }
        public async Task<IPagedList<Inventory>> GetAllDonationList(int pageNumber, int pageSize)
        {
            var inventory = await _foodDbContext.inventories
                .Where(x => x.FoodStatus == false && (x.ExpiryDate == null || x.ExpiryDate > DateTime.UtcNow))
                .OrderBy(x => x.DonationDate)
                .ThenBy(x => x.ExpiryDate)
                .ToListAsync();
            return inventory.ToPagedList(pageNumber, pageSize);
        }

        
        public async Task<IPagedList<AllocatedFood>> GetAllocatedFood(int pageNumber, int pageSize)
        {
            var allocatedFoods = await _foodDbContext.AllocatedFoods.Where(af => af.IsCompletedDonation == false).Include(r => r.Recipient).ToListAsync();
            return allocatedFoods.ToPagedList(pageNumber, pageSize);
        }
    }
}
