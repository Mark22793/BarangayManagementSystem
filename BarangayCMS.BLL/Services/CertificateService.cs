using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarangayCMS.BLL.Interfaces;
using BarangayCMS.DAL.Repository.Interfaces;
using BarangayCMS.DTO;
using BarangayCMS.Entities;

namespace BarangayCMS.BLL.Services
{
    public class CertificateService : ICertificateService
    {
        private readonly ICertificateRepository _certRepo;

        public CertificateService(ICertificateRepository certRepo)
        {
            _certRepo = certRepo;
        }

        public async Task<CertificateDTO?> GetCertificateByIdAsync(int id)
        {
            var cert = await _certRepo.GetByIdAsync(id);
            if (cert == null) return null;

            return new CertificateDTO
            {
                Id = cert.CertificateId,
                ResidentId = cert.ResidentId,
                ResidentName = cert.Resident != null ? $"{cert.Resident.FirstName} {cert.Resident.LastName}" : "Unknown",
                CertificateType = cert.CertificateType,
                Purpose = cert.Purpose,
                ControlNumber = cert.ControlNumber,
                FeePaid = cert.FeePaid,
                OfficialReceiptNumber = cert.OfficialReceiptNumber,
                Status = cert.Status,
                IssuedDate = cert.DateIssued ?? DateTime.MinValue,
                IssuedBy = cert.IssuedBy
            };
        }

        public async Task<IEnumerable<CertificateDTO>> GetAllCertificatesAsync()
        {
            var certs = await _certRepo.GetAllWithResidentAsync();
            return certs.Select(cert => new CertificateDTO
            {
                Id = cert.CertificateId,
                ResidentId = cert.ResidentId,
                ResidentName = cert.Resident != null ? $"{cert.Resident.FirstName} {cert.Resident.LastName}" : "Unknown",
                CertificateType = cert.CertificateType,
                Purpose = cert.Purpose,
                ControlNumber = cert.ControlNumber,
                FeePaid = cert.FeePaid,
                OfficialReceiptNumber = cert.OfficialReceiptNumber,
                Status = cert.Status,
                IssuedDate = cert.DateIssued ?? DateTime.MinValue,
                IssuedBy = cert.IssuedBy
            });
        }

        public async Task<IEnumerable<CertificateDTO>> GetCertificatesByResidentAsync(int residentId)
        {
            var certs = await _certRepo.GetByResidentIdAsync(residentId);
            return certs.Select(cert => new CertificateDTO
            {
                Id = cert.CertificateId,
                ResidentId = cert.ResidentId,
                CertificateType = cert.CertificateType,
                Purpose = cert.Purpose,
                ControlNumber = cert.ControlNumber,
                Status = cert.Status,
                IssuedDate = cert.DateIssued ?? DateTime.MinValue
            });
        }

        public async Task<bool> RequestCertificateAsync(CertificateDTO dto)
        {
            var cert = new Certificate
            {
                ResidentId = dto.ResidentId,
                CertificateType = dto.CertificateType,
                Purpose = dto.Purpose,
                FeePaid = dto.FeePaid,
                OfficialReceiptNumber = dto.OfficialReceiptNumber,
                Status = "Pending",
                DateRequested = DateTime.Now
            };

            await _certRepo.AddAsync(cert);
            return await _certRepo.SaveChangesAsync();
        }

        public async Task<bool> IssueCertificateAsync(int id, string controlNumber, string issuedBy)
        {
            var cert = await _certRepo.GetByIdAsync(id);
            if (cert == null) return false;

            cert.ControlNumber = controlNumber;
            cert.IssuedBy = issuedBy;
            cert.Status = "Issued";
            cert.DateIssued = DateTime.Now;

            _certRepo.Update(cert);
            return await _certRepo.SaveChangesAsync();
        }

        public async Task<bool> UpdateStatusAsync(int id, string status)
        {
            var cert = await _certRepo.GetByIdAsync(id);
            if (cert == null) return false;

            cert.Status = status;
            _certRepo.Update(cert);
            return await _certRepo.SaveChangesAsync();
        }
    }
}