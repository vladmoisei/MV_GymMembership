using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCWithBlazor.Models
{
    public class ReportAbViewModel
    {
        public string Denumire { get; set; }
        [Display(Name = "Nr Total Abonamente")]
        public int NrTotalAbonamente { get; set; }
        [Display(Name = "Incasari")]
        public int TotalBani { get; set; }
    }
}
