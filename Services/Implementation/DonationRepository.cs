using FoodDonationWebApp.Data;
using FoodDonationWebApp.Models;
using FoodDonationWebApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;

namespace FoodDonationWebApp.Services.Implementation
{
    public class DonationRepository : IDonation
    {
        private readonly FoodDbContext _context;

        public DonationRepository (FoodDbContext context)
        {
            _context = context;
        }
        public async Task AddDonationAsync(Donation donation)
        {
            _context.Donations.Add(donation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDonationAsync(int id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation != null)
            {
                _context.Donations.Remove(donation);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IPagedList<Donation>> GetAllDonationsAsync(int pageNumber, int pageSize)
        {
            var donatio =await _context.Donations.Include(d=> d.Donor).Where(d => d.Status == DonationStatus.Available).ToListAsync();

            return donatio.ToPagedList(pageNumber, pageSize);
        }

        public async Task<IPagedList<Donation>> GetDonationsByDonorIdAsync(int pageNumber, int pageSize,string donorId)
        {
            var donation =  await _context.Donations
                .Where(d => d.DonorID == donorId)
                .Include(d => d.Donor)
                .ToListAsync();
            return donation.ToPagedList(pageNumber, pageSize);
        }
        public async Task<Donation> GetDonationByIdAsync(int id)
        {
            return await _context.Donations.Include(d => d.Donor).FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task UpdateDonationAsync(Donation donation)
        {
            _context.Entry(donation).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
