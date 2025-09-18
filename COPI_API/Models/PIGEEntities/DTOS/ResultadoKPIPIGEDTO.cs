namespace COPI_API.Models.PIGEEntities.DTOS
{
    public class ResultadoKPIPIGEInputDTO
    {
        public int UnidadeKPIPIGEId { get; set; }
        public int KPIPIGEId { get; set; }
        public int CicloPIGEId { get; set; }
        public string? Status { get; set; }
        public string? Prova { get; set; }
        public string? AvaliacaoEscrita { get; set; }
    }

    public class ResultadoKPIPIGEOutputDTO
    {
        public int Id { get; set; }
        public string KPI { get; set; } = string.Empty;
        public string Unidade { get; set; } = string.Empty;
        public int CicloId { get; set; }
        public string Ciclo { get; set; } = string.Empty;
        public string Eixo { get; set; } = string.Empty;
        public string Nivel { get; set; } = string.Empty;
        public bool CicloEncerrado { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime DataRegistro { get; set; }
        public string? Prova { get; set; }
        public string? AvaliacaoEscrita { get; set; }
    }
}
