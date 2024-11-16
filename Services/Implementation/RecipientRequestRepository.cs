using FoodDonationWebApp.Data;
using FoodDonationWebApp.Models;
using FoodDonationWebApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

namespace FoodDonationWebApp.Services.Implementation
{
    public class RecipientRequestRepository : IRecipientRequestRepository
    {
        private readonly FoodDbContext _dbContext;

        public RecipientRequestRepository(FoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public Task AddAsync(RecipientRequest request)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IPagedList<RecipientRequest>> GetAllRecipentRequest(int pageNumber, int pageSize)
        {
            var recipientRequest = await _dbContext.RecipientRequests
                .Include(r => r.Recipient).ToListAsync();

            return recipientRequest.ToPagedList(pageNumber, pageSize);
        }

        public Task<RecipientRequest> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(RecipientRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
