namespace COPI_API.Models.PIBPEntities.DTO
{
    public class ResultadoKPIInputDTO
    {
        public int KPIId { get; set; }
        public int UnidadeKPIId { get; set; }
        public int CicloId { get; set; }
        public string? Status { get; set; }
        public string? Prova { get; set; }
        public string? AvaliacaoEscrita { get; set; }
    }
}
