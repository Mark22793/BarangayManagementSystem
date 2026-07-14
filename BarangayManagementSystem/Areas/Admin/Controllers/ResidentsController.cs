using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BarangayCMS.DAL.Context;
using BarangayCMS.Entities;
using BarangayCMS.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization; // NAPAKAHALAGA: Para magamit ang [Authorize]
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarangayCMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // CRITICAL: Tanging ang naka-login na Admin lang ang pwedeng makakita o makagalaw nito!
    public class ResidentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResidentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. GET: Admin/Residents
        public async Task<IActionResult> Index()
        {
            var residents = await _context.Residents
                .Select(r => new ResidentViewModel
                {
                    Id = r.ResidentId,
                    FirstName = r.FirstName,
                    LastName = r.LastName + (!string.IsNullOrEmpty(r.Suffix) ? " " + r.Suffix : ""),
                    MiddleName = r.MiddleName,
                    Gender = r.Gender,
                    BirthDate = r.BirthDate,
                    Address = $"{r.HouseNumber} {r.Street}, {r.SitioPurok}".Trim(' ', ','),
                    ContactNumber = r.ContactNumber,
                    CivilStatus = r.CivilStatus
                }).ToListAsync();

            return View(residents);
        }

        // 2. GET: Admin/Residents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var resident = await _context.Residents
                .FirstOrDefaultAsync(m => m.ResidentId == id);

            if (resident == null) return NotFound();

            var viewModel = new ResidentViewModel
            {
                Id = resident.ResidentId,
                FirstName = resident.FirstName,
                LastName = resident.LastName + (!string.IsNullOrEmpty(resident.Suffix) ? " " + resident.Suffix : ""),
                MiddleName = resident.MiddleName,
                Gender = resident.Gender,
                BirthDate = resident.BirthDate,
                Address = $"{resident.HouseNumber} {resident.Street}, {resident.SitioPurok}".Trim(' ', ','),
                ContactNumber = resident.ContactNumber,
                CivilStatus = resident.CivilStatus
            };

            return View(viewModel);
        }

        // 3. GET: Admin/Residents/Create
        public IActionResult Create()
        {
            return View();
        }

        // 4. POST: Admin/Residents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResidentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resident = new Resident
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    MiddleName = model.MiddleName ?? string.Empty,
                    Gender = model.Gender,
                    BirthDate = model.BirthDate,
                    CivilStatus = model.CivilStatus,
                    ContactNumber = model.ContactNumber ?? string.Empty,
                    Street = model.Address ?? string.Empty,
                    HouseNumber = string.Empty,
                    SitioPurok = string.Empty,
                    Suffix = string.Empty,
                    Email = string.Empty,
                    IsVoter = false,
                    IsResident = true,
                    CreatedAt = DateTime.Now
                };

                _context.Residents.Add(resident);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // 5. GET: Admin/Residents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var resident = await _context.Residents.FindAsync(id);
            if (resident == null) return NotFound();

            var viewModel = new ResidentViewModel
            {
                Id = resident.ResidentId,
                FirstName = resident.FirstName,
                LastName = resident.LastName,
                MiddleName = resident.MiddleName,
                Gender = resident.Gender,
                BirthDate = resident.BirthDate,
                Address = !string.IsNullOrEmpty(resident.HouseNumber) || !string.IsNullOrEmpty(resident.SitioPurok)
                    ? $"{resident.HouseNumber} {resident.Street}, {resident.SitioPurok}".Trim(' ', ',')
                    : resident.Street,
                ContactNumber = resident.ContactNumber,
                CivilStatus = resident.CivilStatus
            };

            return View(viewModel);
        }

        // 6. POST: Admin/Residents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ResidentViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var resident = await _context.Residents.FindAsync(id);
                    if (resident == null) return NotFound();

                    resident.FirstName = model.FirstName;
                    resident.LastName = model.LastName;
                    resident.MiddleName = model.MiddleName ?? string.Empty;
                    resident.Gender = model.Gender;
                    resident.BirthDate = model.BirthDate;
                    resident.CivilStatus = model.CivilStatus;
                    resident.ContactNumber = model.ContactNumber ?? string.Empty;
                    resident.Street = model.Address ?? string.Empty;

                    _context.Residents.Update(resident);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Residents.Any(e => e.ResidentId == model.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // 7. GET: Admin/Residents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var resident = await _context.Residents
                .FirstOrDefaultAsync(m => m.ResidentId == id);

            if (resident == null) return NotFound();

            var viewModel = new ResidentViewModel
            {
                Id = resident.ResidentId,
                FirstName = resident.FirstName,
                LastName = resident.LastName + (!string.IsNullOrEmpty(resident.Suffix) ? " " + resident.Suffix : ""),
                MiddleName = resident.MiddleName,
                Gender = resident.Gender,
                BirthDate = resident.BirthDate,
                Address = $"{resident.HouseNumber} {resident.Street}, {resident.SitioPurok}".Trim(' ', ',')
            };

            return View(viewModel);
        }

        // 8. POST: Admin/Residents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resident = await _context.Residents.FindAsync(id);
            if (resident != null)
            {
                _context.Residents.Remove(resident);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}