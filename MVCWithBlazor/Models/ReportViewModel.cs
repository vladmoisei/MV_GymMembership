using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCWithBlazor.Models
{
    public class ReportViewModel
    {
        public List<ReportAbViewModel> ListaAbonamente { get; set; }

        public List<ReportAntrViewModel> ListaAntrenamente { get; set; }
    }
}
