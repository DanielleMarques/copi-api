namespace COPI_API.Models
{
    public class Avaliacao
    {
        public int Id { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public ICollection<ResultadoKPI>? Resultados { get; set; }

        // Nota por nível
        public decimal NotaNP { get; set; }
        public decimal NotaNI { get; set; }
        public decimal NotaNG { get; set; }

        // Flags indicando se atingiu todos os KPIs do nível
        public bool NivelPadronizadoAtingido { get; set; }
        public bool NivelIntegradoAtingido { get; set; }
        public bool NivelGerenciadoAtingido { get; set; }

        // Nota final do IM-PIBP
        public decimal NotaIMPIBP { get; set; }
    }
}



