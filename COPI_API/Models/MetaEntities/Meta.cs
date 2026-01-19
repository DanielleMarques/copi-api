using System;
using System.Collections.Generic;
namespace COPI_API.Models.MetaEntities
{
    public class Meta
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public string? Status { get; set; }
        public string? Tipo { get; set; }
        public string? Setor { get; set; }
        public string? AvaliacaoDi { get; set; }
        public string? Entregavel { get; set; }
        public string? Objetivo { get; set; }
        public string? Premissas { get; set; }
        public string? Restricoes { get; set; }
        public string? Riscos { get; set; }
        public string? Responsavel { get; set; }
        public string? Aprovador { get; set; }
        public string? Consultado { get; set; }
        public string? Informado { get; set; }
        public int DivisaoId { get; set; }
        public string? OrigemMeta { get; set; }
        public DateTime? DataCumprimento { get; set; }
        public DateTime DataInic { get; set; }
        public DateTime DataFim { get; set; }
        public int Progresso { get; set; }
        public ICollection<AcaoEstrategica>? AcoesEstrategicas { get; set; }
    }
}
