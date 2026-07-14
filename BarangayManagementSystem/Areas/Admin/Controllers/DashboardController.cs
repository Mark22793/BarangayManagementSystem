using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DashboardController : Controller
{
    public IActionResult Index() // Para sa Admin
    {
        return View();
    }

    public IActionResult Residents()
    {
        return View();
    }

    // PARA SA STAFF ACCOUNT (BAGONG DAGDAG)
    public IActionResult StaffProfile()
    {
        return View();
    }
}