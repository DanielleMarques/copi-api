using System.Collections.Generic;
using System;

namespace COPI_API.Models.PIGEEntities
{
    public class AvaliacaoPIGE
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public bool NivelGerenciadoAtingido { get; set; }
        public bool NivelIntegradoAtingido { get; set; }
        public bool NivelPadronizadoAtingido { get; set; }
        public decimal NotaIMPIGE { get; set; }
        public decimal NotaNG { get; set; }
        public decimal NotaNI { get; set; }
        public decimal NotaNP { get; set; }
        public ICollection<ResultadoKPIPIGE>? Resultados { get; set; }
    }
}
