namespace COPI_API.Models
{
    public class UnidadeKPI
    {
        public int Id { get; set; }

        public int UnidadeId { get; set; }
        public Unidade? Unidade { get; set; }

        // Associação com Servidor responsável
        public int ServidorId { get; set; }
        public Servidor? Servidor { get; set; }
        public string? SEI { get; set; }

        

        public List<ResultadoKPI>? Resultados { get; set; }
    }
}