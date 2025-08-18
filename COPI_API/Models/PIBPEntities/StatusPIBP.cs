using System;
using System.Collections.Generic;
namespace COPI_API.Models.PIBPEntities
{
    public static class StatusProcessoUnidadeKPI
    {
        public const string EmAnalise = "EM_ANALISE";
        public const string ParaRevisao = "PARA_REVISAO";
        public const string Aprovado = "APROVADO";
    }
    public class StatusPIBP
    {
        public int Id { get; set; }
        public int UnidadeKPIId { get; set; }
        public UnidadeKPI? UnidadeKPI { get; set; }
        public int CicloId { get; set; }
        public Ciclo? Ciclo { get; set; }
        public string? Status { get; set; }
    }
}
