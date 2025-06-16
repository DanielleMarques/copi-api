namespace COPI_API.Models
{
    public class Divisao
    {
        public int Id { get; set; }
        public string? Nome { get; set; } = string.Empty;

        public ICollection<Servidor>? Servidores { get; set; }
    }
}
