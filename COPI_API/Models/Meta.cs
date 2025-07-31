namespace COPI_API.Models
{
    public class Meta
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public int Progresso { get; set; } // 0 a 100
        public string? Status { get; set; } // "Pendente", "Em andamento", "Concluída", "Descontinuado"
        public string? Tipo { get; set; }
        public string? Setor { get; set; }
        public int DivisaoId { get; set; }
        public string? OrigemMeta { get; set; }
        public DateTime? DataCumprimento { get; set; }
        public DateTime DataInic { get; set; }
        public DateTime DataFim { get; set; }

        // Relação: Meta tem várias AcoesEstrategicas
        public ICollection<AcaoEstrategica>? AcoesEstrategicas { get; set; }
    }
}