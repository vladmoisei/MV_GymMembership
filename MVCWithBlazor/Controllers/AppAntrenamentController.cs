using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCWithBlazor.Data;
using MVCWithBlazor.Models;
using MVCWithBlazor.Services;

namespace MVCWithBlazor.Controllers
{
    public class AppAntrenamentController : Controller
    {
        private readonly ReportDbContext _context;
        private readonly AntrenamentService _antrenamentService;

        public AppAntrenamentController(ReportDbContext context, AntrenamentService antrenamentService)
        {
            _context = context;
            _antrenamentService = antrenamentService;
        }

        // GET: AppAntrenament
        public async Task<IActionResult> Index()
        {
            ViewBag.DataStart = DateTime.Now.ToString("yyyy-MM");
            var lista = _antrenamentService.GetAntrenamentModelsByMonth(DateTime.Now, _context).Result;
            return View(lista);
            //return View(await _context.AntrenamentModels.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Index(DateTime startMonth)
        {
            ViewBag.DataStart = startMonth.ToString("yyyy-MM");
            var lista = _antrenamentService.GetAntrenamentModelsByMonth(startMonth, _context).Result;
            return View(lista);
            //return View(await _context.AntrenamentModels.ToListAsync());
        }

        // GET: AppAntrenament zilnic
        public async Task<IActionResult> IndexZilnic()
        {
            ViewBag.DataStart = DateTime.Now.ToString("yyyy-MM-dd");
            var lista = _antrenamentService.GetAntrenamentModelsByDay(DateTime.Now, _context).Result;
            return View(lista);
        }

        [HttpPost] // Post: AppAntrenament zilnic cu adaugare User per antrenament
        public async Task<IActionResult> IndexZilnic(DateTime startMonth)
        {
            ViewBag.DataStart = startMonth.ToString("yyyy-MM-dd");
            var lista = _antrenamentService.GetAntrenamentModelsByDay(startMonth, _context).Result;
            return View(lista);
            //return View(await _context.AntrenamentModels.ToListAsync());
        }

        // GET: AppAntrenament/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var antrenamentModel = await _context.AntrenamentModels
                .FirstOrDefaultAsync(m => m.AntrenamentModelID == id);
            if (antrenamentModel == null)
            {
                return NotFound();
            }

            return View(antrenamentModel);
        }

        // GET: AppAntrenament/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AppAntrenament/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AntrenamentModelID,Data,OraStart,OraStop,IsPersonalTraining,Grupa")] AntrenamentModel antrenamentModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(antrenamentModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexZilnic));
            }
            return View(antrenamentModel);
        }

        // GET: AppAntrenament/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var antrenamentModel = await _context.AntrenamentModels.FindAsync(id);
            if (antrenamentModel == null)
            {
                return NotFound();
            }
            return View(antrenamentModel);
        }

        // POST: AppAntrenament/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AntrenamentModelID,Data,OraStart,OraStop,IsPersonalTraining,Grupa")] AntrenamentModel antrenamentModel)
        {
            if (id != antrenamentModel.AntrenamentModelID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(antrenamentModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AntrenamentModelExists(antrenamentModel.AntrenamentModelID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(antrenamentModel);
        }

        // GET: AppAntrenament/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var antrenamentModel = await _context.AntrenamentModels
                .FirstOrDefaultAsync(m => m.AntrenamentModelID == id);
            if (antrenamentModel == null)
            {
                return NotFound();
            }

            return View(antrenamentModel);
        }

        // POST: AppAntrenament/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var antrenamentModel = await _context.AntrenamentModels.FindAsync(id);
            _context.AntrenamentModels.Remove(antrenamentModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AntrenamentModelExists(int id)
        {
            return _context.AntrenamentModels.Any(e => e.AntrenamentModelID == id);
        }
    }
}
