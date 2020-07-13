using MVCWithBlazor.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCWithBlazor.Models
{
    public class AbonamentModel
    {
        public int AbonamentModelID { get; set; }

        [Required(ErrorMessage = "Data start abonament este obligatorie.")]
        [Display(Name = "Data Start"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateTime DataStart { get; set; }

        [Display(Name = "Data Stop"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateTime DataStop { get; set; }

        [Display(Name = "Stare")]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public StareAbonament StareAbonament { get; set; }

        [Display(Name = "Nr Sedinte Efectuate")]
        [Range(0, 100, ErrorMessage = "Nu pot fi mai mult de 100 sedinte.")]
        public int NrSedinteEfectuate { get; set; }

        [Required(ErrorMessage = "Te rog selecteaza un tip de abonament!")]
        public int TipAbonamentModelID { get; set; }
        public virtual TipAbonamentModel TipAbonament { get; set; }

        public int? PersoanaModelID { get; set; }

        public virtual PersoanaModel PersoanaModel { get; set; }

    }
}
