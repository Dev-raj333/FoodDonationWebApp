using FoodDonationWebApp.Data;
using FoodDonationWebApp.Models;
using FoodDonationWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;



namespace FoodDonationWebApp.Services.Implementation
{
    public class PickupRequestRepository : IPickupRequestRepository
    {
        private readonly FoodDbContext _context;

        public PickupRequestRepository(FoodDbContext context)
        {
            _context = context;
        }

        public async Task<IPagedList<PickupRequest>> GetAllPickupRequestsAsync(int pageNumber, int pageSize)
        {
            var pickupRequests = await _context.PickupRequests
                .Where(pr => pr.CancelledStatus == false && pr.PickupStatus == PickupStatus.Scheduled)
                .Include(pr => pr.Donation)
                .ThenInclude(d => d.Donor)
                .Include(pr => pr.Volunteer)
                .OrderByDescending(pr => pr.Id)
                .ToListAsync();

            return pickupRequests.ToPagedList(pageNumber,pageSize);
            
        }

        public async Task<PickupRequest> GetPickupRequestByIdAsync(int id)
        {
            var pickupRequest = await _context.PickupRequests
                .Where(pr => pr.CancelledStatus == false && pr.PickupStatus == PickupStatus.Scheduled)
                .Include(pr => pr.Donation)
                .ThenInclude(d => d.Donor)
                .Include(pr => pr.Volunteer)
                .FirstOrDefaultAsync(pr => pr.Id == id);

            if (pickupRequest == null)
                return null;

            return pickupRequest;
        }

        public async Task AddPickupRequestAsync(PickupRequest pickupRequestVm)
        {
            
            _context.PickupRequests.Add(pickupRequestVm);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePickupRequestAsync(PickupRequest pickupRequestVm)
        {
            var pickupRequest = await _context.PickupRequests.FindAsync(pickupRequestVm.Id);
            if (pickupRequest == null)
                return;

            pickupRequest.DonationID = pickupRequestVm.DonationID;
            pickupRequest.VolunteerID = pickupRequestVm.VolunteerID;
            pickupRequest.PickupTime = pickupRequestVm.PickupTime;
            pickupRequest.Notes = pickupRequestVm.Notes;
            pickupRequest.PickupStatus = pickupRequestVm.PickupStatus;

            if (pickupRequestVm.PickupStatus == PickupStatus.Cancelled)
            {
                DeletePickupRequestAsync(pickupRequestVm.Id);
            }
            else if (pickupRequestVm.PickupStatus == PickupStatus.Completed)
            {
                var donation = await _context.Donations.FindAsync(pickupRequestVm.DonationID);
                if (donation != null)
                {
                    donation.Status = DonationStatus.PickedUp;
                    var invetoryItem = new Inventory
                    {
                        FoodType = donation.FoodType,
                        Quantity = donation.Quantity,
                        Description = donation.Description,
                        ExpiryDate = donation.ExpiraryDate,
                        CreatedAt = DateTime.UtcNow,
                        Location = donation.FoodType.ToString(),
                    };
                    _context.inventories.Add(invetoryItem);
                }
                _context.Entry(pickupRequest).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            else
            {
                return;
            }
        }
        [HttpPost]
        public async Task DeletePickupRequestAsync(int id)
        {
            var pickupRequest = await _context.PickupRequests.FindAsync(id);
            if (pickupRequest != null)
            {
                pickupRequest.CancelledStatus = true;
                pickupRequest.PickupStatus = PickupStatus.Cancelled;
                pickupRequest.Donation.Status = DonationStatus.Available; 
                await _context.SaveChangesAsync();
            }
        }
    }
}
