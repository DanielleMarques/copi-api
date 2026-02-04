namespace COPI_API.Models.DPEEntities
{
    public class AuditoriaLog
    {
        public int Id { get; set; }
        public string? Entidade { get; set; }
        public int EntidadeId { get; set; }
        public string? Acao { get; set; } // CREATE, UPDATE, DELETE
        public string? Usuario { get; set; }
        public DateTime DataHora { get; set; }
        public string? DadosAntes { get; set; }
        public string? DadosDepois { get; set; }
    }
}
