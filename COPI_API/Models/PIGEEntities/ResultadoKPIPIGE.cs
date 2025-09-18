using System;

namespace COPI_API.Models.PIGEEntities
{
    public class ResultadoKPIPIGE
    {
        public int Id { get; set; }
        public int UnidadeKPIPIGEId { get; set; }
        public UnidadeKPIPIGE? UnidadeKPIPIGE { get; set; }
        public int KPIPIGEId { get; set; }
        public KPIPIGE? KPIPIGE { get; set; }
        public int CicloPIGEId { get; set; }
        public CicloPIGE? CicloPIGE { get; set; }
        public string? Status { get; set; }
        public DateTime DataRegistro { get; set; }
        public string? Prova { get; set; }
        public string? AvaliacaoEscrita { get; set; }
    }
}
