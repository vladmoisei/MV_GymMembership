using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCWithBlazor.Models
{
    public class TipAbonamentModel
    {
        public int TipAbonamentModelID { get; set; }

        [Display(Name = "Tip Abonament")]
        [Required(ErrorMessage = "Denumire este obligatorie.")]
        [StringLength(100, ErrorMessage = "Nu poate depasi 100 de caractere. ")]
        public string Denumire { get; set; }

        [Display(Name = "Nr Total Sedinte")]
        [Required(ErrorMessage = "Numar total sedinte este obligatoriu.")]
        [Range(0, 100, ErrorMessage = "Nu pot fi mai mult de 100 sedinte.")]
        public int NrTotalSedinte { get; set; }

        [Display(Name = "Antrenor Personal")]
        public bool IsPersonalTraining { get; set; }

        [Range(0, 10000, ErrorMessage = "Pretul nu poate fi mai mare de 10000.")]
        [Required(ErrorMessage = "Pretul este obligatoriu.")]
        public int Pret { get; set; }


    }
}
