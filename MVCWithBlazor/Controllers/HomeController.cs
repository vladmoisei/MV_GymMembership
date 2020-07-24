using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCWithBlazor.Data;
using MVCWithBlazor.Models;
using MVCWithBlazor.Services;

namespace MVCWithBlazor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        AntrenamentService _antrenamentService;
        ReportDbContext _context;

        public HomeController(ILogger<HomeController> logger, AntrenamentService antrenamentService, ReportDbContext context)
        {
            _logger = logger;
            _antrenamentService = antrenamentService;
            _context = context;

            // Refresh Status for each not Finalised Abonament
            _antrenamentService.RefreshStatusAbonamentsActive(_context);
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Member, Admin")]
        [Authorize(Policy = "Dep")]
        public IActionResult Member()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return View();
        }
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
