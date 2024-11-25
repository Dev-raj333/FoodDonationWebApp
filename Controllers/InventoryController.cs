using FoodDonationWebApp.Models;
using FoodDonationWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodDonationWebApp.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryController(IInventoryRepository inventory) { 
            _inventoryRepository = inventory;
        }
        public async Task<IActionResult> Index(int? page, int? pageSize)
        {
            int pageNumber = page ?? 1;
            int itemsPerPage = pageSize ?? 10;
            ViewData["CurrentPageSize"] = itemsPerPage;
            var inventory = await _inventoryRepository.GetAllDonationList(pageNumber,itemsPerPage);
            return View(inventory);
        }

        [HttpGet]
        public async Task<IActionResult> AllocateFood(int? page, int? pageSize)
        {
            int pageNumber = page ?? 1;
            int itemsPerPAge = pageSize ?? 10;
            var allocatedItems = await _inventoryRepository.GetAllocatedFood(pageNumber,itemsPerPAge);
            return View(allocatedItems); 
        }

        
    }
}
