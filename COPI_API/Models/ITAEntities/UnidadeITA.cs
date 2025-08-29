using COPI_API.Models.AdminEntities;
using System;
using System.Collections.Generic;

namespace COPI_API.Models.ITAEntities
{
    public class UnidadeITA
    {
        public int Id { get; set; }

        public int UnidadeId { get; set; }
        public Unidade? Unidade { get; set; }

        public int? ServidorId { get; set; }
        public Servidor? Servidor { get; set; }

        public string? SEI { get; set; }

        // Relacionamento com as avaliações ITA
        public List<AvaliacaoITA>? AvaliacoesITA { get; set; }
    }
}
