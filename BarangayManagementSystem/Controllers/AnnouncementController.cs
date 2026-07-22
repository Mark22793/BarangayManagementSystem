using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BarangayCMS.DAL.Context; // Siguraduhing tama ang namespace ng iyong ApplicationDbContext

namespace BarangayManagementSystem.Controllers
{
    public class AnnouncementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnnouncementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Announcement
        public async Task<IActionResult> Index()
        {
            // Kukunin ang mga announcements na pinaka-bagong pinost (baba ang PublishDate)
            var announcements = await _context.Announcements
                .OrderByDescending(a => a.PublishDate)
                .ToListAsync();

            return View(announcements);
        }
    }
}