using FoodDonationWebApp.Data;
using FoodDonationWebApp.Models;
using FoodDonationWebApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IPagedList<Inventory>> GetAllDonationList(int pageNumber, int pageSize)
        {
            var inventory = await _foodDbContext.inventories.Where(x => x.FoodStatus == false).OrderByDescending(x => x.Id).ToListAsync();
            return inventory.ToPagedList(pageNumber, pageSize);
        }
    }
}
