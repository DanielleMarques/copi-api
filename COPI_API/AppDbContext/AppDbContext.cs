using COPI_API.Models;
using Microsoft.EntityFrameworkCore;

namespace COPI_API.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Ciclo> Ciclos { get; set; }
        public DbSet<Unidade> Unidades { get; set; }
        public DbSet<UnidadeKPI> UnidadesKPI { get; set; }
        public DbSet<KPI> KPIs { get; set; }
        public DbSet<Eixo> Eixos { get; set; }
        public DbSet<Nivel> Niveis { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<ResultadoKPI> ResultadosKPI { get; set; }
        public DbSet<Servidor> Servidores { get; set; }
        public DbSet<Divisao> Divisoes { get; set; }

    }
}
