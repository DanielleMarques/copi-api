namespace COPI_API.Models.DTO
{
    public class ResultadoKPIOutputDTO
    {
        public int Id { get; set; }
        public string KPI { get; set; } = string.Empty;
        public string Unidade { get; set; } = string.Empty;
        public int CicloId { get; set; }  // Adicionado CicloId
        public string Ciclo { get; set; } = string.Empty; // Nome do ciclo
        public string Status { get; set; } = string.Empty;
        public string Prova { get; set; } = string.Empty;
        public string AvaliacaoEscrita { get; set; } = string.Empty;
        public DateTime DataRegistro { get; set; }

        public string? UltimaAlteracaoPor { get; set; }
        public DateTime? UltimaAlteracaoEm { get; set; }
    }
}
