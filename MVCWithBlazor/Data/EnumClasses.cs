using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MVCWithBlazor
{
    public enum Sex
    {
        M,
        F
    }

    public enum StareAbonament
    {
        [EnumMember(Value = "Activ")]
        Activ,
        [EnumMember(Value = "Finalizat")]
        Finalizat,
        [EnumMember(Value = "Extins")]
        Extins
    }

    public enum HelperStareAbonament
    {
        Toate,
        Activ,
        Finalizat,
        Extins
    }
}
