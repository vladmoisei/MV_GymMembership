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
            var reportDbContext = _context.AbonamentModels.Include(a => a.PersoanaModel);
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
            ViewData["PersoanaModelID"] = new SelectList(_context.PersoanaModels, "PersoanaModelID", "NumeComplet");
            return View();
        }

        // POST: AppAbonament/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AbonamentModelID,DataStart,PersoanaModelID")] AbonamentModel abonamentModel)
        {
            if (ModelState.IsValid)
            {
                abonamentModel.DataStop = new DateTime(abonamentModel.DataStart.Year, abonamentModel.DataStart.Month + 1, abonamentModel.DataStart.Day);
                abonamentModel.StareAbonament = StareAbonament.Activ;
                abonamentModel.NrSedinteEfectuate = 0;
                _context.Add(abonamentModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
            ViewData["PersoanaModelID"] = new SelectList(_context.PersoanaModels, "PersoanaModelID", "NumeComplet", abonamentModel.PersoanaModelID);
            return View(abonamentModel);
        }

        // POST: AppAbonament/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AbonamentModelID,DataStart,DataStop,StareAbonament,NrSedinteEfectuate,PersoanaModelID")] AbonamentModel abonamentModel)
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
