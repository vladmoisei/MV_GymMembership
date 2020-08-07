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
        //Get List Of Person per Antrenament
        public List<PersoanaModel> GetListOfPersonByAntrenament(AntrenamentModel antrenament, ReportDbContext context)
        {
            List<PersoanaModel> listaPersoanePerAntrenament = new List<PersoanaModel>();

            var persoaneGasite = context.PersAntrAbTables.Where(p => p.AntrenamentModelID == antrenament.AntrenamentModelID).Include(o => o.Persoana).ToList();

            if (persoaneGasite != null)
            {
                foreach (var item in persoaneGasite)
                {
                    listaPersoanePerAntrenament.Add(item.Persoana);
                }
            }

            return listaPersoanePerAntrenament;
        }

        // Get Antrenament By Month from Antrenament DB
        public List<AntrenamentModel> GetAntrenamentModelsByMonth(DateTime date, ReportDbContext context)
        {
            List<AntrenamentModel> listaAntrenamnete = context.AntrenamentModels.Where(m => m.Data.Year == date.Year && m.Data.Month == date.Month).ToList();

            // Get List of Person By Antrenament
            foreach (var antrenament in listaAntrenamnete)
            {
                antrenament.ListaPersoane = GetListOfPersonByAntrenament(antrenament, context);
            }
            return listaAntrenamnete;
        }

        // Get Abonament By Month from Antrenament DB
        public List<AbonamentModel> GetAbonamentModelsByMonth(DateTime date, ReportDbContext context)
        {
            List<AbonamentModel> listaAbonamente = context.AbonamentModels.Include(t => t.TipAbonament)
                .Include(p => p.PersoanaModel).Where(m => m.DataStart.Year == date.Year && m.DataStart.Month == date.Month).ToList();
            return listaAbonamente;
        }

        // Get Abonament By Year from Antrenament DB
        public List<AbonamentModel> GetAbonamentModelsByYear(DateTime date, ReportDbContext context)
        {
            List<AbonamentModel> listaAbonamente = context.AbonamentModels.Include(t => t.TipAbonament)
                .Include(p => p.PersoanaModel).Where(m => m.DataStart.Year == date.Year).ToList();
            return listaAbonamente;
        }

        // Get Antrenament By Year from Antrenament DB
        public List<AntrenamentModel> GetAntrenamentModelsByYear(DateTime date, ReportDbContext context)
        {
            List<AntrenamentModel> listaAntrenamnete = context.AntrenamentModels.Where(m => m.Data.Year == date.Year).ToList();
            return listaAntrenamnete;
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
                        //listaAbViewModel.Add(new ReportAbViewModel { Denumire = item.Denumire });
                    }
                }

            }

            return listaAbViewModel;
        }

        // Get List Of ReportAbViewModel completed with values per year
        public List<ReportAbViewModel> GetListReportAbViewModelsPerYear(ReportDbContext context, DateTime date)
        {
            List<ReportAbViewModel> listaAbViewModel = GetListOfReportAbViewModelByTypeAb(context);
            List<AbonamentModel> listaAbonamente = GetAbonamentModelsByYear(date, context);

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
                        //listaAbViewModel.Add(new ReportAbViewModel { Denumire = item.Denumire });
                    }
                }

            }

            return listaAbViewModel;
        }

        // Get List Of ReportAntrViewModel completed with values per month
        public List<ReportAntrViewModel> GetListReportAntrViewModelsPerMonth(ReportDbContext context, DateTime date)
        {
            List<AntrenamentModel> listaTotalAntr = GetAntrenamentModelsByMonth(date, context);
            List<AntrenamentModel> listaTotalAntrGr = GetAntrenamentModelsByMonth(date, context).Where(e => !e.IsPersonalTraining).ToList();
            List<AntrenamentModel> listaTotalAntrPT = GetAntrenamentModelsByMonth(date, context).Where(e => e.IsPersonalTraining).ToList();

            double mediePersTotalAntr = 0;
            double mediePersTotalAntrGr = 0;
            double mediePersTotalAntrPT = 0;

            foreach (var antrenament in listaTotalAntr)
            {
                if (antrenament.ListaPersoane != null && antrenament.ListaPersoane.Count > 0)
                {
                    mediePersTotalAntr += antrenament.ListaPersoane.Count;
                    if (!antrenament.IsPersonalTraining)
                    {
                        mediePersTotalAntrGr += antrenament.ListaPersoane.Count;
                    }
                    else mediePersTotalAntrPT += antrenament.ListaPersoane.Count;
                }
            }

            if (listaTotalAntr.Count > 0)
            {
                mediePersTotalAntr /= listaTotalAntr.Count;
            }

            if (listaTotalAntrGr.Count > 0)
            {
                mediePersTotalAntrGr /= listaTotalAntrGr.Count;
            }

            if (listaTotalAntrPT.Count > 0)
            {
                mediePersTotalAntrPT /= listaTotalAntrPT.Count;
            }

            return new List<ReportAntrViewModel>
            {
                new ReportAntrViewModel { Denumire = "Total Antrenamente", NrTotalAntrenamente = listaTotalAntr.Count, MediePersPerAntr = mediePersTotalAntr },
                new ReportAntrViewModel { Denumire = "Total Antrenamente Grupa", NrTotalAntrenamente = listaTotalAntrGr.Count, MediePersPerAntr = mediePersTotalAntrGr },
                new ReportAntrViewModel { Denumire = "Total Antrenamente PT", NrTotalAntrenamente = listaTotalAntrPT.Count, MediePersPerAntr = mediePersTotalAntrPT }
            };
        }

        // Get List Of ReportAntrViewModel completed with values per year
        public List<ReportAntrViewModel> GetListReportAntrViewModelsPerYear(ReportDbContext context, DateTime date)
        {
            List<AntrenamentModel> listaTotalAntr = GetAntrenamentModelsByYear(date, context);
            List<AntrenamentModel> listaTotalAntrGr = GetAntrenamentModelsByYear(date, context).Where(e => !e.IsPersonalTraining).ToList();
            List<AntrenamentModel> listaTotalAntrPT = GetAntrenamentModelsByYear(date, context).Where(e => e.IsPersonalTraining).ToList();

            double mediePersTotalAntr = 0;
            double mediePersTotalAntrGr = 0;
            double mediePersTotalAntrPT = 0;

            foreach (var antrenament in listaTotalAntr)
            {
                if (antrenament.ListaPersoane != null && antrenament.ListaPersoane.Count > 0)
                {
                    mediePersTotalAntr += antrenament.ListaPersoane.Count;
                    if (!antrenament.IsPersonalTraining)
                    {
                        mediePersTotalAntrGr += antrenament.ListaPersoane.Count;
                    }
                    else mediePersTotalAntrPT += antrenament.ListaPersoane.Count;
                }
            }

            if (listaTotalAntr.Count > 0)
            {
                mediePersTotalAntr /= listaTotalAntr.Count;
            }

            if (listaTotalAntrGr.Count > 0)
            {
                mediePersTotalAntrGr /= listaTotalAntrGr.Count;
            }

            if (listaTotalAntrPT.Count > 0)
            {
                mediePersTotalAntrPT /= listaTotalAntrPT.Count;
            }

            return new List<ReportAntrViewModel>
            {
                new ReportAntrViewModel { Denumire = "Total Antrenamente", NrTotalAntrenamente = listaTotalAntr.Count, MediePersPerAntr = mediePersTotalAntr },
                new ReportAntrViewModel { Denumire = "Total Antrenamente Grupa", NrTotalAntrenamente = listaTotalAntrGr.Count, MediePersPerAntr = mediePersTotalAntrGr },
                new ReportAntrViewModel { Denumire = "Total Antrenamente PT", NrTotalAntrenamente = listaTotalAntrPT.Count, MediePersPerAntr = mediePersTotalAntrPT }
            };
        }
    }
}