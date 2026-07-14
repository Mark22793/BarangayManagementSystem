using System.ComponentModel.DataAnnotations;

namespace BarangayCMS.Web.Areas.Admin.Models
{
    public class ResidentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ang Pangalan ay kinakailangan.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ang Apelyido ay kinakailangan.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Middle Name")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Pumili ng Kasarian.")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ilagay ang Kaarawan.")]
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Ilagay ang Kumpletong Address.")]
        public string Address { get; set; } = string.Empty;

        [Display(Name = "Contact Number")]
        [Phone(ErrorMessage = "Maling format ng numero.")]
        public string? ContactNumber { get; set; }

        [Display(Name = "Civil Status")]
        public string CivilStatus { get; set; } = "Single";
    }
}