using FoodDonationWebApp.Models;
using X.PagedList;

namespace FoodDonationWebApp.Services.Interfaces
{ 
    public interface IInventoryRepository
    {
        Task<IPagedList<Inventory>> GetAllDonationList (int pageNumber, int pageSize);
        Task<List<Inventory>> AllocateFoodAsync(FoodType foodType, int requestedQuantity);
        Task<IPagedList<AllocatedFood>> GetAllocatedFood(int pageNumber, int pageSize);
    }
}
