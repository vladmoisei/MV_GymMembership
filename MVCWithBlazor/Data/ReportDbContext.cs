using MVCWithBlazor.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCWithBlazor.Data
{
    public class ReportDbContext : IdentityDbContext
    {
        public ReportDbContext(DbContextOptions<ReportDbContext> options)
            : base(options)
        { }

        public DbSet<PlcModel> Plcs { get; set; }
        public DbSet<TagModel> Tags { get; set; }
        public DbSet<PersoanaModel> PersoanaModels { get; set; }
        public DbSet<TipAbonamentModel> TipAbonamentModels { get; set; }
        public DbSet<AbonamentModel> AbonamentModels { get; set; }
        public DbSet<AntrenamentModel> AntrenamentModels { get; set; }
        public DbSet<PersAntrAbTable> PersAntrAbTables { get; set; }
    }
}
