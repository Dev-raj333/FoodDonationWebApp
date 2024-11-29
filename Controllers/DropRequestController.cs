using FoodDonationWebApp.Models;
using FoodDonationWebApp.Services.Implementation;
using FoodDonationWebApp.Services.Interfaces;
using FoodDonationWebApp.ViewModel;
using FoodDonationWebApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace FoodDonationWebApp.Controllers
{
    public class DropRequestController : Controller
    {
        private readonly IDropRequestRepository _dropRequest;
        private readonly UserManager<ApplicationUser> _userManager;

        public DropRequestController(IDropRequestRepository dropRequest, UserManager<ApplicationUser> userManager)
        {
            _dropRequest = dropRequest;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? page, int? pageSize)
        {
            int pageNumber = page ?? 1;
            int itemsPerPage = pageSize ?? 10;

            ViewData["CurrentPageSize"] = itemsPerPage;
            var dropRequests = await _dropRequest.GetAllDropRequestAsync(pageNumber, itemsPerPage);
            return View(dropRequests);
        }

        public async Task<IActionResult> Details(int id)
        {
            var dropRequest = await _dropRequest.GetDropRequestByIdAsync(id);
            if(dropRequest == null)
            {
                return NotFound();
            }
            return View(dropRequest);
        }

        public async Task<IActionResult> Create(int id)
        {
            var volunteers = await _userManager.GetUsersInRoleAsync("Volunture");
            ViewBag.Volunteers = volunteers.Select(v => new { v.Id, v.FirstName, v.LastName }).ToList();
            var model = new DropRequestVM
            {
                AllocatedFoodID = id 
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DropRequestVM dropRequests)
        {
            if (ModelState.IsValid)
            {
                var dropRequest = new DropRequest
                {
                    AllocatedFoodID = dropRequests.AllocatedFoodID,
                    VolunteerID = dropRequests.VolunteerID,
                    PickupTime = dropRequests.PickupTime,
                    Notes = dropRequests.Notes,
                    PickupStatus = dropRequests.PickupStatus
                };

                dropRequests.AllocatedFoodID = dropRequest.AllocatedFoodID;

                await _dropRequest.AddDropRequestAsync(dropRequest);

                return RedirectToAction("index", "DropRequest");
            }

            var volunteers = await _userManager.GetUsersInRoleAsync("Volunture");
            ViewBag.Volunteers = volunteers.Select(v => new { v.Id, v.FirstName, v.LastName }).ToList();
            return View(dropRequests);
        }

        [HttpGet]
        public async Task<IActionResult> GetRecipiantLocation(string donorId)
        {
            var donor = await _dropRequest.GetRecipiantLocationAsync(donorId);
            if (donor == null)
            {
                return Json(new { success = false, message = "Location not found" });
            }

            return Json(new { success = true, latitude = donor.Value.Latitude, longitude = donor.Value.Longitude });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var pickupRequest = await _dropRequest.GetDropRequestByIdAsync(id);
            if (pickupRequest == null)
            {
                return NotFound();
            }

            return View(pickupRequest);
        }
    }
}
