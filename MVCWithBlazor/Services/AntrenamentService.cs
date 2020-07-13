using MVCWithBlazor.Data;
using MVCWithBlazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCWithBlazor.Services
{
    public class AntrenamentService
    {
        // Get Antrenament By Month from Antrenament DB
        public async Task<List<AntrenamentModel>> GetAntrenamentModelsByMonth(DateTime date, ReportDbContext context)
        {
            IEnumerable<AntrenamentModel> reportDbContext = await Task.FromResult(context.AntrenamentModels.Where(m => m.Data.Year == date.Year && m.Data.Month == date.Month));
            return reportDbContext.ToList();
        }
    }
}
