using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarangayCMS.Entities
{
    public class Certificate
    {
        [Key]
        public int CertificateId { get; set; }

        public int ResidentId { get; set; }

        [Required]
        public string CertificateType { get; set; } = string.Empty; // Clearance, Indigency, atbp.

        public string Purpose { get; set; } = string.Empty;
        public string ControlNumber { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal FeePaid { get; set; }
        public string OfficialReceiptNumber { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending"; // Pending, Approved, Issued, Rejected

        public DateTime DateRequested { get; set; }
        public DateTime? DateIssued { get; set; }
        public string IssuedBy { get; set; } = string.Empty;

        // Navigation Property
        public Resident Resident { get; set; } = null!;
    }
}