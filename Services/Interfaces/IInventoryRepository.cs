using FoodDonationWebApp.Models;
using X.PagedList;

namespace FoodDonationWebApp.Services.Interfaces
{ 
    public interface IInventoryRepository
    {
        Task<IPagedList<Inventory>> GetAllDonationList (int pageNumber, int pageSize); 
    }
}
