using Microsoft.EntityFrameworkCore;
using MVCWithBlazor.Data;
using MVCWithBlazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCWithBlazor.Services
{
    public class ReportService
    {
        // Get Antrenament By Month from Antrenament DB
        public List<AntrenamentModel> GetAntrenamentModelsByMonth(DateTime date, ReportDbContext context)
        {
            List<AntrenamentModel> listaAntrenamnete = context.AntrenamentModels.Where(m => m.Data.Year == date.Year && m.Data.Month == date.Month).ToList();
            return listaAntrenamnete;
        }

        // Get Abonament By Month from Antrenament DB
        public List<AbonamentModel> GetAbonamentModelsByMonth(DateTime date, ReportDbContext context)
        {
            List<AbonamentModel> listaAbonamente = context.AbonamentModels.Include(t => t.TipAbonament)
                .Include(p => p.PersoanaModel).Where(m => m.DataStart.Year == date.Year && m.DataStart.Month == date.Month).ToList();
            return listaAbonamente;
        }

        // Get Antrenament By Year from Antrenament DB
        public List<AntrenamentModel> GetAntrenamentModelsByYear(DateTime date, ReportDbContext context)
        {
            List<AntrenamentModel> listaAntrenamnete = context.AntrenamentModels.Where(m => m.Data.Year == date.Year).ToList();
            return listaAntrenamnete;
        }

        // Get Abonament By Year from Antrenament DB
        public List<AbonamentModel> GetAbonamentModelsByYear(DateTime date, ReportDbContext context)
        {
            List<AbonamentModel> listaAbonamente = context.AbonamentModels.Include(t => t.TipAbonament)
                .Include(p => p.PersoanaModel).Where(m => m.DataStart.Year == date.Year).ToList();
            return listaAbonamente;
        }

        // Get List Of ReportAbViewModel by types of Abonament plus Total of
        public List<ReportAbViewModel> GetListOfReportAbViewModelByTypeAb(ReportDbContext context)
        {
            List<ReportAbViewModel> lista = new List<ReportAbViewModel>();
            lista.Add(new ReportAbViewModel { Denumire = "Total" });
            foreach (var item in context.TipAbonamentModels)
            {
                lista.Add(new ReportAbViewModel { Denumire = item.Denumire, TotalBani = 0 }); ;
            }
            return lista;
        }

        // Get List Of ReportAbViewModel completed with values per month
        public List<ReportAbViewModel> GetListReportAbViewModelsPerMonth(ReportDbContext context, DateTime date)
        {
            List<ReportAbViewModel> listaAbViewModel = GetListOfReportAbViewModelByTypeAb(context);
            List<AbonamentModel> listaAbonamente = GetAbonamentModelsByMonth(date, context);
            foreach (var item in listaAbViewModel)
            {
                if (item.Denumire == "Total")
                { 
                    item.NrTotalAbonamente = listaAbonamente.Count;
                    foreach (var aabt in listaAbonamente)
                    {
                        item.TotalBani += aabt.TipAbonament.Pret;
                    }
                }
                else
                {
                    foreach (var tipAb in context.TipAbonamentModels)
                    {
                        if (item.Denumire == tipAb.Denumire)
                        {
                            List<AbonamentModel> listaAbDeUnTip = listaAbonamente.Where(elem => elem.TipAbonament.Denumire == item.Denumire).ToList();
                            item.NrTotalAbonamente = listaAbDeUnTip.Count;
                            foreach (var ab in listaAbDeUnTip)
                            {
                                item.TotalBani += ab.TipAbonament.Pret;
                            }

                        }
                        listaAbViewModel.Add(new ReportAbViewModel { Denumire = item.Denumire });
                    }
                }

            }

            return listaAbViewModel;
        }

        // TO DO GET LIST OF ANTRENAMENTE per GRUPA or PT
    }
}
