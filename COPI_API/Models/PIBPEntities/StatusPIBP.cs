using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace COPI_API.Models.PIBPEntities
{
    public class StatusPIBP
    {
        [Key]
        public int Id { get; set; }

        public int UnidadeKPIId { get; set; }
        public UnidadeKPI? UnidadeKPI { get; set; }

        public int CicloId { get; set; }
        public Ciclo? Ciclo { get; set; }

        public StatusProcessoUnidadeKPI Status { get; set; } = StatusProcessoUnidadeKPI.EmAnalise;
    }

    public enum StatusProcessoUnidadeKPI
    {
        EmAnalise,
        ParaRevisao,
        Aprovado
    }
}
