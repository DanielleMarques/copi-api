using System;
using System.Collections.Generic;
namespace COPI_API.Models.PIBPEntities
{
    public class Avaliacao
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public ICollection<ResultadoKPI>? Resultados { get; set; }
        public decimal NotaNP { get; set; }
        public decimal NotaNI { get; set; }
        public decimal NotaNG { get; set; }
        public bool NivelPadronizadoAtingido { get; set; }
        public bool NivelIntegradoAtingido { get; set; }
        public bool NivelGerenciadoAtingido { get; set; }
        public decimal NotaIMPIBP { get; set; }
    }
}
