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
        public int DivisaoId { get; set; }
        public string? OrigemMeta { get; set; }
        public DateTime? DataCumprimento { get; set; }
        public DateTime DataInic { get; set; }
        public DateTime DataFim { get; set; }
        public int Progresso { get; set; }
        public ICollection<AcaoEstrategica>? AcoesEstrategicas { get; set; }
    }
}
