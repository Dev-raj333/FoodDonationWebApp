using FoodDonationWebApp.Data;
using FoodDonationWebApp.Models;
using FoodDonationWebApp.Services.Interfaces;
using FoodDonationWebApp.ViewModel;
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

        public Task AddRecpiantRequestAsync(RecipientRequest request)
        { 
            _dbContext.RecipientRequests.Add(request);
            return _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ecipiantRequest = await _dbContext.RecipientRequests.FindAsync(id);
            if (ecipiantRequest != null)
            {
                ecipiantRequest.IsCancelled = true;
                ecipiantRequest.RequestStatus = Models.RequestStatus.Rejected;
                await _dbContext.SaveChangesAsync();
            }

        }

        public async Task<IPagedList<RecipientRequest>> GetAllRecipentRequest(int pageNumber, int pageSize)
        {
            var recipientRequest = await _dbContext.RecipientRequests
                .Include(r => r.Recipient).ToListAsync();

            return recipientRequest.ToPagedList(pageNumber, pageSize);
        }

        public async Task<RecipientRequest> GetByIdAsync(int id)
        {
            var recipiantRequest = await _dbContext.RecipientRequests
                .Where(rr => rr.IsCancelled == false)
                .Include(r => r.Recipient).FirstOrDefaultAsync(rr => rr.Id == id);

            if (recipiantRequest == null)
                return null;
            return recipiantRequest;
        }

        public async Task UpdateAsync(RecipientRequest request)
        {
            // Fetch matching records from the database
            var recipientRequests = await _dbContext.RecipientRequests
                                                    .Where(r => r.Id == request.Id)
                                                    .ToListAsync();

            if (!recipientRequests.Any())
            {
                return; // Exit if no matching records
            }

            // Update records based on the request
            foreach (var recipientRequest in recipientRequests)
            {
                recipientRequest.Quantity = request.Quantity;
                recipientRequest.RequestStatus = request.RequestStatus;
            }

            // Handle special status cases
            if (request.RequestStatus == RequestStatus.Rejected)
            {
                await DeleteAsync(request.Id); // Ensure DeleteAsync is awaited
            }
            else if (request.RequestStatus == RequestStatus.Approved)
            {
                _dbContext.RecipientRequests.UpdateRange(recipientRequests);
                await _dbContext.SaveChangesAsync();
            }
        }

    }
}
