using FoodDonationWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodDonationWebApp.Controllers
{
    public class RecipientRequestController : Controller
    {
        private readonly IRecipientRequestRepository _recipientRequest;

        public RecipientRequestController(IRecipientRequestRepository recipientRequestRepository)
        {
            _recipientRequest = recipientRequestRepository;
        }
        public async Task<IActionResult> Index(int? page, int? pageSize)
        {
            int pageNumber = page ?? 1;
            int itemsPerPage = pageSize ?? 10;

            ViewData["CurrentPageSize"] = itemsPerPage;
            var recipiantRequest = await _recipientRequest.GetAllRecipentRequest(pageNumber, itemsPerPage);
            return View(recipiantRequest);
        }
    }
}
