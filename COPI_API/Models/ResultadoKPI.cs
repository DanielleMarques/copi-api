namespace COPI_API.Models
{
    public class ResultadoKPI
    {
        public int Id { get; set; }

        public int UnidadeKPIId { get; set; }
        public UnidadeKPI? UnidadeKPI { get; set; }

        public int KPIId { get; set; }
        public KPI? KPI { get; set; }

        public int CicloId { get; set; }
        public Ciclo? Ciclo { get; set; }

        public string? Status { get; set; } // "SIM", "NAO", "NAO_APLICAVEL"
        public DateTime DataRegistro { get; set; }
        public string? Prova { get; set; }
        public string? AvaliacaoEscrita { get; set; }

    }
}
