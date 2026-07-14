using System;
using System.Collections.Generic;

namespace BarangayCMS.Web.Areas.Admin.Models
{
    public class ReportsDashboardViewModel
    {
        // Mga Pangunahing Bilang para sa Dashboard Summary (Index)
        public int TotalResidents { get; set; }
        public int TotalComplaints { get; set; }
        public decimal TotalBudget { get; set; }
        public int ActiveProjects { get; set; }
        public int TotalCertificatesIssued { get; set; }
        public int ActiveEvacuees { get; set; }
        public int TotalHealthRecords { get; set; }
    }

    // Gagamitin para sa Residents.cshtml
    public class ResidentReportItem
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Purok { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    // Gagamitin para sa Complaints.cshtml
    public class ComplaintReportItem
    {
        public int CaseId { get; set; }
        public string Complainant { get; set; } = string.Empty;
        public string Respondent { get; set; } = string.Empty;
        public string CaseType { get; set; } = string.Empty; // Blotter, Incident, etc.
        public string Status { get; set; } = string.Empty; // Resolved, Pending
        public DateTime DateFiled { get; set; }
    }

    // Gagamitin para sa Budget.cshtml at Projects.cshtml
    public class ProjectBudgetReportItem
    {
        public string ProjectName { get; set; } = string.Empty;
        public decimal AllocatedBudget { get; set; }
        public decimal ExpensesToDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string FundingSource { get; set; } = string.Empty;
    }

    // Gagamitin para sa Certificates.cshtml
    public class CertificateReportItem
    {
        public string ReferenceNo { get; set; } = string.Empty;
        public string RequestorName { get; set; } = string.Empty;
        public string CertificateType { get; set; } = string.Empty; // Clearance, Indigency
        public decimal AmountPaid { get; set; }
        public DateTime DateIssued { get; set; }
    }

    // Gagamitin para sa Disaster.cshtml
    public class DisasterReportItem
    {
        public string IncidentName { get; set; } = string.Empty;
        public string EvacuationCenter { get; set; } = string.Empty;
        public int FamiliesAccommodated { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    // Gagamitin para sa Health.cshtml
    public class HealthReportItem
    {
        public string ResidentName { get; set; } = string.Empty;
        public string Condition { get; set; } = string.Empty; // PWD, Senior, Pregnant, etc.
        public string AssistanceReceived { get; set; } = string.Empty;
        public DateTime DateChecked { get; set; }
    }
}
