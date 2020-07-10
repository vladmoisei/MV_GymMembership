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
    public class AppTipAbonamentController : Controller
    {
        private readonly ReportDbContext _context;

        public AppTipAbonamentController(ReportDbContext context)
        {
            _context = context;
        }

        // GET: AppTipAbonament
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipAbonamentModels.ToListAsync());
        }

        // GET: AppTipAbonament/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipAbonamentModel = await _context.TipAbonamentModels
                .FirstOrDefaultAsync(m => m.TipAbonamentModelID == id);
            if (tipAbonamentModel == null)
            {
                return NotFound();
            }

            return View(tipAbonamentModel);
        }

        // GET: AppTipAbonament/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AppTipAbonament/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TipAbonamentModelID,Denumire,NrTotalSedinte,IsPersonalTraining,Pret")] TipAbonamentModel tipAbonamentModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipAbonamentModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipAbonamentModel);
        }

        // GET: AppTipAbonament/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipAbonamentModel = await _context.TipAbonamentModels.FindAsync(id);
            if (tipAbonamentModel == null)
            {
                return NotFound();
            }
            return View(tipAbonamentModel);
        }

        // POST: AppTipAbonament/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TipAbonamentModelID,Denumire,NrTotalSedinte,IsPersonalTraining,Pret")] TipAbonamentModel tipAbonamentModel)
        {
            if (id != tipAbonamentModel.TipAbonamentModelID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipAbonamentModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipAbonamentModelExists(tipAbonamentModel.TipAbonamentModelID))
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
            return View(tipAbonamentModel);
        }

        // GET: AppTipAbonament/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipAbonamentModel = await _context.TipAbonamentModels
                .FirstOrDefaultAsync(m => m.TipAbonamentModelID == id);
            if (tipAbonamentModel == null)
            {
                return NotFound();
            }

            return View(tipAbonamentModel);
        }

        // POST: AppTipAbonament/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipAbonamentModel = await _context.TipAbonamentModels.FindAsync(id);
            _context.TipAbonamentModels.Remove(tipAbonamentModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipAbonamentModelExists(int id)
        {
            return _context.TipAbonamentModels.Any(e => e.TipAbonamentModelID == id);
        }
    }
}
