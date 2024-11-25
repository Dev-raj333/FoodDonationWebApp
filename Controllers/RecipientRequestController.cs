using FoodDonationWebApp.Models;
using FoodDonationWebApp.Services.Interfaces;
using FoodDonationWebApp.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodDonationWebApp.Controllers
{
    public class RecipientRequestController : Controller
    {
        private readonly IRecipientRequestRepository _recipientRequest;
        private readonly UserManager<ApplicationUser> _userManager;
        public RecipientRequestController(IRecipientRequestRepository recipientRequestRepository, UserManager<ApplicationUser> userManager)
        {
            _recipientRequest = recipientRequestRepository;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(int? page, int? pageSize)
        {
            int pageNumber = page ?? 1;
            int itemsPerPage = pageSize ?? 10;

            ViewData["CurrentPageSize"] = itemsPerPage;
            var recipiantRequest = await _recipientRequest.GetAllRecipentRequest(pageNumber, itemsPerPage);
            return View(recipiantRequest);
        }
        public async Task<IActionResult> Create()
        {
            var recipiantID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var viewModel = new RecipiantVM
            {
                RecipientId = recipiantID
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(RecipiantVM recipientRequest)
        {
            if(ModelState.IsValid)
            {
                var recipiantRequest = new RecipientRequest
                {
                    RecipientId = recipientRequest.RecipientId,
                    RequestedFoodType = recipientRequest.RequestedFoodType,
                    Quantity = recipientRequest.Quantity,
                    RequestStatus = Models.RequestStatus.Pending,
                };
                await _recipientRequest.AddRecpiantRequestAsync(recipiantRequest);

                return RedirectToAction("index", "RecipientRequest");
            }
            var recipiant = await _userManager.GetUsersInRoleAsync("Recipient");
            ViewBag.Recipiant = recipiant.Select(v => new { v.Id, v.FirstName, v.LastName }).ToList();
            return View(recipientRequest);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var recipiantRequest = await _recipientRequest.GetByIdAsync(id);
            if(recipiantRequest == null)
            {
                return NotFound();
            }
            return View(recipiantRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Edit(int id, RecipientRequest recipientRequest)
        {
            if (id != recipientRequest.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                await _recipientRequest.UpdateAsync(recipientRequest);
                return RedirectToAction(nameof(Index));
            }
            return View(recipientRequest);
        }
    }
}
