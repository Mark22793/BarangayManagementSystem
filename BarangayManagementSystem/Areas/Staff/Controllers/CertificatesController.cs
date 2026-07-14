using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BarangayCMS.Areas.Staff.ViewModels;
using BarangayCMS.BLL.Interfaces;
using BarangayCMS.DTO;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BarangayCMS.Areas.Staff.Controllers
{
    [Area("Staff")]
    public class CertificatesController : Controller
    {
        private readonly ICertificateService _certificateService;
        private readonly IResidentService _residentService;

        public CertificatesController(ICertificateService certificateService, IResidentService residentService)
        {
            _certificateService = certificateService;
            _residentService = residentService;
        }

        // GET: /Staff/Certificates/Index
        public async Task<IActionResult> Index()
        {
            var dtoList = await _certificateService.GetAllCertificatesAsync();
            var viewModelList = dtoList.Select(c => new CertificateViewModel
            {
                CertificateId = c.Id,
                ResidentId = c.ResidentId,
                ResidentName = c.ResidentName,
                CertificateType = c.CertificateType,
                Purpose = c.Purpose,
                ControlNumber = c.ControlNumber,
                AmountPaid = c.FeePaid,
                OfficialReceiptNumber = c.OfficialReceiptNumber,
                Status = c.Status,
                DateIssued = c.IssuedDate != default ? c.IssuedDate : (DateTime?)null,
                IssuedBy = c.IssuedBy
            }).ToList();

            return View(viewModelList);
        }

        // 🔑 POST: /Staff/Certificates/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            // 1. Gumawa ng panibagong Control Number para sa sertipiko
            string controlNumber = $"BRGY-{DateTime.Now.ToString("yyyyMMdd")}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
            string currentStaff = User.Identity?.Name ?? "Staff Admin";

            // 2. Gamitin ang iyong service interface para i-update ang status bilang 'Approved' o 'Issued'
            // Gagamitin natin ang IssueCertificateAsync dahil tumatanggap ito ng control number at tagapirma
            bool isSuccess = await _certificateService.IssueCertificateAsync(id, controlNumber, currentStaff);

            if (!isSuccess)
            {
                // Kung walang pagbabago sa control number, subukan ang fallback na UpdateStatusAsync
                isSuccess = await _certificateService.UpdateStatusAsync(id, "Approved");
            }

            // 3. I-refresh at bumalik sa listahan kapag tapos na
            return RedirectToAction(nameof(Index));
        }

        // GET: /Staff/Certificates/Print/5
        public async Task<IActionResult> Print(int id)
        {
            var certificateDto = await _certificateService.GetCertificateByIdAsync(id);
            if (certificateDto == null) return NotFound();

            var controlNo = string.IsNullOrEmpty(certificateDto.ControlNumber)
                ? $"BRGY-{DateTime.Now.ToString("yyyyMMdd")}-{certificateDto.Id}"
                : certificateDto.ControlNumber;

            var viewModel = new CertificateViewModel
            {
                CertificateId = certificateDto.Id,
                ResidentId = certificateDto.ResidentId,
                ResidentName = certificateDto.ResidentName,
                CertificateType = certificateDto.CertificateType,
                Purpose = certificateDto.Purpose,
                ControlNumber = controlNo,
                AmountPaid = certificateDto.FeePaid,
                OfficialReceiptNumber = certificateDto.OfficialReceiptNumber,
                Status = certificateDto.Status,
                DateIssued = certificateDto.IssuedDate != default ? certificateDto.IssuedDate : DateTime.Now,
                IssuedBy = string.IsNullOrEmpty(certificateDto.IssuedBy) ? "PUNONG BARANGAY" : certificateDto.IssuedBy
            };

            return View(viewModel);
        }

        // GET: /Staff/Certificates/Create
        public async Task<IActionResult> Create()
        {
            var residents = await _residentService.GetAllResidentsAsync();
            ViewBag.ResidentsList = new SelectList(residents.Select(r => new {
                Id = r.Id,
                FullName = $"{r.LastName}, {r.FirstName} {r.MiddleName}"
            }), "Id", "FullName");

            return View(new CertificateViewModel());
        }

        // POST: /Staff/Certificates/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CertificateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newCertificateDto = new CertificateDTO
                {
                    ResidentId = model.ResidentId,
                    CertificateType = model.CertificateType ?? string.Empty,
                    Purpose = model.Purpose ?? string.Empty,
                    FeePaid = model.AmountPaid,
                    OfficialReceiptNumber = model.OfficialReceiptNumber ?? string.Empty,
                    Status = model.Status,
                    ControlNumber = model.Status == "Issued" || model.Status == "Approved" ? $"BRGY-{DateTime.Now.ToString("yyyyMMdd")}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}" : string.Empty,
                    IssuedDate = model.Status == "Issued" || model.Status == "Approved" ? DateTime.Now : default,
                    IssuedBy = User.Identity?.Name ?? "Staff Admin"
                };

                bool isSaved = await _certificateService.RequestCertificateAsync(newCertificateDto);
                if (isSaved) return RedirectToAction(nameof(Index));

                ModelState.AddModelError(string.Empty, "Nagkaroon ng problema sa pag-save.");
            }

            var residents = await _residentService.GetAllResidentsAsync();
            ViewBag.ResidentsList = new SelectList(residents.Select(r => new {
                Id = r.Id,
                FullName = $"{r.LastName}, {r.FirstName} {r.MiddleName}"
            }), "Id", "FullName");

            return View(model);
        }

        // GET: /Staff/Certificates/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var certificateDto = await _certificateService.GetCertificateByIdAsync(id);
            if (certificateDto == null) return NotFound();

            var viewModel = new CertificateViewModel
            {
                CertificateId = certificateDto.Id,
                ResidentId = certificateDto.ResidentId,
                ResidentName = certificateDto.ResidentName,
                CertificateType = certificateDto.CertificateType,
                Purpose = certificateDto.Purpose,
                ControlNumber = certificateDto.ControlNumber,
                AmountPaid = certificateDto.FeePaid,
                OfficialReceiptNumber = certificateDto.OfficialReceiptNumber,
                Status = certificateDto.Status,
                DateIssued = certificateDto.IssuedDate != default ? certificateDto.IssuedDate : (DateTime?)null,
                IssuedBy = certificateDto.IssuedBy
            };

            return View(viewModel);
        }
    }
}