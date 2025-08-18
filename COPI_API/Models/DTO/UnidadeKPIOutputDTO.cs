namespace COPI_API.Models.DTO
{
    // DTOs/UnidadeKPIOutputDTO.cs
    public class UnidadeKPIOutputDTO
    {
        public int Id { get; set; }
        public string Unidade { get; set; } = string.Empty;
        public int UnidadeId { get; set; }

        public string NomeAbreviado { get; set; } = string.Empty;  

        public string Servidor { get; set; } = string.Empty;
        public int ServidorId { get; set; }
        public string SEI { get; set; }

        public List<ResultadoSimplificadoDTO> Resultados { get; set; } = new();
    }


    public class ResultadoSimplificadoDTO
    {
        public int Id { get; set; }
        public string KPI { get; set; } = string.Empty;
        public string Eixo { get; set; } = string.Empty;
        public string Nivel { get; set; } = string.Empty;
        public string Ciclo { get; set; } = string.Empty;
        public bool CicloEncerrado { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime DataRegistro { get; set; }
    }
    // DTOs/UnidadeKPICreateDTO.cs
    public class UnidadeKPICreateDTO
    {
        public int UnidadeId { get; set; }
        public int ServidorId { get; set; }
        public string SEI { get; set; }
    }
}