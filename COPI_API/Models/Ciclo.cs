namespace COPI_API.Models
{
    public class Ciclo
    {
        public int Id { get; set; }
        public string? Nome { get; set; } // Ex: "2025.1"
        public bool? Encerrado { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
    }
}
