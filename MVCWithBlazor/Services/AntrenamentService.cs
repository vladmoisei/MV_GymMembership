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

        // Get Abonament By Person where is not Finished
        public AbonamentModel GetAbonamentByPersonID(int persoanaID, ReportDbContext context)
        {
            List<AbonamentModel> lista = context.AbonamentModels.Include(t => t.TipAbonament)
                .Include(p => p.PersoanaModel).ToList();
            AbonamentModel abonament = lista.LastOrDefault(p => p.PersoanaModelID == persoanaID && p.StareAbonament != StareAbonament.Finalizat);
            
            return abonament;
        }

        // Get string Text nr sedinte efectuate By PersonID
        public string GetNrSedinteEfByPersonID(int persoanaID, ReportDbContext context)
        {
            AbonamentModel abonament = GetAbonamentByPersonID(persoanaID, context);

            string text = $"{abonament.NrSedinteEfectuate} / {abonament.TipAbonament.NrTotalSedinte}";
            return text;
        }

        // Get Lista Abonamente Not Finalised where are personal training
        public List<AbonamentModel> GetListaAbActivePT(ReportDbContext context)
        {
            List<AbonamentModel> abonamente =context.AbonamentModels.Include(t => t.TipAbonament).Include(p => p.PersoanaModel).Where(p => p.StareAbonament != StareAbonament.Finalizat && p.TipAbonament.IsPersonalTraining == true).ToList();

            return abonamente;
        }

        // Get Lista Abonamente Not Finalised where are group training
        public async Task<List<AbonamentModel>> GetListaAbActiveGrupa(ReportDbContext context)
        {
            List<AbonamentModel> abonamente = await Task.FromResult(context.AbonamentModels.Include(t => t.TipAbonament).Include(p => p.PersoanaModel).Where(p => p.StareAbonament != StareAbonament.Finalizat && p.TipAbonament.IsPersonalTraining == false).ToList());

            return abonamente;
        }

        // Get Abonament by AbonamentID
        public AbonamentModel GetAbonamentByAbID(int abonamentID, ReportDbContext context)
        {
            return context.AbonamentModels.
                Include(t => t.TipAbonament).
                Include(p => p.PersoanaModel).FirstOrDefault(item => item.AbonamentModelID == abonamentID);
        }

        // Is Abonament coupled to Antrenament?
        public bool IsAbonamentInAntrenament(AbonamentModel abonament, AntrenamentModel antrenament, ReportDbContext context)
        {
            if (context.PersAntrAbTables.FirstOrDefault(elem => elem.AbonamentModelID == abonament.AbonamentModelID
            && elem.Antrenament.AntrenamentModelID == antrenament.AntrenamentModelID) != null) return true;
            return false;
        }
        // Add Person To Atrenament
        // Add Person Atrenament Abonament to PersAntrAbTable Table
        // Add To aboanament 1 sedinta efectuata
        public void AddPersonToAntrenament(AntrenamentModel antrenament, int abonamentID, ReportDbContext context)
        {
            context.PersAntrAbTables.Add(new PersAntrAbTable { AntrenamentModelID = antrenament.AntrenamentModelID, 
                AbonamentModelID = abonamentID, PersoanaModelID = (int)GetAbonamentByAbID(abonamentID, context).PersoanaModelID});
            context.SaveChanges();
        }
    }
}
