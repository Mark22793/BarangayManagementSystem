using Microsoft.AspNetCore.Mvc;
using BarangayCMS.BLL.Interfaces;

namespace BarangayCMS.Web.Controllers
{
    public class CertificateRequestController : Controller
    {
        private readonly ICertificateService _certificateService;

        public CertificateRequestController(ICertificateService certificateService)
        {
            _certificateService = certificateService;
        }

        public IActionResult Requirements()
        {
            return View();
        }

        public IActionResult Fees()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
