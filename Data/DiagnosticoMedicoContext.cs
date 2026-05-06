using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DiagnosticoMedico.Dominio;

namespace DiagnosticoMedico.Data
{
    public class DiagnosticoMedicoContext : DbContext
    {
        public DiagnosticoMedicoContext (DbContextOptions<DiagnosticoMedicoContext> options)
            : base(options)
        {
        }
        public DbSet<DiagnosticoMedico.Dominio.Examen> Examen { get; set; } = default!;
        public DbSet<DiagnosticoMedico.Dominio.ParametroExamen> ParametroExamen { get; set; } = default!;
        public DbSet<DiagnosticoMedico.Dominio.Resultado> Resultado { get; set; } = default!;
        public DbSet<DiagnosticoMedico.Dominio.Informe> Informe { get; set; } = default!;
        public DbSet<DiagnosticoMedico.Dominio.OrdenLaboratorio> OrdenLaboratorio { get; set; } = default!;
        public DbSet<DiagnosticoMedico.Dominio.OrdenExamen> OrdenExamen { get; set; } = default!;
        public DbSet<DiagnosticoMedico.Dominio.Muestra> Muestra { get; set; } = default!;
        public DbSet<DiagnosticoMedico.Dominio.ValidacionResultado> ValidacionResultado { get; set; } = default!;
    }
}
