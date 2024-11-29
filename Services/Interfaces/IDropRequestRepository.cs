using FoodDonationWebApp.Models;
using X.PagedList;

namespace FoodDonationWebApp.Services.Interfaces
{
    public interface IDropRequestRepository
    {
        Task<IPagedList<DropRequest>> GetAllDropRequestAsync(int pageNumber, int pageSize);
        Task<DropRequest> GetDropRequestByIdAsync(int id);
        Task AddDropRequestAsync(DropRequest dropRequest);
        Task UpdateDropRequesyAsync(DropRequest dropRequest);
        Task<(double Latitude, double Longitude)?> GetRecipiantLocationAsync(string userId);
    }
}
