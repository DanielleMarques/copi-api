namespace COPI_API.Models.PIGEEntities.DTOS
{
    public class KPIPIGEDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int? NivelId { get; set; }
        public int? EixoId { get; set; }
        public double? Pontuacao { get; set; }
    }

    public class KPIListPIGEDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public double? Pontuacao { get; set; }
        public string EixoNome { get; set; } = string.Empty;
        public string NivelNome { get; set; } = string.Empty;
    }
}
