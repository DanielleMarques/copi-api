namespace COPI_API.Models.PIGEEntities.DTOS
{
    public class CicloPIGEOutputDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool? Encerrado { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
    }
}
