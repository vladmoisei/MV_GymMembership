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
    public class AppPersoanaController : Controller
    {
        private readonly ReportDbContext _context;

        public AppPersoanaController(ReportDbContext context)
        {
            _context = context;
        }

        // GET: AppPersoana
        public async Task<IActionResult> Index()
        {
            List<PersoanaModel> reportDbContext = await _context.PersoanaModels.ToListAsync();
            ViewBag.dataSource = reportDbContext;
            return View(reportDbContext);
        }

        // GET: AppPersoana/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persoanaModel = await _context.PersoanaModels
                .FirstOrDefaultAsync(m => m.PersoanaModelID == id);
            if (persoanaModel == null)
            {
                return NotFound();
            }

            return View(persoanaModel);
        }

        // GET: AppPersoana/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AppPersoana/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersoanaModelID,Nume,Prenume,Email,Telefon,DataNastere,Sex,IsAcordGDPR")] PersoanaModel persoanaModel)
        {
            if (ModelState.IsValid)
            {
                PersoanaModel isRegistredPers = _context.PersoanaModels.FirstOrDefault(pers => pers.Email == persoanaModel.Email);
                if ( isRegistredPers != null)
                {
                    ModelState.AddModelError($"User:", $"Email inregistrat pe numele {isRegistredPers.NumeComplet}");
                    return View(persoanaModel);
                }
                _context.Add(persoanaModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(persoanaModel);
        }

        // GET: AppPersoana/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persoanaModel = await _context.PersoanaModels.FindAsync(id);
            if (persoanaModel == null)
            {
                return NotFound();
            }
            return View(persoanaModel);
        }

        // POST: AppPersoana/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PersoanaModelID,Nume,Prenume,Email,Telefon,DataNastere,Sex,IsAcordGDPR")] PersoanaModel persoanaModel)
        {
            if (id != persoanaModel.PersoanaModelID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(persoanaModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersoanaModelExists(persoanaModel.PersoanaModelID))
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
            return View(persoanaModel);
        }

        // GET: AppPersoana/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persoanaModel = await _context.PersoanaModels
                .FirstOrDefaultAsync(m => m.PersoanaModelID == id);
            if (persoanaModel == null)
            {
                return NotFound();
            }

            return View(persoanaModel);
        }

        // POST: AppPersoana/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var persoanaModel = await _context.PersoanaModels.FindAsync(id);
            _context.PersoanaModels.Remove(persoanaModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersoanaModelExists(int id)
        {
            return _context.PersoanaModels.Any(e => e.PersoanaModelID == id);
        }
    }
}
