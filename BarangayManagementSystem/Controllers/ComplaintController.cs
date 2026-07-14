using Microsoft.AspNetCore.Mvc;
using BarangayCMS.Web.Models;

namespace BarangayCMS.Web.Controllers
{
    public class ComplaintController : Controller
    {
        public IActionResult Categories()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ComplaintSubmissionModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Dito mo tatawagin ang IComplaintService sa susunod para i-save sa DB
            // Halimbawa: _complaintService.Add(model);

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}