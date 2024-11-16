using FoodDonationWebApp.Models;
using FoodDonationWebApp.ViewModels;
using X.PagedList;

namespace FoodDonationWebApp.Services.Interfaces
{
    public interface IPickupRequestRepository
    {
        Task<IPagedList<PickupRequest>> GetAllPickupRequestsAsync(int pageNumber, int pageSize);
        Task<PickupRequest> GetPickupRequestByIdAsync(int id);
        Task AddPickupRequestAsync(PickupRequest pickupRequest);
        Task UpdatePickupRequestAsync(PickupRequest pickupRequest);
        Task DeletePickupRequestAsync(int id);
    }
}
