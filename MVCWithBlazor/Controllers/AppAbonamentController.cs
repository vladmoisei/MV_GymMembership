using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCWithBlazor.Data;
using MVCWithBlazor.Models;

namespace MVCWithBlazor.Controllers
{
    public class AppAbonamentController : Controller
    {
        private readonly ReportDbContext _context;

        public AppAbonamentController(ReportDbContext context)
        {
            _context = context;
        }

        // GET: AppAbonament
        public async Task<IActionResult> Index()
        {
            ViewBag.DataStart = DateTime.Now.ToString("yyyy-MM");
            var reportDbContext = _context.AbonamentModels.Include(t => t.TipAbonament).Include(a => a.PersoanaModel);
            return View(await reportDbContext.ToListAsync());
        }

        // GET: AppAbonament/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var abonamentModel = await _context.AbonamentModels
                .Include(t => t.TipAbonament)
                .Include(a => a.PersoanaModel)
                .FirstOrDefaultAsync(m => m.AbonamentModelID == id);
            if (abonamentModel == null)
            {
                return NotFound();
            }

            return View(abonamentModel);
        }

        // GET: AppAbonament/Create
        public IActionResult Create()
        {
            ViewData["TipAbonamentModelID"] = new SelectList(_context.TipAbonamentModels, "TipAbonamentModelID", "Denumire");
            ViewData["PersoanaModelID"] = new SelectList(_context.PersoanaModels, "PersoanaModelID", "NumeComplet");
            return View();
        }

        // POST: AppAbonament/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AbonamentModelID,DataStart,TipAbonamentModelID,PersoanaModelID")] AbonamentModel abonamentModel)
        {
            if (ModelState.IsValid)
            {
                TipAbonamentModel tipAb = _context.TipAbonamentModels.FirstOrDefault(a => a.TipAbonamentModelID == abonamentModel.TipAbonamentModelID);
                abonamentModel.DataStop = new DateTime(abonamentModel.DataStart.Year, abonamentModel.DataStart.Month + 1, abonamentModel.DataStart.Day);
                abonamentModel.StareAbonament = StareAbonament.Activ;
                abonamentModel.NrSedinteEfectuate = 0;
                if (tipAb.NrTotalSedinte == 1) // Daca facem 1 abonament cu o singura sedinta il incheiem pe loc, nu mai e nevoie
                    // sa il incheiem din antrenament
                {
                    abonamentModel.DataStop = abonamentModel.DataStart;
                    abonamentModel.StareAbonament = StareAbonament.Finalizat;
                    abonamentModel.NrSedinteEfectuate = 1;
                }
                // Daca persoana nu are atrenament Finalizat nu putem sa o adaugam pe un alt abonament
                AbonamentModel isAbExpiredPers = _context.AbonamentModels.Include(p => p.PersoanaModel)
                    .Include(t => t.TipAbonament)
                    .FirstOrDefault(ab => ab.PersoanaModelID == abonamentModel.PersoanaModelID);
                if (isAbExpiredPers != null)
                    if (isAbExpiredPers.StareAbonament == StareAbonament.Activ || isAbExpiredPers.StareAbonament == StareAbonament.Extins)
                    {
                        ModelState.AddModelError($"User:", $"Persoana are abonament: {isAbExpiredPers.StareAbonament} si expira pe: " +
                            isAbExpiredPers.DataStop.ToString("dd.MM.yyyy"));
                        return View(abonamentModel);
                    }
                _context.Add(abonamentModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipAbonamentModelID"] = new SelectList(_context.TipAbonamentModels, "TipAbonamentModelID", "Denumire", abonamentModel.TipAbonamentModelID);
            ViewData["PersoanaModelID"] = new SelectList(_context.PersoanaModels, "PersoanaModelID", "NumeComplet", abonamentModel.PersoanaModelID);
            return View(abonamentModel);
        }

        // GET: AppAbonament/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var abonamentModel = await _context.AbonamentModels.FindAsync(id);
            if (abonamentModel == null)
            {
                return NotFound();
            }
            ViewData["TipAbonamentModelID"] = new SelectList(_context.TipAbonamentModels, "TipAbonamentModelID", "Denumire", abonamentModel.TipAbonamentModelID);
            ViewData["PersoanaModelID"] = new SelectList(_context.PersoanaModels, "PersoanaModelID", "NumeComplet", abonamentModel.PersoanaModelID);
            return View(abonamentModel);
        }

        // POST: AppAbonament/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AbonamentModelID,DataStart,DataStop,StareAbonament,NrSedinteEfectuate,TipAbonamentModelID,PersoanaModelID")] AbonamentModel abonamentModel)
        {
            if (id != abonamentModel.AbonamentModelID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(abonamentModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AbonamentModelExists(abonamentModel.AbonamentModelID))
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
            ViewData["TipAbonamentModelID"] = new SelectList(_context.TipAbonamentModels, "TipAbonamentModelID", "Denumire", abonamentModel.TipAbonamentModelID);
            ViewData["PersoanaModelID"] = new SelectList(_context.PersoanaModels, "PersoanaModelID", "NumeComplet", abonamentModel.PersoanaModelID);
            return View(abonamentModel);
        }

        // GET: AppAbonament/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var abonamentModel = await _context.AbonamentModels
                .Include(t => t.TipAbonament)
                .Include(a => a.PersoanaModel)
                .FirstOrDefaultAsync(m => m.AbonamentModelID == id);
            if (abonamentModel == null)
            {
                return NotFound();
            }

            return View(abonamentModel);
        }

        // POST: AppAbonament/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var abonamentModel = await _context.AbonamentModels.FindAsync(id);
            _context.AbonamentModels.Remove(abonamentModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AbonamentModelExists(int id)
        {
            return _context.AbonamentModels.Any(e => e.AbonamentModelID == id);
        }
    }
}
