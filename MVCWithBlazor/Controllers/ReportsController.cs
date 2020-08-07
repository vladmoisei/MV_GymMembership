using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCWithBlazor.Data;
using MVCWithBlazor.Models;
using MVCWithBlazor.Services;

namespace MVCWithBlazor.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly ReportDbContext _context;
        private readonly AntrenamentService _antrenamentService;
        private readonly ReportService _reportService;

        public ReportsController(ReportDbContext context, AntrenamentService antrenamentService, ReportService reportService)
        {
            _context = context;
            _antrenamentService = antrenamentService;
            _reportService = reportService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> RaportLunar()
        {
            // Refresh Status for each not Finalised Abonament
            _antrenamentService.RefreshStatusAbonamentsActive(_context);

            ViewBag.DataStart = DateTime.Now.ToString("yyyy-MM");

            ReportViewModel reportViewModel = new ReportViewModel();
            reportViewModel.ListaAbonamente = _reportService.GetListReportAbViewModelsPerMonth(_context, DateTime.Now);
            reportViewModel.ListaAntrenamente = _reportService.GetListReportAntrViewModelsPerMonth(_context, DateTime.Now);
            return View(reportViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> RaportLunar(DateTime startMonth)
        {
            ViewBag.DataStart = startMonth.ToString("yyyy-MM");
            ReportViewModel reportViewModel = new ReportViewModel();
            reportViewModel.ListaAbonamente = _reportService.GetListReportAbViewModelsPerMonth(_context, startMonth);
            reportViewModel.ListaAntrenamente = _reportService.GetListReportAntrViewModelsPerMonth(_context, startMonth);
            return View(reportViewModel);
        }

        public async Task<IActionResult> RaportAnual()
        {
            // Refresh Status for each not Finalised Abonament
            _antrenamentService.RefreshStatusAbonamentsActive(_context);

            ViewBag.DataStart = DateTime.Now.ToString("yyyy-MM");

            ReportViewModel reportViewModel = new ReportViewModel();
            reportViewModel.ListaAbonamente = _reportService.GetListReportAbViewModelsPerYear(_context, DateTime.Now);
            reportViewModel.ListaAntrenamente = _reportService.GetListReportAntrViewModelsPerYear(_context, DateTime.Now);
            return View(reportViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> RaportAnual(DateTime startMonth)
        {
            ViewBag.DataStart = startMonth.ToString("yyyy-MM");
            ReportViewModel reportViewModel = new ReportViewModel();
            reportViewModel.ListaAbonamente = _reportService.GetListReportAbViewModelsPerYear(_context, startMonth);
            reportViewModel.ListaAntrenamente = _reportService.GetListReportAntrViewModelsPerYear(_context, startMonth);
            return View(reportViewModel);
        }
    }
}
