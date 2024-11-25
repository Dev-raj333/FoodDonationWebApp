using FoodDonationWebApp.Data;
using FoodDonationWebApp.Models;
using FoodDonationWebApp.Services.Interfaces;
using FoodDonationWebApp.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OsmSharp.API;
using X.PagedList;
using X.PagedList.Extensions;

namespace FoodDonationWebApp.Services.Implementation
{
    public class RecipientRequestRepository : IRecipientRequestRepository
    {
        private readonly FoodDbContext _dbContext;
        private readonly IInventoryRepository _inventory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RecipientRequestRepository(FoodDbContext dbContext,UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IInventoryRepository inventory)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _inventory = inventory;
            _httpContextAccessor = httpContextAccessor; 
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
            var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext?.User);
            var userRoles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(userId));

            var userRole = userRoles.FirstOrDefault();


            var query = _dbContext.RecipientRequests
                .Include(r => r.Recipient)
                .Where(rr => rr.IsCancelled == false);

            if(userRole == "Admin")
            {
                query = query.Where(rr => rr.RequestStatus == RequestStatus.Pending);
            }else if (userRole == "Recipient")
            {
                query = query.Where(rr => rr.RequestStatus != RequestStatus.Completed);
            }

            var recipientRequest = await query.ToListAsync();
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
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var recipientRequests = await _dbContext.RecipientRequests
                                                        .Where(r => r.Id == request.Id)
                                                        .ToListAsync();
                if (!recipientRequests.Any())
                {
                    return; 
                }

                foreach (var recipientRequest in recipientRequests)
                {
                    recipientRequest.Quantity = request.Quantity;
                    recipientRequest.RequestStatus = request.RequestStatus;
                }

                if (request.RequestStatus == RequestStatus.Rejected)
                {
                    await DeleteAsync(request.Id);
                }
                else if (request.RequestStatus == RequestStatus.Approved)
                {
                    _dbContext.RecipientRequests.UpdateRange(recipientRequests);
                    var allocationResult = await _inventory.AllocateFoodAsync(request.RequestedFoodType, request.Quantity);
                    if (allocationResult == null)
                    {
                        await transaction.RollbackAsync();
                        return;
                    }
                    var allocatedFood = new AllocatedFood
                    {
                        RecipientId = request.RecipientId,
                        RequestedFoodType = request.RequestedFoodType,
                        Quantity = request.Quantity,
                        DonatedDate = DateTime.UtcNow,
                        IsCompletedDonation = false
                    };
                    await _dbContext.AllocatedFoods.AddAsync(allocatedFood);
                    await _dbContext.SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


    }
}
