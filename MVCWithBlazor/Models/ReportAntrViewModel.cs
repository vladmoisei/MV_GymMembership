using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCWithBlazor.Models
{
    public class ReportAntrViewModel
    {
        public string Denumire { get; set; }
        [Display(Name = "Nr Total Antrenamente")]
        public int NrTotalAntrenamente { get; set; }
        [Display(Name = "Medie Persoane / Antrenament")]
        public int MediePersPerAntr { get; set; }
    }
}
