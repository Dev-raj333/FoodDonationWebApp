using FoodDonationWebApp.Data;
using FoodDonationWebApp.Models;
using FoodDonationWebApp.Services.Interfaces;
using FoodDonationWebApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

namespace FoodDonationWebApp.Services.Implementation
{
    public class DropRequestRepository : IDropRequestRepository
    {
        private readonly FoodDbContext _context;
        public DropRequestRepository(FoodDbContext context)
        {
            _context = context;
        }
        public async Task AddDropRequestAsync(DropRequest dropRequest)
        {
            _context.DropRequests.Add(dropRequest);
            await _context.SaveChangesAsync();
        }

        

        public async Task<IPagedList<DropRequest>> GetAllDropRequestAsync(int pageNumber, int pageSize)
        {
            var dropRequests = await _context.DropRequests
                .Include( dr => dr.AllocatedFood)
                .Include(dr => dr.Volunteer)
                .OrderByDescending(dr => dr.Id)
                .ToListAsync();
            return dropRequests.ToPagedList(pageNumber, pageSize);
        }

        public async Task<DropRequest> GetDropRequestByIdAsync(int id)
        {
            return await _context.DropRequests.Include(d => d.AllocatedFood)
                                            .ThenInclude(d => d.Recipient)
                                           .Include(d => d.Volunteer)
                                           .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<(double Latitude, double Longitude)?> GetRecipiantLocationAsync(string userId)
        {
            var result = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new { u.Latitude, u.Longitude })
                .FirstOrDefaultAsync();
            if (result == null || result.Latitude == null || result.Longitude == null)
            {
                return null;
            }
            return (result.Latitude.Value, result.Longitude.Value);
        }

        public async Task UpdateDropRequesyAsync(DropRequest dropRequest)
        {
            var dropRequests = await _context.DropRequests.FindAsync(dropRequest.Id);
            if (dropRequests == null)
                return;
            dropRequests.AllocatedFoodID = dropRequest.AllocatedFoodID;
            //dropRequests.Volunteer = dropRequest.VolunteerID;
            dropRequests.PickupTime = dropRequest.PickupTime;
            dropRequests.Notes = dropRequest.Notes;
            dropRequests.PickupStatus = dropRequest.PickupStatus;

            if(dropRequest.PickupStatus == PickupStatus.Cancelled)
            {

            }else if(dropRequest.PickupStatus == PickupStatus.Completed)
            {
                var allocatedFood = await _context.AllocatedFoods.FindAsync(dropRequest.AllocatedFoodID);
                if(allocatedFood != null)
                {
                    allocatedFood.IsCompletedDonation = true;
                }
                var recipientRequest = _context.RecipientRequests.FindAsync(dropRequest.AllocatedFood.Recipient.Id);
                if (recipientRequest != null){
                  //  recipientRequest.RequestStatus = 
                }
                _context.Entry(recipientRequest).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            else
            {
                return;
            }
        }
    }
}
