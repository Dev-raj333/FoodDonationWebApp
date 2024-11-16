using FoodDonationWebApp.Models;
using FoodDonationWebApp.Services.Interfaces;
using FoodDonationWebApp.ViewModel;
using FoodDonationWebApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList.Mvc.Core;

namespace FoodDonationWebApp.Controllers
{
    public class PickupRequestController : Controller
    {
        private readonly IPickupRequestRepository _pickupRequestRepository;
        private readonly IDonation  _donation;
        private readonly UserManager<ApplicationUser> _userManager;

        public PickupRequestController(IPickupRequestRepository pickupRequestRepository,IDonation donation, UserManager<ApplicationUser> userManager)
        {
            _pickupRequestRepository = pickupRequestRepository;
            _userManager = userManager;
            _donation = donation;
        }

        public async Task<IActionResult> Index(int? page, int? pageSize)
        {
            int pageNumber = page ?? 1;
            int itemsPerPage = pageSize ?? 10;

            ViewData["CurrentPageSize"] = itemsPerPage;
            var pickupRequests = await _pickupRequestRepository.GetAllPickupRequestsAsync(pageNumber, itemsPerPage);
            return View(pickupRequests); 
        }

        public async Task<IActionResult> Details(int id)
        {
            var pickupRequest = await _pickupRequestRepository.GetPickupRequestByIdAsync(id);
            if (pickupRequest == null)
            {
                return NotFound();
            }
            return View(pickupRequest);  
        }

        public async Task<IActionResult> Create(int donationId)
        {
            var volunteers = await _userManager.GetUsersInRoleAsync("Volunture");
            ViewBag.Volunteers = volunteers.Select(v => new { v.Id, v.FirstName, v.LastName }).ToList();
            ViewBag.DonationId = donationId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PickupRequestVM pickupRequest)
        {
            if (ModelState.IsValid)
            {
                var pickupRequests = new PickupRequest
                {
                    DonationID = pickupRequest.DonationID,
                    VolunteerID = pickupRequest.VolunteerID,
                    PickupTime = pickupRequest.PickupTime,
                    Notes = pickupRequest.Notes,
                    PickupStatus = pickupRequest.PickupStatus
                };
                pickupRequest.DonationID = pickupRequests.DonationID;

                await _pickupRequestRepository.AddPickupRequestAsync(pickupRequests);
                var donation = await _donation.GetDonationByIdAsync(pickupRequests.DonationID);
                if (donation != null) 
                {
                    donation.Status = Models.DonationStatus.Pending;
                    await _donation.UpdateDonationAsync(donation);
                }

                return RedirectToAction("index", "Donation");
            }

            var volunteers = await _userManager.GetUsersInRoleAsync("Volunture");
            ViewBag.Volunteers = volunteers.Select(v => new { v.Id, v.FirstName, v.LastName }).ToList();
            return View(pickupRequest);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var pickupRequest = await _pickupRequestRepository.GetPickupRequestByIdAsync(id);
            if (pickupRequest == null)
            {
                return NotFound();
            }

            return View(pickupRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PickupRequest pickupRequest)
        {
            if (id != pickupRequest.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _pickupRequestRepository.UpdatePickupRequestAsync(pickupRequest);
                return RedirectToAction(nameof(Index));
            }

            return View(pickupRequest);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var pickupRequest = await _pickupRequestRepository.GetPickupRequestByIdAsync(id);
            if (pickupRequest == null)
            {
                return NotFound();
            }
            return View(pickupRequest);  
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _pickupRequestRepository.DeletePickupRequestAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
