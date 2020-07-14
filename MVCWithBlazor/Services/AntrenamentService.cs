using Microsoft.EntityFrameworkCore;
using MVCWithBlazor.Data;
using MVCWithBlazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCWithBlazor.Services
{
    public class AntrenamentService
    {
        // Get Antrenament By Month from Antrenament DB
        public async Task<List<AntrenamentModel>> GetAntrenamentModelsByMonth(DateTime date, ReportDbContext context)
        {
            IEnumerable<AntrenamentModel> reportDbContext = await Task.FromResult(context.AntrenamentModels.Where(m => m.Data.Year == date.Year && m.Data.Month == date.Month));
            return reportDbContext.ToList();
        }

        // Get Antrenament By Day from Antrenament DB
        public async Task<List<AntrenamentModel>> GetAntrenamentModelsByDay(DateTime date, ReportDbContext context)
        {
            IEnumerable<AntrenamentModel> reportDbContext = await Task.FromResult(context.AntrenamentModels.Where(m => m.Data.Year == date.Year && m.Data.Month == date.Month && m.Data.Day == date.Day));
            return reportDbContext.ToList();
        }

        // Get List Of Persons By Antrenament
        public async Task<List<PersoanaModel>> GetListOfPersByAntrID(int antrenamentID, ReportDbContext context)
        {
            IEnumerable<PersoanaModel> listaPersoane = await Task.FromResult(context.PersAntrAbTables.Include(p => p.Persoana).Include(ab => ab.Abonament).Include(antr => antr.Antrenament).Where(item => item.AntrenamentModelID == antrenamentID).Select(pers => pers.Persoana));

            return listaPersoane.ToList();
        }

        // Get Abonament By Person
        public async Task<AbonamentModel> GetAbonamentByPersonID(int persoanaID, ReportDbContext context)
        {
            AbonamentModel abonament = await Task.FromResult(context.AbonamentModels.Include(t => t.TipAbonament).Include(p => p.PersoanaModel).LastOrDefault(p => p.PersoanaModelID == persoanaID && p.StareAbonament != StareAbonament.Finalizat));

            return abonament;
        }

        // Get string Text nr sedinte efectuate By PersonID
        public async Task<string> GetNrSedinteEfByPersonID(int persoanaID, ReportDbContext context)
        {
            AbonamentModel abonament = GetAbonamentByPersonID(persoanaID, context).Result;

            string text = $"{abonament.NrSedinteEfectuate} / {abonament.TipAbonament.NrTotalSedinte}";
            return text;
        }
    }
}
