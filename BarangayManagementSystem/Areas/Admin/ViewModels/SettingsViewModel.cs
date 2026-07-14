using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BarangayCMS.Web.Areas.Admin.Models
{
    public class BarangayProfileViewModel
    {
        [Required]
        [Display(Name = "Pangalan ng Barangay")]
        public string BarangayName { get; set; } = "Barangay Poblacion Uno";

        [Required]
        public string Municipality { get; set; } = "Quezon City";

        [Required]
        public string Province { get; set; } = "Metro Manila";

        [Display(Name = "Official Email")]
        public string ContactEmail { get; set; } = "info@barangaycms.gov.ph";

        public string Vision { get; set; } = "Isang progresibo at ligtas na komunidad.";
        public string Mission { get; set; } = "Magbigay ng tapat na serbisyo sa mamamayan.";
    }

    public class OfficialViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

    public class AuditLogViewModel
    {
        public DateTime LogDate { get; set; }
        public string User { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
    }

    public class CertificateTemplateViewModel
    {
        public string TemplateName { get; set; } = string.Empty;
        public decimal Fee { get; set; }
        public bool IsDigitalSignEnabled { get; set; }
    }
}
