using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCWithBlazor.Models
{
    public class PersAntrAbTable
    {
        public int PersAntrAbTableID { get; set; }

        public int PersoanaModelID { get; set; }
        public virtual PersoanaModel Persoana { get; set; }
        public int AntrenamentModelID { get; set; }
        public virtual AntrenamentModel Antrenament { get; set; }
        public int AbonamentModelID { get; set; }
        public virtual AbonamentModel Abonament { get; set; }
    }
}
