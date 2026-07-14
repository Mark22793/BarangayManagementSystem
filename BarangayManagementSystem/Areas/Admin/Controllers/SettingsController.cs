using Microsoft.AspNetCore.Mvc;
using BarangayCMS.Web.Areas.Admin.Models;

namespace BarangayCMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingsController : Controller
    {
        // 1. Dashboard ng Settings
        public IActionResult Index() => View();

        // 2. Profile ng Barangay
        public IActionResult BarangayProfile() => View(new BarangayProfileViewModel());

        [HttpPost]
        public IActionResult BarangayProfile(BarangayProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                TempData["Message"] = "Profile successfully updated!";
                return RedirectToAction(nameof(BarangayProfile));
            }
            return View(model);
        }

        // 3. Talaan ng mga Opisyal
        public IActionResult Officials()
        {
            var officials = new List<OfficialViewModel> {
                new OfficialViewModel { Id = 1, Name = "Juan Dela Cruz", Position = "Punong Barangay" },
                new OfficialViewModel { Id = 2, Name = "Maria Clara", Position = "Barangay Secretary" },
                new OfficialViewModel { Id = 3, Name = "Apolinario Mabini", Position = "Barangay Treasurer" }
            };
            return View(officials);
        }

        // 4. Kasaysayan ng Galaw sa System
        public IActionResult AuditLogs()
        {
            var logs = new List<AuditLogViewModel> {
                new AuditLogViewModel { LogDate = DateTime.Now.AddMinutes(-5), User = "admin", Action = "Updated Resident Record", Module = "Residents" },
                new AuditLogViewModel { LogDate = DateTime.Now.AddHours(-2), User = "encoder1", Action = "Issued Barangay Clearance", Module = "Certificates" }
            };
            return View(logs);
        }

        // 5. Pag-ayos ng Templates
        public IActionResult CertificateTemplates()
        {
            var templates = new List<CertificateTemplateViewModel> {
                new CertificateTemplateViewModel { TemplateName = "Barangay Clearance", Fee = 50.00m, IsDigitalSignEnabled = true },
                new CertificateTemplateViewModel { TemplateName = "Certificate of Indigency", Fee = 0.00m, IsDigitalSignEnabled = false }
            };
            return View(templates);
        }

        // 6. Backup and Restore
        public IActionResult Backup() => View();
    }
}
