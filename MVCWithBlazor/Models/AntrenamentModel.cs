using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BoxMembershipApp.Models
{
    public class AntrenamentModel
    {
        public int AntrenamentModelID { get; set; }

        [Required(ErrorMessage = "Data start este obligatorie.")]
        [Display(Name = "Data"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Ora start este obligatorie.")]
        [Display(Name = "Ora Start"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime OraStart { get; set; }

        //[Required(ErrorMessage = "Ora stop este obligatorie.")]
        [Display(Name = "Ora Stop"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? OraStop { get; set; }

        [Display(Name = "Antrenor Personal"), DataType(DataType.Date)]
        public bool IsPersonalTraining { get; set; }

        [Range(0, 20, ErrorMessage = "Nu pot fi mai mult de 20 grupe.")]
        [Required(ErrorMessage = "Grupa este obligatorie.")]
        public int Grupa { get; set; }
    }
}
