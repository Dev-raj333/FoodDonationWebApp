using Microsoft.AspNetCore.Mvc;

namespace FoodDonationWebApp.Controllers
{
    public class DropRequestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
