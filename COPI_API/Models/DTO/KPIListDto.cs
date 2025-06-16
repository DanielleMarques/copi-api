namespace COPI_API.Models.DTO
{
    public class KPIListDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public double? Pontuacao { get; set; }

        public string EixoNome { get; set; } = string.Empty;
        public string NivelNome { get; set; } = string.Empty;
    }
}