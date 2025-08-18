namespace COPI_API.DTOs
{
    public class ResultadoAvaliacaoDTO
    {
        public decimal NotaIMPIBP { get; set; }
        public Dictionary<string, decimal> NotasPorNivel { get; set; } = new();
        public Dictionary<string, bool> NiveisAtingidos { get; set; } = new();
        public string? NomeUnidade { get; set; }
        public string? NomeCiclo { get; set; }
        public List<KpiResultadoDTO>? ResultadosDetalhados { get; set; } = new();
    }

    public class KpiResultadoDTO
    {
        public string NomeKPI { get; set; } = string.Empty;
        public decimal Pontuacao { get; set; }
        public string? Prova { get; set; }
        public string? AvaliacaoEscrita { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Eixo { get; set; } 

    }
}
