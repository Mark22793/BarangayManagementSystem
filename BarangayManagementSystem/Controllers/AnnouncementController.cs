using Microsoft.AspNetCore.Mvc;

namespace BarangayManagementSystem.Controllers
{
    public class AnnouncementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
