using Microsoft.EntityFrameworkCore;
using COPI_API.Models.PIBPEntities;
using COPI_API.Models.AdminEntities;
using COPI_API.Models.MetaEntities;
using COPI_API.Models.PIGEEntities;
using System;
using System.Collections.Generic;

namespace COPI_API
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // PIBP
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
        public DbSet<StatusPIBP> StatusPIBP { get; set; }
        public DbSet<Meta> Metas { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }
        public DbSet<AcaoEstrategica> AcoesEstrategicas { get; set; }

        // PIGE
        public DbSet<CicloPIGE> CiclosPIGE { get; set; }
        public DbSet<UnidadeKPIPIGE> UnidadesKPIPIGE { get; set; }
        public DbSet<KPIPIGE> KPIsPIGE { get; set; }
        public DbSet<EixoPIGE> EixosPIGE { get; set; }
        public DbSet<NivelPIGE> NiveisPIGE { get; set; }
        public DbSet<AvaliacaoPIGE> AvaliacoesPIGE { get; set; }
        public DbSet<ResultadoKPIPIGE> ResultadosKPIPIGE { get; set; }
        public DbSet<StatusPIGE> StatusPIGE { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Relação Meta -> AcoesEstrategicas
            modelBuilder.Entity<Meta>()
                .HasMany(m => m.AcoesEstrategicas)
                .WithOne(a => a.Meta)
                .HasForeignKey(a => a.MetaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relação AcaoEstrategica -> Tarefas
            modelBuilder.Entity<AcaoEstrategica>()
                .HasMany(a => a.Tarefas)
                .WithOne(t => t.AcaoEstrategica)
                .HasForeignKey(t => t.AcaoEstrategicaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
