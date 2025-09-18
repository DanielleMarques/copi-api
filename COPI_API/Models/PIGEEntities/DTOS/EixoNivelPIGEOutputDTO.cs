namespace COPI_API.Models.PIGEEntities.DTOS
{
    public class EixoPIGEOutputDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Peso { get; set; }
    }

    public class NivelPIGEOutputDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Valor { get; set; }
    }
}
