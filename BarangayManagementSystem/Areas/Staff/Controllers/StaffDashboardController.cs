using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarangayCMS.Web.Areas.Staff.Controllers
{
    [Area("Staff")]
    // 🔑 LUNAS SA 403: Pinayagan natin ang parehong Role name ("Staff" at "Staff / Encoder") 
    // para kahit alin ang nakatala sa database account, papasukin siya ng controller na ito.
    [Authorize(Roles = "Staff,Staff / Encoder")]
    [Route("Staff/[controller]/[action]")] // Pwersahang sinasabi ang URL pattern
    public class StaffDashboardController : Controller
    {
        [Route("~/Staff")] // Kapag itinype ang localhost:7268/Staff, dito agad papasok
        [Route("~/Staff/StaffDashboard")]
        public IActionResult Index()
        {
            // Siguraduhing turo sa eksaktong physical file location
            return View("~/Areas/Staff/Views/StaffDashboard/Index.cshtml");
        }
    }
}