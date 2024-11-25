using FoodDonationWebApp.Models;
using X.PagedList;

namespace FoodDonationWebApp.Services.Interfaces
{
    public interface IDonation
    {
        Task<IPagedList<Donation>> GetAllDonationsAsync(int pageNumber, int pageSize);
        Task<IPagedList<Donation>> GetDonationsByDonorIdAsync(int pageNumber, int pageSize,string donorId);
        Task<Donation> GetDonationByIdAsync(int id);
        Task AddDonationAsync(Donation donation);
        Task UpdateDonationAsync(Donation donation);
        Task DeleteDonationAsync(int id);
    }
}
