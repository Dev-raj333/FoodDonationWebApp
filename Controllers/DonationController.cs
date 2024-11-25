using FoodDonationWebApp.Models;
using FoodDonationWebApp.Services.Implementation;
using FoodDonationWebApp.Services.Interfaces;
using FoodDonationWebApp.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodDonationWebApp.Controllers
{
    public class DonationController : Controller
    {
        private readonly IDonation _donation;
        private readonly UserManager<ApplicationUser> _userManager;

        public DonationController(IDonation donation, UserManager<ApplicationUser> userManager) { 
            _donation = donation;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(int? page, int? pageSize)
        {
            int pageNumber = page ?? 1;
            int itemsPerPage = pageSize ?? 10;
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            if (User.IsInRole("Admin"))
            {
                var donations = await _donation.GetAllDonationsAsync(pageNumber, itemsPerPage);
                return View(donations);
            }
            else if (User.IsInRole("Doner"))
            {
                var donations = await _donation.GetDonationsByDonorIdAsync(pageNumber, itemsPerPage,userId);
                return View(donations);
            }

            return RedirectToAction("AccessDenied", "Account");
        }

        public async Task<IActionResult> Details(int id)
        {
            var donation = await _donation.GetDonationByIdAsync(id);
            if (donation == null)
            {
                return NotFound();
            }
            return View(donation);
        }

        [Authorize(Roles = "Doner")]
        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var viewModel = new DonationCreateVM
            {
                DonorID = userId
            };
            return View(viewModel);
        }

        [Authorize(Roles = "Doner")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DonationCreateVM viewModel)
        {
            if (ModelState.IsValid)
            {
                var donation = new Donation
                {
                    DonorID = viewModel.DonorID,
                    FoodType = (Models.FoodType)viewModel.FoodType,
                    Quantity = viewModel.Quantity,
                    ExpiraryDate = viewModel.ExpiraryDate,
                    Description = viewModel.Description,
                    Status = (Models.DonationStatus)viewModel.Status
                };

                await _donation.AddDonationAsync(donation);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete (int id)
        {
            var donation = await _donation.GetDonationByIdAsync(id);
            if (donation == null)
            {
                return NotFound();
            }
            return View(donation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _donation.DeleteDonationAsync(id);
            return RedirectToAction("Index", "Donation");
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var donation = await _donation.GetDonationByIdAsync(id);
            if (donation == null)
            {
                return NotFound();
            }
            return View(donation);
        }

        // Edit POST: Save the updated donation
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Donation donation)
        {
            if (id != donation.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _donation.UpdateDonationAsync(donation);
                return RedirectToAction(nameof(Index));
            }

            return View(donation);
        }


    }
}
