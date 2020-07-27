using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCWithBlazor.Data;
using MVCWithBlazor.Models;
using MVCWithBlazor.Services;

namespace MVCWithBlazor.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ReportDbContext _context;
        private readonly AntrenamentService _antrenamentService;

        public ReportsController(ReportDbContext context, AntrenamentService antrenamentService)
        {
            _context = context;
            _antrenamentService = antrenamentService;
        }
        public IActionResult Index()
        {
            return View();
        }

        // Index: AppAbonament
        public async Task<IActionResult> IndexAbonament()
        {
            // Refresh Status for each not Finalised Abonament
            _antrenamentService.RefreshStatusAbonamentsActive(_context);

            ViewBag.DataStart = DateTime.Now.ToString("yyyy-MM");
            var reportDbContext = _context.AbonamentModels.Include(t => t.TipAbonament).Include(a => a.PersoanaModel).Where(m => m.DataStart.Year == DateTime.Now.Year && m.DataStart.Month == DateTime.Now.Month);

            List<AbonamentViewmodel> listDeAfisat = new List<AbonamentViewmodel>();
            foreach (var item in reportDbContext)
            {
                listDeAfisat.Add(new AbonamentViewmodel
                {
                    AbonamentModelID = item.AbonamentModelID,
                    DataStart = item.DataStart,
                    DataStop = item.DataStop,
                    StareAbonament = item.StareAbonament,
                    NrSedinteEfectuate = item.NrSedinteEfectuate,
                    TipAbonamentModelID = item.TipAbonamentModelID,
                    TipAbonament = item.TipAbonament,
                    PersoanaModelID = item.PersoanaModelID,
                    PersoanaModel = item.PersoanaModel,
                    ComenziHtml = String.Format("<a href=\'/AppAbonament/Edit/{0}\'>Edit</a> |" +
                    "< a href = \'/AppAbonament/Details/{0}\' > Details </ a > |" +
                    "< a href = \'/AppAbonament/Delete/{0}\' > Delete </ a >", item.AbonamentModelID)
                });
            }

            //ViewBag.dataSource = reportDbContext.ToList();
            //return View(await reportDbContext.ToListAsync());
            ViewBag.dataSource = listDeAfisat;
            return View(listDeAfisat);
        }

        // POSt: AppAbonament
        [HttpPost]
        public async Task<IActionResult> IndexAbonament(DateTime startMonth)
        {
            // Refresh Status for each not Finalised Abonament
            _antrenamentService.RefreshStatusAbonamentsActive(_context);

            ViewBag.DataStart = startMonth.ToString("yyyy-MM");
            // Facem selectie abonamente in functie de data si astare abonament
            var reportDbContext = _context.AbonamentModels.Include(t => t.TipAbonament).Include(a => a.PersoanaModel).Where(m => m.DataStart.Year == startMonth.Year && m.DataStart.Month == startMonth.Month);
            //switch (stareAb)
            //{
            //    case HelperStareAbonament.Activ:
            //        reportDbContext = reportDbContext.Where(m => m.StareAbonament == StareAbonament.Activ);
            //        break;
            //    case HelperStareAbonament.Finalizat:
            //        reportDbContext = reportDbContext.Where(m => m.StareAbonament == StareAbonament.Finalizat);
            //        break;
            //    case HelperStareAbonament.Extins:
            //        reportDbContext = reportDbContext.Where(m => m.StareAbonament == StareAbonament.Extins);
            //        break;
            //    default:
            //        break;
            //}
            return View(await reportDbContext.ToListAsync());
        }
    }
}
