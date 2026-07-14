using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BarangayCMS.DAL.Context;
using BarangayCMS.Entities;
using BarangayCMS.Web.Areas.Admin.Models;

namespace BarangayCMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CertificatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CertificatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. GET: Admin/Certificates
        public async Task<IActionResult> Index()
        {
            var certificates = await _context.Certificates
                .Include(c => c.Resident)
                .Select(c => new CertificateViewModel
                {
                    Id = c.CertificateId,
                    ResidentId = c.ResidentId,
                    ResidentFullName = c.Resident != null ? $"{c.Resident.LastName}, {c.Resident.FirstName}" : "Unknown Resident",
                    CertificateType = c.CertificateType,
                    DateRequested = c.DateRequested,
                    DateIssued = c.DateIssued,
                    Status = c.Status
                }).ToListAsync();

            return View(certificates);
        }

        // 2. GET: Admin/Certificates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var cert = await _context.Certificates
                .Include(c => c.Resident)
                .FirstOrDefaultAsync(m => m.CertificateId == id);

            if (cert == null) return NotFound();

            var viewModel = new CertificateViewModel
            {
                Id = cert.CertificateId,
                ResidentId = cert.ResidentId,
                ResidentFullName = cert.Resident != null ? $"{cert.Resident.LastName}, {cert.Resident.FirstName} {cert.Resident.MiddleName}" : "Unknown Resident",
                CertificateType = cert.CertificateType,
                DateRequested = cert.DateRequested,
                DateIssued = cert.DateIssued,
                Status = cert.Status
            };

            return View(viewModel);
        }

        // 3. GET: Admin/Certificates/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new CertificateViewModel
            {
                DateRequested = DateTime.Now,
                ResidentList = await GetResidentSelectList()
            };
            return View(viewModel);
        }

        // 4. POST: Admin/Certificates/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CertificateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var certificate = new Certificate
                {
                    ResidentId = model.ResidentId,
                    CertificateType = model.CertificateType,
                    DateRequested = model.DateRequested,
                    DateIssued = model.Status == "Issued" ? model.DateIssued ?? DateTime.Now : model.DateIssued,
                    Status = model.Status
                };

                _context.Add(certificate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            model.ResidentList = await GetResidentSelectList();
            return View(model);
        }

        // 5. GET: Admin/Certificates/Generate/5
        public async Task<IActionResult> Generate(int? id)
        {
            if (id == null) return NotFound();

            var cert = await _context.Certificates
                .Include(c => c.Resident)
                .FirstOrDefaultAsync(m => m.CertificateId == id);

            if (cert == null) return NotFound();

            var viewModel = new CertificateViewModel
            {
                Id = cert.CertificateId,
                ResidentId = cert.ResidentId,
                ResidentFullName = cert.Resident != null ? $"{cert.Resident.LastName}, {cert.Resident.FirstName}" : "Unknown Resident",
                CertificateType = cert.CertificateType,
                DateRequested = cert.DateRequested,
                DateIssued = cert.DateIssued ?? DateTime.Now,
                Status = cert.Status
            };

            return View(viewModel);
        }

        // 6. POST: Admin/Certificates/Generate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Generate(int id, CertificateViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var cert = await _context.Certificates.FindAsync(id);
                    if (cert == null) return NotFound();

                    cert.DateIssued = model.DateIssued ?? DateTime.Now;
                    cert.Status = "Issued";

                    _context.Update(cert);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Preview), new { id = cert.CertificateId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Certificates.Any(e => e.CertificateId == model.Id)) return NotFound();
                    else throw;
                }
            }
            return View(model);
        }

        // 7. GET: Admin/Certificates/Preview/5
        public async Task<IActionResult> Preview(int? id)
        {
            if (id == null) return NotFound();

            var cert = await _context.Certificates
                .Include(c => c.Resident)
                .FirstOrDefaultAsync(m => m.CertificateId == id);

            if (cert == null) return NotFound();

            var viewModel = new CertificateViewModel
            {
                Id = cert.CertificateId,
                ResidentFullName = cert.Resident != null ? $"{cert.Resident.FirstName} {cert.Resident.MiddleName} {cert.Resident.LastName}".ToUpper() : "UNKNOWN RESIDENT",
                CertificateType = cert.CertificateType,
                DateRequested = cert.DateRequested,
                DateIssued = cert.DateIssued,
                Status = cert.Status
            };

            return View(viewModel);
        }

        // 8. GET: Admin/Certificates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var cert = await _context.Certificates
                .Include(c => c.Resident)
                .FirstOrDefaultAsync(m => m.CertificateId == id);

            if (cert == null) return NotFound();

            var viewModel = new CertificateViewModel
            {
                Id = cert.CertificateId,
                ResidentFullName = cert.Resident != null ? $"{cert.Resident.LastName}, {cert.Resident.FirstName}" : "Unknown Resident",
                CertificateType = cert.CertificateType,
                Status = cert.Status
            };

            return View(viewModel);
        }

        // 9. POST: Admin/Certificates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cert = await _context.Certificates.FindAsync(id);
            if (cert != null)
            {
                _context.Certificates.Remove(cert);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetResidentSelectList()
        {
            return await _context.Residents
                .OrderBy(r => r.LastName)
                .Select(r => new SelectListItem
                {
                    Value = r.ResidentId.ToString(),
                    Text = $"{r.LastName}, {r.FirstName}"
                }).ToListAsync();
        }
    }
}