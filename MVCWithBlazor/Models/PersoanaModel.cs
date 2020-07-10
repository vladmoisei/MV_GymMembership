using MVCWithBlazor.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVCWithBlazor.Models
{
    public class PersoanaModel
    {
        public int PersoanaModelID { get; set; }

        [Required(ErrorMessage = "Nume este obligatoriu.")]
        [StringLength(50, ErrorMessage = "Nu poate depasi 50 de caractere. ")]
        public string Nume { get; set; }

        [Required(ErrorMessage = "Prenume este obligatoriu. ")]
        [StringLength(50, ErrorMessage = "Nu poate depasi 50 de caractere. ")]
        public string Prenume { get; set; }
        
        [Display(Name = "Nume Persoana")]
        public string NumeComplet
        {
            get
            {
                return Prenume + " " + Nume;
            }
        }
        [Display(Name = "Adresa Email")]
        [Required(ErrorMessage = "Email este obligatoriu.")]
        [EmailAddress(ErrorMessage = "Adresa de email invalida.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon este obligatoriu.")]
        [StringLength(50, ErrorMessage = "Nu poate depasi 50 de caractere. ")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong mobile")]
        public string Telefon { get; set; }

        [Required(ErrorMessage = "Data nastere este obligatorie.")]
        [Display(Name = "Data Nastere"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateTime DataNastere { get; set; }

        [Required(ErrorMessage = "Acest camp este obligatoriu.")]
        public Sex Sex { get; set; }

        [Display(Name = "Acord GDPR")]
        [Required(ErrorMessage = "Acest camp este obligatoriu.")]
        public bool IsAcordGDPR { get; set; }

        [NotMapped]
        public virtual List<AntrenamentModel> ListaAntrenamente { get; set; }
    }
}
