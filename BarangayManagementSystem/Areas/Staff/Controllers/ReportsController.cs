using Microsoft.AspNetCore.Mvc;
using BarangayCMS.Areas.Staff.ViewModels;
using BarangayCMS.DAL.Context; // 🛠️ Idinagdag para mahanap ang ApplicationDbContext mo
using System;
using System.Collections.Generic;
using System.Linq;

namespace BarangayCMS.Areas.Staff.Controllers
{
    [Area("Staff")]
    public class ReportsController : Controller
    {
        // 1. Idineklara ang database context field
        private readonly ApplicationDbContext _context;

        // 2. Constructor upang i-inject ang DB Context mula sa system configuration
        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 3. Binago ang GetBaseSummary para humila na ng TOTOONG BILANG sa Database
        private ReportsViewModel GetBaseSummary()
        {
            return new ReportsViewModel
            {
                // Isinaayos ang pagbibilang gamit ang totoong DbSets ng iyong ApplicationDbContext
                // (Palitan ang pangalan ng Tables kung iba ang tawag mo sa kanila sa DbContext mo)
                TotalResidents = _context.Residents.Count(r => r.IsResident),
                TotalCertificatesIssued = _context.Certificates.Count(),
                ActiveComplaints = _context.Complaints.Count(c => c.Status == "Active" || c.Status == "Pending"),
                ActiveDisasters = _context.Disasters.Count(d => d.EvacuationCenterStatus == "Active" || d.ReliefDistributionStatus == "Active Response"),
                GeneratedDate = DateTime.Now
            };
        }

        // GET: /Staff/Reports/Index
        public IActionResult Index()
        {
            var model = GetBaseSummary();
            model.ReportTitle = "System Comprehensive Executive Summary";
            return View(model);
        }

        // GET: /Staff/Reports/Certificates
        public IActionResult Certificates()
        {
            var model = GetBaseSummary();
            model.ReportTitle = "Barangay Clearance & Certification Logs";
            model.TableData = new List<Dictionary<string, string>>
            {
                new() { { "ID", "CERT-091" }, { "Resident", "Juan Dela Cruz" }, { "Type", "Barangay Clearance" }, { "Fee", "₱50.00" }, { "Status", "Paid" } },
                new() { { "ID", "CERT-092" }, { "Resident", "Maria Santos" }, { "Type", "Certificate of Indigency" }, { "Fee", "Free" }, { "Status", "Issued" } }
            };
            return View(model);
        }

        // GET: /Staff/Reports/Complaints
        public IActionResult Complaints()
        {
            var model = GetBaseSummary();
            model.ReportTitle = "Blotter Intake & Incident Reports Analysis";
            model.TableData = new List<Dictionary<string, string>>
            {
                new() { { "CaseID", "BLTR-202" }, { "Complainant", "Pedro Penduko" }, { "Subject", "Public Disturbance" }, { "Date", "07/08/2026" }, { "Status", "Pending" } },
                new() { { "CaseID", "BLTR-203" }, { "Complainant", "Alice Reyes" }, { "Subject", "Boundary Dispute" }, { "Date", "07/05/2026" }, { "Status", "Ongoing" } }
            };
            return View(model);
        }

        // GET: /Staff/Reports/Disaster
        public IActionResult Disaster()
        {
            var model = GetBaseSummary();
            model.ReportTitle = "Crisis Management Operations Record";
            model.TableData = new List<Dictionary<string, string>>
            {
                new() { { "EventID", "DIS-01" }, { "Incident", "Typhoon Flooding" }, { "Location", "Purok 3 Riverside" }, { "Status", "Active Response" } }
            };
            return View(model);
        }

        // GET: /Staff/Reports/Environment
        public IActionResult Environment()
        {
            var model = GetBaseSummary();
            model.ReportTitle = "Ecological Activities & Clean-up Drive Reports";
            model.TableData = new List<Dictionary<string, string>>
            {
                new() { { "ActID", "ENV-88" }, { "Activity", "Community Tree Planting" }, { "Venue", "Eco-Park" }, { "Date", "07/02/2026" } }
            };
            return View(model);
        }

        // GET: /Staff/Reports/Health
        public IActionResult Health()
        {
            var model = GetBaseSummary();
            model.ReportTitle = "Community Vaccination & Health Mission Records";
            model.TableData = new List<Dictionary<string, string>>
            {
                new() { { "LogID", "HLTH-04" }, { "Program", "Infant Vaccination" }, { "Attendees", "45 Infants" }, { "Medic", "Dr. Ramos" } }
            };
            return View(model);
        }

        // GET: /Staff/Reports/Residents
        public IActionResult Residents()
        {
            var model = GetBaseSummary();
            model.ReportTitle = "Resident Demographic Census Analytics";
            model.TableData = new List<Dictionary<string, string>>
            {
                new() { { "ResidentID", "RES-001" }, { "Fullname", "Juan Dela Cruz" }, { "Age", "28" }, { "VoterStatus", "Registered" } },
                new() { { "ResidentID", "RES-002" }, { "Fullname", "Maria Santos" }, { "Age", "34" }, { "VoterStatus", "Registered" } }
            };
            return View(model);
        }
    }
}