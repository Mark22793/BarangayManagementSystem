using Microsoft.AspNetCore.Mvc;
using BarangayCMS.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;

namespace BarangayCMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReportsController : Controller
    {
        // 1. GET: Admin/Reports/Index
        public IActionResult Index()
        {
            // Mock data para sa mga dashboard counters ng ulat
            var dashboardData = new ReportsDashboardViewModel
            {
                TotalResidents = 1420,
                TotalComplaints = 18,
                TotalBudget = 350000.00m,
                ActiveProjects = 4,
                TotalCertificatesIssued = 245,
                ActiveEvacuees = 12,
                TotalHealthRecords = 89
            };

            return View(dashboardData);
        }

        // 2. GET: Admin/Reports/Residents
        public IActionResult Residents()
        {
            var residentsList = new List<ResidentReportItem>
            {
                new ResidentReportItem { Name = "Juan Dela Cruz", Age = 34, Gender = "Lalaki", Purok = "Purok 1", Status = "Registered Voter" },
                new ResidentReportItem { Name = "Maria Clara", Age = 28, Gender = "Babae", Purok = "Purok 3", Status = "Registered Voter" },
                new ResidentReportItem { Name = "Pedro Penduko", Age = 17, Gender = "Lalaki", Purok = "Purok 2", Status = "Non-Voter" }
            };

            return View(residentsList);
        }

        // 3. GET: Admin/Reports/Complaints
        public IActionResult Complaints()
        {
            var complaintsList = new List<ComplaintReportItem>
            {
                new ComplaintReportItem { CaseId = 101, Complainant = "Jose Rizal", Respondent = "Emilio Aguinaldo", CaseType = "Simbahang Usapin", Status = "Resolved", DateFiled = DateTime.Now.AddDays(-15) },
                new ComplaintReportItem { CaseId = 102, Complainant = "Andres Bonifacio", Respondent = "Deodato Arellano", CaseType = "Alitan sa Hangganan", Status = "Pending", DateFiled = DateTime.Now.AddDays(-2) }
            };

            return View(complaintsList);
        }

        // 4. GET: Admin/Reports/Budget
        public IActionResult Budget()
        {
            var budgetList = new List<ProjectBudgetReportItem>
            {
                new ProjectBudgetReportItem { ProjectName = "Barangay Hall Renovation", AllocatedBudget = 150000.00m, ExpensesToDate = 120000.00m, Status = "Ongoing", FundingSource = "SK Fund" },
                new ProjectBudgetReportItem { ProjectName = "Streetlight Installation", AllocatedBudget = 80000.00m, ExpensesToDate = 80000.00m, Status = "Completed", FundingSource = "General Fund" },
                new ProjectBudgetReportItem { ProjectName = "Drainage Declogging", AllocatedBudget = 50000.00m, ExpensesToDate = 150000.00m, Status = "Ongoing", FundingSource = "Calamity Fund" }
            };

            return View(budgetList);
        }

        // 5. GET: Admin/Reports/Projects
        public IActionResult Projects()
        {
            var projectsList = new List<ProjectBudgetReportItem>
            {
                new ProjectBudgetReportItem { ProjectName = "Barangay Hall Renovation", AllocatedBudget = 150000.00m, Status = "Ongoing (75%)", FundingSource = "SK Fund" },
                new ProjectBudgetReportItem { ProjectName = "Streetlight Installation", AllocatedBudget = 80000.00m, Status = "Completed (100%)", FundingSource = "General Fund" },
                new ProjectBudgetReportItem { ProjectName = "Health Center Repair", AllocatedBudget = 120000.00m, Status = "Planning (0%)", FundingSource = "Local Government" }
            };

            return View(projectsList);
        }

        // 6. GET: Admin/Reports/Certificates
        public IActionResult Certificates()
        {
            var certificatesList = new List<CertificateReportItem>
            {
                new CertificateReportItem { ReferenceNo = "BC-2026-0001", RequestorName = "Apolinario Mabini", CertificateType = "Barangay Clearance", AmountPaid = 50.00m, DateIssued = DateTime.Now.AddDays(-5) },
                new CertificateReportItem { ReferenceNo = "BI-2026-0032", RequestorName = "Melchora Aquino", CertificateType = "Certificate of Indigency", AmountPaid = 0.00m, DateIssued = DateTime.Now.AddDays(-3) }
            };

            return View(certificatesList);
        }

        // 7. GET: Admin/Reports/Disaster
        public IActionResult Disaster()
        {
            var disasterList = new List<DisasterReportItem>
            {
                new DisasterReportItem { IncidentName = "Bagyong Aghon", EvacuationCenter = "Barangay Gym", FamiliesAccommodated = 25, Status = "Closed" },
                new DisasterReportItem { IncidentName = "Purok 4 Flash Flood", EvacuationCenter = "Elementary School", FamiliesAccommodated = 12, Status = "Active" }
            };

            return View(disasterList);
        }

        // 8. GET: Admin/Reports/Health
        public IActionResult Health()
        {
            var healthList = new List<HealthReportItem>
            {
                new HealthReportItem { ResidentName = "Juan Luna", Condition = "Senior Citizen", AssistanceReceived = "Maintenance Vitamins", DateChecked = DateTime.Now.AddDays(-10) },
                new HealthReportItem { ResidentName = "Gabriela Silang", Condition = "Pregnant", AssistanceReceived = "Prenatal Checkup & Milk", DateChecked = DateTime.Now.AddDays(-4) }
            };

            return View(healthList);
        }

        // 9. GET: Admin/Reports/Export
        public IActionResult Export()
        {
            return View();
        }

        // 10. GET: Admin/Reports/ExportExcel
        public IActionResult ExportExcel()
        {
            var builder = new System.Text.StringBuilder();

            // Pamagat ng Excel Document
            builder.AppendLine("PANGKALAHATANG ULAT NG BARANGAY (MOCK DATA)");
            builder.AppendLine($"Petsa ng Pag-export: {DateTime.Now:yyyy-MM-dd HH:mm}");
            builder.AppendLine();

            // Seksyon 1: Summary Counters
            builder.AppendLine("MGA PANGUNAHING METRIKO,BILANG/HALAGA");
            builder.AppendLine("Kabuuang Residente,1420");
            builder.AppendLine("Mga Kaso ng Blotter,18");
            builder.AppendLine("Inilaang Badyet,350000.00");
            builder.AppendLine("Aktibong Proyekto,4");
            builder.AppendLine("Naipamahaging Sertipiko,245");
            builder.AppendLine();

            // Seksyon 2: Resident Sample List
            builder.AppendLine("TALAAN NG RESIDENTE (SAMPLE)");
            builder.AppendLine("Pangalan,Edad,Kasarian,Purok,Status");
            builder.AppendLine("Juan Dela Cruz,34,Lalaki,Purok 1,Registered Voter");
            builder.AppendLine("Maria Clara,28,Babae,Purok 3,Registered Voter");
            builder.AppendLine("Pedro Penduko,17,Lalaki,Purok 2,Non-Voter");
            builder.AppendLine();

            // Seksyon 3: Complaints Sample List
            builder.AppendLine("TALAAN NG REKLAMO / BLOTTER (SAMPLE)");
            builder.AppendLine("Case ID,Nagluhog,Ipinagsakdal,Uri ng Kaso,Status");
            builder.AppendLine("101,Jose Rizal,Emilio Aguinaldo,Simbahang Usapin,Resolved");
            builder.AppendLine("102,Andres Bonifacio,Deodato Arellano,Alitan sa Hangganan,Pending");

            // I-convert ang teksto gamit ang UTF-8 string encoding
            var fileBytes = System.Text.Encoding.UTF8.GetBytes(builder.ToString());

            // I-download bilang .csv file (Awtomatikong mag-le-layout ng kolum sa Excel)
            return File(fileBytes, "text/csv", $"Barangay_Summary_Report_{DateTime.Now:yyyyMMdd}.csv");
        }
    }
}