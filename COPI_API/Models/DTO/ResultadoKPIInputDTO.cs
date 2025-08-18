namespace COPI_API.Models.DTO
{
    public class ResultadoKPIInputDTO
    {
        public int KPIId { get; set; }
        public int UnidadeKPIId { get; set; }
        public int CicloId { get; set; }  // Adicionado CicloId
        public string Status { get; set; } = string.Empty;
        public string Prova { get; set; } = string.Empty;
        public string AvaliacaoEscrita { get; set; } = string.Empty;
    }
}