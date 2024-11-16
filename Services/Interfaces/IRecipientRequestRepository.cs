using FoodDonationWebApp.Models;
using X.PagedList;

namespace FoodDonationWebApp.Services.Interfaces
{
    public interface IRecipientRequestRepository
    {
        Task<IPagedList<RecipientRequest>> GetAllRecipentRequest(int pageNumber, int pageSize);
        Task<RecipientRequest> GetByIdAsync(int id);
        Task AddAsync(RecipientRequest request);
        Task UpdateAsync(RecipientRequest request);
        Task DeleteAsync(int id);
    }
}
