using Microsoft.EntityFrameworkCore;
using MVCWithBlazor.Data;
using MVCWithBlazor.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Syncfusion.Blazor.Gantt;
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
            List<AbonamentModel> abonamente = context.AbonamentModels.Include(t => t.TipAbonament).Include(p => p.PersoanaModel).Where(p => p.StareAbonament != StareAbonament.Finalizat && p.TipAbonament.IsPersonalTraining == true).ToList();

            return abonamente;
        }

        // Get Lista Abonamente Not Finalised where are group training
        public async Task<List<AbonamentModel>> GetListaAbActiveGrupa(ReportDbContext context)
        {
            List<AbonamentModel> abonamente = await Task.FromResult(context.AbonamentModels.Include(t => t.TipAbonament).Include(p => p.PersoanaModel).Where(p => p.StareAbonament != StareAbonament.Finalizat && p.TipAbonament.IsPersonalTraining == false).ToList());

            return abonamente;
        }

        // Get Lista Abonamente Not Finalised
        public async Task<List<AbonamentModel>> GetListaAbActive(ReportDbContext context)
        {
            List<AbonamentModel> abonamente = await Task.FromResult(context.AbonamentModels.Include(t => t.TipAbonament).Include(p => p.PersoanaModel).Where(p => p.StareAbonament != StareAbonament.Finalizat).ToList());

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
        public string AddPersonToAntrenament(AntrenamentModel antrenament, int abonamentID, ReportDbContext context)
        {
            AbonamentModel ab = GetAbonamentByAbID(abonamentID, context);
            if (!context.PersAntrAbTables.Any(item => item.AntrenamentModelID == antrenament.AntrenamentModelID &&
                item.AbonamentModelID == abonamentID))
            {
                if (ab.StareAbonament == StareAbonament.Finalizat)
                    return $"Persoana {ab.PersoanaModel.NumeComplet} are abonamentul finalizat!!!";
             
                context.PersAntrAbTables.Add(new PersAntrAbTable
                {
                    AntrenamentModelID = antrenament.AntrenamentModelID,
                    AbonamentModelID = abonamentID,
                    PersoanaModelID = (int)GetAbonamentByAbID(abonamentID, context).PersoanaModelID
                });
                context.SaveChanges();
                UpdateNrSedinteEfAbonamentbyID(1, abonamentID, context); // Update nr sedinte efectuate abonament
                return $"Persoana {ab.PersoanaModel.NumeComplet} a fost adaugata la antrenament!";
            }
            return $"Persoana {ab.PersoanaModel.NumeComplet} este deja adaugata in antrenament";
        }

        // Remove Person From Atrenament
        // Remove Person Atrenament Abonament From PersAntrAbTable Table
        // Remove From aboanament 1 sedinta efectuata
        public string RemovePersonFromAntrenament(AntrenamentModel antrenament, int abonamentID, ReportDbContext context)
        {
            PersoanaModel persoana= GetAbonamentByAbID(abonamentID, context).PersoanaModel;
            var persAntrAbTable = context.PersAntrAbTables
                .Include(t => t.Abonament)
                .Include(a => a.Persoana)
                .Include(aa => aa.Antrenament)
                .FirstOrDefault(m => m.AntrenamentModelID == antrenament.AntrenamentModelID && m.AbonamentModelID == abonamentID);
            if (persAntrAbTable == null)
                return $"Nu s-a gasit persoana cu numele: {persoana.NumeComplet} in antrenament!";
            context.PersAntrAbTables.Remove(persAntrAbTable);
            context.SaveChanges();
            UpdateNrSedinteEfAbonamentbyID(-1, abonamentID, context); // Update nr sedinte efectuate abonament
            return $"Persoana {persoana.NumeComplet} a fost stearsa din antrenament";
        }

        // Update Nr sedinte efectuate 
        // Daca o persoana a fost adaugata la un antrenament
        public void UpdateNrSedinteEfAbonamentbyID(int nr,int abonamentID, ReportDbContext context)
        {
            var abonamentModel = context.AbonamentModels
                .Include(t => t.TipAbonament)
                .Include(a => a.PersoanaModel)
                .FirstOrDefault(m => m.AbonamentModelID == abonamentID);

            abonamentModel.NrSedinteEfectuate += nr;

            context.Update(abonamentModel);
            context.SaveChanges();
        }

        // Update status Of Abonament
        // Verificam daca a expirat abonament (in functie de data)
        // Verificam daca a atins nr maxim de sedinte
        public void RefreshStatusAbonament(int abonamentID, ReportDbContext context)
        {
            AbonamentModel abonament = GetAbonamentByAbID(abonamentID, context);
            if (abonament == null)
                return;
            if (abonament.NrSedinteEfectuate >= abonament.TipAbonament.NrTotalSedinte)
            {
                abonament.StareAbonament = StareAbonament.Finalizat;
                return;
            }
            if (abonament.DataStop >= DateTime.Now)
                abonament.StareAbonament = StareAbonament.Finalizat;
            else if ((abonament.DataStop - abonament.DataStart).TotalDays > 32)
                abonament.StareAbonament = StareAbonament.Extins;
            else abonament.StareAbonament = StareAbonament.Activ;

            context.Update(abonament);
            context.SaveChanges();
        }

        // Update status Of Abonamente dupa zi
        public void RefreshStatusAbonamentsPerDay(DateTime date, ReportDbContext context)
        {
            var listaAbActive = GetListaAbActive(context).Result;
            foreach (var item in listaAbActive)
            {
                RefreshStatusAbonament(item.AbonamentModelID, context);
            }
        }

    }
}
